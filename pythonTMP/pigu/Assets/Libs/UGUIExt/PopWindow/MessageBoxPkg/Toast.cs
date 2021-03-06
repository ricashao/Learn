﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Timers;
using System.Collections.Generic;
using ZhuYuU3d.Game;
using Libs;

namespace ZhuYuU3d
{
	public class Toast:MonoBehaviour
	{

    public enum Type
    {
        ERROR,
        WARNING,
        MESSAGE
    }

    public enum Gravity
    {
        BOTTOM,
        CENTER,
        TOP,
        TOP_RIGHT,
        BOTTOM_RIGHT
    }

    struct ToastHolder
    {
        public MonoBehaviour toastContext;
        public string toastMessage;
        public int toastDuration;
        public Type toastType;
        public Gravity toastGravity;
        public int toastSize;

        public ToastHolder(MonoBehaviour mContext, string mMessage, int mDuration, Type mType, int mSize, Gravity mGravity)
        {
            toastContext = mContext;
            toastMessage = mMessage;
            toastDuration = mDuration;
            toastType = mType;
            toastSize = mSize;
            toastGravity = mGravity;
        }
    } 

    static public bool isActive { get; private set; }
    
    GameObject toastCanvas;
    MonoBehaviour ctxt;
    int durationSecs;
    static private Queue<ToastHolder> toastQueue = new Queue<ToastHolder>();
    private Coroutine currentTimer;
    private readonly int DEFAULT_SIZE = 20;

		public static void StartShow(MonoBehaviour caller, string message, int duration, Type type, Gravity gravity,int size)
		{
			if (isActive)
			{
				//Debug.Log("ENQUEUED ONE, GRAVITY DEFAULT");
				toastQueue.Enqueue(new ToastHolder(caller, message, duration, type, size, gravity));
				return;
			}
			else
			{
				Libs.AM.I.CreateFromCache ("ToastPanel", (string assetName,UnityEngine.Object objInstantiateTp)=>
					{
						GameObject objInstantiate =(GameObject)GameObject.Instantiate((GameObject)objInstantiateTp);
						objInstantiate.name = objInstantiate.name.Replace("(Clone)","");

						objInstantiate.transform.SetParent(GameObject.Find("PopupCanvas").transform,false);

						if (objInstantiate != null)
						{
							Toast tips=objInstantiate.AddComponent<Toast>();

							tips.Show (CoroutineController.Instance, message, duration, ZhuYuU3d.Toast.Type.WARNING, ZhuYuU3d.Toast.Gravity.CENTER,size);
						}
					}
				);

			}
		}

    public void Show(MonoBehaviour caller, string message, int duration, Type type, Gravity gravity,int size)
    {
        
                        toastCanvas = gameObject;

                        //Get the text within the toast and set it.
                        toastCanvas.GetComponentInChildren<Text>().text = message;
                        toastCanvas.GetComponentInChildren<Text>().fontSize = DEFAULT_SIZE;       //Default font size is 20!

                        //Get the toast type and set the mood accordingly.
                        switch (type)
                        {
                            case Type.ERROR:
                                toastCanvas.GetComponentInChildren<Image>().color = new Color(1f, 0.011f, 0.011f);
                                break;
                            case Type.WARNING:
                                toastCanvas.GetComponentInChildren<Image>().color = new Color(1f, 0.53125f, 0.03125f);
                                break;
                            case Type.MESSAGE:
                                toastCanvas.GetComponentInChildren<Image>().color = new Color(0.95f, 0.95f, 0.95f);
                                break;
                        }

                        isActive = true;
                        durationSecs = duration;
                        ctxt = caller;
                        currentTimer = ctxt.StartCoroutine(DestroyToast());

                        toastCanvas.GetComponentInChildren<Text>().fontSize = size;
                        //Play the animation
                        string animString = getGravityString(gravity);
                        AnimationClip anim = Resources.Load<AnimationClip>(animString);
                        anim.legacy = true;
                        toastCanvas.GetComponentInChildren<Animation>().Stop();
                        toastCanvas.GetComponent<Animation>().AddClip(anim, animString);
                        toastCanvas.GetComponentInChildren<Animation>().clip = anim;
                        toastCanvas.GetComponentInChildren<Animation>().Play();

    }



    IEnumerator DestroyToast()
    {
        yield return new WaitForSeconds(durationSecs);
		toastCanvas.SetActive (false);

         //GameObject.Destroy(toastCanvas);
        //Debug.Log("DELETED ONE");
        
			if (toastQueue.Count > 0) {
				ToastHolder topToast = toastQueue.Dequeue ();
           
				Show (topToast.toastContext, topToast.toastMessage, topToast.toastDuration, topToast.toastType, topToast.toastGravity, topToast.toastSize);
			} else {
				GameObject.Destroy (toastCanvas);
				isActive = false;
			}
    }

    private string getGravityString(Gravity gravity)
    {
        switch(gravity)
        {
            case Gravity.BOTTOM:
                return "Bottom";
            case Gravity.BOTTOM_RIGHT:
                return "BottomRight";
            case Gravity.CENTER:
                return "Middle";
            case Gravity.TOP:
                return "Top";
            case Gravity.TOP_RIGHT:
                return "TopRight";
        }
        return "Bottom";    //If something bad/wrong happens, just show the toast from the bottom.
    }


    /// <summary>
    /// Dismisses the current toast and dumps the entire toast queue
    /// </summary>
    /// <returns>A boolean, TRUE if a Toast was dismissed, FALSE if there was no active Toast to dismiss.</returns>
//    public bool Dismiss()
//    {
//        if (isActive)
//        {
//            isActive = false;
//            GameObject.Destroy(toastCanvas);
//            ctxt.StopCoroutine(currentTimer);
//            toastQueue.Clear();
//            return true;
//        }
//        else
//            return false;
//    }
//
//
//    /// <summary>
//    /// Dismisses the current toast being displayed, if any, and activates the next toast in the queue, if any.
//    /// </summary>
//    /// <returns>A boolean, TRUE if there was another toast to show and a toast to dismiss, FALSE if there is no toast to show or no toast to dismiss.</returns>
//    public bool DismissNext()
//    {
//        if (isActive)
//        {
//            isActive = false;
//            GameObject.Destroy(toastCanvas);
//            ctxt.StopCoroutine(currentTimer);
//            if (toastQueue.Count>0)
//            {
//                ToastHolder topToast = toastQueue.Dequeue();
//                //if (topToast.toastSize > 0)
//                //    Toast.Show(topToast.toastContext, topToast.toastMessage, topToast.toastDuration, topToast.toastType, topToast.toastSize);
//                //else
//                //    Toast.Show(topToast.toastContext, topToast.toastMessage, topToast.toastDuration, topToast.toastType);
//                Toast.Show(topToast.toastContext, topToast.toastMessage, topToast.toastDuration, topToast.toastType, topToast.toastGravity, topToast.toastSize);
//                return true;
//            }
//            return false;
//        }
//        else
//            return false;
//    }
}
}