using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 控制摇杆在左侧自由控制
/// </summary>
public class LeftFreeCtrl :MonoBehaviour 
{    
    PlayerDirController joystick;
    RectTransform joystickRectTransform;

    private void Start()
    {
        //joystick = GameObject.FindObjectOfType<ETCJoystick>();
        joystick = GameObject.FindObjectOfType<PlayerDirController>();

        joystickRectTransform = joystick.GetComponent<RectTransform>();
        ReSetPointer();

        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(OnPointerDown);
        EventTrigger.Entry         entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener(callback);

        Image image= GetComponent<Image>();
        GetComponent<EventTrigger>().triggers.Add(entry);

        Add(EventTriggerType.PointerDown, OnPointerDown);
        Add(EventTriggerType.PointerUp, OnPointerUp);
        //Add(EventTriggerType.PointerEnter, OnPointerEnter);
        Add(EventTriggerType.Drag, OnDrag);
        Add(EventTriggerType.BeginDrag, OnBeginDrag);
    }

    public void Add(EventTriggerType type, UnityAction<BaseEventData> callbackp) {
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(callbackp);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener( callback);
        GetComponent<EventTrigger>().triggers.Add(entry);
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        Show();
 
        PointerEventData pointerEventData = eventData   as  PointerEventData;
   
        Vector2 pos ;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, 
            pointerEventData.position,
            pointerEventData.enterEventCamera, 
            out pos) )
        {
            joystickRectTransform.anchoredPosition = pos;
        }
 
		//joystickRectTransform.anchoredPosition  =  pointerEventData.position;
        joystick.OnBeginDrag(pointerEventData);
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        //Debug.LogWarning("LeftFreeCtrl.OnPointerUp" + eventData);
        //throw new NotImplementedException();
        joystick.OnPointerUp(pointerEventData );
        //joystick.visible = false;
        joystick.ReSetPosition();
    }

    public void OnPointerEnter(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        //throw new NotImplementedException();
        //joystick.OnPointerEnter(pointerEventData);
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        //throw new NotImplementedException();
        joystick.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        //throw new NotImplementedException();
        joystick.OnBeginDrag(pointerEventData);
    }

    public void Show() {
        //joystick.visible = true;
    }

    public void ReSetPointer() {

        //joystickRectTransform.position = initPot;
        //joystickRectTransform.localPosition = initPot;
        //joystickRectTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(115f,115f);
    }
    /*
    public void OnPointerDown(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        //throw new NotImplementedException();
        joystick.OnPointerDown(pointerEventData);
    }*/
}
