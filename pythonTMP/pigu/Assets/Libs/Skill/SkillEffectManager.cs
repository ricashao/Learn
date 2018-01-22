using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能特效的管理

public class SkillEffectManager : MonoBehaviour {

    private static SkillEffectManager instance;
    public static SkillEffectManager getInstance()
    {
        return instance;
    }

    public void Awake()
    {
        instance = this;
    }

    struct ResData
    {
        public int effectId;
        public string bundleName;
        public string resName;
        public bool loadAndPlay;
        public Transform playPos;
        public float endTime;
    }

    struct EffectData
    {
        public int effectId;
        public GameObject effectObj;
        public float startTime;
        public float endTime;
    }

    Dictionary<int, List<EffectData>> effectDic = new Dictionary<int, List<EffectData>>();

    Queue<ResData> ResLoadQue = new Queue<ResData>();   //因为是异步加载，所有需要使用队列来保证加载顺序
    private bool m_isPacketProcessing = false;
    ResData curLoadRes;         //用来保存当前正在加载的资源
    // Use this for initialization
    void Start () {
        
    }

    public void LoadEffect(int _effectId)
    {
        //直接加载资源
        LoadSkillEffect(_effectId, false, null);
    }

    public void PlayEffect(int _effectId,Transform _pos)
    {
        //先查找是否加载过资源
        if (!effectDic.ContainsKey(_effectId))
        {
            //没有加载过需要进行加载
            LoadSkillEffect(_effectId,true, _pos);
            return;
        }
        else
        {
            //查询是否有未使用的特效
            for(int i = 0; i < effectDic[_effectId].Count; ++i)
            {
                EffectData _data = effectDic[_effectId][i];
                if (!_data.effectObj.activeSelf)
                {
                    _data.effectObj.transform.position = _pos.position;
                    _data.effectObj.transform.forward = _pos.forward;
                    _data.effectObj.SetActive(true);
                    _data.startTime = Time.time;
                    effectDic[_effectId][i] = _data;
                    return;
                }
            }

            //未找到，需要新实例化一个进行使用
            EffectData _effect = new EffectData();
            _effect.effectObj = GameObject.Instantiate(effectDic[_effectId][0].effectObj, this.transform, false);
            _effect.effectObj.transform.position = _pos.position;
            _effect.effectObj.transform.forward = _pos.forward;
            _effect.effectObj.SetActive(true);
            _effect.startTime = Time.time;
            _effect.endTime = effectDic[_effectId][0].endTime;
            effectDic[_effectId].Add(_effect);
        }
    }

    public void OnDestroy()
    {
        foreach(var Value in effectDic)
        {
            for(int i = 0; i < Value.Value.Count; ++i)
            {
                GameObject.Destroy(Value.Value[i].effectObj);
            }
        }
    }

    public void LoadSkillEffect(int _effectId,bool _isPlay,Transform _pos)
    {
        //读取配置表 加载特效名
        CSVFile _config = CVS.Instance.getFile("Effect");
        if (_config == null)
        {
            return;
        }
        string path_str = _config.GetDataByIdAndName(_effectId, "ActionSpecialEffects");
        if(path_str.Length <= 0)
        {
            return;
        }

        string[] pathArr = path_str.Split('|');

        if(pathArr.Length > 0)
        {
            ResData _res = new ResData();
            _res.effectId = _effectId;
            _res.bundleName = pathArr[0] + ".effect";
            _res.resName = pathArr[1];
            _res.playPos = _pos;
            _res.loadAndPlay = _isPlay;
            _res.endTime = _config.GetDataByIdAndNameToFloat(_effectId, "StopTime") / 1000;
            ResLoadQue.Enqueue(_res);
        }
        
    }

    void OnLoadAssetBundle(string eventName, AssetBundle assetBundle)
    {
        Libs.AssetManager.getInstance().CreateAsync(assetBundle, curLoadRes.resName, OnCreate);
    }

    void OnCreate(string eventName, Object data)
    {
        //加载成功
        GameObject obj = data as GameObject;

        if (!effectDic.ContainsKey(curLoadRes.effectId))
        {
            //第一次
            List<EffectData> _list = new List<EffectData>();
            effectDic[curLoadRes.effectId] = _list;
        }

        EffectData _effect = new EffectData();
        _effect.effectObj = GameObject.Instantiate(obj, this.transform, false);
        _effect.endTime = curLoadRes.endTime;
        if (curLoadRes.loadAndPlay)
        {
            _effect.effectObj.transform.position = curLoadRes.playPos.position;
            _effect.effectObj.transform.forward = curLoadRes.playPos.forward;
            _effect.effectObj.SetActive(true);
            _effect.startTime = Time.time;
        }
        else
        {
            _effect.effectObj.SetActive(false);
        }

        effectDic[curLoadRes.effectId].Add(_effect);

        m_isPacketProcessing = false;
    }

    public void ClearAll()
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
        if(ResLoadQue.Count > 0)
        {
            //有资源需要加载
            if (!m_isPacketProcessing)
            {
                //开始加载资源
                curLoadRes = ResLoadQue.Dequeue();
                m_isPacketProcessing = true;

                Libs.AssetBundleManagar.getInstance().Load(curLoadRes.bundleName, OnLoadAssetBundle);
            }
        }

        //控制特效是否需要关闭
        foreach(var effectList in effectDic)
        {
            for(int i = 0; i < effectList.Value.Count; ++i)
            {
                if (!effectList.Value[i].effectObj.activeSelf)
                {
                    continue;
                }
                if(Time.time - effectList.Value[i].startTime > effectList.Value[i].endTime + 0.5f)
                {
                    effectList.Value[i].effectObj.SetActive(false);
                }
            }
        }
	}
}
