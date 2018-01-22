using UnityEngine;
using System.Collections;
using System;
using XLua;

namespace ZhuYuU3d.Platform
{
	[LuaCallCSharp]
    public class CallManager : DDOLSingleton<CallManager>
    {
        BaseCall mInsCall = null;

        void getIns()
        {
#if UNITY_ANDROID
            mInsCall = AndroidCall.AndroidCallIns;
#elif UNITY_IOS
            mInsCall = IOSCall.IOSCallIns;
#endif
        }

        void Start()
        {
			
            getIns();
        }

//#if UNITY_IOS
//        public bool IsChineseSimplifiedIOS()
//        {
//            if (mInsCall != null)
//            {
//                IOSCall iOSCall = mInsCall as IOSCall;
//                if (iOSCall != null)
//                    return iOSCall.IsChineseSimplifiedIOS();
//            }
//            return true;
//        }
//#endif

		static public string GetDeviceUnqueID()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}


    }
}