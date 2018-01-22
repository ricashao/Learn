using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;
using System;
/*
 function OnPointerClick()

	print('',this.eventData.pointerPressRaycast.gameObject.name)

 end
*/
namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public class LuaSceneClick : LuaBaseBehaviour ,IPointerClickHandler{

		public PointerEventData curEventData;
		private	Action luaOnPointerClick;

		public override void Init()
		{
			base.Init ();

			scriptEnv.Get("OnPointerClick", out luaOnPointerClick);
		}
		
		public void OnPointerClick (PointerEventData eventData){

			curEventData = eventData;
			//Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);

			if (luaOnPointerClick != null) {
				luaOnPointerClick ();
			} else {
				Debug.LogWarningFormat ("OnPointerClick but not find lua OnPointerClick fun !");
			}
		}
	}

}