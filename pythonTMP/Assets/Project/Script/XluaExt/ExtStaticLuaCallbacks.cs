using System.Collections;
using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using XLua;

#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

public class ExtStaticLuaCallbacks {

	[MonoPInvokeCallback(typeof(LuaCSFunction))]
	internal static int LoadFromResourceLuaFile(RealStatePtr L)
	{
		try
		{
			string filename = LuaAPI.lua_tostring(L, 1).Replace('.', '/') + ".lua";

			string filepath = UnityEngine.Application.streamingAssetsPath.Replace("StreamingAssets","Resources")+ "/" + filename;

			if (File.Exists(filepath))
			{
				// string text = File.ReadAllText(filepath);
				var bytes = File.ReadAllBytes(filepath);

				UnityEngine.Debug.LogWarning("load lua file from Resource LuaFile is obsolete, filename:" + filename);
				if (LuaAPI.xluaL_loadbuffer(L, bytes, bytes.Length, "@" + filename) != 0)
				{
					return LuaAPI.luaL_error(L, String.Format("error loading module {0} from streamingAssetsPath, {1}",
						LuaAPI.lua_tostring(L, 1), LuaAPI.lua_tostring(L, -1)));
				}
			}
			else
			{
				LuaAPI.lua_pushstring(L, string.Format(
					"\n\tno such file '{0}' in streamingAssetsPath!", filename));
			}

			return 1;
		}
		catch (System.Exception e)
		{
			return LuaAPI.luaL_error(L, "c# exception in LoadFromResource:" + e);
		}
	}
}
