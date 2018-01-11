using UnityEngine;
using System.Collections;
using System;
namespace ZhuYuU3d.Platform
{
    public class AndroidCall : BaseCall
    {
#if UNITY_ANDROID
        private const string ANDROID_ACTIVITY_NAME = "";
#endif
        static AndroidCall androidCallIns;
        static public AndroidCall AndroidCallIns
        {
            get
            {
                if (androidCallIns == null)
                {
                    GameObject goIns = new GameObject();
                    goIns.name = "AndroidCall";
                    androidCallIns = goIns.AddComponent<AndroidCall>();
                    DontDestroyOnLoad(goIns);
                }
                return androidCallIns;
            }
        }

        //执行java调用
        private void DoJavaCall(string strJavaFunName, params object[] args)
        {

#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaClass jc = new AndroidJavaClass(ANDROID_ACTIVITY_NAME))
			{
			using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("unityWrapperHandler"))
				{
					jo.Call(strJavaFunName, args);
				}
			}
#endif
        }

        //执行java调用
        private T DoJavaCall<T>(string strJavaFunName, params object[] args)
        {
            T t = default(T);
#if UNITY_ANDROID && !UNITY_EDITOR
			using (AndroidJavaClass jc = new AndroidJavaClass(ANDROID_ACTIVITY_NAME))
			{
			using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("unityWrapperHandler"))
				{
					t = jo.Call<T>(strJavaFunName, args);
				}
			}
#endif
            return t;
        }



        

    }
}