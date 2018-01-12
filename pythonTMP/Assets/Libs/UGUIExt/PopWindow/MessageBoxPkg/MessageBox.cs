using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Timers;
using System.Collections.Generic;
using ZhuYuU3d.Game;
using Libs;

namespace ZhuYuU3d
{


public class MessageBox:MonoBehaviour {

    public enum Type
    {
        ERROR,
        WARNING,
        MESSAGE
    }

    public enum Result
    {
        OK,
        CANCEL,
        YES,
        NO,
        DISMISSED
    }

    public static bool isActive { get; private set; }

    static string title;
    static string message;
    static GameObject messageBoxCanvas;
    static Action<Result> resultCallback;

//        [SerializeField]
        GameObject Button1, Button2, Button3;

//        [SerializeField]
        Text mtxtTitle, mtxtContent;



		void Awake()
		{
			Button1 = transform.Find ("Button1").gameObject;
			Button2 = transform.Find ("Button2").gameObject;
			Button3 = transform.Find ("Button3").gameObject;

			mtxtTitle = transform.Find ("MBTitle").GetComponent<Text> ();
			mtxtContent = transform.Find ("MBMessage").GetComponent<Text> ();
		}

    /// <summary>
    /// Displays a MessageBox with an OK button.
    /// </summary>
    /// <param name="title">The messageBox title.</param>
    /// <param name="message">The content of the massage.</param>
    /// <param name="type">The type of MessageBox this is.</param>
    /// <param name="callback">The method to call when the user clicks a button.</param>
    public static void ShowOK(string title, string message, MessageBox.Type type, Action<Result> callback)
    {
        if (isActive)
            return;

		var prefab = ResourceLoader.Instance.LoadInstanceAsset("MessageBoxPrefab",(UnityEngine.Object objins)=> 
        {
            if (objins == null)
            {
                Debug.LogAssertion("Prefab missing!");
            }
            messageBoxCanvas = (GameObject)objins;
            MessageBox MBInstance = messageBoxCanvas.GetComponent<MessageBox>();
            if (MBInstance != null)
            {
                MBInstance.ShowOK(title, message, callback);
            }
        },
        LoadResourceWay.FromAssetbundle
        );

        //if (prefab == null)
        //{
        //    Debug.LogAssertion("Prefab missing!");
        //    return;
        //}
        //messageBoxCanvas = (GameObject)prefab;
        //MessageBox  MBInstance=messageBoxCanvas.GetComponent<MessageBox>();
        //    if (MBInstance != null)
        //    {
        //        MBInstance.ShowOK(title,message,callback);
        //    }


      
    }

        public void ShowOK(string title, string message, Action<Result> callback,string sokDesc="ok")
        {
            Button2.SetActive(false);
            Button3.SetActive(false);
            mtxtTitle.text = title;
            mtxtContent.text = message;
            resultCallback = callback;
            Button1.GetComponent<Button>().onClick.AddListener(() => OKClicked());
            Button1.GetComponentInChildren<Text>().text = sokDesc;
            isActive = true;
        }



    /// <summary>
    /// Displays a MessageBox with an OK button.
    /// </summary>
    /// <param name="title">The messageBox title.</param>
    /// <param name="message">The content of the massage.</param>
    /// <param name="type">The type of MessageBox this is.</param>
    /// <param name="callback">The method to call when the user clicks a button.</param>
    public static void ShowOK(string title, string message, MessageBox.Type type, Action<Result> callback, string okText)
    {
        ShowOK(title, message, type, callback,okText);
        
    }

    /// <summary>
    /// Displays a MessageBox with an OKAY and CANCEL button.
    /// </summary>
    /// <param name="title">The messageBox title.</param>
    /// <param name="message">The content of the massage.</param>
    /// <param name="type">The type of MessageBox this is.</param>
    /// <param name="callback">The method to call when the user clicks a button.</param>
    public static void ShowOKCancel(string title, string message, MessageBox.Type type, Action<Result> callback)
    {
        if (isActive)
            return;
		

        var prefab = ResourceLoader.Instance.LoadInstanceAsset("MessageBoxPrefab",
            (UnityEngine.Object objins) =>
            {
                if (objins == null)
                {
                    Debug.LogAssertion("Prefab missing!");
                }
                messageBoxCanvas = (GameObject)objins;
                MessageBox MBInstance = messageBoxCanvas.GetComponent<MessageBox>();
                if (MBInstance != null)
                {
                    MBInstance.ShowOKCancel(title, message, callback);
                }
            },LoadResourceWay.FromAssetbundle
        );
        //if (prefab == null)
        //{
        //    Debug.LogAssertion("Prefab missing!");
        //    return;
        //}
        //messageBoxCanvas = (GameObject)UnityEngine.Object.Instantiate(prefab);
        //GameObject.Find("Button1").GetComponentInChildren<Text>().text = "Cancel";
        //GameObject.Find("Button1").GetComponent<Button>().onClick.AddListener(() => CancelClicked());
        //GameObject.Find("Button2").GetComponentInChildren<Text>().text = "OK";
        //GameObject.Find("Button2").GetComponent<Button>().onClick.AddListener(() => OKClicked());
        //GameObject.Find("Button3").SetActive(false);
        //GameObject.Find("MBTitle").GetComponent<Text>().text = title;
        //GameObject.Find("MBMessage").GetComponent<Text>().text = message;
        //resultCallback = callback;
        //isActive = true;
    }

