#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    
    public class XLuaTestMyEnumWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(XLuaTest.MyEnum), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(XLuaTest.MyEnum), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(XLuaTest.MyEnum), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E1", XLuaTest.MyEnum.E1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E2", XLuaTest.MyEnum.E2);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(XLuaTest.MyEnum), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushXLuaTestMyEnum(L, (XLuaTest.MyEnum)LuaAPI.xlua_tointeger(L, 1));
            }
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "E1"))
                {
                    translator.PushXLuaTestMyEnum(L, XLuaTest.MyEnum.E1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "E2"))
                {
                    translator.PushXLuaTestMyEnum(L, XLuaTest.MyEnum.E2);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for XLuaTest.MyEnum!");
                }
            }
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for XLuaTest.MyEnum! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineRuntimePlatformWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.RuntimePlatform), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.RuntimePlatform), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.RuntimePlatform), L, null, 35, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OSXEditor", UnityEngine.RuntimePlatform.OSXEditor);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OSXPlayer", UnityEngine.RuntimePlatform.OSXPlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WindowsPlayer", UnityEngine.RuntimePlatform.WindowsPlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OSXDashboardPlayer", UnityEngine.RuntimePlatform.OSXDashboardPlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WindowsEditor", UnityEngine.RuntimePlatform.WindowsEditor);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IPhonePlayer", UnityEngine.RuntimePlatform.IPhonePlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Android", UnityEngine.RuntimePlatform.Android);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LinuxPlayer", UnityEngine.RuntimePlatform.LinuxPlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LinuxEditor", UnityEngine.RuntimePlatform.LinuxEditor);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WebGLPlayer", UnityEngine.RuntimePlatform.WebGLPlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WSAPlayerX86", UnityEngine.RuntimePlatform.WSAPlayerX86);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WSAPlayerX64", UnityEngine.RuntimePlatform.WSAPlayerX64);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WSAPlayerARM", UnityEngine.RuntimePlatform.WSAPlayerARM);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TizenPlayer", UnityEngine.RuntimePlatform.TizenPlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PSP2", UnityEngine.RuntimePlatform.PSP2);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PS4", UnityEngine.RuntimePlatform.PS4);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PSM", UnityEngine.RuntimePlatform.PSM);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "XboxOne", UnityEngine.RuntimePlatform.XboxOne);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SamsungTVPlayer", UnityEngine.RuntimePlatform.SamsungTVPlayer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WiiU", UnityEngine.RuntimePlatform.WiiU);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "tvOS", UnityEngine.RuntimePlatform.tvOS);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Switch", UnityEngine.RuntimePlatform.Switch);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.RuntimePlatform), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineRuntimePlatform(L, (UnityEngine.RuntimePlatform)LuaAPI.xlua_tointeger(L, 1));
            }
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "OSXEditor"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.OSXEditor);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "OSXPlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.OSXPlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WindowsPlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.WindowsPlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "OSXDashboardPlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.OSXDashboardPlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WindowsEditor"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.WindowsEditor);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IPhonePlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.IPhonePlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Android"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.Android);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LinuxPlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.LinuxPlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LinuxEditor"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.LinuxEditor);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WebGLPlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.WebGLPlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WSAPlayerX86"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.WSAPlayerX86);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WSAPlayerX64"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.WSAPlayerX64);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WSAPlayerARM"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.WSAPlayerARM);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TizenPlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.TizenPlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PSP2"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.PSP2);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PS4"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.PS4);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PSM"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.PSM);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "XboxOne"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.XboxOne);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SamsungTVPlayer"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.SamsungTVPlayer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WiiU"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.WiiU);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "tvOS"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.tvOS);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Switch"))
                {
                    translator.PushUnityEngineRuntimePlatform(L, UnityEngine.RuntimePlatform.Switch);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.RuntimePlatform!");
                }
            }
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.RuntimePlatform! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineEventSystemsEventTriggerTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, null, 18, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerEnter", UnityEngine.EventSystems.EventTriggerType.PointerEnter);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerExit", UnityEngine.EventSystems.EventTriggerType.PointerExit);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerDown", UnityEngine.EventSystems.EventTriggerType.PointerDown);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerUp", UnityEngine.EventSystems.EventTriggerType.PointerUp);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PointerClick", UnityEngine.EventSystems.EventTriggerType.PointerClick);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Drag", UnityEngine.EventSystems.EventTriggerType.Drag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Drop", UnityEngine.EventSystems.EventTriggerType.Drop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Scroll", UnityEngine.EventSystems.EventTriggerType.Scroll);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpdateSelected", UnityEngine.EventSystems.EventTriggerType.UpdateSelected);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Select", UnityEngine.EventSystems.EventTriggerType.Select);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Deselect", UnityEngine.EventSystems.EventTriggerType.Deselect);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Move", UnityEngine.EventSystems.EventTriggerType.Move);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InitializePotentialDrag", UnityEngine.EventSystems.EventTriggerType.InitializePotentialDrag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BeginDrag", UnityEngine.EventSystems.EventTriggerType.BeginDrag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EndDrag", UnityEngine.EventSystems.EventTriggerType.EndDrag);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Submit", UnityEngine.EventSystems.EventTriggerType.Submit);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Cancel", UnityEngine.EventSystems.EventTriggerType.Cancel);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.EventSystems.EventTriggerType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineEventSystemsEventTriggerType(L, (UnityEngine.EventSystems.EventTriggerType)LuaAPI.xlua_tointeger(L, 1));
            }
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "PointerEnter"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerEnter);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerExit"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerExit);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerDown"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerDown);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerUp"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerUp);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PointerClick"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.PointerClick);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Drag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Drag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Drop"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Drop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Scroll"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Scroll);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpdateSelected"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.UpdateSelected);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Select"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Select);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Deselect"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Deselect);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Move"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Move);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InitializePotentialDrag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.InitializePotentialDrag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BeginDrag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.BeginDrag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EndDrag"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.EndDrag);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Submit"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Submit);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Cancel"))
                {
                    translator.PushUnityEngineEventSystemsEventTriggerType(L, UnityEngine.EventSystems.EventTriggerType.Cancel);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.EventSystems.EventTriggerType!");
                }
            }
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.EventSystems.EventTriggerType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class TutorialTestEnumWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Tutorial.TestEnum), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Tutorial.TestEnum), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Tutorial.TestEnum), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E1", Tutorial.TestEnum.E1);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E2", Tutorial.TestEnum.E2);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Tutorial.TestEnum), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushTutorialTestEnum(L, (Tutorial.TestEnum)LuaAPI.xlua_tointeger(L, 1));
            }
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "E1"))
                {
                    translator.PushTutorialTestEnum(L, Tutorial.TestEnum.E1);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "E2"))
                {
                    translator.PushTutorialTestEnum(L, Tutorial.TestEnum.E2);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Tutorial.TestEnum!");
                }
            }
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Tutorial.TestEnum! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class TutorialDrivenClassTestEnumInnerWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Tutorial.DrivenClass.TestEnumInner), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Tutorial.DrivenClass.TestEnumInner), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Tutorial.DrivenClass.TestEnumInner), L, null, 3, 0, 0);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E3", Tutorial.DrivenClass.TestEnumInner.E3);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "E4", Tutorial.DrivenClass.TestEnumInner.E4);
            
			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Tutorial.DrivenClass.TestEnumInner), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushTutorialDrivenClassTestEnumInner(L, (Tutorial.DrivenClass.TestEnumInner)LuaAPI.xlua_tointeger(L, 1));
            }
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {
			    if (LuaAPI.xlua_is_eq_str(L, 1, "E3"))
                {
                    translator.PushTutorialDrivenClassTestEnumInner(L, Tutorial.DrivenClass.TestEnumInner.E3);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "E4"))
                {
                    translator.PushTutorialDrivenClassTestEnumInner(L, Tutorial.DrivenClass.TestEnumInner.E4);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Tutorial.DrivenClass.TestEnumInner!");
                }
            }
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Tutorial.DrivenClass.TestEnumInner! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}