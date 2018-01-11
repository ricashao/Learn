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
                //case NetworkUseType.WiFI:

                    Toast.Show(CoroutineController.Instance, "您当前未连接至网络！", 4, Toast.Type.WARNING, Toast.Gravity.CENTER, 30);

                    break;

                case NetworkUseType.MobileNet:
                
					MessageBox.ShowYesNo("Tips", "当前的网络环境不是WiFI，可能产生费用，是否更新！", MessageBox.Type.MESSAGE, (MessageBox.Result res) =>
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
