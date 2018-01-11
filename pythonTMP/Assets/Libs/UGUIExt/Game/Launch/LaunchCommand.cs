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

            string strFileName = "/ui/launchpanel.panel";

            //string strLoadPath =Application.persistentDataPath + strFileName;

            //if (File.Exists(strLoadPath))
            //{
            //    AssetBundleManagar.getInstance().LoadOne("ui/launchpanel.panel", (string path, AssetBundle assetBundle) =>
            //    {
            //        Debug.Log("Load Over");
            //        LoadAssetFromAB(assetBundle);
            //    }
            //    );
            //}
            //else
            //{
            //    strLoadPath = Application.streamingAssetsPath + strFileName;
            //    if(File.Exists(strLoadPath))
            //    {
            //        AssetBundle abLaunch = AssetBundle.LoadFromFile(strLoadPath);
            //        if (abLaunch != null)
            //        {
            //            LoadAssetFromAB(abLaunch);
            //            abLaunch.Unload(false);
            //        }
            //    }

            //}
            GameObject goCanvas = GameObject.Find("Canvas");
            ABLoaderHelper.Instance.LoadAB(strFileName, goCanvas, "LaunchPanel", (GameObject go) =>
               {
                   if (go != null)
                   {
                       go.AddComponent<LaunchPage>();
                   }
               }
            );

            
        }

    }
}