using System;
using PureMVC.Patterns;
using UnityEngine;
using Libs;
using System.IO;

namespace ZhuYuU3d.Game
{
    public class HotFixTipsCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            base.Execute(notification);

			string strFileName = "ui/hotfixpanel";

//            GameObject goCanvas = GameObject.Find("Canvas");

//			Libs.AM.I.CreateFromCache (strFileName, (string assetName,UnityEngine.Object objInstantiateTp)=>
//				{
//					GameObject objInstantiate =(GameObject)GameObject.Instantiate((GameObject)objInstantiateTp);
//					objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
//
//					objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);
//
//					if (objInstantiate != null)
//					{
//						objInstantiate.AddComponent<HotFixTipsPage>();
//					}
//				}
//			);
//

			ABLoaderHelper.Instance.LoadAB
			(
				strFileName, GameObject.Find("Canvas") , "HotFixPanel", (GameObject go) =>
				{
					go.AddComponent<HotFixTipsPage>();
				}
			);

//            ABLoaderHelper.Instance.LoadAB(strFileName, goCanvas, "HotFixPanel", (GameObject go) =>
//            {
//                if (go != null)
//                {
//                    go.AddComponent<HotFixTipsPage>();
//                }
//            }
//            );


        }

    }
}