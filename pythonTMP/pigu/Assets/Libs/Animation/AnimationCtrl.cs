using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCtrl : MonoBehaviour {

	public Animation animation;
	public AnimationClip animationClip;
	// Use this for initialization
	void Start () {

		if (animation == null) {
			animation = GetComponent<Animation> ();
		}

		animationClip = animation.clip;
		/*
		AnimationEvent animationEvent = new AnimationEvent ();

		animationEvent.functionName = "OnAnimationEventFloat";
		animationEvent.floatParameter = 10f;
		//animationEvent.objectReferenceParameter = animationClip;
		animationEvent.time = 1f;

		//animationClip.AddEvent (animationEvent);

		//animationClip.apparentSpeed
		//平均
		//animationClip.averageAngularSpeed;
		//animationClip.averageSpeed;

		AddEventObject (animationClip,1f,"OnAnimationEventObject",animation);
		//AddEvent (animationClip,2f,"OnAnimationEventObject1",animation);
		*/
	}

	public static void AddEventFloat(AnimationClip animationClip,float time, string functionName,float param){
		AnimationEvent animationEvent = new AnimationEvent ();

		animationEvent.functionName = functionName;
		animationEvent.floatParameter = param;
		animationEvent.time = time;

		animationClip.AddEvent (animationEvent);
	}

	public static void AddEventObject(AnimationClip animationClip,float time, string functionName,Object param){
		
		AnimationEvent animationEvent = new AnimationEvent ();

		animationEvent.functionName = functionName;
		animationEvent.objectReferenceParameter = param;
		animationEvent.time = time;

		animationClip.AddEvent (animationEvent);
	}

	// Use this for initialization
	void OnMarqueeAnimationEnd () {
		Debug.LogError ("OnMarqueeAnimationEnd");
	}

	void OnAnimationEventFloat (float param) {
		Debug.LogErrorFormat ("OnAnimationEventFloat {0}",param);
	}

	void OnAnimationEventInt (float param) {
		Debug.LogErrorFormat ("OnAnimationEventInt {0}",param);
	}

	void OnAnimationEventString (string param) {
		Debug.LogErrorFormat ("OnAnimationEventString {0}",param);
	}

	void OnAnimationEventObject (Object param) {
		Debug.LogErrorFormat ("OnAnimationEventObject {0}",param);

		Animation animation = param as Animation;
		
		//animation.Stop ();
		//animation = false;

		animation ["MarqueeAnimation"].speed = 0;
	}

	public void AnimationTop(){
		//animation.enabled = false;

		animation ["MarqueeAnimation"].speed = 0;
	}

	public void AnimationRun(){
		//animation.Play ();

		//animation.enabled = true;

		animation ["MarqueeAnimation"].speed = 1;
	}
}
