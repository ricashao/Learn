using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC;
using PureMVC.Patterns;

namespace ZhuYuU3d.Game
{
	
		public class AssetUpdateInfo 
		{
			public string mstrAssetServerUrl="";
		//	string mstrAssetLocalUrl="";
			public string mstrRemoteMD5FilePath="";
		//	string mstrRemoteMD5FilePath="";
			public bool bEnableHotFix=true;
		}

		public class AssetInfoRead
		{
			public AssetInfoRead(string sPath)
			{
				mstrFilePath = sPath;
			}

			AssetUpdateInfo mdata=null;
			protected string mstrFilePath="";
			public virtual void ReadData ()
			{
				
			}

			public AssetUpdateInfo getdata()
			{
				return mdata;
			}

			protected void resolveData(string sJsonData)
			{
				Debug.Log ("resolveData:"+sJsonData);
				System.Object objdes=MiniJSON.Json.Deserialize (sJsonData);
				if (objdes != null) 
				{
					Debug.Log ("Objdes is not null");//+sJsonData);

					Dictionary<string,System.Object> dictret = objdes as Dictionary<string,System.Object>;
					if (dictret != null) 
					{
						
						AssetUpdateInfo aui = new AssetUpdateInfo ();
						aui.mstrAssetServerUrl = dictret ["AssetServerUrl"].ToString();
						aui.mstrRemoteMD5FilePath=dictret ["RemoteMD5FilePath"].ToString();
						aui.bEnableHotFix = bool.Parse (dictret ["EnableHotfix"].ToString ());
						mdata = aui;

						Debug.Log ("Back");//+sJsonData);
					}

				}
				return;
			}

		}

		public class ResourceInfoRead:AssetInfoRead
		{
			public override void ReadData ()
			{
				TextAsset ta=(TextAsset) Resources.Load (mstrFilePath);
				if (ta != null) {
					resolveData (ta.text);
				}

			}

			public ResourceInfoRead(string sp):base(sp)
			{
				
			}

		}

		public class StreamPathInfoRead:AssetInfoRead
		{
			public override void ReadData ()
			{
				string strstreamingpath = "";
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					strstreamingpath = "file:///" + Application.dataPath + "/Raw/"; 
				} else if (Application.platform == RuntimePlatform.Android) {
					strstreamingpath = Application.streamingAssetsPath;
				} else {
					strstreamingpath = "file:///" + Application.dataPath + "/StreamingAssets/";
				}

				strstreamingpath += mstrFilePath;

				ZhuYuU3d.CoroutineController.Instance.StartCoroutine (Load(strstreamingpath));
				

			}

			IEnumerator Load(string spath)
			{
				WWW w = new WWW (spath);
				yield return w;
				if (w.error != null) {
					Debug.Log (w.error);
				}
				resolveData (w.text);
			}

			public StreamPathInfoRead(string sp):base(sp)
			{

			}

		}


		public class AssetUpdateInfoCommand:SimpleCommand
		{
			public AssetUpdateInfo getData()
			{
				return mair.getdata ();
			}
			public enum UpdateConfigLoadWay
			{
				LoadFromResource,
				LoadFromStream,
			}

			UpdateConfigLoadWay mlw=UpdateConfigLoadWay.LoadFromResource;

			AssetInfoRead mair=null;

			public AssetUpdateInfoCommand(UpdateConfigLoadWay uw,string sp)
			{
				if (mlw == UpdateConfigLoadWay.LoadFromResource) 
				{
					mair = new ResourceInfoRead (sp);		
				}
				else if(mlw == UpdateConfigLoadWay.LoadFromStream)
				{
					mair = new StreamPathInfoRead (sp);
				}

			}


			public override void Execute(INotification notification)
			{
				mair.ReadData ();
				SendNotification (NotificationType.LoadLaunchUI);
			}

		}

}
	