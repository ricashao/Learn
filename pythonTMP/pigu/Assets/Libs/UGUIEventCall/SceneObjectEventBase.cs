using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
3d 场景对象点击

1.Main Camera 添加 PhysicsRaycaster

2.设置 event Mask 与 场景对象layer 相同

3.将脚本挂在 接受事件的场景对象 物件缩放不能为 0

4.接受事件的场景对象必须有 collider

5.添加EventSystem

*/
using UnityEngine.Events;

public class SceneObjectEventBase : MonoBehaviour{

	void Awake(){
		
		PhysicsRaycaster physicsRaycaster = Camera.main.GetComponent<PhysicsRaycaster> ();
		if (physicsRaycaster == null)
			Camera.main.gameObject.AddComponent <PhysicsRaycaster> ();
		//gameObject.layer
		physicsRaycaster.eventMask += gameObject.layer;

		Collider collider = GetComponent<Collider> ();
		if (collider == null) {
			Debug.LogWarningFormat ("Collider not find gameobject {0}",name);
		}
		#if UNITY_EDITOR
		AddEvent(EventTriggerType.PointerClick,OnScriptControll);
		#endif
	}

	public void AddEvent(EventTriggerType eventTriggerType,UnityAction<BaseEventData> callback){
		
		EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
		if (trigger == null)
			trigger = gameObject.AddComponent<EventTrigger>();
	
		// 实例化delegates
		trigger.triggers = new List<EventTrigger.Entry>();
		// 定义需要绑定的事件类型。并设置回调函数
		EventTrigger.Entry entry = new EventTrigger.Entry();
		// 设置 事件类型 EventTriggerType.PointerClick;
		entry.eventID = eventTriggerType;
		// 设置回调函数
		entry.callback = new EventTrigger.TriggerEvent();
		//UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(OnScriptControll);
		entry.callback.AddListener(callback);
		// 添加事件触发记录到GameObject的事件触发组件
		trigger.triggers.Add(entry);
	}

	#if UNITY_EDITOR
	void OnScriptControll(BaseEventData arg0)
	{
		PointerEventData eventData = arg0 as PointerEventData;
		Debug.Log("Test Click "+ eventData.pointerCurrentRaycast);
 	}
	#endif
}
