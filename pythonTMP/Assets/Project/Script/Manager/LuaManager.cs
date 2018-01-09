using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

namespace ZhuYuU3d{

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

		internal static float lastGCTime = 0;
		internal const float GCInterval = 1;//1 second 

		private	Action luaUpdate;

		string _initDoString;

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
			_env.AddSearcher(ExtStaticLuaCallbacks.LoadFromResourceLuaFile, -1);
			#endif
			//模块注册
			_env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
			_env.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
			_env.AddBuildin("protobuf.c", XLua.LuaDLL.Lua.LoadProtobufC);
		}

		public void SetLuaUpdate(string luaUpdateFunName){
			
			if(_env == null){
				Debug.LogErrorFormat("_env is null !");
				return;
			}
			_env.Global.Get(luaUpdateFunName, out luaUpdate);
			//luaUpdate = _env.Global.Get<Action> (luaUpdateFunName);
			if (luaUpdate == null) {
				Debug.LogErrorFormat ("not find {0} function !",luaUpdateFunName);
			}
		}

		// Use this for initialization
		void Start () {

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
	}

}