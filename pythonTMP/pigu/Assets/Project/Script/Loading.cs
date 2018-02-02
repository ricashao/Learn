using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ZhuYuU3d;
using XLua;
using ZhuYuU3d.Game;

namespace ZhuYuU3d{

	public class Loading : MonoBehaviour {

		Slider slider;

		LuaEnv env;
		LuaManager luaManager;

		static bool isCopyCmp = true;

		void Awake(){

			Debug.LogWarning ("Loading Awake!");

			string strFileName = "ui/launchpanel.panel";

			/*
			Libs.AM.I.CreateFromCache("launchpanel.panel", delegate (string eventName, Object objInstantiateTp){
				GameObject objInstantiate = Instantiate(objInstantiateTp as GameObject);

				objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform);
			});
			*/

			Libs.ABM.I.LoadOne (strFileName,delegate (string name,AssetBundle assetBundle){
				GameObject go = assetBundle.LoadAsset<GameObject>("LaunchPanel");
				GameObject objInstantiate = Instantiate (go);

				objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);

				slider = objInstantiate.transform.GetComponentInChildren<Slider>();

				RunState ();
			});

			/*
			ABLoaderHelper.Instance.LoadAB
			(
				strFileName, GameObject.Find("Canvas") , "LaunchPanel", (GameObject go) =>
				{
					//go.AddComponent<LaunchPage>();
					slider = go.transform.GetComponentInChildren<Slider>();
				
				}
			);
			*/


			if (isCopyCmp) {
				luaManager = LuaManager.GetInstance ();
				luaManager.InitDoString = "require 'lua/game/GameState' \n game_state_init() \n";
				env = luaManager.LuaEnvGetOrNew ();
			} else {
				StartCoroutine (CopyToPersistentDataPath ());
			}
		
			Debug.LogWarning ("Loading Awake end!");
		}

		static bool isInit = true;

		// Use this for initialization
		void Start () {

			Debug.LogWarning ("Loading Start!");
			/* 执行 
			if (isCopyCmp) 
				RunState ();
			*/ 
			Debug.LogWarning ("Loading Start end!");
		}

		void Init(){
			if (isInit) {
				/*
				Libs.ABM.I.LoadAssetBundleManifestAdd ("StreamingAssets_u3d_res_project");
				Libs.AM.I.InitAssetName2abPathDic ("StreamingAssets_u3d_xlua_project");
				Libs.AM.I.InitAssetName2abPathDic ("StreamingAssets_u3d_res_project");
				*/
				isInit = false;
				Debug.LogWarning ("初始化完成！");
			} else {
				Debug.LogWarning ("已经初始化！");
			}
		}

		IEnumerator CopyToPersistentDataPath(){

			string dataPath = Application.persistentDataPath;//Util.DataPath;  //数据目录
			string resPath = PathTools.AppContentPath();//Util.AppContentPath(); //游戏包资源目录

			if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
			Directory.CreateDirectory(dataPath);

			string infile = resPath + "md5filelist.txt";
			string outfile = dataPath + "md5filelist.txt";

			if (File.Exists (outfile)) {
				File.Delete (outfile);
			}

			string message = "正在解包文件:>files.txt";
			Debug.Log(infile);
			Debug.Log(outfile);

			if (Application.platform == RuntimePlatform.Android) {
				WWW www = new WWW(infile);

				while (true){
					if (www.isDone || !string.IsNullOrEmpty(www.error)){
						System.Threading.Thread.Sleep(50); 
						if (!string.IsNullOrEmpty(www.error)){
							Debug.LogError(www.error);
						}else{
							File.WriteAllBytes(outfile, www.bytes);
						}
						break;
					}
				}
				yield return 0;
			} else File.Copy(infile, outfile, true);
			yield return new WaitForEndOfFrame();

			//释放所有文件到数据目录
			string[] files = File.ReadAllLines(outfile);
			foreach (var file in files) {
				string[] fs = file.Split('=');

				if (fs.Length == 1) {
					Debug.LogWarning ("跳过 >>" + file);
					continue;
				}

				if (resPath.EndsWith ("/") && fs [0].StartsWith ("/"))
					resPath = resPath.Substring (0, resPath.Length - 1);
				
				if (dataPath.EndsWith ("/") && fs [0].StartsWith ("/"))
					dataPath = dataPath.Substring (0, dataPath.Length - 1);
				
				infile =  resPath +  fs[0];  //
				outfile = dataPath + fs[0];

				message = "正在解包文件:>" + fs[0];
				Debug.Log("正在解包文件:>" + infile);

				string dir = Path.GetDirectoryName(outfile);
				if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

				if (Application.platform == RuntimePlatform.Android) {
					WWW www = new WWW(infile);

					while (true){
						if (www.isDone || !string.IsNullOrEmpty(www.error)){
							System.Threading.Thread.Sleep(50); 
							if (!string.IsNullOrEmpty(www.error)){
								Debug.LogError(www.error);
							}else{
								File.WriteAllBytes(outfile, www.bytes);
								Debug.LogWarning (">>" + outfile+">>"+www.bytes.Length);
							}
							break;
						}
					}
					/*
					yield return www;

					if (www.isDone) {
						File.WriteAllBytes(outfile, www.bytes);
						Debug.LogWarning (">>" + outfile+">>"+www.bytes.Length);
					}
					*/
					yield return 0;
				} else {
					if (File.Exists(outfile)) {
						File.Delete(outfile);
					}
					File.Copy(infile, outfile, true);
				}
				yield return new WaitForEndOfFrame();
			}
			message = "解包完成!!!";

			luaManager = LuaManager.GetInstance ();
			luaManager.InitDoString = "require 'lua/game/GameState' \n game_state_init() \n";
			env = luaManager.LuaEnvGetOrNew ();

			yield return new WaitForSeconds (1f);

			RunState ();

			isCopyCmp = true;
		}

		void RunState(){

			Init ();

			env.DoString ("game_state_loading()");
			//luaManager.SetLuaUpdate ("game_state_update");
		}

		// Update is called once per frame
		void Update () {
			
			if (slider != null) {
		
				slider.value += Time.deltaTime * 2f;
			}
		}

		void OnDestroy()
		{
			env = null;
			luaManager = null;
		}
	}

}