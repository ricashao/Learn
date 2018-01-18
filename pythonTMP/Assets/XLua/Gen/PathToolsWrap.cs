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
    public class PathToolsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(PathTools);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 12, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAssetPathForLoadPath1", _m_GetAssetPathForLoadPath1_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Combine", _m_Combine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetStreamingAssetsPath", _m_GetStreamingAssetsPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPersistentPath", _m_GetPersistentPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAppContentPath", _m_GetAppContentPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ExistsPersistentPath", _m_ExistsPersistentPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ExistsStreamingAssetsPath", _m_ExistsStreamingAssetsPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PersistentOrStreamingAssetsPath", _m_PersistentOrStreamingAssetsPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetAssetPath", _m_GetAssetPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PersistentDataPath", _m_PersistentDataPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AppContentPath", _m_AppContentPath_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "UpdateRoot", _g_get_UpdateRoot);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "UpdateRoot", _s_set_UpdateRoot);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					PathTools __cl_gen_ret = new PathTools();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to PathTools constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetPathForLoadPath1_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = PathTools.GetAssetPathForLoadPath1( assetName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Combine_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string path0 = LuaAPI.lua_tostring(L, 1);
                    string path1 = LuaAPI.lua_tostring(L, 2);
                    
                        string __cl_gen_ret = PathTools.Combine( path0, path1 );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetStreamingAssetsPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = PathTools.GetStreamingAssetsPath( assetName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPersistentPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = PathTools.GetPersistentPath( assetName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAppContentPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = PathTools.GetAppContentPath( assetName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExistsPersistentPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = PathTools.ExistsPersistentPath( assetName );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExistsStreamingAssetsPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        bool __cl_gen_ret = PathTools.ExistsStreamingAssetsPath( assetName );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PersistentOrStreamingAssetsPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = PathTools.PersistentOrStreamingAssetsPath( assetName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string assetName = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = PathTools.GetAssetPath( assetName );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PersistentDataPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string __cl_gen_ret = PathTools.PersistentDataPath(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AppContentPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string __cl_gen_ret = PathTools.AppContentPath(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UpdateRoot(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, PathTools.UpdateRoot);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UpdateRoot(RealStatePtr L)
        {
		    try {
                
			    PathTools.UpdateRoot = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
