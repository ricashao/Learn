using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

namespace ZhuYuU3d{
	
	[LuaCallCSharp]
	public class LuaManager : MonoBehaviour {

		static LuaManager instance;

		static public LuaManager GetInstance(){

			if(instance){
				return instance;
			}
			GameObject gameObject = new GameObject (typeof(LuaManager).Name);
			GameObject.DontDestroyOnLoad (gameObject);

			instance = gameObject.AddComponent<LuaManager>();
			return instance;
		}

		static public void SetFixedUpdateFun(string luaFixedUpdateFunName){
			LuaManager.GetInstance ().SetLuaFixedUpdate (luaFixedUpdateFunName);
		}
		static public void SetUpdateFun(string luaUpdateFunName){
			LuaManager.GetInstance ().SetLuaUpdate (luaUpdateFunName);
		}
		static public void SetLateUpdateFun(string luaLateUpdateFunName){
			LuaManager.GetInstance ().SetLuaLateUpdate (luaLateUpdateFunName);
		}

		internal static float lastGCTime = 0;
		internal const float GCInterval = 1;//1 second 

		private	Action luaFixedUpdate;
		private	Action luaUpdate;
		private	Action luaLateUpdate;

		string _initDoString = "";

		public string InitDoString {
			set{
				_initDoString = value;
			}
			get{
				return _initDoString;
			}
		}

		LuaEnv _env;

		public LuaEnv env {
			get{
				return _env;
			}
		}

		void Awake(){
			if(instance == null)
				instance = this;
		}

		public LuaEnv LuaEnvGetOrNew(){
			if (_env == null)
				return LuaEnvNew();
			
			return _env;
		}

		public LuaEnv LuaEnvNew(){

			if (_env != null) {
				Debug.LogWarning ("LuaManager New luaenv != NULL !");
				_env.Dispose ();
				_env = null;
			}
			_env = new LuaEnv();
			LuaEnvInit ();
			_env.DoString (_initDoString);
			return _env;
		}

		public void LuaEnvDispose(){

			if (_env == null) {
				Debug.LogError ("Error LuaManager Dispose luaenv == NULL !");
				return;
			}
			_env.Dispose ();
			_env = null;
		}

		void LuaEnvInit(){
			//lua 文件查找目录

			#if UNITY_EDITOR
			//先找 Resource 路径 -> StreamingAssets
			//Resource 路径
			_env.AddSearcher(ExtStaticLuaCallbacks.LoadLuaFileFromResource, -4);
			#else
			//先找 PersistentDataPath 路径 -> StreamingAssets
			//沙盒路径
			_env.AddSearcher(ExtStaticLuaCallbacks.LoadLuaFileFromPersistentDataPath, -4);
			#endif
			//模块注册
			_env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
			_env.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
			_env.AddBuildin("protobuf.c", XLua.LuaDLL.Lua.LoadProtobufC);
		}

		public void SetLuaFixedUpdate(string luaFixedUpdateFunName){
			if(_env == null){
				Debug.LogErrorFormat("_env is null !");
				return;
			}
			_env.Global.Get(luaFixedUpdateFunName, out luaFixedUpdate);
			if (luaFixedUpdate == null) {
				Debug.LogErrorFormat ("not find {0} function !",luaFixedUpdateFunName);
			}
		}

		public void SetLuaUpdate(string luaUpdateFunName){
			if(_env == null){
				Debug.LogErrorFormat("_env is null !");
				return;
			}
			_env.Global.Get(luaUpdateFunName, out luaUpdate);
			if (luaUpdate == null) {
				Debug.LogErrorFormat ("not find {0} function !",luaUpdateFunName);
			}
		}

		public void SetLuaLateUpdate(string luaLateUpdateFunName){
			if(_env == null){
				Debug.LogErrorFormat("_env is null !");
				return;
			}
			_env.Global.Get(luaLateUpdateFunName, out luaLateUpdate);
			if (luaLateUpdate == null) {
				Debug.LogErrorFormat ("not find {0} function !",luaLateUpdateFunName);
			}
		}

		// Use this for initialization
		void Start () {

		}

		void FixedUpdate () {
			
			if (_env == null)
				return;

			if (luaFixedUpdate != null)
				luaFixedUpdate ();
		}
			
		// Update is called once per frame
		void Update () {

			if (_env == null)
				return;

			if (luaUpdate != null)
				luaUpdate ();

			if (Time.time - lastGCTime > GCInterval)
			{
				_env.Tick();
				lastGCTime = Time.time;
			}
		}

		void LateUpdate(){

			if (_env == null)
				return;

			if (luaLateUpdate != null)
				luaLateUpdate ();
		}

	}

}