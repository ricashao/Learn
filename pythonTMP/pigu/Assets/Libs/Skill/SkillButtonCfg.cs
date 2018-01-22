using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>

/// </summary>
public class SkillButtonCfg : MonoBehaviour
{
    static public bool isUndo = false;
    public int index;
    public string playerTag = "Player";
    public GameObject player;
    Animation ani;
    public string skillName = "Attack1";
    //必须保证有取消释放
    public GameObject UndoBtn;

    public Transform skillReleaseDot;
    public Transform skillAttackDot;

    Skill skill;

    Image m_cdImage;
    Text m_cdText;
    bool m_IsCDRun = false;
    float m_cdTime = 0;     //cd的总事件
    float m_cdStart = 0;

    SkillAttackRange m_SkillRange;
    public SkillReleaseRange skillReleaseRange;

    bool IsOnDrag = false;

    public Image skillControllerEventBg;

    #region 释放状态提示
    //public GameObject skillReleaseBg;
    //public GameObject skillReleaseThumb;
    #endregion

    #region 释放状态提示2
    public SkillReleaseDirController skillReleaseDirController;
    #endregion

    public SkillCastType skillCastType = SkillCastType.SKILL_SECTOR;
    // Use this for initialization
    void Start()
    {
        /*
        skill = new Skill();
        skill.m_CastType = SkillCastType.SKILL_SECTOR;  
        //skill.m_CastType = SkillCastType.SKILL_RECT;
        //skill.m_CastType = SkillCastType.SKILL_CIRCLE;
        */
        #region 拖拽事件监听
        Add(EventTriggerType.BeginDrag, OnBeginDrag);
        Add(EventTriggerType.Drag, OnDrag);
        Add(EventTriggerType.EndDrag, OnEndDrag);
        Add(EventTriggerType.PointerClick, OnClick);
        Add(EventTriggerType.PointerDown, OnBtnDown);  
        Add(EventTriggerType.PointerUp, OnBtnUp);
        #endregion

        #region 释放状态提示
        /*
        skillReleaseBg = GameObject.Find("SkillReleaseBg");
        if (skillReleaseBg) {
            skillReleaseBg.GetComponent<Image>().enabled = false;
        }
        skillReleaseThumb = GameObject.Find("SkillReleaseThumb");
        if (skillReleaseThumb) {
            skillReleaseThumb.GetComponent<Image>().enabled = false;
            //释放提示点不接受事件
            skillReleaseThumb.GetComponent<Image>().raycastTarget = false;
        }
        */
        #endregion

        #region 背景，事件捕捉层
        if (skillControllerEventBg == null) {
            skillControllerEventBg = transform.parent.GetComponent<Image>();
            skillControllerEventBg.enabled = false;
        }
        #endregion

        #region 释放状态提示2
        if (skillReleaseDirController == null)
        skillReleaseDirController = FindObjectOfType<SkillReleaseDirController>();
        skillReleaseDirController.Hide();
        #endregion

        #region cd控件绑定
        m_cdImage = transform.Find("CD").GetComponent<Image>();
        m_cdText = transform.Find("Text").GetComponent<Text>();
        if( UndoBtn == null )
        UndoBtn = transform.parent. Find("Undo").gameObject;
        #endregion
    }
 
    public void BeginCoolDown(float _time)
    {
        m_cdImage.gameObject.SetActive(true);
        m_cdText.gameObject.SetActive(true);
        m_cdTime = _time;
        m_cdText.text = m_cdTime.ToString();
        m_IsCDRun = true;
        m_cdStart = Time.time;
    }

    public void CoolDownUpdate()
    {
        if (m_IsCDRun)
        {
            //先计算当前的比例
            float _interval = Time.time - m_cdStart;

            if (m_cdTime - _interval <= 0)
            {
                //cd结束
                CoolDownEnd();
                return;
            }

            float _ratio = (m_cdTime - _interval) / m_cdTime;

            m_cdImage.fillAmount = _ratio;

            float _showTime = m_cdTime - _interval;
            m_cdText.text = _showTime.ToString("f1");
        }
    }

    public void CoolDownEnd()
    {
        m_IsCDRun = false;
        m_cdImage.gameObject.SetActive(false);
        m_cdText.gameObject.SetActive(false);
        //skill.ClearCD();
    }

