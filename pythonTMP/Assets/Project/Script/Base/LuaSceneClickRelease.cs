using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.EventSystems;
using System;

namespace ZhuYuU3d
{
    [LuaCallCSharp]
    public class LuaSceneClickRelease : LuaBaseBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public PointerEventData curEventData;
        private Action luaOnPointerDown;
        private Action luaOnPointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            curEventData = eventData;
            if (luaOnPointerDown != null)
            {
                luaOnPointerDown();
            }
            else
            {
                Debug.LogWarningFormat("OnPointerDown but not find lua OnPointerDown fun !");
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            curEventData = eventData;
            if (luaOnPointerUp != null)
            {
                luaOnPointerUp();
            }
            else
            {
                Debug.LogWarningFormat("OnPointerUp but not find lua OnPointerUp fun !");
            }
        }

        public override void Init()
        {
            base.Init();

            scriptEnv.Get("OnPointerDown", out luaOnPointerDown);
            scriptEnv.Get("OnPointerUp", out luaOnPointerUp);
        }
    }
}
