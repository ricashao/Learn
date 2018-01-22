using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//public delegate void MoveListen();
/// <summary>
/// 技能释放方向控制
/// </summary>
public class SkillReleaseDirController : MonoBehaviour {

    public string tag = "Player";

    public bool enabledMove = true;

    public Transform target;
    public CharacterController ccr;
    /*
        遥感  Transform ，此节点不接受事件 
    */
    public RectTransform thumb;
    public Animation ani;
    public Vector3 move = new Vector3();
    public float speed = 1f;
    public float angle;
    public float distance;
    public float distanceMax = 68;

    public MoveListen m_OnMoveIng;
    public MoveListen m_OnMoveEnd;
    //PlayerAnimation m_PlayerAni;
    IRoleMoveAnimationPlayer m_AniPlayer;

    public static bool isOnLine = false;

    RectTransform rectTransform;

    void Start()
    {
        //thumb = transform.GetChild(0);

        rectTransform = GetComponent<RectTransform>();

        move.y = -.2f;

        Add(EventTriggerType.BeginDrag, OnBeginDrag);
        Add(EventTriggerType.Drag, OnDrag);
        Add(EventTriggerType.EndDrag, OnEndDrag);
    }

    public void Show() {
        GetComponent<Image>().enabled = true;
        thumb.GetComponent<Image>().enabled = true;
    }

    public void Hide()
    {
        GetComponent<Image>().enabled = false;
        thumb.GetComponent<Image>().enabled = false;
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        thumb.position = pointerEventData.position;
        //OnDrag(eventData);
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        if (!enabledMove) return;
        PointerEventData pointerEventData = eventData as PointerEventData;

        OnEndDrag(eventData);
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        if (!enabledMove) return;
        PointerEventData pointerEventData = eventData as PointerEventData;

        OnDrag(eventData);
    }

    Vector3 oldThumbPosition;
    Vector3 curThumbPosition;

    public void OnDrag(BaseEventData eventData)
    {
        if (!enabledMove) return;

        oldThumbPosition = thumb.position;

        PointerEventData pointerEventData = eventData as PointerEventData;

        curThumbPosition = pointerEventData.position;

        curThumbPosition = Input.mousePosition;

        //把屏幕坐标转换成当前节点坐标
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform,
            pointerEventData.position,
            pointerEventData.enterEventCamera,
            out pos))
        {
            curThumbPosition = pos;
        }

        thumb.anchoredPosition = curThumbPosition;
        
        //Debug.LogWarningFormat(" curThumbPosition {0}", thumb.anchoredPosition);
        //遥杆与摇杆背景处于同级
        //计算角度
        angle = Mathf.Atan((rectTransform.anchoredPosition.y - thumb.anchoredPosition.y) / (rectTransform.anchoredPosition.x - thumb.anchoredPosition.x)) * Mathf.Rad2Deg;
        //计算距离
        distance = Vector3.Distance(rectTransform.anchoredPosition, thumb.anchoredPosition);


        if (distance > distanceMax)
        {
            if (thumb.anchoredPosition.x < rectTransform.anchoredPosition.x)
            {
                curThumbPosition.x = rectTransform.anchoredPosition.x - distanceMax * Mathf.Cos((angle) * Mathf.Deg2Rad);
                curThumbPosition.y = rectTransform.anchoredPosition.y - distanceMax * Mathf.Sin((angle) * Mathf.Deg2Rad);
                thumb.anchoredPosition = curThumbPosition;
            }
            else
            {
                curThumbPosition.x = rectTransform.anchoredPosition.x + distanceMax * Mathf.Cos((angle) * Mathf.Deg2Rad);
                curThumbPosition.y = rectTransform.anchoredPosition.y + distanceMax * Mathf.Sin((angle) * Mathf.Deg2Rad);
                thumb.anchoredPosition = curThumbPosition;
            }
        }

        if (target)
        {
            //相机和人物正方向的夹角 45f
            target.eulerAngles = new Vector3(0, -angle + 45f, 0);
        }
        //Debug.LogWarning("angle = " + angle + ",cross = " + cross + ",distance = " + distance + " , move = " + move);
    }

    public void OnEndDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;

        OnDrag(eventData);

        thumb.position = transform.position;

        distance = 0;

        //PlayIdle();
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

    Vector3 curAttackPosition;
    /// <summary>
    /// 获取场景释放点
    /// </summary>
    /// <param name="attackPosition">攻击点</param>
    /// <param name="skillDistanceMax">技能最大释放距离</param>
    /// <returns></returns>
    public Vector3 GetCurReleasePosition(Vector3 attackPosition, float skillDistanceMax) {

        float attackDistance = skillDistanceMax * ( (distance > distanceMax ? distanceMax : distance) / distanceMax ); 

        if (thumb.position.x < transform.position.x)
        {
            curAttackPosition.x = attackPosition.x - attackDistance * Mathf.Cos((angle) * Mathf.Deg2Rad);
            curAttackPosition.z = attackPosition.z - attackDistance * Mathf.Sin((angle) * Mathf.Deg2Rad);
        }
        else
        {
            curAttackPosition.x = attackPosition.x + attackDistance * Mathf.Cos((angle) * Mathf.Deg2Rad);
            curAttackPosition.z = attackPosition.z + attackDistance * Mathf.Sin((angle) * Mathf.Deg2Rad);
        }

        curAttackPosition.y = attackPosition.y;

        return curAttackPosition;
    }
}
