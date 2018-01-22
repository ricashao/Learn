using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneObjectDrag :  MonoBehaviour ,
								IInitializePotentialDragHandler,
								IBeginDragHandler,
								IDragHandler,
								IEndDragHandler,
								IDropHandler{

	public void OnInitializePotentialDrag(PointerEventData eventData){
		//当鼠标在A对象按下还没开始拖拽时 A对象响应此事件
		Debug.LogFormat ("OnInitializePotentialDrag {0}",eventData.pointerCurrentRaycast);
		OnEevent (eventData);
	}
	public void OnBeginDrag (PointerEventData eventData){
		//当鼠标在A对象按下并开始拖拽时 A对象响应此事件
		Debug.LogFormat ("OnBeginDrag {0}",eventData.pointerCurrentRaycast);
		OnEevent (eventData);
	}

	public void OnDrag (PointerEventData eventData){
		//当鼠标抬起时 A对象响应此事件
		Debug.LogFormat ("OnDrag {0}",eventData.pointerCurrentRaycast);
		OnEevent (eventData);
	}

	public void OnEndDrag (PointerEventData eventData){
		Debug.LogFormat ("OnEndDrag {0}",eventData.pointerCurrentRaycast);
		OnEevent (eventData);
	}

	public void OnDrop (PointerEventData eventData){
		//A、B对象必须均实现IDropHandler接口，且A至少实现IDragHandler接口
		//当鼠标从A对象上开始拖拽，在B对象上抬起时 B对象响应此事件
		//此时name获取到的是B对象的name属性
		//eventData.pointerDrag表示发起拖拽的对象（GameObject）
		Debug.LogFormat ("{0} OnDrop to {1}",eventData.pointerDrag.name , name);
		OnEevent (eventData);
	}

	protected void OnEevent(PointerEventData eventData){

		//Debug.LogFormat ("OnEevent {0}",eventData.ToString());

	}
		
}
