 
using PureMVC.Patterns;
using UnityEngine;

namespace ZhuYuU3d.Game
{
	public class ApplicationFacade : Facade
    {

        public ApplicationFacade()
        {
            
        }

		public void StartUp(string sconfigpath=null)
		{
			RegisterCommand (NotificationType.ReadUpdateInfo, new AssetUpdateInfoCommand (AssetUpdateInfoCommand.UpdateConfigLoadWay.LoadFromResource,
				sconfigpath)
			);

			SendNotification(NotificationType.ReadUpdateInfo);
		}

	    public void ShutDown()
	    {

	    }

		protected override void InitializeController ()
		{

            base.InitializeController ();

            RegisterCommand(NotificationType.LoadLaunchUI,new LaunchCommand());

            RegisterCommand(NotificationType.NetWorkCheck, new NetworkCheckCommand());

            RegisterCommand(NotificationType.LoadHotFixTipsUI, new HotFixTipsCommand());



        }
    }
}