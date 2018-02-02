using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using ZhuYuU3d;
using UnityEngine.UI;

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
	public string unSelectFunName;
    [SerializeField]
    private Sprite[] sprits;
    [SerializeField]
    public string awakefunctionName;

    private Image img;

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
        img = this.GetComponent<Image>();
        OnSelectItem luafun_UILoopItem_Awake = luaEnv.Global.GetInPath<OnSelectItem>(awakefunctionName);
        if (luafun_UILoopItem_Awake != null) luafun_UILoopItem_Awake(index, transform, GetData());
    }

     

	public override void OnSelect ()
    {
        if (luafun_OnSelect != null)
            luafun_OnSelect (index,transform,data);

        if(sprits!=null && sprits.Length > 1)
        {
            img.sprite = sprits[1];
            img.SetNativeSize();
        }



	}

	public override void UnSelect (){
        if(luafun_UnSelect !=null)
		luafun_UnSelect (index,transform,data);
        if (sprits != null && sprits.Length > 1)
        {
            img.sprite = sprits[0];
            img.SetNativeSize();
        }
    }
}
