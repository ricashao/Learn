using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public delegate void MoveListen();

public class PlayerDirController : MonoBehaviour {

    public string playerTag = "Player";

    public bool enabledMove = true;
    public Vector2 initPot;// new Vector2(115f, 115f);
    public Transform target;
    /// <summary>
    /// y轴旋转偏移量
    /// </summary>
    public float targetEulerAnglesY = 45f;
    public float distanceMax = 68f; 
    public CharacterController ccr;
    public RectTransform thumb;
    public Animation ani;
    public Vector3 move = new Vector3();
    public float speed = 1f;
    public float angle;
    public float distance;

    public string defRunAniName = "";
    public string defIdleAniName = "";

    public MoveListen m_OnMoveIng;
    public MoveListen m_OnMoveEnd;

    IRoleMoveAnimationPlayer m_AniPlayer;
   
    public static bool isOnLine = false;

    public bool dragEventSelf = false;

    void Start()
    {
        thumb = (RectTransform) transform.GetChild(0);
       
        initPot = (transform as RectTransform).anchoredPosition;
        
        move.y = -.2f;

        if(dragEventSelf){
            Add(EventTriggerType.BeginDrag, OnBeginDrag);
            Add(EventTriggerType.Drag, OnDrag);
            Add(EventTriggerType.EndDrag, OnEndDrag);
        }
    }

