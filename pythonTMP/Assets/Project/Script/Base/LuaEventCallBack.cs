using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using XLua;
/*
function function_click( eventdata )
	-- body
	print( 'function_click = '..eventdata.position.x);
end
*/
[CSharpCallLua]
public delegate void OnEvent(BaseEventData arg0);

namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public class LuaEventCallBack :LuaBaseBehaviour {

		OnEvent callback;

		public EventTriggerType curEventTriggerType;
		public string callbackFunName;

		public override void Init()
		{
			base.Init ();

			if (!string.IsNullOrEmpty (callbackFunName)) {

				scriptEnv.Get(callbackFunName, out callback);
				if (callback != null) {
					AddEvent (curEventTriggerType, callback);
				}
			}
		}

		public void AddEvent(EventTriggerType eventTriggerType,OnEvent callback){

			this.callback = callback;

			Add (eventTriggerType, OnEvent);
		}

		void OnEvent(BaseEventData arg0){
			//PointerEventData eventData = arg0 as PointerEventData;
			//eventData.position.x;
			callback (arg0);
		}

		// Use this for initialization
		void Add(EventTriggerType eventTriggerType,UnityAction<BaseEventData> callback){

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
	}

}