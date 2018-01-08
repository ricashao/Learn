using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public class LuaUpdateBehaviour :LuaBaseBehaviour {

		private Action luaUpdate;

		protected override void Awake(){

			base.Awake ();

			scriptEnv.Get("update", out luaUpdate);
		}
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
			if (luaUpdate != null)
			{
				luaUpdate();
			}
		}

		void OnDestroy()
		{
			base.OnDestroy ();

			luaUpdate = null;
		}
	}
}