using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillUndo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Add(EventTriggerType.Drop, OnDrop);
    }

    public void OnDrop(BaseEventData eventData) {
        PointerEventData pointerEventData = eventData as PointerEventData;
        //Debug.LogWarning("SkillUndo.OnDrop" + pointerEventData.position);

        SkillButtonCfg.isUndo = true; 
    }

    public void Add(EventTriggerType type, UnityAction<BaseEventData> callbackp)
    {
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(callbackp);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);
        GetComponent<EventTrigger>().triggers.Add(entry);
    }
}
