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
			return luaBaseBehaviour.scriptEnv;
		}

		static public LuaTable Load(string gameObjectName,string luaFilePath)
		{
			GameObject gameObject = new GameObject (gameObjectName);
			luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<LuaBaseBehaviour> ();

			return luaBaseBehaviour.scriptEnv;
		}

		static public LuaTable LoadDontDestroy(string gameObjectName,string luaFilePath)
		{
			GameObject gameObject = new GameObject (gameObjectName);
			DontDestroyOnLoad (gameObject);

			luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<LuaBaseBehaviour> ();

			return luaBaseBehaviour.scriptEnv;
		}

		static public void Rem(GameObject go)
		{
			GameObject.Destroy(go.GetComponent<LuaBaseBehaviour> ());
		}

		protected virtual void Awake()
		{
			luaFilePathDic.TryGetValue (gameObject,out luaPath);
			luaFilePathDic.Remove (gameObject);

			if (luaPath == null) {
				Debug.LogError (" luaPath = null !");
				return;
			} else {
				if (!luaPath.Trim().EndsWith (".lua")) {
					luaPath = luaPath + ".lua";
				}
			}

			string luaCode;

			string filePath = Application.persistentDataPath + "/" + luaPath;

			if (!System.IO.File.Exists (filePath)) {
				filePath = Application.streamingAssetsPath + "/" + luaPath;
			}

			#if UNITY_EDITOR
			/* 在编辑器下从 Resources 下加载 */
			if (!System.IO.File.Exists (filePath)) {
				filePath = Application.dataPath + "/Resources/" + luaPath;
			}
			#endif
			if (!System.IO.File.Exists (filePath)) {
				Debug.LogError (" not find "+ luaPath);
				return;
			}

			Debug.LogWarningFormat ("load file {0}",filePath);
				
			luaCode = System.IO.File.ReadAllText (filePath,encoding);

			//WWW www = new WWW(Application.streamingAssetsPath+"/"+luaPath+".lua");  
			//while (!www.isDone) {}

			//Transform transformbt  = transform.Find ("Button");
			//transform.GetComponent<Text>().text = 
			//gameObject.SetActive(false);

			luaEnv = LuaManager.GetInstance ().LuaEnvGetOrNew();
	
			scriptEnv = luaEnv.NewTable();

			LuaTable meta = luaEnv.NewTable();
			meta.Set("__index", luaEnv.Global);
			scriptEnv.SetMetaTable(meta);
			meta.Dispose();

			scriptEnv.Set("self", this);
			scriptEnv.Set("scriptEnv", scriptEnv);
				
			if (injections != null) 
			{
				foreach (var injection in injections)
				{
					scriptEnv.Set (injection.name, injection.value);
				}
			}

			scriptEnv.Set ("luaPath",luaPath);

			luaEnv.DoString(luaCode,"LuaBaseBehaviour",scriptEnv);

			//luaEnv.DoString(string.Format("require '{0}'",luaPath), "LuaBaseBehaviour_"+gameObject.GetInstanceID(), scriptEnv);
			//luaEnv.DoString("function awake()\n\nend\t\n\nfunction start()\n\tprint(\"LuaBaseBehaviour start...\"..self.luaPath)\nend\n\nfunction update()\n\tlocal r = CS.UnityEngine.Vector3.up * CS.UnityEngine.Time.deltaTime\n\tself.transform:Rotate(r)\nend\n\nfunction ondestroy()\n    print(\"LuaBaseBehaviour destroy...\"..self.luaPath)\nend", "LuaBaseBehaviour", scriptEnv);

			if (luaAwake == null) {
				luaAwake = scriptEnv.Get<Action> ("awake");

				if (luaAwake != null){
					luaAwake();
				}
			}

			scriptEnv.Get("start", out luaStart);
			//scriptEnv.Get("update", out luaUpdate);
			scriptEnv.Get("ondestroy", out luaOnDestroy);

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