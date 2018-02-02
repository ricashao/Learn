using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Text;
using XLua;

namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public enum LoginType {
		WeChat = 0,
		QQ = 1,
	}
		
	public delegate void OnPlatformCallBack(string sdk,string calltype,string callBackType,string data);

	[LuaCallCSharp]
	public class PlatformAPI : MonoBehaviour {

		static public PlatformAPI instance;

		static public PlatformAPI GetInstance(){
			return instance; 
		}
			
		LoginType curLoginType ;

		OnPlatformCallBack onPlatformCallBack;

		void Awake(){

			instance = this;
		
			GameObject.DontDestroyOnLoad (gameObject);
		}

		// Use this for initialization
		void Start () {

			if (Application.platform == RuntimePlatform.Android) {

			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {

			} else if (Application.platform == RuntimePlatform.WebGLPlayer) {

			} else if (Application.platform == RuntimePlatform.WindowsPlayer) {

			} else if (Application.platform == RuntimePlatform.OSXPlayer){

			} else if (Application.platform == RuntimePlatform.WindowsEditor) {

			} else if (Application.platform == RuntimePlatform.OSXEditor){

			}
		}

		static public Boolean IsEditor(){
			#if UNITY_EDITOR 
			return true;
			#else 
			return false;
			#endif
		}

		/// <summary>
		/// 获取 mac 地址
		/// </summary>
		/// <returns>The mac.</returns>
		static public string GetMac(){

			string macString = "";
		
			if (Application.platform == RuntimePlatform.Android) {
				
				using (AndroidJavaClass utilsFun = new AndroidJavaClass ("com.zhuyu.game.UtilsFun")) {
					macString = utilsFun.CallStatic<string> ("getMAC");
				}
				Debug.LogWarningFormat ("Mac:{0}", macString);
			} 
			else
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
					
				macString = CallPlatformGetMac ();
			}

			return macString;
		} 
		/// <summary>
		/// 是否在 wifi 环境中 
		/// </summary>
		static public bool IsWifi(){

			bool isOnWifi = true;

			if (Application.platform == RuntimePlatform.Android) {
				
				using (AndroidJavaClass utilsFun = new AndroidJavaClass ("com.zhuyu.game.UtilsFun")) {
					isOnWifi = utilsFun.CallStatic<bool> ("isWifi");
				}
				Debug.LogWarningFormat ("IsWifi:{0}", isOnWifi);
			}
			else
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				isOnWifi = CallPlatformIsWifi ();
			}
			
			return isOnWifi;
		}

		// Update is called once per frame
		void Update () {

		}

		/* sdk 类型 */
		const string Sdk_Wechat = "Sdk_Wechat";
		const string Sdk_QQ     = "Sdk_QQ";
		/* 请求类型 */
		const string PlatformCallType_Login  = "Login";
		const string PlatformCallType_Logout = "Logout";
		/*  */
		const string PlatformCallBackType_OnSucceed = "OnSucceed";
		const string PlatformCallBackType_OnFault  = "OnFault";
		const string PlatformCallBackType_OnCancel = "OnCancel";

		/// <summary>
		/// 平台层回调接口
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="data">Data.</param>
		[MonoPInvokeCallback(typeof(OnPlatformCallBack))]
		public static void OnPlatformCallBack(string sdk,string callType,string callBackType,string data){

			if (callType.Equals (PlatformCallType_Login)) {
				
				if (callType.Equals (PlatformCallBackType_OnSucceed)) {

				}else if (callType.Equals (PlatformCallBackType_OnFault)) {

				}else if (callType.Equals (PlatformCallBackType_OnCancel)) {

				}

			} else if (callType.Equals (PlatformCallType_Logout)) {

				Debug.LogWarning (PlatformCallType_Logout);
			}

		}

		//#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
		const string DLL = "__Internal";
		/// <summary>
		/// 平台调用接口
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="data">Data.</param>
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void CallPlatform(string sdk,string callType,string data);
		/// <summary>
		/// is wifi.
		/// </summary>
		/// <returns><c>true</c>, if platform is wifi was called, <c>false</c> otherwise.</returns>
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool CallPlatformIsWifi ();
		/// <summary>
		/// get mac.
		/// </summary>
		/// <returns><c>true</c>, if platform get mac was called, <c>false</c> otherwise.</returns>
		[DllImport(DLL, CallingConvention = CallingConvention.Cdecl)]
		public static extern string CallPlatformGetMac ();
		//#endif

		void LoginIOS(){

			if (curLoginType == LoginType.WeChat) {

				CallPlatform (Sdk_Wechat,PlatformCallType_Login,"");

			} else if (curLoginType == LoginType.QQ) {

				CallPlatform (Sdk_QQ,PlatformCallType_Login,"");
			}	
		}

		void LoginAndroid(){

			if (curLoginType == LoginType.WeChat) {

				using (AndroidJavaClass utilsFun = new AndroidJavaClass ("com.zhuyu.game.WEChatApi")) {
					utilsFun.CallStatic ("Login");
				}
			} else if (curLoginType == LoginType.QQ) {

				using (AndroidJavaClass utilsFun = new AndroidJavaClass ("com.zhuyu.game.QQApi")) {
					utilsFun.CallStatic ("Login");
				}

			}	
		}

		public void Login(LoginType curLoginType){

			this.curLoginType = curLoginType;

			if (Application.platform == RuntimePlatform.Android) {
				LoginAndroid ();
			} 
			else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				LoginIOS ();
			} 
		}
		/// <summary>
		/// 登录成功 the succeed.
		/// </summary>
		/// <param name="accessToken">Access token.</param>
		public void onSucceed(string data){

			Debug.LogWarningFormat ("onSucceed data {0}",data);

			if (curLoginType == LoginType.WeChat) {


			} else if (curLoginType == LoginType.QQ) {

			}
		}
		/// <summary>
		/// 登录错误 Ons the fault.
		/// </summary>
		/// <param name="info">Info.</param>
		public void onFault(string info){

			Debug.LogWarningFormat ("onFault info {0}",info);

			if (curLoginType == LoginType.WeChat) {


			} else if (curLoginType == LoginType.QQ) {

			}
		}
		/// <summary>
		/// 取消回调 Ons the cancel.
		/// </summary>
		public void onCancel(){
			Debug.LogWarningFormat ("onCancel ");

			if (curLoginType == LoginType.WeChat) {


			} else if (curLoginType == LoginType.QQ) {

			}
		}

		public void LoginCallbacks(string key, string access_token){

			Debug.LogWarningFormat (" LoginCallbacks key {0},data {1}",key,access_token);
		}

		private string stringToEdit = "string";

		bool isShow =  false;
		/*
		void OnGUI()
		{
			//绘制一个输入框接收用户输入
			stringToEdit = GUILayout.TextField (stringToEdit, GUILayout.Width (300), GUILayout.Height (20));

			if (GUILayout.Button ("Test", GUILayout.Height (30))) {
				isShow = !isShow;
			}

			if (isShow) {
				
				if (GUILayout.Button ("GetMac", GUILayout.Height (50))) {
				
					GetMac ();

				} else if (GUILayout.Button ("Login WeChat", GUILayout.Height (50))) {
				
					Login (LoginType.WeChat);

				}
			}
			
		}
		*/

		public void OpenWebView(string url){
			//CS.ZhuYuU3d.instance:OpenWebView()

			if (Application.platform == RuntimePlatform.Android) {

				AndroidJavaClass jc = new AndroidJavaClass("com.runrain.platform.UnityPlayerActivity");
				AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
				if (jo != null) {
					jo.Call ("StartWebView", url);
				}
			}
			else
			if (Application.platform == RuntimePlatform.IPhonePlayer) {

			}

			Debug.LogWarningFormat ("url -> {0}",url);
					
		}
	}

}