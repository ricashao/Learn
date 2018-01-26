local MailPanelItemContentView =
{
	 
}

local this = MailPanelItemContentView

local LuaUIManager = require 'lua/game/LuaUIManager'
 

function MailPanelItemContentView:init(trans)

	this.transform=trans;

	this.BackButton=GetComponentInPath(trans,"BackButton",typeof(CS.UnityEngine.UI.Button));

	this.Img_SystemMail=GetComponentInPath(trans,"content/UpPanel/img_System",typeof(CS.UnityEngine.UI.Image));

	this.Img_ServiceMail=GetComponentInPath(trans,"content/UpPanel/img_Service",typeof(CS.UnityEngine.UI.Image));

	this.Txt_MailTime=GetComponentInPath(trans,"content/UpPanel/txt_date",typeof(CS.UnityEngine.UI.Text));

	this.Txt_title=GetComponentInPath(trans,"content/CenterPanel/txt_Mail_Title",typeof(CS.UnityEngine.UI.Text));

	this.Txt_content=GetComponentInPath(trans,"content/CenterPanel/txt_Mail_Content",typeof(CS.UnityEngine.UI.Text));

	this.btn_getReward=GetComponentInPath(trans,"content/btn_quick_get",typeof(CS.UnityEngine.UI.Button));

	this.trans_BottomPanel=GetComponentInPath(trans,"content/BottomPanel",typeof(CS.UnityEngine.Transform));


end

function MailPanelItemContentView:SetTimeLabel(strTime)
	self.Txt_MailTime.text=strTime;
end

function MailPanelItemContentView:SetTitle(strTitle)
	self.Txt_title.text=strTitle;
end

function MailPanelItemContentView:SetContent(strContent)
	self.Txt_content.text=strContent;
end



return MailPanelItemContentView