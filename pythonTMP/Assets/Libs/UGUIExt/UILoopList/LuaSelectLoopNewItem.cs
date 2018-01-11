using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using ZhuYuU3d;

[RequireComponent(typeof(LuaSelectItem))]
public class LuaSelectLoopNewItem : LuaLoopItem,ISelectAble
{
    internal LuaEnv luaEnv;

    [CSharpCallLua]
    public delegate void OnSelectItem(int indexd, Transform transform, object data);

    OnSelectItem luafun_OnSelect;
    OnSelectItem luafun_UnSelect;


    [SerializeField]
    public string onSelectFunName;
    [SerializeField]
    public string unSelectFunName;
    private void Awake()
    {
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
        group.AddItem(this);
        foreach(Button btn in btns)
        {
            btn.onClick.AddListener(() => { group.SelectByIndex(this.index); });
        }

        if (luafun_OnSelect == null)
        {
            luaEnv = LuaManager.GetInstance().LuaEnvGetOrNew();
            //luaEnv = new LuaEnv();

            if (onSelectFunName == null || onSelectFunName.Equals(""))
            {
                //luaEnv.DoString(script);

                luafun_OnSelect = luaEnv.Global.GetInPath<OnSelectItem>("OnSelectItem");
                luafun_UnSelect = luaEnv.Global.GetInPath<OnSelectItem>("UnSelectItem");
            }
            else
            {

                luafun_OnSelect = luaEnv.Global.GetInPath<OnSelectItem>(onSelectFunName);
                luafun_UnSelect = luaEnv.Global.GetInPath<OnSelectItem>(unSelectFunName);
            }
        }
    }
    protected SelectGroup selectGroup;
    public override void Data(object data)
    {
        base.Data(data);

        if (luafun_UILoopItem_Set != null)
            luafun_UILoopItem_Set(itemIndex, transform, data);
        this.data = data;
    }

    public void SetSelectGroup(SelectGroup selectGroup)
    {
        this.selectGroup = selectGroup;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public int GetIndex()
    {
        return index;
    }
    
    public int index;

    public void Select()
    {
        selectGroup.SelectByIndex(index);
    }

    public void OnSelect()
    {
        if (luafun_OnSelect != null)
            luafun_OnSelect(itemIndex, transform, data);
    }

    public void UnSelect()
    {
        if (luafun_UnSelect != null)
            luafun_UnSelect(index, transform, data);
    }
}
