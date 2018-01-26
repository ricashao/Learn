local MailPanelView =
{
	 
}

local this = MailPanelView

local LuaUIManager = require 'lua/game/LuaUIManager'
 

function MailPanelView:init(trans)

	this.transform=trans;

	this.tabGroupControllWidget=GetComponentInPath(this.transform,"Left_Panel/TabGroup",typeof(CS.SelectGroup));

	this.btn_Back=GetComponentInPath(this.transform,"BackButton",typeof(CS.UnityEngine.UI.Button));

	this.LoopListObj=GetComponentInPath(this.transform,"Right_Up_Panel/Scroll View/Viewport/Content",typeof(CS.UILoopList));

	this.TabEntry_System_Mail=GetComponentInPath(this.transform,"Left_Panel/TabGroup/TabButton0/Select2",typeof(CS.UnityEngine.Transform));

	this.TabEntry_System_Service=GetComponentInPath(this.transform,"Left_Panel/TabGroup/TabButton1/Select2",typeof(CS.UnityEngine.Transform));

	this.btn_Quick_Del=GetComponentInPath(this.transform,"Right_Bottom_Panel/btn_quick_del",typeof(CS.UnityEngine.UI.Button));

	this.btn_Quick_GetReward=GetComponentInPath(this.transform,"Right_Bottom_Panel/btn_quick_get",typeof(CS.UnityEngine.UI.Button));

end	



return MailPanelView