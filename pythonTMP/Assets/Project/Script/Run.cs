using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
namespace ZhuYuU3d{

	public class Run : MonoBehaviour {
		
		LuaEnv env;

		void Awake(){
			
			env = LuaManager.GetInstance ().LuaEnvGetOrNew ();
			env.DoString ("game_state_run()");

			//LuaTable gameState = env.Global.Get<LuaTable> ("GameState");
			//LuaTable curLuaScene = gameState.Get<LuaTable> ("curLuaScene");
			/*
			LuaTable meta = env.NewTable();
			meta.Set("__index", env.Global);
			curLuaScene.SetMetaTable(meta);
			meta.Dispose();*/


		}
		bool isLoad;
		// Use this for initialization
		void Start () {
			/* 执行 */

			/*
			Dictionary<string,string> abDic = new Dictionary<string, string>();
			Libs.ManifestFileTools.ReadAssetsName2AssetBundleInDic("StreamingAssets_u3d_xlua_project"+"_AssetsName2AssetBundleAll.txt" ,abDic);

			string ab = abDic["DemoPanel"];
			Debug.Log(ab);
			*/
			/*
			Libs.ABM.I.Load (ab,delegate (string name,AssetBundle assetBundle){
				GameObject go = assetBundle.LoadAsset<GameObject>("DemoPanel");
				GameObject objInstantiate = Instantiate (go);
				objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);
			});
			*/
			/**/

			/*
			 Libs.AM.I.CreateFromCache("DemoPanel", delegate (string assetName, Object objInstantiateTp){
				
				GameObject objInstantiate = Instantiate((GameObject)objInstantiateTp);
				objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
				objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);

				isLoad = true;
			}) ;
			*/
			//gameObject.AddComponent<UIManager>();
		}

		// Update is called once per frame
		void Update () {
			if (isLoad) {
				//env.DoString ("game_state_run()");

				isLoad = false;
			}
		}
	}
}