local MailPanelItemView=
{
	["ID"]=-1,
	["RootTrans"]=nil,
	["Img_MailStatus_NoOpen"]=nil,
	["Img_MailStatus_Opened"]=nil,
	["Txt_Title"]=nil,
	["Txt_Time"]=nil,
	["Img_Selected_BG"]=nil,
	["Btn_Item"]=nil,
};

function  MailPanelItemView:New(TransRoot,IDParam)

	local self = {}

	setmetatable( self , {__index = MailPanelItemView})

	self.RootTrans=TransRoot;

	self.ID=IDParam;

	self:InitWidget();

	return self;

end

function MailPanelItemView:GetID()
	return self.ID;
end

function MailPanelItemView:SetID(nID)
	self.ID=nID;
end

function MailPanelItemView:InitWidget()

	self.Img_Selected_BG=GetComponentInPath(self.RootTrans,"Img_Select_BG",typeof(CS.UnityEngine.UI.Image));

	self.Txt_Time=GetComponentInPath(self.RootTrans,"txt_datetime",typeof(CS.UnityEngine.UI.Text));

	self.Txt_Title=GetComponentInPath(self.RootTrans,"txt_content",typeof(CS.UnityEngine.UI.Text));

	self.Img_MailStatus_NoOpen=GetComponentInPath(self.RootTrans,"Img_Mail_NoOpen",typeof(CS.UnityEngine.UI.Image));

	self.Img_MailStatus_Opened=GetComponentInPath(self.RootTrans,"Img_Mail_Opened",typeof(CS.UnityEngine.UI.Image));

	self.Btn_Item=self.RootTrans:GetComponent(typeof(CS.UnityEngine.UI.Button));


end

function MailPanelItemView:InitTitle(strTitle)
	self.Txt_Title.text=strTitle;
end

function MailPanelItemView:InitTime(strTime)
	self.Txt_Time.text=strTime;
end

function MailPanelItemView:InitMailStatus(bOpened)

	if bOpened==true then
		self.Img_MailStatus_Opened.gameObject:SetActive(true);
		self.Img_MailStatus_NoOpen.gameObject:SetActive(false);
	else
		self.Img_MailStatus_Opened.gameObject:SetActive(false);
		self.Img_MailStatus_NoOpen.gameObject:SetActive(true);
	end

end

function MailPanelItemView:InitSelectState(bSelected)
	if bSelected==true then
		self.Img_Selected_BG.gameObject:SetActive(true);
	else
		self.Img_Selected_BG.gameObject:SetActive(false);
	end
end

return MailPanelItemView;
