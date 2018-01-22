using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace ZhuYuU3d
{
	[LuaCallCSharp]
	public class LuaBehaviourFactory  {

		static public LuaTable AddBase(string gameObjectName,string luaFilePath){
			return Add<LuaBaseBehaviour> (gameObjectName, luaFilePath);
		}

		static public LuaTable AddUpdate(string gameObjectName,string luaFilePath){
			return Add<LuaUpdateBehaviour> (gameObjectName, luaFilePath);
		}

		static public LuaTable LDD(string gameObjectName,string luaFilePath){
			return LoadDontDestroy<LuaUpdateBehaviour> (gameObjectName, luaFilePath);
		}

		static LuaTable Add<T>(string gameObjectName,string luaFilePath) where T : LuaBaseBehaviour {

			GameObject gameObject = GameObject.Find (gameObjectName);
			LuaBaseBehaviour.luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<T> ();
			luaBaseBehaviour.Init ();
			return luaBaseBehaviour.scriptEnv;
		}

		static LuaTable Load<T>(string gameObjectName,string luaFilePath) where T : LuaBaseBehaviour
		{
			GameObject gameObject = new GameObject (gameObjectName);
			LuaBaseBehaviour.luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<T> ();
			luaBaseBehaviour.Init ();
			return luaBaseBehaviour.scriptEnv;
		}

		static LuaTable LoadDontDestroy<T>(string gameObjectName,string luaFilePath) where T : LuaBaseBehaviour
		{
			GameObject gameObject = new GameObject (gameObjectName);
			GameObject. DontDestroyOnLoad (gameObject);

			LuaBaseBehaviour.luaFilePathDic.Add (gameObject,luaFilePath);
			LuaBaseBehaviour luaBaseBehaviour = gameObject.AddComponent<T> ();
			luaBaseBehaviour.Init ();
			return luaBaseBehaviour.scriptEnv;
		}
			
	}

	[LuaCallCSharp]
	public class LBF:LuaBehaviourFactory{
		
	}
}