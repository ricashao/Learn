using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAniCtrl : MonoBehaviour ,IPointerClickHandler{

		public Animation animation;
		public AnimationClip animationClip;
		public float[] stopTimeArr = new float[] { 1f,1.5f,2f,2.5f};
		public float curTime = 0;
		public int curStopTimeIndex;
		public int curTagetTimeIndex = 1;
		public int initTagetTimeIndex = 0;
		public int arrIndex;

		public CardAniCtrl [] cardAniCtrlArr;

		public Queue stepQueue = new Queue ();
		// Use this for initialization
		void Start () {

			if (animation == null) {
				animation = GetComponent<Animation> ();
			}

			animationClip = animation.clip;

			for(int i = 0;i < stopTimeArr.Length;i++){

				AnimationCtrl.AddEventFloat(animationClip,stopTimeArr[i],"OnPlayTime",stopTimeArr[i]);
			}

			cardAniCtrlArr = transform.parent.GetComponentsInChildren<CardAniCtrl> ();

			for (int i = 0; i < cardAniCtrlArr.Length; i++) {
				if(cardAniCtrlArr[i].Equals(this)){
					this.arrIndex = i;
				}
			}

			/*
		AnimationCtrl.AddEvent (animationClip,1f,"OnAnimationEventFloat",1f);
		AnimationCtrl.AddEvent (animationClip,1.5f,"OnAnimationEventFloat",1.5f);
		AnimationCtrl.AddEvent (animationClip,2f,"OnAnimationEventFloat",2f);
		AnimationCtrl.AddEvent (animationClip,2.5f,"OnAnimationEventFloat",2.5f);*/
		}

		public void Play(){
			animation [animationClip.name].speed = 1;
			animation.Play ();
		}

		public void NextStep(){
			if (curTagetTimeIndex == cardAniCtrlArr.Length - 1) {
				curTagetTimeIndex = 1;
			} else {
				curTagetTimeIndex++;
			}
		}

		public void EndStep(){
			curTagetTimeIndex = cardAniCtrlArr.Length - 1;
		}

		public void RunStepQueue(){
			if (stepQueue.Count > 0) {
				curTagetTimeIndex = (int)stepQueue.Dequeue ();
				/*
			if (curTagetTimeIndex >= stopTimeArr.Length) {
				curTagetTimeIndex = 1;
			}*/
			}
		}

		public void SaveStepQueue(int step){

			stepQueue.Clear ();

			int cur = curTagetTimeIndex;

			for (int i = 0; i< step && i < stopTimeArr.Length; i++) {
				if (cur + 1 == stopTimeArr.Length) {
					cur = 0;
				} 
				cur++;
				stepQueue.Enqueue (cur);
			}
		}

		public void OnPointerClick (PointerEventData eventData){

			Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);

			OnPointerUp (eventData);
		}

		public void OnPointerUp (PointerEventData eventData){

			Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
			/*
		if (curStopTimeIndex == 5) {
			curTagetTimeIndex = 1;
		} else {
			curTagetTimeIndex = 5;
		}*/

			int curTagetTimeIndexArr = curTagetTimeIndex;

			//EndStep ();

			for(int i = 0 ;i<cardAniCtrlArr.Length;i++ ){

				//cardAniCtrlArr [i].SaveStepQueue(( stopTimeArr.Length - curTagetTimeIndex));

				cardAniCtrlArr [i].SaveStepQueue (1);

				cardAniCtrlArr [i].RunStepQueue ();
				//cardAniCtrlArr [i].RunStepQueue (( stopTimeArr.Length - 1 - curTagetTimeIndex));

			/*(
			if (cardAniCtrlArr [i].Equals (this)) {
				//curTagetTimeIndex = cardAniCtrlArr.Length - 1;
			} else {
				NextStep ();
			}
			*/
				cardAniCtrlArr [i].Play ();
			}

			//Paly ();
		}

		void OnPlayTime (float param) {

			Debug.LogFormat ("OnPlayTime top {0}",param);

			//animation ["MarqueeAnimation"].speed = 0;
			animation [animationClip.name].speed = 0;
			curTime = param;

			for (int i = 0; i < stopTimeArr.Length; i++) {
				if (stopTimeArr [i] == curTime) {
					curStopTimeIndex = i;
				}
			}


			/*
		if (stepQueue.Count > 0) {
			curTagetTimeIndex = (int)stepQueue.Dequeue ();
		}*/

			RunStepQueue ();

			if (curStopTimeIndex == 5) {
				//curTagetTimeIndex = 0;
			}

			if (curTagetTimeIndex != curStopTimeIndex) {
				Play ();
			}

		}

	}
