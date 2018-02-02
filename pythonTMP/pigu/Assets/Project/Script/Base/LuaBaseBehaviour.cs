using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using System;

namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public class LuaBaseBehaviour : MonoBehaviour 
	{

		internal LuaEnv luaEnv;

		public BehaviourInjection[] injections;

		bool luaAwakeInited = false;

		private	Action luaAwake;
		private Action luaStart;
		//private Action luaUpdate;
		private Action luaOnDestroy;

		private List<System.Object> refObject = new List<System.Object> ();

		public string luaPath;

		public LuaTable scriptEnv;

		static private System.Text.Encoding encoding = new System.Text.UTF8Encoding();

		static public Dictionary<GameObject,string > luaFilePathDic = new Dictionary<GameObject,string > ();

		static public LuaTable Add(string gameObjectName,string luaFilePath){
			
			GameObject gameObject = GameObject.Find (gameObjectName);
			luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<LuaBaseBehaviour> ();
			luaBaseBehaviour.Init ();
			return luaBaseBehaviour.scriptEnv;
		}

		static public LuaTable Load(string gameObjectName,string luaFilePath)
		{
			GameObject gameObject = new GameObject (gameObjectName);
			luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<LuaBaseBehaviour> ();
			luaBaseBehaviour.Init ();
			return luaBaseBehaviour.scriptEnv;
		}

		static public LuaTable LoadDontDestroy(string gameObjectName,string luaFilePath)
		{
			GameObject gameObject = new GameObject (gameObjectName);
			DontDestroyOnLoad (gameObject);

			luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<LuaBaseBehaviour> ();
			luaBaseBehaviour.Init ();
			return luaBaseBehaviour.scriptEnv;
		}

		static public void Rem(GameObject go)
		{
			GameObject.Destroy(go.GetComponent<LuaBaseBehaviour> ());
		}

		public virtual void Init()
		{
			if (scriptEnv != null)
				return;

			if (string.IsNullOrEmpty( luaPath )) {

				luaFilePathDic.TryGetValue (gameObject,out luaPath);
				luaFilePathDic.Remove (gameObject);
			} 

			if (string.IsNullOrEmpty (luaPath)) {
				Debug.LogError (" luaPath = null !");
				return;
			}
		
			if (!luaPath.Trim().EndsWith (".lua")) {
				luaPath = luaPath + ".lua";
			}
				
			string luaCode;

			string filePath;

			byte[] code = null;
				
			#if UNITY_EDITOR
			/* 在编辑器下从 Resources 下加载 */
			filePath = Application.dataPath + "/Resources/" + luaPath;
			code = System.IO.File.ReadAllBytes(filePath);
			Debug.LogWarningFormat ("LuaBaseBehaviour load file {0}",filePath);
			#else
			//filePath = Application.persistentDataPath + "/" + luaPath;
			code = ReadRes.ReadByte (luaPath);
			#endif
		
			if (code == null) {
				Debug.LogError ("lua code is null "+ luaPath);
				return;
			}
		
			luaEnv = LuaManager.GetInstance ().LuaEnvGetOrNew();
	
			scriptEnv = luaEnv.NewTable();

			LuaTable meta = luaEnv.NewTable();
			meta.Set("__index", luaEnv.Global);
			scriptEnv.SetMetaTable(meta);
			meta.Dispose();

			scriptEnv.Set("self", this);
			scriptEnv.Set("scriptEnv", scriptEnv );
				
			if (injections != null) 
			{
				foreach (var injection in injections)
				{
					scriptEnv.Set (injection.name, injection.value);
				}
			}

			scriptEnv.Set ("luaPath",luaPath);

			luaEnv.DoString(code,"LuaBaseBehaviour",scriptEnv);

			//luaEnv.DoString(string.Format("require '{0}'",luaPath), "LuaBaseBehaviour_"+gameObject.GetInstanceID(), scriptEnv);
			//luaEnv.DoString("function awake()\n\nend\t\n\nfunction start()\n\tprint(\"LuaBaseBehaviour start...\"..self.luaPath)\nend\n\nfunction update()\n\tlocal r = CS.UnityEngine.Vector3.up * CS.UnityEngine.Time.deltaTime\n\tself.transform:Rotate(r)\nend\n\nfunction ondestroy()\n    print(\"LuaBaseBehaviour destroy...\"..self.luaPath)\nend", "LuaBaseBehaviour", scriptEnv);

			scriptEnv.Get("awake", out luaAwake);

			/*
			if (luaAwake == null) {
				Debug.LogError ("awake null");
			} else {
				Debug.LogError ("awake ");
			}
			*/

			//#if UNITY_EDITOR
			if (luaAwake != null && !luaAwakeInited) {
				luaAwake ();
				luaAwakeInited = true;
			}
			//#endif

			scriptEnv.Get("start", out luaStart);
			//scriptEnv.Get("update", out luaUpdate);
			scriptEnv.Get("ondestroy", out luaOnDestroy);

		}

		protected virtual void Awake () {
			
			if (scriptEnv == null) {
				Init ();
			}
				
			if (luaAwake != null && !luaAwakeInited){
				luaAwake();
				luaAwakeInited = true;
			}
		}

		// Use this for initialization
		protected virtual void Start () 
		{
			if (luaStart != null)
			{
				luaStart();
			}
		}

		public System.Object GetDelegate(string funName){
			
			System.Object luaFun = scriptEnv.Get<Libs.OnCreate>(funName);
			//refObject.Add (luaFun);

			return luaFun;
		}

		// Update is called once per frame
		/*
		void Update () 
		{
			if (luaUpdate != null)
			{
				luaUpdate();
			}
		}
		*/

		protected virtual void OnDestroy()
		{
			if (luaOnDestroy != null)
			{
				luaOnDestroy();
			}
			refObject.Clear ();
			refObject = null;

			luaOnDestroy = null;
			//luaUpdate = null;
			luaStart = null;
			if(scriptEnv != null)
			scriptEnv.Dispose();
			injections = null;
		}
	}
		
}