    public void ReSetPosition() {

        (transform as RectTransform).anchoredPosition = initPot;
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        //PointerEventData pointerEventData = eventData as PointerEventData;
        //thumb.position = pointerEventData.position;

        thumb.position = Vector3.zero;

        //((RectTransform)thumb).anchoredPosition = pointerEventData.position;
        //OnDrag(eventData);
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        if (!enabledMove) return;
        //PointerEventData pointerEventData = eventData as PointerEventData;

        OnEndDrag(eventData);
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        if (!enabledMove) return;

        if (ccr){
            ccr.enabled = true;
        }

        PointerEventData pointerEventData = eventData as PointerEventData;

        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform,
            pointerEventData.position,
            pointerEventData.enterEventCamera,
            out pos))
        {
            thumb.anchoredPosition = pos;
        }
    }

    Vector3 curThumbPosition;

    public void OnDrag(BaseEventData eventData)
    {
        if (!enabledMove) return;
        PointerEventData pointerEventData = eventData as PointerEventData;
        //thumb.position = pointerEventData.position;

        //RectTransformUtility.WorldToScreenPoint(pointerEventData.enterEventCamera, pointerEventData.position);
        //把屏幕坐标转换成当前节点坐标
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform,
            pointerEventData.position,
            pointerEventData.enterEventCamera,
            out pos))
        {
            thumb.anchoredPosition = pos;
        }
        
        /*
        angle = Mathf.Acos(Vector3.Dot(transform.right.normalized, thumb.localPosition.normalized)) * Mathf.Rad2Deg;
        Vector3 cross = Vector3.Cross(transform.right.normalized, thumb.localPosition.normalized);
        angle = cross.z < 0 ? -angle : angle;

        distance = Vector3.Distance(Vector3.zero, thumb.localPosition);
        //float angle = Mathf.Atan(  thumb.localPosition.y / thumb.localPosition.x ) * Mathf.Rad2Deg;
        if (target)
        {
            //相机和人物正方向的夹角 45f
            target.eulerAngles = new Vector3(0, -angle + targetEulerAnglesY, 0);
            //target.eulerAngles = new Vector3(0, -angle , 0);
        }
        */

        //angle = Mathf.Atan((transform.position.y - thumb.position.y) / (transform.position.x - thumb.position.x)) * Mathf.Rad2Deg;
        angle = Mathf.Atan((-thumb.anchoredPosition.y) / (-thumb.anchoredPosition.x)) * Mathf.Rad2Deg;
        //Debug.LogWarningFormat(" angle = {0} ", angle);
        
        if (thumb.anchoredPosition.x < 0 && thumb.anchoredPosition.y > 0 && angle < 0)
            angle =  180f + angle;
        if (thumb.anchoredPosition.x < 0 && thumb.anchoredPosition.y < 0 && angle > 0)
            angle = -90f - (90f - angle);
       
        distance = Vector3.Distance(Vector3.zero , thumb.anchoredPosition); 
        //float distanceMax = 100;
        /*
        if (distance > distanceMax)
        {
            if (pointerEventData.position.x < transform.position.x)
            {
                curThumbPosition.x = transform.position.x - distanceMax * Mathf.Cos((angle) * Mathf.Deg2Rad);
                curThumbPosition.y = transform.position.y - distanceMax * Mathf.Sin((angle) * Mathf.Deg2Rad);
                thumb.position = curThumbPosition;
            }
            else
            {
                curThumbPosition.x = transform.position.x + distanceMax * Mathf.Cos((angle) * Mathf.Deg2Rad);
                curThumbPosition.y = transform.position.y + distanceMax * Mathf.Sin((angle) * Mathf.Deg2Rad);
                thumb.position = curThumbPosition;
            }
        }
        */
        if (target)
        {
            //相机和人物正方向的夹角 45f
            target.eulerAngles = new Vector3(0, -angle + targetEulerAnglesY, 0);
            //target.eulerAngles = new Vector3(0, -angle , 0);
        }

        PlayRun();

        //Debug.LogWarning("angle = " + angle + ",cross = " + cross + ",distance = " + distance + " , move = " + move);
    }

    public void OnEndDrag(BaseEventData eventData)
    {
        //PointerEventData pointerEventData = eventData as PointerEventData;
        
        thumb.localPosition = Vector3.zero;

        distance = 0;

        PlayIdle();
    }

    public void Add(EventTriggerType type, UnityAction<BaseEventData> callbackp)
    {
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(callbackp);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);
        if (GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }
        GetComponent<EventTrigger>().triggers.Add(entry);
    }

    void PlayRun()
    {
        if (m_AniPlayer != null)
        {
            m_AniPlayer.PlayRun(speed, speed);
        }

        if (m_OnMoveIng != null && enabledMove)  
        {
            m_OnMoveIng();
        }
    }

    void PlayIdle()
    {
        if (m_AniPlayer != null)
        {
            m_AniPlayer.PlayIdle();
        }

        if (m_OnMoveEnd != null && enabledMove)
        {
            m_OnMoveEnd();
        } 
    }

    public void initPlayerByTag() {
        
        GameObject gameObject = GameObject.FindGameObjectWithTag(playerTag);
        if (gameObject) {
            target = gameObject.transform;
            ccr = gameObject.GetComponent<CharacterController>();
            ani = gameObject.GetComponent<Animation>();
           
            MonoBehaviour[] monoBehaviour = gameObject.GetComponentsInChildren<MonoBehaviour>();
            for ( int i = 0;i< monoBehaviour.Length;i++) {
                if (monoBehaviour[i] is IRoleMoveAnimationPlayer) {
                    m_AniPlayer = monoBehaviour[i] as IRoleMoveAnimationPlayer;
                }
            }
            if(m_AniPlayer == null)
                m_AniPlayer = gameObject.AddComponent<DefaultMoveAnimationPlayer>();
        }
    }

    void Update()
    {
        if (target == null) {
            initPlayerByTag(); 
        }
		/*
        if (GameMain.getInstance().m_SelfPlayer.IsCanControl)
        {
            //玩家处于僵直状态,禁止移动
            return;
        }
		*/
        if (ccr && distance > 0)
        {
            if (isOnLine)
            {
                //speed = (float)DataManager.getInstance().m_userData.GetAttrByKey(AttributeEnum.MoveSpeed) / 1000;
            }
           // move.x = Mathf.Cos((targetEulerAnglesY + angle) * Mathf.Deg2Rad) * speed * Time.deltaTime;
           // move.z = Mathf.Sin((targetEulerAnglesY + angle) * Mathf.Deg2Rad) * speed * Time.deltaTime;

            move = target.transform.forward * speed * Time.deltaTime;

        }
        else {
            move.x = move.z = 0;
        }

        if (ccr){
            move.y = -.2f;
            ccr.Move(move);
        }
      
    }
}
