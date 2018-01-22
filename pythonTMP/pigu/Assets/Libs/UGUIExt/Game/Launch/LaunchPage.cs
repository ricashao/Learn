using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using ZhuYuU3d;
using UnityEngine.UI;

namespace ZhuYuU3d.Game
{ 
    public class LaunchPage : MonoBehaviour
    {
        public enum LaunchPageState
        {
            Init,
            UpdateResource,
            UpdateOver,
        }

        LaunchPageState mps = LaunchPageState.Init;

        LaunchMeditor mlm = null;

        T FindComponentByPath<T>(string strPath)
        {
            Transform transChild=transform.Find(strPath);
            if(transChild!=null)
            {
                return transChild.GetComponent<T>();
            }
            return default(T);
        }

	    void Start ()
        {

            mlm = new LaunchMeditor(this);
            Facade.Instance.RegisterMediator(mlm);

            

            msliProgress=FindComponentByPath<Slider>("slider_Progress");
            EnableProgress(false);

            mtxtVersion = FindComponentByPath<Text>("txt_version");

            mtxtTips = FindComponentByPath<Text>("txt_Tips");

			Facade.Instance.SendNotification (NotificationType.V2M_BeginCheckResource);

        }

        Slider msliProgress;

        Text mtxtVersion,mtxtTips;

        public void EnableProgress(bool b)
        {
            msliProgress.gameObject.SetActive(b);
        }

        public Slider getCurrentProgressControll()
        {
            return msliProgress;
        }

        public void setTxtVersion(string s)
        {
            mtxtVersion.text = s;
        }

        public void setTxtTips(string s)
        {
            mtxtTips.text = s;
        }

        public void setState(LaunchPageState lps)
        {
            mps = lps;
        }

        private void Update()
        {
            switch(mps)
            {
			case LaunchPageState.Init:
					setTxtTips ("正在检查资源....");
                    EnableProgress(false);
                    break;
                case LaunchPageState.UpdateResource:
					setTxtTips ("正在更新资源....");

                    EnableProgress(true);
                    //msliProgress.value=
                    break;
                case LaunchPageState.UpdateOver:
					setTxtTips ("资源更新结束.");
                    break;
            }
        }




    }

}