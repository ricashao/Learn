using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonExt : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{

    public bool invokeOnce = false;
    public bool hadInvoke = false;
    public float interval = 0.1f;
    private bool isPointerDown = false;
    private float recordTime;
    public UnityEvent onPress = new UnityEvent();
    public UnityEvent onRelease = new UnityEvent();
    public void OnPointerDown(PointerEventData eventData)
    {
        recordTime = Time.time;
        isPointerDown = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hadInvoke = false;
        isPointerDown = false;
        onRelease.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        hadInvoke = false;
        isPointerDown = false;
        onRelease.Invoke();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (invokeOnce && hadInvoke) return;
        if (isPointerDown)
        {
            if ((Time.time - recordTime) > interval)
            {
                onPress.Invoke();
                hadInvoke = true;
            }
        }
    }
}
