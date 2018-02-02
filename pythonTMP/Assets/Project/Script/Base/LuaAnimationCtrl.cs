using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
/*

function start()

	self:AddEvent("CubeAnimation",0.5,"function_event_flaot",0.5)
	self:AddEvent("CubeAnimation",0.5,"function_event",'data')

end	

function function_event_flaot( data)
	-- body
	print('function_event_flaot = '..data)

end

function function_event( data)
	-- body
	print('function_event = '..data)

end
*/
[CSharpCallLua]
delegate void LuaOnAnimationEventFloat (float param) ;
[CSharpCallLua]
delegate void LuaOnAnimationEventInt (int param) ;
[CSharpCallLua]
delegate void LuaOnAnimationEventString (string param) ;
[CSharpCallLua]
delegate void LuaOnAnimationEventUnityEngineObject (UnityEngine.Object param) ;
[CSharpCallLua]
delegate void LuaOnAnimationEventObject (object param) ;

namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public class LuaAnimationCtrl : LuaBaseBehaviour {

		LuaOnAnimationEventFloat luaOnAnimationEventFloat;
		LuaOnAnimationEventInt luaOnAnimationEventInt;
		LuaOnAnimationEventString luaOnAnimationEventString;
		LuaOnAnimationEventUnityEngineObject luaOnAnimationEventUnityEngineObject;

		//LuaOnAnimationEventObject luaOnAnimationEventObject;

		//List <AnimationEvent> animationEventList = new List<AnimationEvent> ();

		Dictionary<string ,LuaOnAnimationEventObject> funDic = new Dictionary<string, LuaOnAnimationEventObject> ();
		Dictionary<string ,object> funParamDic = new Dictionary<string, object> ();

		public Animation ani;
		public AnimationClip animationClip;

		public string initPalyName;

		public override void Init()
		{
			base.Init ();

			scriptEnv.Get("luaOnAnimationEventFloat", out luaOnAnimationEventFloat);
			scriptEnv.Get("luaOnAnimationEventInt", out luaOnAnimationEventInt);
			scriptEnv.Get("luaOnAnimationEventString", out luaOnAnimationEventString);
			scriptEnv.Get("luaOnAnimationEventUnityEngineObject", out luaOnAnimationEventUnityEngineObject);

			//scriptEnv.Get("luaOnAnimationEventObject", out luaOnAnimationEventObject);
		}
		// Use this for initialization
		protected override void Awake (){

			if (ani == null) {
				ani = GetComponent<Animation> ();
			}

			animationClip = ani.clip;

			if (!string.IsNullOrEmpty (initPalyName)) {
				Play (initPalyName);
			}

			base.Awake ();
		}

		public void Play(string animationClipName){
			ani[animationClipName].speed = 1;
			ani.Play ();
		}

		public void Stop(string animationClipName){
			ani[animationClipName].speed = 0;
		}

		public void StopToStart(string animationClipName){
			ani[animationClipName].speed = 0;
			ani[animationClipName].time = 0;
		}

		public AnimationState GetAni(string animationClipName){
			return ani [animationClipName];
		}

		/// <summary>
		/// Adds the event. 
		/// 添加 lua层回调监听
		/// </summary>
		/// <param name="animationClipName">Animation clip name.</param>
		/// <param name="time">Time.</param>
		/// <param name="functionName">Function name.</param>
		/// <param name="param">Parameter.</param>
		public void AddEvent(string animationClipName,float time, string functionName,object param){

			if (string.IsNullOrEmpty (animationClipName)) {
				Debug.LogError ("animationClipName = null !");
				return;
			}

			LuaOnAnimationEventObject luaOnAnimationEventObject;
			scriptEnv.Get(functionName, out luaOnAnimationEventObject);

			if (luaOnAnimationEventObject == null) {
				Debug.LogError ("functionName = null !");
				return;
			}

			funDic.Add (functionName,luaOnAnimationEventObject);
			funParamDic.Add (functionName, param);

			AnimationClip clip = ani [animationClipName].clip;

			AnimationEvent animationEvent = new AnimationEvent ();

			animationEvent.functionName = "OnAnimationEventObject";
			animationEvent.stringParameter = functionName;
			animationEvent.time = time;

			if(clip == null)
				animationClip.AddEvent (animationEvent);
			else
				clip.AddEvent (animationEvent);

			//animationEventList.Add (animationEvent);
		}
		/// <summary>
		/// Rems the event.
		/// 移除lua层回调监听
		/// </summary>
		/// <param name="functionName">Function name.</param>
		public void RemEvent(string functionName){
			funDic.Remove (functionName);
			funParamDic.Remove (functionName);
		}

		public void AddEventFloat(AnimationClip clip,float time,float param){

			AnimationEvent animationEvent = new AnimationEvent ();

			animationEvent.functionName = "OnAnimationEventFloat";
			animationEvent.floatParameter = param;
			animationEvent.time = time;

			animationClip.AddEvent (animationEvent);

			//animationEventList.Add (animationEvent);
		}

		public void AddEventInt(AnimationClip clip,float time, string functionName,int param){

			AnimationEvent animationEvent = new AnimationEvent ();

			animationEvent.functionName = "OnAnimationEventInt";
			animationEvent.intParameter = param;
			animationEvent.time = time;
			if(clip == null)
				animationClip.AddEvent (animationEvent);
			else
				clip.AddEvent (animationEvent);

			//animationEventList.Add (animationEvent);
		}

		public void AddEventString (AnimationClip clip,float time, string functionName,string param){

			AnimationEvent animationEvent = new AnimationEvent ();

			animationEvent.functionName = "OnAnimationEventString";
			animationEvent.stringParameter = param;
			animationEvent.time = time;

			if(clip == null)
				animationClip.AddEvent (animationEvent);
			else
				clip.AddEvent (animationEvent);

			//animationEventList.Add (animationEvent);
		}

		public void AddEventUnityEngineObject (AnimationClip clip,float time, string functionName,Object param){

			AnimationEvent animationEvent = new AnimationEvent ();

			animationEvent.functionName = "OnAnimationEventUnityEngineObject";
			animationEvent.objectReferenceParameter = param;
			animationEvent.time = time;

			if(clip == null)
				animationClip.AddEvent (animationEvent);
			else
				clip.AddEvent (animationEvent);

			//animationEventList.Add (animationEvent);
		}

		void OnAnimationEventFloat (float param) {
			//Debug.LogErrorFormat ("OnAnimationEventFloat {0}",param);
			if (luaOnAnimationEventFloat != null ) {
				luaOnAnimationEventFloat (param);
			}
		}

		void OnAnimationEventInt (int param) {
			//Debug.LogErrorFormat ("OnAnimationEventInt {0}",param);
			if (luaOnAnimationEventInt != null ) {
				luaOnAnimationEventInt (param);
			}
		}

		void OnAnimationEventString (string param) {
			//Debug.LogErrorFormat ("OnAnimationEventString {0}",param);
			if (luaOnAnimationEventString != null ) {
				luaOnAnimationEventString (param);
			}
		}

		void OnAnimationEventUnityEngineObject (UnityEngine.Object param) {
			//Debug.LogErrorFormat ("OnAnimationEventString {0}",param);
			if (luaOnAnimationEventUnityEngineObject != null ) {
				luaOnAnimationEventUnityEngineObject (param);
			}
		}

		void OnAnimationEventObject (string param) {
			//Debug.LogErrorFormat ("OnAnimationEventString {0}",param);
			LuaOnAnimationEventObject luaOnAnimationEventObject = funDic[param]; 
			if (luaOnAnimationEventObject != null ) {
				luaOnAnimationEventObject (funParamDic[param]);
			}
		}

		protected virtual void OnDestroy(){
			
			base.OnDestroy ();

			funDic.Clear ();
			funParamDic.Clear ();

			//animationEventList.Clear ();
		}
	}
}