using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhuYuU3d;
using XLua;

namespace ZhuYuU3d{

	public class Loading : MonoBehaviour {

		LuaEnv env;
		LuaManager luaManager;

		void Awake(){
			luaManager = LuaManager.GetInstance ();
			luaManager.InitDoString = "require 'lua/game/GameState' \n game_state_init() \n";
			env = luaManager.LuaEnvGetOrNew ();
		}

		static bool isInit = true;

		// Use this for initialization
		void Start () {
			/* 执行 */ 
			RunState ();
		}

		void Init(){
			if (isInit) {
				Libs.AssetManager.getInstance ().InitAssetName2abPathDic ("StreamingAssets_u3d_xlua_project");
				isInit = false;
				Debug.LogWarning ("初始化完成！");
			} else {
				Debug.LogWarning ("已经初始化！");
			}
		}

		void RunState(){

			Init ();

			env.DoString ("game_state_loading()");
			luaManager.SetLuaUpdate ("game_state_update");
		}

		// Update is called once per frame
		//void Update () {

		//}

		void OnDestroy()
		{
			env = null;
			luaManager = null;
		}
	}

}