using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

namespace ZhuYuU3d.Platform
{
    public class IOSCall : BaseCall
    {
        static IOSCall iosCallIns;
        static public IOSCall IOSCallIns
        {
            get
            {
                if (iosCallIns == null)
                {
                    GameObject goIns = new GameObject();
                    goIns.name = "iOSCall";
                    iosCallIns = goIns.AddComponent<IOSCall>();
                }
                return iosCallIns;

            }

        }



        

//		[DllImport("__Internal")]
//		private static extern void OnSetVideoParentPath(string sVideoPath);
//
//		public override void SetVideoPath(string sVideoPath)
//		{
//			#if UNITY_IOS && !UNITY_EDITOR
//				OnSetVideoParentPath(sVideoPath);
//			#endif
//
//		}
//

    }
}
