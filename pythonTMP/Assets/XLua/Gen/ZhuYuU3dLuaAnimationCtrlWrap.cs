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
    public class ZhuYuU3dLuaAnimationCtrlWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ZhuYuU3d.LuaAnimationCtrl);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Stop", _m_Stop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEvent", _m_AddEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemEvent", _m_RemEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventFloat", _m_AddEventFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventInt", _m_AddEventInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventString", _m_AddEventString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventUnityEngineObject", _m_AddEventUnityEngineObject);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ani", _g_get_ani);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "animationClip", _g_get_animationClip);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ani", _s_set_ani);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "animationClip", _s_set_animationClip);
            
			
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
					
					ZhuYuU3d.LuaAnimationCtrl __cl_gen_ret = new ZhuYuU3d.LuaAnimationCtrl();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ZhuYuU3d.LuaAnimationCtrl constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string animationClipName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.Play( animationClipName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Stop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string animationClipName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.Stop( animationClipName );
                    
                    
                    
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
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string animationClipName = LuaAPI.lua_tostring(L, 2);
                    float time = (float)LuaAPI.lua_tonumber(L, 3);
                    string functionName = LuaAPI.lua_tostring(L, 4);
                    object param = translator.GetObject(L, 5, typeof(object));
                    
                    __cl_gen_to_be_invoked.AddEvent( animationClipName, time, functionName, param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string functionName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.RemEvent( functionName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AnimationClip clip = (UnityEngine.AnimationClip)translator.GetObject(L, 2, typeof(UnityEngine.AnimationClip));
                    float time = (float)LuaAPI.lua_tonumber(L, 3);
                    float param = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    __cl_gen_to_be_invoked.AddEventFloat( clip, time, param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AnimationClip clip = (UnityEngine.AnimationClip)translator.GetObject(L, 2, typeof(UnityEngine.AnimationClip));
                    float time = (float)LuaAPI.lua_tonumber(L, 3);
                    string functionName = LuaAPI.lua_tostring(L, 4);
                    int param = LuaAPI.xlua_tointeger(L, 5);
                    
                    __cl_gen_to_be_invoked.AddEventInt( clip, time, functionName, param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AnimationClip clip = (UnityEngine.AnimationClip)translator.GetObject(L, 2, typeof(UnityEngine.AnimationClip));
                    float time = (float)LuaAPI.lua_tonumber(L, 3);
                    string functionName = LuaAPI.lua_tostring(L, 4);
                    string param = LuaAPI.lua_tostring(L, 5);
                    
                    __cl_gen_to_be_invoked.AddEventString( clip, time, functionName, param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventUnityEngineObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AnimationClip clip = (UnityEngine.AnimationClip)translator.GetObject(L, 2, typeof(UnityEngine.AnimationClip));
                    float time = (float)LuaAPI.lua_tonumber(L, 3);
                    string functionName = LuaAPI.lua_tostring(L, 4);
                    UnityEngine.Object param = (UnityEngine.Object)translator.GetObject(L, 5, typeof(UnityEngine.Object));
                    
                    __cl_gen_to_be_invoked.AddEventUnityEngineObject( clip, time, functionName, param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ani(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.ani);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_animationClip(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.animationClip);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ani(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.ani = (UnityEngine.Animation)translator.GetObject(L, 2, typeof(UnityEngine.Animation));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_animationClip(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ZhuYuU3d.LuaAnimationCtrl __cl_gen_to_be_invoked = (ZhuYuU3d.LuaAnimationCtrl)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.animationClip = (UnityEngine.AnimationClip)translator.GetObject(L, 2, typeof(UnityEngine.AnimationClip));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
