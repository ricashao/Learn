using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace ZhuYuU3d{

	public delegate void OnGameCmp(string assetName);

	class UIManagerLoadItem{

		public string layer;
		public OnGameCmp onGameCmp;

		public UIManagerLoadItem(string layer,OnGameCmp onGameCmp){
			this.layer = layer;
			this.onGameCmp = onGameCmp;
		}
	}

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
			
		Dictionary <string,UIManagerLoadItem> luaCallBackDic = new Dictionary<string, UIManagerLoadItem> ();

		OnGameCmp onGameCmp;

		LuaEnv	env;

		string dfLayer = "Canvas";

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
		public void Load(string panelName,string funName,string layer){

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
			luaCallBackDic.Add (panelName,new UIManagerLoadItem(layer,onGameCmp));

			Libs.AM.I.CreateFromCache (panelName, OnCmp);
		}

		void OnCmp (string assetName, Object objInstantiateTp){

			UIManagerLoadItem curLoadItem;

			luaCallBackDic.TryGetValue (assetName,out curLoadItem);

			string Layer = dfLayer;

			if (curLoadItem.layer != null && curLoadItem.layer != "")
				Layer = curLoadItem.layer;

			GameObject objInstantiate = Instantiate((GameObject)objInstantiateTp);
			objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
			objInstantiate.transform.SetParent(GameObject.Find(Layer).transform,false);

			if(curLoadItem != null)
				curLoadItem.onGameCmp (assetName);

			luaCallBackDic.Remove (assetName);
		}

		// Update is called once per frame
		void Update () {

		}
	}

}