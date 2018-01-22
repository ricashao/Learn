using System;
using PureMVC.Patterns;
using UnityEngine;
using Libs;
using System.IO;

namespace ZhuYuU3d.Game
{
    public class LaunchCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);

            Debug.Log("Load LaunchCommand");

			string md5filelist = "md5filelist.txt";

			string dataPath = Application.persistentDataPath;//Util.DataPath;  //数据目录
			string resPath = PathTools.AppContentPath();//Util.AppContentPath(); //游戏包资源目录

			string infile = PathTools.Combine( resPath , "md5filelist.txt");
			string outfile = PathTools.Combine( dataPath ,"md5filelist.txt");

			Debug.Log(infile);

			if (PathTools.ExistsPersistentPath ("md5filelist.txt")) {
				Debug.LogWarning ("非首次启动！");
			} else {
				Debug.LogWarning ("首次启动！");

				//if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
				if (!Directory.Exists(dataPath))
				Directory.CreateDirectory(dataPath);

				string message = "正在解包文件:>md5filelist.txt";

				if (Application.platform == RuntimePlatform.Android) {
					WWW www = new WWW(infile);

					while (true){
						if (www.isDone || !string.IsNullOrEmpty(www.error)){
							System.Threading.Thread.Sleep(50); 
							if (!string.IsNullOrEmpty(www.error)){
								Debug.LogError(www.error);
							}else{
								File.WriteAllBytes(outfile, www.bytes);
								Debug.LogWarning(">>" + outfile);
							}
							break;
						}
					}

				} else File.Copy(infile, outfile, true);
			}

			string path = PathTools.Combine (PathTools. PersistentDataPath(), md5filelist);
			Debug.Log(path + ","+System.IO.File.Exists(path));

			string strFileName = "ui/launchpanel.panel";

//			Libs.AM.I.CreateFromCache (strFileName, (string assetName,UnityEngine.Object objInstantiateTp)=>
//				{
//					GameObject objInstantiate =(GameObject)GameObject.Instantiate((GameObject)objInstantiateTp);
//					objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
//
//					objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);
//
//					if (objInstantiate != null)
//					{
//						objInstantiate.AddComponent<LaunchPage>();
//					}
//				}
//			);
//
            
            ABLoaderHelper.Instance.LoadAB
			(
				strFileName, GameObject.Find("Canvas") , "LaunchPanel", (GameObject go) =>
               {
					go.AddComponent<LaunchPage>();
               }
            );

            
        }

    }
}