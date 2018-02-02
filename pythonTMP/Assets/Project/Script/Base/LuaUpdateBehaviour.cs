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
		private Action luaLateUpdate;

		public override void Init(){

			base.Init ();

			scriptEnv.Get("update", out luaUpdate);
			scriptEnv.Get("lateUpdate", out luaLateUpdate);
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

		// Update is called once per frame
		void LateUpdate () {

			if (luaLateUpdate != null)
			{
				luaLateUpdate();
			}
		}

		void OnDestroy()
		{
			base.OnDestroy ();

			luaUpdate = null;
		}
	}
}