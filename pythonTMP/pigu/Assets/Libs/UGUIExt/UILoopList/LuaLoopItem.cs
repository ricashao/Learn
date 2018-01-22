using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using ZhuYuU3d;

public class LuaLoopItem : UILoopItem
{

    internal LuaEnv luaEnv;
    [CSharpCallLua]
    public delegate void UILoopItem_Set(int indexd, Transform transform, object data);

    protected UILoopItem_Set luafun_UILoopItem_Set;

    private string script = " function UILoopItem_Set(index,transform,data)  print(index..transform.name) " +
        "for i,v in pairs(data) do\n\t\tprint(\"v >> \"..v)" +
        " end ";

    [SerializeField]
    public string functionName;
    [SerializeField]
    public string awakefunctionName;

    void Awake()
    {

        if (luafun_UILoopItem_Set == null)
        {
            luaEnv = LuaManager.GetInstance().LuaEnvGetOrNew();
            //luaEnv = new LuaEnv();

            if (functionName == null && functionName.Equals(""))
            {
                luaEnv.DoString(script);

                luafun_UILoopItem_Set = luaEnv.Global.GetInPath<UILoopItem_Set>("UILoopItem_Set");
            }
            else
            {

                luafun_UILoopItem_Set = luaEnv.Global.GetInPath<UILoopItem_Set>(functionName);
            }
            UILoopItem_Set luafun_UILoopItem_Awake = luaEnv.Global.GetInPath<UILoopItem_Set>(awakefunctionName);
            if (luafun_UILoopItem_Awake != null) luafun_UILoopItem_Awake(itemIndex, transform, GetData());
        }
    }

    public override void Data(object data)
    {
        base.Data(data);

        if (luafun_UILoopItem_Set != null)
            luafun_UILoopItem_Set(itemIndex, transform, data);
        /*
		Text t = GetComponentInChildren<Text>();
		if (t != null)
		{
			t.text = data.ToString();
		}
		*/
    }
}
