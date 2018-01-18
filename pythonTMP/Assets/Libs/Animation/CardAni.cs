using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAni : MonoBehaviour {

	public CardClick[] angleCrtlArr;

	// Use this for initialization
	void Start () {
		
		float x = -1.5f;
		float y = -6.79f;
		float z = 9f;

		float angleInit = -90f;

		float angleStep = (90f - 46f) / angleCrtlArr.Length;

		CardClick angleCrtl;

		for(int i = 0;i< angleCrtlArr.Length;i++){
			
			angleCrtl = angleCrtlArr[i];
			angleCrtl.arrIndex = i;
			angleCrtl.maxIndex = angleCrtlArr.Length - 1;

			if (i == 0)
				angleCrtl.prev = angleCrtlArr[angleCrtlArr.Length - 1];
			else
				angleCrtl.prev = angleCrtlArr[i - 1];
			
			if (i == angleCrtlArr.Length-1 )
				angleCrtl.next = angleCrtlArr[0];
			else
				angleCrtl.next = angleCrtlArr[i + 1];
			
			/*
			angleCrtl.gameObject.transform.position = new Vector3 (angleCrtl.gameObject.transform.position.x,
																   angleCrtl.gameObject.transform.position.y,
																   angleCrtl.gameObject.transform.position.z-.1f); 
																   */

			angleCrtl.gameObject.transform.position = new Vector3(-1.5f,0.21f,9f);
			angleCrtl.basePoint = new Vector3 (x,y,z+=.001f);
			angleCrtl.targetAngle = angleInit + angleStep * i;
			angleCrtl.angleStep = angleStep;
			//angleCrtl.tagetAngle = angleCrtl.angle; 
			angleCrtl.Set ();

			angleCrtl.onCardClick += OnCardClick;
		}

	}

	public void RunByIndex(int index){

	}

	public void OnCardClick(CardClick cardClick){

		cardClick.targetIndex = 0;
		int targetIndex = 0;
		int cardClickIndex = 0;

		for(int i = 0;i < angleCrtlArr.Length; i++){
			
			if(angleCrtlArr [i] == cardClick){
				cardClickIndex = i;
			}
		}

		for (int i = cardClickIndex; i < angleCrtlArr.Length; i++) {
			angleCrtlArr [i].targetIndex = targetIndex++;
		}

		for (int i = 0; i < cardClickIndex; i++) {
			angleCrtlArr [i].targetIndex = targetIndex++;
		}

		/*
		cardClick.index = 0;
		int curTargetIndex = 0;
		CardClick curCardClick = cardClick.prev;

		while(curCardClick != cardClick){
			//i--;
			//z = 9 +.001f*i ;
			curCardClick.targetIndex = ++curTargetIndex;

			curCardClick = curCardClick.prev;
		}
		*/

		//cardClick.tagetAngle = -90f;
		CardClick curCardClick = cardClick;
		//curCardClick = cardClick;

		float tagetAngleDis =  Mathf.Abs(-90f) - Mathf.Abs(cardClick.angle);

		int index = 0;

		if (curCardClick.angle <= -90f) {
			tagetAngleDis = curCardClick.angleStep;
			curCardClick.targetAngle = curCardClick.angle - tagetAngleDis;

		} else {
			curCardClick.targetAngle = -90f;
		}

		for (int i = 0; i < angleCrtlArr.Length; i++) {

			curCardClick = angleCrtlArr [i];

			if(curCardClick == cardClick){
				index = i;
			}else{
				curCardClick.targetAngle = curCardClick.angle - tagetAngleDis;
			}
		}

		if (curCardClick.angle <= -90f) {
			index++;
		}

		if(index == angleCrtlArr.Length){
			index = 0;
		}

		float z = 9f;

		int curIndex = 0;

		/*
		for (int i = index; i < angleCrtlArr.Length; i++) {

			curCardClick = angleCrtlArr [i];
			curCardClick.basePoint = new Vector3 (curCardClick.basePoint.x,curCardClick.basePoint.y,z+=.001f );

			curIndex++;
		}
		for (int i = 0 ; i < index; i++) {

			curCardClick = angleCrtlArr [i];
			curCardClick.basePoint = new Vector3 (curCardClick.basePoint.x,curCardClick.basePoint.y,z+=.001f );

			curIndex++;
		}
		*/
	}

	// Update is called once per frame
	void Update () {
		
	}
}
