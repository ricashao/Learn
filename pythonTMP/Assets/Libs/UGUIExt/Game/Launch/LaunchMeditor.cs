using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using UnityEngine.SceneManagement;

namespace ZhuYuU3d.Game
{
    public partial class NotificationType
    {
        public const string V2V_BeginUpdateResource = "V2V_BeginUpdateResource";
		public const string V2M_BeginCheckResource = "V2M_BeginCheckResource";
		public const string M2M_ResourceUpdateOver = "M2M_ResourceUpdateOver";

    }
public class LaunchMeditor :Mediator
{
		string mstrRemoveUrl="";
		string mstrRemoteMD5Url="";


		bool mbHotFixEnable=true;

        AsyncOperation asyncOperation;
        public new const string NAME = "LaunchMediator";
        public LaunchPage View
        {
            get { return _view; }
        }

        public LaunchMeditor(LaunchPage view) : base(NAME, view)
        {
            _view = view;
			AssetUpdateInfoCommand auic=(AssetUpdateInfoCommand)Facade.getCommand (NotificationType.ReadUpdateInfo);
			if (auic != null) 
			{
				mstrRemoveUrl = auic.getData ().mstrAssetServerUrl;
				mstrRemoteMD5Url = auic.getData ().mstrRemoteMD5FilePath;
				mbHotFixEnable = auic.getData ().bEnableHotFix;
			}
        }

        LaunchPage _view = null;


        public override void OnRegister()
        {
            base.OnRegister();
        }
        public override void OnRemove()
        {
            base.OnRemove();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
			case NotificationType.V2V_BeginUpdateResource:
                    
				View.setState(LaunchPage.LaunchPageState.UpdateResource);
				BeginUpdateResource ();
                break;

			case NotificationType.V2M_BeginCheckResource:

				if (mbHotFixEnable) {
					BeginCheckUpdateResource ();
				} else {
					SendNotification (NotificationType.M2M_ResourceUpdateOver);
				}

				break;

			case NotificationType.M2M_ResourceUpdateOver:
				View.setState (LaunchPage.LaunchPageState.UpdateOver);
				Libs.AM.I.ClearABDic ();
				BeginLoadLoadingScene ();
				break;
            }
        }

		void BeginLoadLoadingScene()
		{
			CoroutineController.Instance.StartCoroutine (doloadscene ());
		}

		IEnumerator doloadscene()
		{
			yield return new WaitForSeconds (1.0f);
			SceneManager.LoadScene (1);
		}

        void BeginCheckUpdateResource()
        {
            //热更新url
			AssetsUpdateManager.assetsSeverUrl =mstrRemoveUrl;// "http://127.0.0.1";
            //保存路径
			string slocalfile="";
			#if UNITY_EDITOR
			slocalfile=Application.dataPath;
			AssetsUpdateManager.assetsUpdatePath = slocalfile + "/StreamingAssetsUpdate";
			#else
			slocalfile=Application.persistentDataPath;
			AssetsUpdateManager.assetsUpdatePath = Application.persistentDataPath;
			#endif

			CheckVersions (mstrRemoteMD5Url);// "file:///D:/StreamingAssets/md5filelist.txt");

        }

		void BeginUpdateResource()
		{
			AssetsUpdateManager.getInstance ().Check 
				(mstrRemoteMD5Url,// "file:///D:/StreamingAssets/md5filelist.txt",
				OnAssetsUpdateCmp,
				OnAssetsUpdateProgress
			);
		}

        void CheckVersions(string url)
        {
			AssetsUpdateManager.getInstance ().StartCheck 
			(
				url,// "file:///D:/StreamingAssets/md5filelist.txt",
				(string[]arrayUpdatePath) => 
				{
					if(arrayUpdatePath!=null&&arrayUpdatePath.Length>0)
					{
						Facade.SendNotification(NotificationType.NetWorkCheck);		
					}
					else
					{
//						Facade.SendNotification(NotificationType.V2V_BeginUpdateResource);
						Facade.SendNotification(NotificationType.M2M_ResourceUpdateOver);
					}
				}
			);
        }

        void OnAssetsUpdateProgress(DownLoadBatch dlb)
        {
            Debug.Log("DownLoad Progress...");
			if (dlb != null) 
			{
				UnityEngine.UI.Slider sl = View.getCurrentProgressControll ();
				if (sl != null)
					sl.value = (float)dlb.progress;
			}
//			else 
//			{
//				
//			}
        }

        void OnAssetsUpdateCmp()
        {
            View.setState(LaunchPage.LaunchPageState.UpdateOver);

            Loom.QueueOnMainThread(() =>
            {
            	    Debug.LogWarning("更新成功！");
				
					Facade.SendNotification(NotificationType.M2M_ResourceUpdateOver);
                /* 跳转到 loading */
                //asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            });

        }






        public override IList<string> ListNotificationInterests()
        {
            List<string> lstcon = new List<string>();

            lstcon.Add(NotificationType.V2V_BeginUpdateResource);

			lstcon.Add(NotificationType.V2M_BeginCheckResource);

			lstcon.Add(NotificationType.M2M_ResourceUpdateOver);

            return lstcon;
        }

    }

}


