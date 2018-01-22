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
    public class ZhuYuU3dLuaEventCallBackWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ZhuYuU3d.LuaEventCallBack);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEvent", _m_AddEvent);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "curEventTriggerType", _g_get_curEventTriggerType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "callbackFunName", _g_get_callbackFunName);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "curEventTriggerType", _s_set_curEventTriggerType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "callbackFunName", _s_set_callbackFunName);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					ZhuYuU3d.LuaEventCallBack __cl_gen_ret = new ZhuYuU3d.LuaEventCallBack();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ZhuYuU3d.LuaEventCallBack constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaEventCallBack __cl_gen_to_be_invoked = (ZhuYuU3d.LuaEventCallBack)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaEventCallBack __cl_gen_to_be_invoked = (ZhuYuU3d.LuaEventCallBack)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.EventTriggerType eventTriggerType;translator.Get(L, 2, out eventTriggerType);
                    OnEvent callback = translator.GetDelegate<OnEvent>(L, 3);
                    
                    __cl_gen_to_be_invoked.AddEvent( eventTriggerType, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_curEventTriggerType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaEventCallBack __cl_gen_to_be_invoked = (ZhuYuU3d.LuaEventCallBack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineEventSystemsEventTriggerType(L, __cl_gen_to_be_invoked.curEventTriggerType);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_callbackFunName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaEventCallBack __cl_gen_to_be_invoked = (ZhuYuU3d.LuaEventCallBack)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.callbackFunName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_curEventTriggerType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaEventCallBack __cl_gen_to_be_invoked = (ZhuYuU3d.LuaEventCallBack)translator.FastGetCSObj(L, 1);
                UnityEngine.EventSystems.EventTriggerType __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.curEventTriggerType = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_callbackFunName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaEventCallBack __cl_gen_to_be_invoked = (ZhuYuU3d.LuaEventCallBack)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.callbackFunName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