        void ShowOKCancel(string title, string message,Action<Result> callback)
        {
           // messageBoxCanvas = (GameObject)UnityEngine.Object.Instantiate(prefab);
			messageBoxCanvas = this.gameObject;

            Button1.GetComponentInChildren<Text>().text = "Cancel";
            Button1.GetComponent<Button>().onClick.AddListener(() => CancelClicked());
            Button2.GetComponentInChildren<Text>().text = "OK";
            Button2.GetComponent<Button>().onClick.AddListener(() => OKClicked());
            Button3.SetActive(false);
            mtxtTitle.GetComponent<Text>().text = title;
            mtxtContent.GetComponent<Text>().text = message;
            resultCallback = callback;
            isActive = true;
        }

    
    /// <summary>
    /// Displays a MessageBox with a YES and NO button.
    /// </summary>
    /// <param name="title">The messageBox title.</param>
    /// <param name="message">The content of the massage.</param>
    /// <param name="type">The type of MessageBox this is.</param>
    /// <param name="callback">The method to call when the user clicks a button.</param>
    public static void ShowYesNo(string title, string message, MessageBox.Type type, Action<Result> callback)
    {
        if (isActive)
            return;

            string strFileName = "/ui/MessageBoxPrefab.panel";

            ABLoaderHelper.Instance.LoadAB
                    (
                    strFileName,
                    null,
                    "MessageBoxPrefab",
                    (GameObject objins) =>
                    {
                        if (objins == null)
                        {
                            Debug.LogAssertion("Prefab missing!");
                        }
                        messageBoxCanvas = (GameObject)objins;
                        MessageBox MBInstance = messageBoxCanvas.GetComponent<MessageBox>();
                        if (MBInstance != null)
                        {
                            MBInstance.ShowYesNo(title, message, callback);
                        }
                    }
                    );


                 //       var prefab = assetbuhelp l("",
                 //(UnityEngine.Object objins) =>
                 //{
                     
                 //},
                 //LoadResourceWay.FromAssetbundle
             //);
      
    }

        public void ShowYesNo(string title, string message,  Action<Result> callback)
        {
            Button1.GetComponentInChildren<Text>().text = "Yes";
            Button1.GetComponent<Button>().onClick.AddListener(() => YesClicked());
            Button2.GetComponentInChildren<Text>().text = "No";
            Button2.GetComponent<Button>().onClick.AddListener(() => NoClicked());
            Button3.SetActive(false);
            mtxtTitle.GetComponent<Text>().text = title;
            mtxtContent.GetComponent<Text>().text = message;
            resultCallback = callback;
            isActive = true;
        }

    private void NoClicked()
    {
        isActive = false;
		if (messageBoxCanvas != null) {

			messageBoxCanvas.SetActive (false);
			GameObject.Destroy (messageBoxCanvas);
			messageBoxCanvas = null;
		}

        if(resultCallback!=null)
            resultCallback(Result.NO);
    }

    private void YesClicked()
    {
        isActive = false;
			if (messageBoxCanvas != null) {
				messageBoxCanvas.SetActive (false);
				GameObject.Destroy (messageBoxCanvas);
				messageBoxCanvas = null;

			}
        if (resultCallback != null)
            resultCallback(Result.YES);
    }

    private void OKClicked()
    {
        isActive = false;
		if (messageBoxCanvas != null) 
		{
			messageBoxCanvas.SetActive (false);
			GameObject.Destroy (messageBoxCanvas);
				messageBoxCanvas = null;
		}
        if (resultCallback != null)
            resultCallback(Result.OK);
    }

    private void CancelClicked()
    {
        isActive = false;
		if (messageBoxCanvas != null) {
			messageBoxCanvas.SetActive (false);
			GameObject.Destroy (messageBoxCanvas);
				messageBoxCanvas = null;

		}
        if (resultCallback != null)
            resultCallback(Result.CANCEL);
    }

    /// <summary>
    /// Static method to dismiss any active MessageBox in the scene.
    /// </summary>
    /// <returns>A boolean, TRUE if a MessageBox was indeed dismissed, FALSE if there was no active MessageBox.</returns>
    public bool Dismiss()
    {
        if (isActive)
        {
            isActive = false;
				if (messageBoxCanvas != null) {
					messageBoxCanvas.SetActive (false);
					GameObject.Destroy (messageBoxCanvas);
					messageBoxCanvas = null;

				}
            if (resultCallback != null)
                resultCallback(Result.DISMISSED);
            return true;
        }
        else
            return false;
    }

  }

}
