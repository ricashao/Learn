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
    public class ZhuYuU3dLuaManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ZhuYuU3d.LuaManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 2, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LuaEnvGetOrNew", _m_LuaEnvGetOrNew);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LuaEnvNew", _m_LuaEnvNew);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LuaEnvDispose", _m_LuaEnvDispose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaFixedUpdate", _m_SetLuaFixedUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaUpdate", _m_SetLuaUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLuaLateUpdate", _m_SetLuaLateUpdate);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "InitDoString", _g_get_InitDoString);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "env", _g_get_env);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "InitDoString", _s_set_InitDoString);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInstance", _m_GetInstance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetFixedUpdateFun", _m_SetFixedUpdateFun_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetUpdateFun", _m_SetUpdateFun_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetLateUpdateFun", _m_SetLateUpdateFun_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					ZhuYuU3d.LuaManager __cl_gen_ret = new ZhuYuU3d.LuaManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ZhuYuU3d.LuaManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        ZhuYuU3d.LuaManager __cl_gen_ret = ZhuYuU3d.LuaManager.GetInstance(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFixedUpdateFun_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string luaFixedUpdateFunName = LuaAPI.lua_tostring(L, 1);
                    
                    ZhuYuU3d.LuaManager.SetFixedUpdateFun( luaFixedUpdateFunName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetUpdateFun_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string luaUpdateFunName = LuaAPI.lua_tostring(L, 1);
                    
                    ZhuYuU3d.LuaManager.SetUpdateFun( luaUpdateFunName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLateUpdateFun_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string luaLateUpdateFunName = LuaAPI.lua_tostring(L, 1);
                    
                    ZhuYuU3d.LuaManager.SetLateUpdateFun( luaLateUpdateFunName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LuaEnvGetOrNew(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        XLua.LuaEnv __cl_gen_ret = __cl_gen_to_be_invoked.LuaEnvGetOrNew(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LuaEnvNew(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        XLua.LuaEnv __cl_gen_ret = __cl_gen_to_be_invoked.LuaEnvNew(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LuaEnvDispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.LuaEnvDispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaFixedUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string luaFixedUpdateFunName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.SetLuaFixedUpdate( luaFixedUpdateFunName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string luaUpdateFunName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.SetLuaUpdate( luaUpdateFunName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLuaLateUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string luaLateUpdateFunName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.SetLuaLateUpdate( luaLateUpdateFunName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InitDoString(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.InitDoString);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_env(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.env);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InitDoString(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaManager __cl_gen_to_be_invoked = (ZhuYuU3d.LuaManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.InitDoString = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
