using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DynamicRectThc;

public delegate void  GameObjectLightmapAssetBundleLoaderCmp(AssetBundle ab);

public class GameObjectLightmapAssetBundleLoader : MonoBehaviour
{
    public static Dictionary<string, GameObjectLightmapAssetBundleLoaderCmp> lmCmpDic = new Dictionary<string, GameObjectLightmapAssetBundleLoaderCmp>();
    public static Dictionary<string, AssetBundle> lmAbDic = new Dictionary<string, AssetBundle>();

    public static Dictionary<string, Texture2D> lightmapColorDic = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Texture2D> lightmapDirDic = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Texture2D> lightmapLightDic = new Dictionary<string, Texture2D>();

    public static LightmapData[] tempMapDatas;

    public int index;
    public Vector4 scaleOffset;
    public int count;

    public String assetBundlePath;

    public AssetBundle assetBundle;

    public Texture2D texture2DlightmapColor;
    public Texture2D texture2DlightmapDir;
    public Texture2D texture2DlightmapLight;

    public Boolean isReLoad = false;

    public static void Init(int count)
    {
        if (tempMapDatas == null)
        {
            tempMapDatas = new LightmapData[count];
            for (int i = 0; i < count; i++)
            {
                tempMapDatas[i] = new LightmapData();
            }
        }
    }

    public static void Clear() {

        foreach (AssetBundle lmAb in lmAbDic.Values) {
            lmAb.Unload(true);
        }

        lmCmpDic.Clear();
        lmAbDic.Clear();
        lightmapColorDic.Clear();
        lightmapDirDic.Clear();
        lightmapLightDic.Clear();

        tempMapDatas = null;
    }

    public void StartLoad()
    {
        //1.如果已经加载过 
        AssetBundle lmAssetBundle = null;
        GameObjectLightmapAssetBundleLoader.lmAbDic.TryGetValue(assetBundlePath, out lmAssetBundle);

        if (lmAssetBundle != null)
        {
            InitByAssetBundle(lmAssetBundle);
            return;
        }
        //2.如果正在加载中
        GameObjectLightmapAssetBundleLoaderCmp gameObjectLightmapAssetBundleLoaderCmp = null;
        lmCmpDic.TryGetValue(assetBundlePath, out gameObjectLightmapAssetBundleLoaderCmp);

        if (gameObjectLightmapAssetBundleLoaderCmp == null)
        {
            gameObjectLightmapAssetBundleLoaderCmp = InitByAssetBundle;
            lmCmpDic.Add(assetBundlePath, gameObjectLightmapAssetBundleLoaderCmp);
            StartCoroutine(loadLightMapAssetBundle());
        }
        else
        {
            lmCmpDic[assetBundlePath] += InitByAssetBundle;
        }
        isReLoad = false;
    }

    void Start() {
        StartLoad();
    }

    IEnumerator loadLightMapAssetBundle()
    {
        WWW www = new WWW("file://" + Application.streamingAssetsPath + "/"+assetBundlePath);     

        yield return www;  

        if (www.isDone)
        {
            assetBundle = www.assetBundle;

            GameObjectLightmapAssetBundleLoader.lmAbDic.Add(assetBundlePath, assetBundle);

            //InitByAssetBundle();

            String[] allAssetNames = assetBundle.GetAllAssetNames();
            LightmapData data = tempMapDatas[index];
            if (allAssetNames.Length == 1)
            {
                texture2DlightmapLight = assetBundle.LoadAsset(allAssetNames[0]) as Texture2D;

                data.lightmapColor = texture2DlightmapLight;
            }
            else
            if (allAssetNames.Length == 2)
            {
                texture2DlightmapColor = assetBundle.LoadAsset(allAssetNames[0]) as Texture2D;
                texture2DlightmapDir = assetBundle.LoadAsset(allAssetNames[1]) as Texture2D;

                //LightmapData data = new LightmapData();
                data.lightmapColor = texture2DlightmapColor;
                data.lightmapDir = texture2DlightmapDir;
            }

            GameObjectLightmapAssetBundleLoaderCmp gameObjectLightmapAssetBundleLoaderCmp = null;
            lmCmpDic.TryGetValue(assetBundlePath, out gameObjectLightmapAssetBundleLoaderCmp);

            if (gameObjectLightmapAssetBundleLoaderCmp != null) {

                //gameObjectLightmapAssetBundleLoaderCmp(assetBundle);
                Delegate[] invocationList = gameObjectLightmapAssetBundleLoaderCmp.GetInvocationList();
                foreach (Delegate i in invocationList)
                {
                    i.DynamicInvoke( assetBundle );
                }

                lmCmpDic.Remove(assetBundlePath);
            }
        }
    }

