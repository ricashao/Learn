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
    public class ZhuYuU3dLuaCallCsFunWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ZhuYuU3d.LuaCallCsFun);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 8, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "JumpScene", _m_JumpScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "JumpSceneName", _m_JumpSceneName_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "JumpToRun", _m_JumpToRun_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "JumpToLoading", _m_JumpToLoading_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "JumpToLauncher", _m_JumpToLauncher_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadByte", _m_ReadByte_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReadByteForLua", _m_ReadByteForLua_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					ZhuYuU3d.LuaCallCsFun __cl_gen_ret = new ZhuYuU3d.LuaCallCsFun();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ZhuYuU3d.LuaCallCsFun constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JumpScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 1);
                    
                    ZhuYuU3d.LuaCallCsFun.JumpScene( index );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JumpSceneName_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string sceneName = LuaAPI.lua_tostring(L, 1);
                    
                    ZhuYuU3d.LuaCallCsFun.JumpSceneName( sceneName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JumpToRun_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    ZhuYuU3d.LuaCallCsFun.JumpToRun(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JumpToLoading_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    ZhuYuU3d.LuaCallCsFun.JumpToLoading(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_JumpToLauncher_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    ZhuYuU3d.LuaCallCsFun.JumpToLauncher(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadByte_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string fileName = LuaAPI.lua_tostring(L, 1);
                    
                        byte[] __cl_gen_ret = ZhuYuU3d.LuaCallCsFun.ReadByte( fileName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadByteForLua_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string fileName = LuaAPI.lua_tostring(L, 1);
                    
                        byte[] __cl_gen_ret = ZhuYuU3d.LuaCallCsFun.ReadByteForLua( fileName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
