using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 方向控制基础
/// |-Thumb 遥感中心
/// |-Joystick-|
///            |-BasePoint 背景
/// 
/// 1. Joystick 接受拖拽事件
/// 2. Thumb BasePoint 不接受事件
/// 3. Joystick Thumb 同一为中心点布局
/// </summary>
public class DirectionController : MonoBehaviour {

    public bool enabledMove = true;

    protected RectTransform basePoint;
    public RectTransform thumb;

    public float angle;
    public float distance;
    public float distanceMax = 68;

    public MoveListen m_OnMoveIng;
    public MoveListen m_OnMoveEnd;

    void Start()
    {
        if(basePoint == null)
        basePoint = GetComponent<RectTransform>();

        //Add(EventTriggerType.Move, OnBeginDrag);

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

    virtual public void OnDrag(BaseEventData eventData)
    {
        if (!enabledMove) return;

        oldThumbPosition = thumb.position;

        PointerEventData pointerEventData = eventData as PointerEventData;

		if ( pointerEventData.position.x < 0 || pointerEventData.position.y < 0
			||pointerEventData.position.x > Screen.width || pointerEventData.position.y > Screen.height) {
			return;
		}
        //curThumbPosition = pointerEventData.position;
        //curThumbPosition = Input.mousePosition;
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
        //遥杆与摇杆背景处于同级
        //计算角度
        angle = Mathf.Atan((basePoint.anchoredPosition.y - thumb.anchoredPosition.y) / (basePoint.anchoredPosition.x - thumb.anchoredPosition.x)) * Mathf.Rad2Deg;
        //计算距离
        distance = Vector3.Distance(basePoint.anchoredPosition, thumb.anchoredPosition);
        //限制最大距离
        if (distance > distanceMax)
        {
            if (thumb.anchoredPosition.x > basePoint.anchoredPosition.x)
            {
                curThumbPosition.x = basePoint.anchoredPosition.x + distanceMax * Mathf.Cos((angle) * Mathf.Deg2Rad);
                curThumbPosition.y = basePoint.anchoredPosition.y + distanceMax * Mathf.Sin((angle) * Mathf.Deg2Rad);   
            }
            else
            {
                curThumbPosition.x = basePoint.anchoredPosition.x - distanceMax * Mathf.Cos((angle) * Mathf.Deg2Rad);
                curThumbPosition.y = basePoint.anchoredPosition.y - distanceMax * Mathf.Sin((angle) * Mathf.Deg2Rad);
            }

            thumb.anchoredPosition = curThumbPosition;
        }

        /*
        if (target)
        {
            //相机和人物正方向的夹角 45f
            target.eulerAngles = new Vector3(0, -angle + 45f, 0);
        }
        */
        //Debug.LogWarning("angle = " + angle + ",cross = " + cross + ",distance = " + distance + " , move = " + move);
    }

    virtual public void OnEndDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;

        OnDrag(eventData);

        thumb.position = transform.position;
        distance = 0;
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

    //Vector3 curAttackPosition;
    /// <summary>
    /// 获取场景释放点
    /// </summary>
    /// <param name="attackPosition">攻击点</param>
    /// <param name="skillDistanceMax">技能最大释放距离</param>
    /// <returns></returns>
    /*
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
    */
}
