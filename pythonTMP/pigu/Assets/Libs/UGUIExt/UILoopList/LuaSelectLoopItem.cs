using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using ZhuYuU3d;

[RequireComponent(typeof(LuaSelectItem))]
public class LuaSelectLoopItem : LuaLoopItem
{
    private LuaSelectItem selectItem;

    public bool isAutoListener = false;
    internal LuaEnv luaEnv;
    private void Awake()
    {
        selectItem = this.transform.GetComponent<LuaSelectItem>();
        if (luafun_UILoopItem_Set == null)
        {
            luaEnv = LuaManager.GetInstance().LuaEnvGetOrNew();
            if (functionName == null && functionName.Equals(""))
            {
                luafun_UILoopItem_Set = luaEnv.Global.GetInPath<UILoopItem_Set>("UILoopItem_Set");
            }
            else
            {
                luafun_UILoopItem_Set = luaEnv.Global.GetInPath<UILoopItem_Set>(functionName);
            }
        }
        if (isAutoListener)
        {
            Button[] btns = this.transform.GetComponentsInChildren<Button>();
            SelectGroup group = this.transform.parent.GetComponent<SelectGroup>();
            group.AddItem(selectItem);
            foreach (Button btn in btns)
            {
                btn.onClick.AddListener(() => { group.SelectByIndex(selectItem.index); });
            }
        }
        UILoopItem_Set luafun_UILoopItem_Awake = luaEnv.Global.GetInPath<UILoopItem_Set>(awakefunctionName);
        if (luafun_UILoopItem_Awake != null) luafun_UILoopItem_Awake(itemIndex, transform, GetData());
    }

    public override void Data(object data)
    {
        base.Data(data);

        if (luafun_UILoopItem_Set != null)
            luafun_UILoopItem_Set(itemIndex, transform, data);
        selectItem.data = data;
    }

}
