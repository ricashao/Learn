using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;
using System;

namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public class LuaUGUIEvent :LuaBaseBehaviour, IBeginDragHandler ,ICancelHandler ,IDeselectHandler,IDragHandler,IDropHandler,
	IEndDragHandler,IInitializePotentialDragHandler,IMoveHandler,IPointerClickHandler,IPointerDownHandler,IPointerEnterHandler,
	IPointerExitHandler,IPointerUpHandler,IScrollHandler,ISelectHandler,ISubmitHandler,IUpdateSelectedHandler
	{
		private	Action luaOnBeginDrag;
		private	Action luaOnCancel;
		private	Action luaOnDeselect;
		private	Action luaOnDrag;
		private	Action luaOnDrop;
		private	Action luaOnEndDrag;
		private	Action luaOnInitializePotentialDrag;
		private	Action luaOnMove;
		private	Action luaOnPointerClick;
		private	Action luaOnPointerDown;
		private	Action luaOnPointerEnter;
		private	Action luaOnPointerExit;
		private	Action luaOnPointerUp;
		private	Action luaOnScroll;
		private	Action luaOnSelect;
		private	Action luaOnSubmit;
		private	Action luaOnUpdateSelected;

		public BaseEventData curEventData;

		public override void Init()
		{
			base.Init ();

			scriptEnv.Get("OnBeginDrag", out luaOnBeginDrag);
			scriptEnv.Get("OnCancel", out luaOnCancel);
			scriptEnv.Get("OnDeselect", out luaOnDeselect);
			scriptEnv.Get("OnDrag", out luaOnDrag);
			scriptEnv.Get("OnDrop", out luaOnDrop);
			scriptEnv.Get("OnEndDrag", out luaOnEndDrag);
			scriptEnv.Get("OnInitializePotentialDrag", out luaOnInitializePotentialDrag);
			scriptEnv.Get("OnMove", out luaOnMove);
			scriptEnv.Get("OnPointerClick", out luaOnPointerClick);
			scriptEnv.Get("OnPointerDown", out luaOnPointerDown);
			scriptEnv.Get("OnPointerEnter", out luaOnPointerEnter);
			scriptEnv.Get("OnPointerExit", out luaOnPointerExit);
			scriptEnv.Get("OnPointerUp", out luaOnPointerUp);
			scriptEnv.Get("OnScrolll", out luaOnScroll);
			scriptEnv.Get("OnSelect", out luaOnSelect);
			scriptEnv.Get("OnSubmit", out luaOnSubmit);
			scriptEnv.Get("OnUpdateSelected", out luaOnUpdateSelected);
		}
			
		public void OnBeginDrag (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnBeginDrag != null) {
				luaOnBeginDrag ();
			} else {
				Debug.LogWarningFormat ("OnBeginDrag but not find lua OnBeginDrag fun !");
			}
		}

		public void OnCancel (BaseEventData eventData){
			curEventData = eventData;

			if (luaOnCancel != null) {
				luaOnCancel ();
			} else {
				Debug.LogWarningFormat ("OnCancel but not find lua OnCancel fun !");
			}
		}

		public void OnDeselect (BaseEventData eventData){
			curEventData = eventData;

			if (luaOnDeselect != null) {
				luaOnDeselect ();
			} else {
				Debug.LogWarningFormat ("luaOnDeselect but not find lua luaOnDeselect fun !");
			}
		}

		public void OnDrag (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnDrag != null) {
				luaOnDrag ();
			} else {
				Debug.LogWarningFormat ("OnDrag but not find lua OnDrag fun !");
			}
		}

		public void OnDrop (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnDrop != null) {
				luaOnDrop ();
			} else {
				Debug.LogWarningFormat ("OnDrop but not find lua OnDrop fun !");
			}
		}

		public void OnEndDrag (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnEndDrag != null) {
				luaOnEndDrag ();
			} else {
				Debug.LogWarningFormat ("OnEndDrag but not find lua OnEndDrag fun !");
			}
		}

		public void OnInitializePotentialDrag (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnInitializePotentialDrag != null) {
				luaOnInitializePotentialDrag ();
			} else {
				Debug.LogWarningFormat ("OnInitializePotentialDrag but not find lua OnInitializePotentialDrag fun !");
			}
		}

		public void OnMove (AxisEventData eventData){
			curEventData = eventData;

			if (luaOnMove != null) {
				luaOnMove ();
			} else {
				Debug.LogWarningFormat ("OnMove but not find lua OnMove fun !");
			}
		}

		public void OnPointerClick (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnPointerClick != null) {
				luaOnPointerClick ();
			} else {
				Debug.LogWarningFormat ("OnPointerClick but not find lua OnPointerClick fun !");
			}
		}

		public void OnPointerDown (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnPointerDown != null) {
				luaOnPointerDown ();
			} else {
				Debug.LogWarningFormat ("OnPointerDown but not find lua OnPointerDown fun !");
			}
		}

		public void OnPointerEnter (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnPointerEnter != null) {
				luaOnPointerEnter ();
			} else {
				Debug.LogWarningFormat ("OnPointerEnter but not find lua OnPointerEnter fun !");
			}
		}

		public void OnPointerExit (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnPointerExit != null) {
				luaOnPointerExit ();
			} else {
				Debug.LogWarningFormat ("OnPointerExit but not find lua OnPointerExit fun !");
			}
		}

		public void OnPointerUp (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnPointerUp != null) {
				luaOnPointerUp ();
			} else {
				Debug.LogWarningFormat ("OnPointerUp but not find lua OnPointerUp fun !");
			}
		}

		public void OnScroll (PointerEventData eventData){
			curEventData = eventData;

			if (luaOnScroll != null) {
				luaOnScroll ();
			} else {
				Debug.LogWarningFormat ("OnScroll but not find lua OnScroll fun !");
			}

		}

		public void OnSelect (BaseEventData eventData){
			curEventData = eventData;

			if (luaOnSelect != null) {
				luaOnSelect ();
			} else {
				Debug.LogWarningFormat ("OnSelect but not find lua OnSelect fun !");
			}

		}

		public void OnSubmit (BaseEventData eventData){
			curEventData = eventData;

			if (luaOnSubmit != null) {
				luaOnSubmit ();
			} else {
				Debug.LogWarningFormat ("OnSubmit but not find lua OnSubmit fun !");
			}
		}

		public void OnUpdateSelected (BaseEventData eventData){
			curEventData = eventData;

			if (luaOnUpdateSelected != null) {
				luaOnUpdateSelected ();
			} else {
				Debug.LogWarningFormat ("OnUpdateSelected but not find lua OnUpdateSelected fun !");
			}

		}
	}
}