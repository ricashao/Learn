using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using ZhuYuU3d;

namespace ZhuYuU3d.Game
{
    public enum NetworkUseType
    {
        NoUse,
        WiFI,
        MobileNet,
    }
    public class NetworkCheckCommand : SimpleCommand
    {
        public override void Execute(INotification notification)
        {
            Debug.Log("Network Check Command");

            NetworkUseType mut = getUseNetworkType();

            Debug.Log("Cur State:" + mut.ToString());

            switch (mut)
            {
                case NetworkUseType.NoUse:

				ABLoaderHelper.Instance.LoadAB
				(
					"ui/toastpanel", GameObject.Find("Canvas") , "ToastPanel", (GameObject go) =>
					{
						Toast tips=go.AddComponent<Toast>();

						tips.Show(CoroutineController.Instance,"您当前未连接至网络！", 2, Toast.Type.WARNING, Toast.Gravity.CENTER, 30);
					}
				);


//				Libs.AM.I.CreateFromCache ("ToastPanel", (string assetName,UnityEngine.Object objInstantiateTp)=>
//					{
//						GameObject objInstantiate =(GameObject)GameObject.Instantiate((GameObject)objInstantiateTp);
//						objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
//
//						objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);
//
//						if (objInstantiate != null)
//						{
//							Toast tips=objInstantiate.AddComponent<Toast>();
//
//							tips.Show(CoroutineController.Instance,"您当前未连接至网络！", 2, Toast.Type.WARNING, Toast.Gravity.CENTER, 30);
//						}
//					}
//				);
//
                    break;

                case NetworkUseType.MobileNet:

//				Libs.AM.I.CreateFromCache ("MessageBoxPanel", (string assetName,UnityEngine.Object objInstantiateTp)=>
//					{
//						GameObject objInstantiate =(GameObject)GameObject.Instantiate((GameObject)objInstantiateTp);
//						objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
//
//						objInstantiate.transform.SetParent(GameObject.Find("Canvas").transform,false);
//
//						if (objInstantiate != null)
//						{
//							MessageBox tips=objInstantiate.AddComponent<MessageBox>();
//
//							tips.ShowYesNo("Tips", "当前的网络环境不是WiFI，可能产生费用，是否更新！",  (MessageBox.Result res) =>
//								{
//									Debug.Log("Click Res:" + res.ToString());
//									if (res == MessageBox.Result.YES)
//									{
//										Facade.SendNotification(NotificationType.LoadHotFixTipsUI);
//									}
//									else
//									{
//										Application.Quit();
//									}
//								}
//							);
//						}
//					}
//				);

				ABLoaderHelper.Instance.LoadAB
				(
					"ui/messageboxpanel", GameObject.Find("Canvas") , "MessageBoxPanel", (GameObject go) =>
					{
						MessageBox tips=go.AddComponent<MessageBox>();

						tips.ShowYesNo("Tips", "当前的网络环境不是WiFI，可能产生费用，是否更新！",  (MessageBox.Result res) =>
							{
								Debug.Log("Click Res:" + res.ToString());
								if (res == MessageBox.Result.YES)
								{
									Facade.SendNotification(NotificationType.LoadHotFixTipsUI);
								}
								else
								{
									Application.Quit();
								}
							}
						);
					}
				);

                break;

                case NetworkUseType.WiFI:


                   Facade.SendNotification(NotificationType.LoadHotFixTipsUI);
                   break;
            }


        }

        public  static NetworkUseType getUseNetworkType()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return NetworkUseType.NoUse;
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                return NetworkUseType.MobileNet;
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                return NetworkUseType.WiFI;

            return NetworkUseType.NoUse;
        }


    }
}