    public void InitByAssetBundle(AssetBundle ab)
    {
        Debug.LogWarning(">>>>>> InitByAssetBundle =  " + assetBundlePath + " " + ab);

        assetBundle = ab;

        LightmapData data = tempMapDatas[index];

        String[] allAssetNames = assetBundle.GetAllAssetNames();
     
        if (allAssetNames.Length == 1)
        {
            texture2DlightmapLight = assetBundle.LoadAsset(allAssetNames[0]) as Texture2D;
            data.lightmapColor = texture2DlightmapLight;
        }
        else
        if (allAssetNames.Length == 2)
        {
            texture2DlightmapColor = assetBundle.LoadAsset(allAssetNames[0]) as Texture2D;
            texture2DlightmapDir = assetBundle.LoadAsset(allAssetNames[1]) as Texture2D;

            //LightmapData data = new LightmapData();
            data.lightmapColor = texture2DlightmapColor;
            data.lightmapDir = texture2DlightmapDir;
        }

        if (LightmapSettings.lightmaps == null || LightmapSettings.lightmaps.Length == 0)
        {
            //LightmapData[] tempMapDatas = new LightmapData[count];
            //LightmapSettings.lightmaps = tempMapDatas;
        }

       // tempMapDatas[index] = data;
        LightmapSettings.lightmaps = tempMapDatas;
        //LightmapSettings.lightmaps[index] = data;

        Renderer tempRenderer = GetComponent<Renderer>();
        tempRenderer.lightmapIndex = index;
        tempRenderer.lightmapScaleOffset = scaleOffset;

        if (LightmapSettings.lightmapsMode != LightmapsMode.NonDirectional)
            //设置原来烘焙时的光照模式，这个不设置正确，默认模式就会只显示光照贴图
            LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
    }

    public static GameObjectLightmapAssetBundleLoader Load(string path) {

        GameObject go = GameObject.Find(path);
        if (go != null) {
            GameObjectLightmapAssetBundleLoader gameObjectLightmapAssetBundleLoader = go.GetComponent<GameObjectLightmapAssetBundleLoader>();
            if (gameObjectLightmapAssetBundleLoader) {
                gameObjectLightmapAssetBundleLoader.isReLoad = true;
                return gameObjectLightmapAssetBundleLoader;
            }
            return go.AddComponent<GameObjectLightmapAssetBundleLoader>();
        }
        return null;
    }

}

public class TestLoadLightmapAssetBundle : MonoBehaviour
{
    /// <summary>
    /// AssetBundles 在 streamingAssets 下的路径
    /// </summary>
    public String streamingAssetsPath = "/Windows/scenelightmapdata.unity3d";

    public Texture2D[] lmArr;
    public AssetBundle assetBundle;

    void Start()
    {
        //GameObject goRoot = GameObject.Instantiate(Resources.Load("SceneRoot") as GameObject);
        //goRoot.name = "SceneRoot";
    }  

