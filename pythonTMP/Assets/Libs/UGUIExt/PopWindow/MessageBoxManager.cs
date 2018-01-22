using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace ZhuYuU3d
{


[LuaCallCSharp]
public class MessageBoxManager : DDOLSingleton<MessageBoxManager>
{

	// Use this for initialization
	void Start () {
		
	}

	public void Load(string sName,LuaFunction onLoadSuccessCallback,LuaFunction onLoadFailedCallback)
	{
		Libs.AM.I.CreateFromCache (sName,(string assetName, Object objInstantiateTp)=>
			{
				try
				{
					GameObject objInstantiate =(GameObject)Instantiate((GameObject)objInstantiateTp);
					objInstantiate.name = objInstantiate.name.Replace("(Clone)","");
					if(onLoadSuccessCallback!=null&&objInstantiate!=null)
					{
						onLoadSuccessCallback.Call(new object[]{objInstantiate},new System.Type[]{typeof(GameObject)});
					}
				}
				catch(System.Exception e)
				{
						if(onLoadFailedCallback!=null&&objInstantiateTp==null)
						{
							onLoadFailedCallback.Call();
						}
				}

			});
	}



}
}
