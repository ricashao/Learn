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

    public bool isActive { get; private set; }

    string title;
    string message;
    GameObject messageBoxCanvas;
    Action<Result> resultCallback;

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

			if(resultCallback!=null)
				resultCallback(Result.NO);
			
//		if (messageBoxCanvas != null) {
			gameObject.SetActive (false);
			GameObject.Destroy (gameObject);

//		}

        
    }

    private void YesClicked()
    {
        isActive = false;

			if (resultCallback != null)
            resultCallback(Result.YES);

			gameObject.SetActive (false);
			GameObject.Destroy (gameObject);
    }

    private void OKClicked()
    {
        isActive = false;
		
        if (resultCallback != null)
            resultCallback(Result.OK);

			gameObject.SetActive (false);
			GameObject.Destroy (gameObject);
    }

    private void CancelClicked()
    {
        isActive = false;
		
        if (resultCallback != null)
            resultCallback(Result.CANCEL);

			gameObject.SetActive (false);
			GameObject.Destroy (gameObject);
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

            if (resultCallback != null)
                resultCallback(Result.DISMISSED);

				gameObject.SetActive (false);
				GameObject.Destroy (gameObject);
            return true;
        }
        else
            return false;
    }

  }

}
