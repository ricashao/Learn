using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void OnCardClick(CardClick cardClick);

public class CardClick : AngleCrtl ,
						IPointerClickHandler ,
						IPointerDownHandler,
						IPointerEnterHandler,
						IPointerUpHandler
{
	/// <summary>
	/// The index of the arr.
	/// 数组下标
	/// </summary>
	public int arrIndex;
	/// <summary>
	/// The index of the current.
	/// 当前显示队列下标
	/// </summary>
	public int curIndex;
	/// <summary>
	/// The index of the target.
	/// 目标显示队列下标
	/// </summary>
	public int targetIndex;
	/// <summary>
	/// The index max.
	/// 目标显示队列下标最大值
	/// </summary>
	public int maxIndex;

	public float angleRectStart;
	public float angleRectEnd;

	public int dis = 1;

	public CardClick prev;
	public CardClick next;

	public OnCardClick onCardClick;

	public void OnPointerClick (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}

	public void OnPointerDown (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}

	public void OnPointerEnter (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}

	public void OnPointerUp (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);

		onCardClick (this);
	}

	public bool IsStart(){
		return curIndex == 0;
	}

	public bool IsEnd(){
		return curIndex == maxIndex;
	}

	public int GetNextIndex(){
		return IsEnd () ? 0 : curIndex + 1;
	}
		
	public int GetPrevIndex(){
		return IsStart () ? maxIndex : curIndex - 1;
	}
		
	public bool Next(){

		curIndex = GetNextIndex ();
		return curIndex == 0;
	}

	public bool Prev(){

		curIndex = GetNextIndex ();
		return curIndex == maxIndex;
	}

	public void SetTargetAngleByCurIndex(){
		
		targetAngle = Mathf.Lerp (angleRectStart,angleRectEnd,curIndex / maxIndex);

	}

	void MoveCmp(){
		
		if(curIndex != targetAngle){
			if (dis >= 0)
				Next ();
			else 
				Prev();
		}

		SetTargetAngleByCurIndex ();
	}

	void StartToEnd(){
		
	}

	void EndToStart(){
		
	}

	// Update is called once per frame
	override protected void Update () {

		if (targetAngle != angle) {
			/*
			if (Mathf.Abs (targetAngle % 360) < -90) {

				if (Mathf.Abs (targetAngle - angle) > angleSpeed) {
					angle += (targetAngle - angle) * Time.deltaTime * angleSpeed;
				} else {
					angle = targetAngle;
				}
			} else {
*/
				if (Mathf.Abs (targetAngle - angle) > angleSpeed) {
					angle += (targetAngle - angle) * Time.deltaTime * angleSpeed;
				} else {
					angle = targetAngle;

				//MoveCmp ();

				}
			//}
			Set ();
		}

		if (IsRunToEnd()) {
			angle = -53f + angleStep * .5f;
			targetAngle = -53f;

			int i = 6;
			float z = 9 +.001f*i ;
			basePoint = new Vector3 (basePoint.x,basePoint.y,z );

			//basePoint = new Vector3 (basePoint.x,basePoint.y,z );

			CardClick curCardClick = this.prev;

			while(curCardClick != this){
				i--;
				z = 9 +.001f*i ;
				curCardClick.basePoint = new Vector3 (curCardClick.basePoint.x,curCardClick.basePoint.y,z);

				curCardClick = curCardClick.prev;
			}

		} else {

		}
	}

	public bool IsRunToEnd(){
		
		if (Mathf.Abs (angle - (-90f - angleStep * .5f) )< 0.1f) {
			return true;
		} 
		return false;
	}

	override public void Set(){
		transform.position = GetPosition_R_Z(basePoint,angle,radius);
		eulerAngles.z = angle + 90f;
		transform.eulerAngles = eulerAngles;
	}
}
