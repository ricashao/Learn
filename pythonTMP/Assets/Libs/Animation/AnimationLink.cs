using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLink : MonoBehaviour {

	public Animation animation;
	public bool isLinked = true;
	// Use this for initialization
	void Start () {

		if (animation == null) {
			animation = GetComponent<Animation> ();
		}

	}

	void OnAnimationEventPaly(){

		if(isLinked)
			animation.Play ();
	}
	/*
	void OnAnimationEventStop (string param) {
		//Debug.LogErrorFormat ("OnAnimationEventString {0}",param);

		animation [param].speed = 0;
	}
	void OnAnimationEventRun (string param) {
		//Debug.LogErrorFormat ("OnAnimationEventString {0}",param);

		animation [param].speed = 1;
	}
	*/
}
