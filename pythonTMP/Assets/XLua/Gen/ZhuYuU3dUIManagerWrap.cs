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
    public class ZhuYuU3dUIManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ZhuYuU3d.UIManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Load", _m_Load);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PopWindow", _m_PopWindow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToastTips", _m_ToastTips);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInstance", _m_GetInstance_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					ZhuYuU3d.UIManager __cl_gen_ret = new ZhuYuU3d.UIManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ZhuYuU3d.UIManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        ZhuYuU3d.UIManager __cl_gen_ret = ZhuYuU3d.UIManager.GetInstance(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Load(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.UIManager __cl_gen_to_be_invoked = (ZhuYuU3d.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string panelName = LuaAPI.lua_tostring(L, 2);
                    string funName = LuaAPI.lua_tostring(L, 3);
                    string layer = LuaAPI.lua_tostring(L, 4);
                    
                    __cl_gen_to_be_invoked.Load( panelName, funName, layer );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PopWindow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.UIManager __cl_gen_to_be_invoked = (ZhuYuU3d.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string panelName = LuaAPI.lua_tostring(L, 2);
                    string strTitle = LuaAPI.lua_tostring(L, 3);
                    string strContent = LuaAPI.lua_tostring(L, 4);
                    int nType = LuaAPI.xlua_tointeger(L, 5);
                    string layer = LuaAPI.lua_tostring(L, 6);
                    XLua.LuaFunction onOK = (XLua.LuaFunction)translator.GetObject(L, 7, typeof(XLua.LuaFunction));
                    XLua.LuaFunction onCancel = (XLua.LuaFunction)translator.GetObject(L, 8, typeof(XLua.LuaFunction));
                    XLua.LuaFunction onLoadOver = (XLua.LuaFunction)translator.GetObject(L, 9, typeof(XLua.LuaFunction));
                    
                    __cl_gen_to_be_invoked.PopWindow( panelName, strTitle, strContent, nType, layer, onOK, onCancel, onLoadOver );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToastTips(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.UIManager __cl_gen_to_be_invoked = (ZhuYuU3d.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string strContent = LuaAPI.lua_tostring(L, 2);
                    int ntime = LuaAPI.xlua_tointeger(L, 3);
                    int nfontsize = LuaAPI.xlua_tointeger(L, 4);
                    int ndirection = LuaAPI.xlua_tointeger(L, 5);
                    XLua.LuaFunction onover = (XLua.LuaFunction)translator.GetObject(L, 6, typeof(XLua.LuaFunction));
                    
                    __cl_gen_to_be_invoked.ToastTips( strContent, ntime, nfontsize, ndirection, onover );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