    IEnumerator loadSceneLightmapData(Action<SceneLightmapData> callback)
    {
        int sds = 0;
        if (assetBundle != null)
        {
            String[] allAssetNames = assetBundle.GetAllAssetNames();
            if (allAssetNames.Length == 1)
            {
                SceneLightmapData sceneLightmapData2 = assetBundle.LoadAsset(allAssetNames[0]) as SceneLightmapData;
                Debug.Log(sceneLightmapData2.m_sceneName);
                callback(sceneLightmapData2);
               // StopCoroutine("loadSceneLightmapData");
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            WWW www = new WWW("file://" + Application.streamingAssetsPath + streamingAssetsPath);

            yield return www;
            if (www.isDone)
            {
                assetBundle = www.assetBundle;
                String[] allAssetNames = assetBundle.GetAllAssetNames();
                if (allAssetNames.Length == 1)
                {
                    SceneLightmapData sceneLightmapData2 = www.assetBundle.LoadAsset(allAssetNames[0]) as SceneLightmapData;
                    Debug.Log(sceneLightmapData2.m_sceneName);
                    callback(sceneLightmapData2);
                }
            }
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 110, 100, 100), "加载光照贴图1"))
        {
            StartCoroutine(
                            loadSceneLightmapData(
                                                (sceneLightmapData) =>
                                                {
                                                    //设置场景的灯光信息
                                                    SceneLightmapData scenemapData = sceneLightmapData as SceneLightmapData;
                                                    int Count = scenemapData.m_lightMapNearName.Length;

                                                    LightmapData[] tempMapDatas = new LightmapData[Count];

                                                    for (int i = 0; i < Count; i++)
                                                    {
                                                        LightmapData data = new LightmapData();
                                                        //Texture2D near = Resources.Load(scenemapData.m_lightMapNearName[i]) as Texture2D;
                                                        //Texture2D far = Resources.Load(scenemapData.m_lightMapFarName[i]) as Texture2D;
                                                        Texture2D near = lmArr[i] as Texture2D;
                                                        //Texture2D far = lmArr[i * 2 + 1] as Texture2D;

                                                        //data.lightmapColor = far;
                                                        //data.lightmapDir = near;
                                                        data.lightmapColor = near;
                                                        //data.shadowMask = near;

                                                        tempMapDatas[i] = data;
                                                    }

                                                    LightmapSettings.lightmaps = tempMapDatas;
                                                    for (int i = 0; i < scenemapData.m_gameObjectList.Count; i++)
                                                    {
                                                        GameObject go = GameObject.Find(scenemapData.m_gameObjectList[i].m_name);
                                                        if (go == null)
                                                            continue;
                                                        Transform tempTrans = go.transform;
                                                        Renderer tempRenderer = tempTrans.GetComponent<Renderer>();
                                                        tempRenderer.lightmapIndex = scenemapData.m_gameObjectList[i].m_lightmapIndex;
                                                        tempRenderer.lightmapScaleOffset = scenemapData.m_gameObjectList[i].m_lightmapScaleOffset;

                                                        //tempRenderer.sharedMaterial.shader = Shader.Find(tempRenderer.sharedMaterial.shader.name);
                                                    }

                                                    //设置原来烘焙时的光照模式，这个不设置正确，默认模式就会只显示光照贴图
                                                    LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;

                                                } 
                                            )
                       );
            
        }
        if (GUI.Button(new Rect(10 + 100, 10, 100, 100), "重置光照贴图")) {
            GameObjectLightmapAssetBundleLoader.Clear();   
        }
        if (GUI.Button(new Rect(10, 10, 100, 100), "加载光照贴图"))
        {
            StartCoroutine(loadSceneLightmapData((sceneLightmapData) => {
                int z = 0;
                //设置场景的灯光信息
                SceneLightmapData scenemapData = sceneLightmapData as SceneLightmapData;
                int Count = scenemapData.m_lightMapNearName.Length;

                GameObjectLightmapAssetBundleLoader.Init(Count);
              
                AssetBundle lmAssetBundle = null;
                for (int i = 0; i < scenemapData.m_gameObjectList.Count; i++ ) {

                    lmAssetBundle = null;
                    GameObjectLightmapAssetBundleLoader.lmAbDic.TryGetValue(scenemapData.m_gameObjectList[i].lightMapName,out lmAssetBundle);
                    if (lmAssetBundle == null)
                    {
                        GameObjectLightmapAssetBundleLoader gameObjectLightmapAssetBundleLoader = GameObjectLightmapAssetBundleLoader.Load(scenemapData.m_gameObjectList[i].m_name);
                        if (gameObjectLightmapAssetBundleLoader != null) {
                            gameObjectLightmapAssetBundleLoader.assetBundlePath = scenemapData.m_gameObjectList[i].lightMapName;
                            gameObjectLightmapAssetBundleLoader.index = scenemapData.m_gameObjectList[i].m_lightmapIndex;
                            gameObjectLightmapAssetBundleLoader.scaleOffset = scenemapData.m_gameObjectList[i].m_lightmapScaleOffset;
                            gameObjectLightmapAssetBundleLoader.count = Count;

                            if (gameObjectLightmapAssetBundleLoader.isReLoad) {
                                gameObjectLightmapAssetBundleLoader.StartLoad(); 
                            }
                        }

                    } else {
                        GameObject go = GameObject.Find(scenemapData.m_gameObjectList[i].m_name);
                        if (go == null)
                            continue;
                        Transform tempTrans = go.transform;
                        Renderer tempRenderer = tempTrans.GetComponent<Renderer>();
                        tempRenderer.lightmapIndex = scenemapData.m_gameObjectList[i].m_lightmapIndex;
                        tempRenderer.lightmapScaleOffset = scenemapData.m_gameObjectList[i].m_lightmapScaleOffset;
                    }
                }

                //设置原来烘焙时的光照模式，这个不设置正确，默认模式就会只显示光照贴图
                LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;

            }));
            
            #region 加载
            /*
            StartCoroutine(loadTest((obj) => {

                //设置场景的灯光信息
                SceneLightmapData scenemapData = obj as SceneLightmapData;
                int Count = scenemapData.m_lightMapNearName.Length;
                LightmapData[] tempMapDatas = new LightmapData[Count];
                for (int i = 0; i < Count; i++)
                {
                    LightmapData data = new LightmapData();
                    Texture2D near = Resources.Load(scenemapData.m_lightMapNearName[i]) as Texture2D;
                    Texture2D far = Resources.Load(scenemapData.m_lightMapFarName[i]) as Texture2D;
                    data.lightmapColor = far;
                    data.lightmapDir = near;
                    tempMapDatas[i] = data;
                }
                LightmapSettings.lightmaps = tempMapDatas;

                //设置每一个GameObject的lightmapIndex，lightmapScaleOffset
                Transform sceneRootTrans = GameObject.Find(scenemapData.m_gameObjectRoot).transform;
                for (int i = 0; i < scenemapData.m_gameObjectList.Count; i++)
                {
                    Transform tempTrans = sceneRootTrans.Find(scenemapData.m_gameObjectList[i].m_name);
                    Renderer tempRenderer = tempTrans.GetComponent<Renderer>();
                    tempRenderer.lightmapIndex = scenemapData.m_gameObjectList[i].m_lightmapIndex;
                    tempRenderer.lightmapScaleOffset = scenemapData.m_gameObjectList[i].m_lightmapScaleOffset;
                }

                //动态加载完之后需要调用一下StaticBatchingUtility.Combine(RootGameObject)来将整个场景静态化，使得batching能正常使用。
                StaticBatchingUtility.Combine(sceneRootTrans.gameObject);

            }));
            */
            #endregion
        }
    }

}
