using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using ZhuYuU3d;

public class TestLuaSetData : MonoBehaviour {

	// Use this for initialization
	void Start () {

		LuaEnv luaEnv = LuaManager.GetInstance ().LuaEnvGetOrNew ();
		UILoopList loopList = GameObject.Find ("Canvas/Scroll View/Viewport/Content").GetComponent("UILoopList") as UILoopList;
		luaEnv.DoString(@"
						loopList = CS.UnityEngine.GameObject.Find (""Canvas/Scroll View/Viewport/Content""):GetComponent(""UILoopList"")
						loopListData = { { id = 1 ,name ='name1' },{ id = 2 ,name ='name2' } }
						loopList:Data(loopListData)
						");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
