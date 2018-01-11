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

            string strFileName = "/ui/hotfixpanel";

            GameObject goCanvas = GameObject.Find("Canvas");

            ABLoaderHelper.Instance.LoadAB(strFileName, goCanvas, "HotFixPanel", (GameObject go) =>
            {
                if (go != null)
                {
                    go.AddComponent<HotFixTipsPage>();
                }
            }
            );


        }

    }
}