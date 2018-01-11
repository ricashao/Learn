using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using ZhuYuU3d;

[RequireComponent(typeof(LuaSelectItem))]
public class LuaSelectLoopItem : LuaLoopItem
{
    //[SerializeField]
    //public string awakefunctionName;
    private LuaSelectItem selectItem;

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
        //UILoopItem_Set luafun_UILoopItem_Awake = luaEnv.Global.GetInPath<UILoopItem_Set>(awakefunctionName);
        //if (luafun_UILoopItem_Awake != null) luafun_UILoopItem_Awake(itemIndex, transform, GetData());
        Button[] btns = this.transform.GetComponentsInChildren<Button>();
        SelectGroup group = this.transform.parent.GetComponent<SelectGroup>();
        group.AddItem(selectItem);
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => { group.SelectByIndex(selectItem.index); });
        }
    }

    public override void Data(object data)
    {
        base.Data(data);

        if (luafun_UILoopItem_Set != null)
            luafun_UILoopItem_Set(itemIndex, transform, data);
        selectItem.data = data;
    }

}