    public void Add(EventTriggerType type, UnityAction<BaseEventData> callbackp)
    {
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(callbackp);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);
        GetComponent<EventTrigger>().triggers.Add(entry);
    }

    public void OnBtnDown(BaseEventData eventData)
    {
        if (m_IsCDRun)
        {
            //技能正在cd中
            return;
        }
        PointerEventData pointerEventData = eventData as PointerEventData;
        //初始化技能释放点位置
        skillAttackDot.position = player.transform.position;
        if (skillAttackRange != null)
        {
            if (skillCastType == SkillCastType.SKILL_SECTOR)
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Sector;
            else if (skillCastType == SkillCastType.SKILL_RECT)
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Rect;
            else if (skillCastType == SkillCastType.SKILL_CIRCLE)
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Circle;
            else if (skillCastType == SkillCastType.SKILL_SELF_CIRCLE)
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.SelfCircle;
        }
        if (skill != null)
        {
            skillAttackRange.rangeParameter = skill.m_angle;
            skillAttackRange.distance = skill.m_MaxRadius;
            skillAttackRange.directionAngle = player.transform.eulerAngles.y;
            ShowSkillReleaseDot();
            ShowSkillReleaseRange();
        }
    }

    public void OnBtnUp(BaseEventData eventData)
    {
        //禁用背景，停止接受事件
        skillControllerEventBg.enabled = false;

        PointerEventData pointerEventData = eventData as PointerEventData;
        if (!IsOnDrag)
        {
            HideSkillReleaseRange();
            HideSkillAttackRange();
        }
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        if (m_IsCDRun)
        {
            //技能正在cd中
            return;
        }

        skillControllerEventBg.enabled = true;

        PointerEventData pointerEventData = eventData as PointerEventData;
        //Debug.LogWarning("LeftFreeCtrl.OnBeginDrag" + pointerEventData.position);
        //初始化技能释放点位置
        if (skillCastType == SkillCastType.SKILL_SECTOR ||
            skillCastType == SkillCastType.SKILL_RECT
            ) {
            skillAttackDot.position = player.transform.position;
        }
        if (skillCastType == SkillCastType.SKILL_CIRCLE) {
            skillAttackDot.position = skillReleaseDot.position;
            //skillAttackDot.position = player.transform.position;
        }

        if (skillAttackRange != null)
        {
            if (skillCastType == SkillCastType.SKILL_SECTOR)
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Sector;
            else if (skillCastType == SkillCastType.SKILL_RECT)
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Rect;
            else if (skillCastType == SkillCastType.SKILL_CIRCLE)
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Circle;
            else if(skillCastType == SkillCastType.SKILL_SELF_CIRCLE)
            {
                skillAttackRange.skillAttackRangeType = SkillAttackRangeType.SelfCircle;
            }
        }
        if (skill == null)
        {
            //测试数据  
            if (skillCastType == SkillCastType.SKILL_SECTOR)
                skillAttackRange.rangeParameter = 90f;
            else if (skillCastType == SkillCastType.SKILL_RECT)
                skillAttackRange.rangeParameter = 2f;
            else if (skillCastType == SkillCastType.SKILL_CIRCLE)
                skillAttackRange.rangeParameter = 2.6f;
            //ene 测试数据
        } else {

            skillAttackRange.rangeParameter = skill.m_angle;
            skillAttackRange.distance = skill.m_MaxRadius;
        }
        //设置背景圈坐标
        /*
        skillReleaseBg.GetComponent<Image>().enabled = true;
        skillReleaseBg.transform.position = transform.position;
        skillReleaseThumb.GetComponent<Image>().enabled = true;
        skillReleaseThumb.transform.position = transform.position;
        */
        //设置技能释放提示
        skillReleaseDirController.transform.position = transform.position;
        skillReleaseDirController.Show();
        skillReleaseDirController.OnBeginDrag(eventData);
        ShowSkillReleaseDot();

        ShowSkillReleaseRange();

        IsOnDrag = true;

        //ShowSkillAttackRange(pointerEventData.position);
    }

    public void OnDrag(BaseEventData eventData)
    {
        if (m_IsCDRun)
        {
            //技能正在cd中
            return;
        }

        UndoBtn.SetActive(true);

        PointerEventData pointerEventData = eventData as PointerEventData;
        //Debug.LogWarning("LeftFreeCtrl.OnDrag" + pointerEventData.position);

        //初始化技能释放点位置
        if (skillCastType == SkillCastType.SKILL_SECTOR ||
            skillCastType == SkillCastType.SKILL_RECT
            )
        {
            skillAttackDot.position = player.transform.position;
        }
        if (skillCastType == SkillCastType.SKILL_CIRCLE)
        {
            skillAttackDot.position = skillReleaseDot.position;
            //skillAttackDot.position = player.transform.position;
        }

        //skillReleaseThumb.transform.position = pointerEventData.position;
        //更新释放点位置
        //ShowSkillAttackRange(pointerEventData.position);
        //技能释放控制器
        //skillReleaseDirController.Show();
        skillReleaseDirController.OnDrag(eventData);
        ShowSkillReleaseDot();
        if (skillCastType == SkillCastType.SKILL_CIRCLE)
        {
            m_SkillRange.releasePot = skillReleaseDot.position;
        }
        if (skillCastType == SkillCastType.SKILL_SECTOR ||
           skillCastType == SkillCastType.SKILL_RECT
           )
        {
            m_SkillRange.releasePot = player.transform.position;
        }

        /*
        Vector3 curReleasePosition = skillReleaseDirController.GetCurReleasePosition(player.transform.position);
        skillReleaseDot.position = curReleasePosition;
        skillAttackRange.LookAt(skillReleaseDot.position);
        */
        ShowAttackAblePlayer();
    }

    public void OnClick(BaseEventData eventData)
    {
        HideSkillReleaseRange();
        HideSkillAttackRange();
        /*
        GameMain gameMain = GameMain.getInstance();
        if (gameMain != null)
        {
            Transform _target = skill.QuickSelectTarget();
            if (_target != null)
            {
                skillAttackRange.SetAttackTarget(_target.position);
                gameMain.m_SkillMgr.UseSkill(index, _target.position);
            }
            else
            {
                Vector3 _dir = player.transform.position + player.transform.forward;
                gameMain.m_SkillMgr.UseSkill(index, _dir);
            }

        }
        */
    }

    public void OnEndDrag(BaseEventData eventData)  
    {
        if (m_IsCDRun)
        {
            //技能正在cd中
            return;
        }
        //设置背景圈坐标
        /*
        skillReleaseBg.GetComponent<Image>().enabled = false;
        skillReleaseThumb.GetComponent<Image>().enabled = false;
        */

        skillControllerEventBg.enabled = false;

        //隐藏释放提示
        skillReleaseDirController.OnEndDrag(eventData);
 
        UndoBtn.SetActive(false);
        //撤销释放
        if (isUndo == true)
        {
            HideSkillReleaseRange();
            HideSkillAttackRange();
            HideAttackAblePlayer();
            skillReleaseDirController.Hide();
            isUndo = false;
            return;
        }
        PointerEventData pointerEventData = eventData as PointerEventData;
        //Debug.LogWarning("LeftFreeCtrl.OnEndDrag" + pointerEventData.position); 

        //ShowSkillAttackRange(pointerEventData.position);  
          
        Release();

        HideSkillReleaseRange();
        HideSkillAttackRange();

        HideAttackAblePlayer();

        skillReleaseDirController.Hide();

        IsOnDrag = false;
        /*
        foreach (var player in GameMain.getInstance().m_PlayerMgr.m_otherPlayerGroup)
        {
            player.Value.objInstantiate.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        */
    }
        
    public void ShowSkillReleaseRange()
    {
        if (skillReleaseRange == null) {
            skillReleaseRange = GameObject.FindObjectOfType<SkillReleaseRange>();
        }
        if (skillReleaseRange == null) {
            Debug.LogError("没有找到 SkillReleaseRange 组件");
            return;
        }

        skillReleaseRange.visible = true;
    }

    public void HideSkillReleaseRange()
    {
        if (skillReleaseRange == null)
        {
            return;
        }
        skillReleaseRange.visible = false;
        //skillAttackDot.position = Vector3.zero;
    }

    public SkillAttackRange skillAttackRange;

    public void ShowSkillReleaseDot() {

        Vector3 curReleasePosition = skillReleaseDirController.GetCurReleasePosition(
                                                                                    player.transform.position,
                                                                                    skillAttackRange.distance);
        skillReleaseDot.position = curReleasePosition;

        if (skillCastType == SkillCastType.SKILL_CIRCLE) {
            //范围技能不进行朝向设置
        }
        else
        {
            skillAttackRange.LookAt(skillReleaseDot.position);
        }
        skillAttackRange.visible = true;
    }

    //设置释放点位置
    public void ShowSkillAttackRange(Vector3 p) { ShowSkillAttackRange(p.x, p.y); }
    //设置释放点位置
    public void ShowSkillAttackRange(float x, float y)
    {
        if (skillAttackRange == null)
        {
            skillAttackRange = GameObject.FindObjectOfType<SkillAttackRange>();
        }
        if (skillAttackRange == null)
        {
            Debug.LogError("没有找到 SkillReleaseRange 组件");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
        RaycastHit hit;

        int layerMask;

        layerMask = LayerMask.GetMask("Road");
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            //targetPosition = hit.point;
            // skillReleaseDot;
            skillReleaseDot.position = hit.point;

            skillAttackRange.LookAt(skillReleaseDot.position);

        } else {
            skillReleaseDot.position = Vector3.zero;
        }

        skillAttackRange.visible = true;
    }

    public void HideSkillAttackRange() {

        //skillReleaseDot.position = Vector3.zero;

        skillAttackRange.visible = false;
    }

    void TryInit()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
        }
        if (skillReleaseRange == null)
        {
            skillReleaseRange = GameObject.FindObjectOfType<SkillReleaseRange>();
        }
        if (skillAttackRange == null)
        {
            skillAttackRange = GameObject.FindObjectOfType<SkillAttackRange>();

            if (skillAttackRange != null)
            {
                if (skillCastType == SkillCastType.SKILL_SECTOR)
                    skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Sector;
                else if (skillCastType == SkillCastType.SKILL_RECT)
                    skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Rect;
                else if (skillCastType == SkillCastType.SKILL_CIRCLE)
                    skillAttackRange.skillAttackRangeType = SkillAttackRangeType.Circle;
            }
        }
        if (skillReleaseDot == null)
        {
            GameObject skillReleaseDotGo = GameObject.Find("SkillReleaseDot");
            if (skillReleaseDotGo == null) {
                skillReleaseDotGo = new GameObject("SkillReleaseDot");
            }
            if(skillReleaseDotGo != null)
            skillReleaseDot = skillReleaseDotGo.transform;
        }
        if (skillAttackDot == null)
        {
            GameObject skillAttackDotGo = GameObject.Find("SkillAttackDot");
            if (skillAttackDotGo == null)
            {
                skillAttackDotGo = new GameObject("SkillAttackDot");
            }
            if (skillAttackDotGo != null && skillAttackRange != null)
            {
                skillAttackDot = skillAttackDotGo.transform;
                skillAttackRange.target = skillAttackDot;
            }
        }
        if (player != null)
        {
            ani = player.GetComponent<Animation>();
            //gameObject.GetComponent<Button>().onClick.AddListener(OnClick);  
        }
        if (skill == null) {
            //测试数据
            if (skillCastType == SkillCastType.SKILL_SECTOR)
                skillAttackRange.rangeParameter = 90f;
            else if (skillCastType == SkillCastType.SKILL_RECT)
                skillAttackRange.rangeParameter = 2f;
            else if (skillCastType == SkillCastType.SKILL_CIRCLE)
                skillAttackRange.rangeParameter = 2.6f;
            //ene 测试数据
        }
        if (m_SkillRange == null)
        {
            m_SkillRange = Camera.main.GetComponent<SkillAttackRange>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (player == null)
        //{
        TryInit();
        //}
        CoolDownUpdate();
    }

    public void SetSkillCfg(int _index)
    {
        this.index = _index;
        //skill = GameMain.getInstance().m_SkillMgr.GetSkill(this.index);
        if (skill != null)
        {
            skillCastType = skill.m_CastType;
        }
    }

    /// <summary>
    /// 释放技能
    /// </summary>
    public virtual void Release()
    {
        /*
        GameMain gameMain = GameMain.getInstance();
        if (gameMain != null)
        {
            gameMain.m_SkillMgr.UseSkill(index, skillReleaseDot.position);
        }
        */
    }

    void ShowAttackAblePlayer()
    {
        if (skill == null) return;
        /*
        List<stSceneHarmerInfo> _list = skill.CheckAttackObject();
        foreach (var player in GameMain.getInstance().m_PlayerMgr.m_otherPlayerGroup)
        {
            ulong _id = player.Value.m_sceneObjId;
            bool _isFind = false;
            for (int i = 0; i < _list.Count; ++i)
            {
                if (_id == _list[i].qwObjId)
                {
                    _isFind = true;
                    break;
                }
            }

            if (_isFind)
            {
                player.Value.objInstantiate.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
            else
            {
                player.Value.objInstantiate.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        */
    }

    void HideAttackAblePlayer() {
        /*
        GameMain gameMain = GameMain.getInstance();
        if (gameMain == null)
        {
            return;
        }
        foreach (var player in gameMain.m_PlayerMgr.m_otherPlayerGroup)
        {
            player.Value.objInstantiate.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        */
    }
}
