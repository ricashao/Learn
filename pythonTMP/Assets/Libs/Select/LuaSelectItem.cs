using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using ZhuYuU3d;

[CSharpCallLua]
public class LuaSelectItem : SelectItem {

	// Use this for initialization
	internal LuaEnv luaEnv;
	[CSharpCallLua]
	public delegate void OnSelectItem(int indexd, Transform transform,object data);

	OnSelectItem luafun_OnSelect;
    OnSelectItem luafun_UnSelect;

	private string script = " function OnSelectItem(index,transform,data)  print(index..transform.name) " +
		"--[[for i,v in pairs(data) do\t\tprint(\"v >> \"..v) end]]" +
		"   end";

	[SerializeField]
	public string onSelectFunName ;
	[SerializeField]
	public string unSelectFunName ;

	void Awake(){

		if(luafun_OnSelect == null){
			luaEnv = LuaManager.GetInstance ().LuaEnvGetOrNew ();
			//luaEnv = new LuaEnv();

			if (onSelectFunName == null || onSelectFunName.Equals ("")) {
				luaEnv.DoString (script);

				luafun_OnSelect = luaEnv.Global.GetInPath<OnSelectItem> ("OnSelectItem");
				luafun_UnSelect = luaEnv.Global.GetInPath<OnSelectItem> ("OnSelectItem");
			} else {

				luafun_OnSelect = luaEnv.Global.GetInPath<OnSelectItem> (onSelectFunName);
				luafun_UnSelect = luaEnv.Global.GetInPath<OnSelectItem> (unSelectFunName);
			}
		}
	}

	public override void OnSelect ()
    {
        if (luafun_OnSelect != null)
            luafun_OnSelect (index,transform,data);
	}

	public override void UnSelect ()
    {
        if (luafun_UnSelect != null)
            luafun_UnSelect (index,transform,data);
	}
}
