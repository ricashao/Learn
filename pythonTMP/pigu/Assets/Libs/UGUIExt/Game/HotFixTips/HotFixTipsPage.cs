using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using ZhuYuU3d;
using UnityEngine.UI;

namespace ZhuYuU3d.Game
{
    public static partial class GobalTool
    {
        static public T FindComponentByPath<T>(Transform tp, string strPath)
        {
            Transform transChild = tp.Find(strPath);
            if (transChild != null)
            {
                return transChild.GetComponent<T>();
            }
            return default(T);
        }


    }

    public class HotFixTipsPage : MonoBehaviour
    {
        HotFixTipsMeditor mfm = null;

        Text mtxtVersion;

        Text mtxtUpdateContent;


	    void Start ()
        {

            mfm = new HotFixTipsMeditor(this);

            Facade.Instance.RegisterMediator(mfm);

            mtxtVersion = GobalTool.FindComponentByPath<Text>(transform, "content/HotFixTipsContent/txt_title");

            mtxtUpdateContent = GobalTool.FindComponentByPath<Text>(transform, "content/HotFixTipsContent/bg/txt_infotitle");

             Button btnClose= GobalTool.FindComponentByPath<Button>(transform, "btn_Close");
            if (btnClose != null)
            {
                btnClose.onClick.AddListener(() =>
                {
                    Debug.Log("Close Click");
                    CloseUI();
                });
            }

            Button btnCancel = GobalTool.FindComponentByPath<Button>(transform, "btn_NO");
            if (btnCancel != null)
            {
                btnCancel.onClick.AddListener(() =>
                {
                    Debug.Log("No Click");
                    CloseUI();

                });
            }

            Button btnConfirm = GobalTool.FindComponentByPath<Button>(transform, "btn_OK");
            if (btnConfirm != null)
            {
                btnConfirm.onClick.AddListener(() =>
                {
                    Debug.Log("Yes Click");
                    Facade.Instance.SendNotification(NotificationType.V2V_BeginUpdateResource);
                    CloseUI();
                });
            }

        }

        void CloseUI()
        {
            mfm = null;
            GameObject.Destroy(this.gameObject);
        }

        public void setDstVersion(string s)
        {
            string sv=mtxtVersion.text;
            sv=string.Format(sv, s);
            mtxtVersion.text = sv;
        }

        public void setUpdateContent(string s)
        {
            mtxtUpdateContent.text=s;
        }
        
    }

}