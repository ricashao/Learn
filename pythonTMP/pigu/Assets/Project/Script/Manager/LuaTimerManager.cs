using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using Libs;
using ZhuYuU3d;

namespace ZhuYuU3d{
/// <summary>
	/// 
	/// local timer = CS.ZhuYuU3d.LuaTimerManager.getInstance()
	/// timer:Add(1,luaTable,'luaTimeForUpdate','TimerUpdate_xxx')
	/// timer:Rem('timerUpdateMarqueeText')
/// </summary>
[CSharpCallLua]
public delegate void LuaTimerUpdate(LuaTimerInfo timerInfo);

[LuaCallCSharp]
public class LuaTimerInfo:TimerInfo{

	LuaTimerUpdate luaTimerUpdate;

		public LuaTimerInfo(float interval ,string className,LuaTimerUpdate luaTimerUpdate){
			this.interval = interval;
			this.surplus = interval;

		this.className = className;
		this.luaTimerUpdate = luaTimerUpdate;
		delete = false;
	}

	public override void Update (float curInterval)
	{
			surplus = surplus - curInterval;

			if (surplus < 0) {
				luaTimerUpdate (this);
				surplus = interval; 
			}
	}
}

[LuaCallCSharp]
public class LuaTimerManager : TimerManager {

	private static LuaTimerManager instance;
	public static LuaTimerManager getInstance()
	{
		if (instance == null)
		{
			GameObject gameObject = new GameObject("LuaTimerManager");
			DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<LuaTimerManager>();
		}
		return instance;
	}
	public static LuaTimerManager initForGameObject(GameObject dontDestroyOnLoadGameObject)
	{
		if (instance == null)
		{
			instance = dontDestroyOnLoadGameObject.AddComponent<LuaTimerManager>();
		}
		return instance;
	}

	void Awake(){
		if(instance == null)
			instance = this;
	
	}

	LuaEnv env;

	public LuaTimerInfo Add(float time,LuaTable luaTable,string key,string funName){
		
		if(env == null)
		env = LuaManager.GetInstance ().env;
		
		LuaTimerUpdate luaTimerUpdate;

		if (luaTable == null) {
			env.Global.Get (funName,out luaTimerUpdate);
		} else {
			luaTable.Get (funName, out luaTimerUpdate);
		}

		if (luaTimerUpdate == null) {
			Debug.LogErrorFormat ("can not find lua function {0} ", funName);
			return null;
		}
		LuaTimerInfo luaTimerInfo = new LuaTimerInfo (time,key, luaTimerUpdate);

		this.AddTimerEvent (luaTimerInfo);

		return luaTimerInfo;

	}

	public void Rem(string key){
		
		for(int i = objects.Count - 1; i >= 0; i-- ){
			
			if (objects [i].className.Equals (key)) {
				objects.RemoveAt (i);

					Debug.LogWarningFormat ("Rem LuaTimerUpdate key{0}",key);
			}
		}

	}

}
}