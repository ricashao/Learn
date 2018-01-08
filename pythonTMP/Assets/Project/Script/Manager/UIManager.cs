using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace ZhuYuU3d{

	public delegate void OnGameCmp(string assetName);

	public class UIManager : MonoBehaviour {

		static UIManager instance;

		static public UIManager GetInstance(){

			if(instance){
				return instance;
			}
			GameObject gameObject = new GameObject (typeof(UIManager).Name);
			GameObject.DontDestroyOnLoad (gameObject);

			instance = gameObject.AddComponent<UIManager>();
			return instance;
		}
			
		Dictionary <string,OnGameCmp> luaCallBackDic = new Dictionary<string, OnGameCmp> ();

		OnGameCmp onGameCmp;

		LuaEnv	env;

		void Awake(){
			if(instance == null)
				instance = this;
			env = LuaManager.GetInstance ().env;
		}
		// Use this for initialization
		void Start () {
			//Load ();
		}
		/// <summary>
		/// Load the specified panelName and funName.
		/// </summary>
		/// <param name="panelName">Panel name.</param>
		/// <param name="funName">Fun name. 默认在 GameState.curLuaScene 找，如果没有在 Global 找 </param>
		public void Load(string panelName,string funName){

			LuaTable gameState = env.Global.Get<LuaTable> ("GameState");
			LuaTable curLuaScene = gameState.Get<LuaTable> ("curLuaScene");

			onGameCmp = curLuaScene.Get<OnGameCmp> (funName);

			if (onGameCmp == null) {
				Debug.LogWarningFormat("can not find lua function {0} in GameState.curLuaScene ",funName);
				onGameCmp = env.Global.Get<OnGameCmp> (funName);
			}
			if (onGameCmp == null) {
				Debug.LogErrorFormat ("can not find lua function {0} ",funName);
				return;
			}
			luaCallBackDic.Add (panelName,onGameCmp);

			Libs.AM.I.CreateFromCache (panelName, OnCmp);
		}

		void OnCmp (string assetName, Object objInstantiateTp){

			GameObject objInstantiate = Instantiate((GameObject)objInstantiateTp);
			objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
			objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);

			OnGameCmp curOnGameCmp;

			luaCallBackDic.TryGetValue (assetName,out curOnGameCmp);
			if(curOnGameCmp != null)
				curOnGameCmp (assetName);
			
			luaCallBackDic.Remove (assetName);
		}

		// Update is called once per frame
		void Update () {

		}
	}

}