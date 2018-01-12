using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace ZhuYuU3d
{
	public delegate void OnGameCmp(string assetName);

	class UIManagerLoadItem{

		public string layer;
		public OnGameCmp onGameCmp;

		public UIManagerLoadItem(string layer,OnGameCmp onGameCmp){
			this.layer = layer;
			this.onGameCmp = onGameCmp;
		}
	}

	[LuaCallCSharp]
	public class UIManager : MonoBehaviour
	{

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
		public void Load(string panelName,string funName,string layer)
		{

			LuaTable gameState = env.Global.Get<LuaTable> ("GameState");
			if (gameState != null) 
			{
				LuaTable curLuaScene = gameState.Get<LuaTable> ("curLuaScene");

				onGameCmp = curLuaScene.Get<OnGameCmp> (funName);

				if (onGameCmp == null) {
					Debug.LogWarningFormat ("can not find lua function {0} in GameState.curLuaScene ", funName);
					onGameCmp = env.Global.Get<OnGameCmp> (funName);
				}
				if (onGameCmp == null) {
					Debug.LogErrorFormat ("can not find lua function {0} ", funName);
				}
			}

			luaCallBackDic[panelName]=new UIManagerLoadItem(layer,onGameCmp);

			Libs.AM.I.CreateFromCache (panelName, OnCmp);
		}


		void OnCmp (string assetName, Object objInstantiateTp)
		{

			UIManagerLoadItem curLoadItem;

			luaCallBackDic.TryGetValue (assetName,out curLoadItem);

			string Layer = dfLayer;

			if (curLoadItem != null && curLoadItem.layer != "")
				Layer = curLoadItem.layer;

			GameObject objInstantiate =(GameObject)Instantiate((GameObject)objInstantiateTp);
			objInstantiate.name = objInstantiate.name.Replace("(Clone)","");

			objInstantiate.transform.SetParent(GameObject.Find(Layer).transform,false);

			if(curLoadItem != null&&curLoadItem.onGameCmp!=null)
				curLoadItem.onGameCmp (assetName);

			luaCallBackDic.Remove (assetName);
		}

		// Update is called once per frame
		void Update () {

		}


		/// <summary>
		/// 出一个通用提示框
		///  </summary>
		/// <param name="panelName">Panel name.</param>
		/// <param name="strTitle">String title.</param>
		/// <param name="strContent">String content.</param>
		/// <param name="nType">N type.</param>
		/// <param name="layer">Layer.</param>
		/// <param name="onOK">On O.</param>
		/// <param name="onCancel">On cancel.</param>
		public void PopWindow(string panelName,string strTitle,string strContent,int nType,string layer,LuaFunction onOK,LuaFunction onCancel,LuaFunction onLoadOver)
		{
				
			GameObject go = null;
			Libs.AM.I.CreateFromCache(panelName, (string assetName, UnityEngine.Object objInstantiateTp) =>
			{
					go = GameObject.Instantiate(objInstantiateTp) as GameObject ;
					go.transform.SetParent(GameObject.Find(layer).transform,false);

					MessageBox MBInstance=go.AddComponent<MessageBox>();

					if(onLoadOver!=null)
					{
						onLoadOver.Call(new object[]{go},new System.Type[]{typeof(GameObject)});
					}

					if (MBInstance != null)
					{

						if (nType == 0) 
						{
							MBInstance.ShowOK(strTitle,strContent,
								(MessageBox.Result ret) => 
								{
									//UIManager.GetInstance().ClosePage(this.name);

									if (ret == MessageBox.Result.OK) 
									{
										if (onOK != null) {
											onOK.Call ();
										}
									}
								}
							);

						} else if (nType == 1) {
							MBInstance.ShowYesNo(strTitle, strContent, (MessageBox.Result ret) => {
								//UIManager.GetInstance().ClosePage(this.name);

								if (ret == MessageBox.Result.YES) {
									if (onOK != null) {
										onOK.Call ();
									}
								}
								else if (ret == MessageBox.Result.NO) 
								{
									if (onCancel != null) {
										onCancel.Call ();
									}
								}
							});
						}

					}

				});


		}


		/// <summary>
		///   打开Toast面板
		/// </summary>
		/// <param name="strContent">内容</param>
		/// <param name="ntime">time</param>
		/// <param name="nfontsize">文本大小</param>
		/// <param name="ndirection">方位</param>
		/// <param name="onover">结束回调.</param>
		public void ToastTips(string strContent,int ntime,int nfontsize,int ndirection,LuaFunction onover)
		{
			if(ndirection==0)
				ZhuYuU3d.Toast.Show (this, strContent, ntime, ZhuYuU3d.Toast.Type.WARNING, ZhuYuU3d.Toast.Gravity.CENTER,nfontsize);
			else if(ndirection==1)
				ZhuYuU3d.Toast.Show (this, strContent, ntime, ZhuYuU3d.Toast.Type.WARNING, ZhuYuU3d.Toast.Gravity.BOTTOM_RIGHT,nfontsize);
			else if(ndirection==2)
				ZhuYuU3d.Toast.Show (this, strContent, ntime, ZhuYuU3d.Toast.Type.WARNING, ZhuYuU3d.Toast.Gravity.BOTTOM,nfontsize);
			else if(ndirection==3)
				ZhuYuU3d.Toast.Show (this, strContent, ntime, ZhuYuU3d.Toast.Type.WARNING, ZhuYuU3d.Toast.Gravity.TOP,nfontsize);
			else if(ndirection==4)
				ZhuYuU3d.Toast.Show (this, strContent, ntime, ZhuYuU3d.Toast.Type.WARNING, ZhuYuU3d.Toast.Gravity.TOP_RIGHT,nfontsize);
		}


	}

}