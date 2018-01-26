local MailPanelItemContentView = require 'lua/modules/Lobby/modules/Mail/MailPanelItemContentView'



MailPanelItemContentCtrl=
{
	["OnMessage"]=
	function(paramobj)
		print("Handle Message"..paramobj);
		MailPanelItemContentCtrl.ID=paramobj;
		es("C2S_MailModules_GetMailDataByID",paramobj );
	end,

	["ID"]=-1,
	["Data"]=nil,

};

local this=MailPanelItemContentCtrl;

LuaUIManager = require 'lua/game/LuaUIManager'

function MailPanelItemContentCtrl.HandleMessage(strMessageName,objParam)

	if strMessageName=="S2C_MailModules_GetDataByID" then
		this.Data=objParam;

		if(this.Data==nil)then
			return;
		end

		this.InitMailImg();

		this.InitTime();

		this.InitTitle();

		this.InitContent();

		this:InitRewardNode();

		this:InitMailStatus();
	elseif strMessageName=="S2C_MailModules_SetSingleMailStatus" then

		print("S2C_MailModules_SetSingleMailStatus:ContentCtrl");

		if(this.Data.status==Define.E_EmailStatus.ES_REWARD)then

			MailPanelItemContentView.btn_getReward.gameObject:SetActive(false);

		end
	end

end

function awake()

	MailPanelItemContentView:init(self.transform);

	MailPanelItemContentView.BackButton.onClick:AddListener(function ()
		LuaUIManager:CloseTopPage();
	end
	);

	MailPanelItemContentView.btn_getReward.onClick:AddListener(function ()
		 this.OnClkGetReward();
	end
	);

	el("S2C_MailModules_GetDataByID",MailPanelItemContentCtrl.HandleMessage);

	el("S2C_MailModules_SetSingleMailStatus",MailPanelItemContentCtrl.HandleMessage);

end

--function MailPanelItemContentCtrl.

function MailPanelItemContentCtrl.InitMailImg()
	print(this.Data.type);
	if this.Data.type==Define.E_EmailType.ET_SYSTEM then
		MailPanelItemContentView.Img_SystemMail.gameObject:SetActive(true);
		MailPanelItemContentView.Img_ServiceMail.gameObject:SetActive(false);
	else
		MailPanelItemContentView.Img_SystemMail.gameObject:SetActive(false);
		MailPanelItemContentView.Img_ServiceMail.gameObject:SetActive(true);
	end
end

function MailPanelItemContentCtrl.InitTime()
	MailPanelItemContentView:SetTimeLabel(this.Data.time);
end

function MailPanelItemContentCtrl.InitTitle()
	MailPanelItemContentView:SetTitle(this.Data.title);
end

function MailPanelItemContentCtrl.InitContent()
	MailPanelItemContentView:SetContent(this.Data.content);
end

function MailPanelItemContentCtrl.OnClkGetReward()

	print("Clk Get Reward");

	this:ChangeMailStatus(Define.E_EmailStatus.ES_REWARD);


end

--function MailPanelItemContentCtrl.

function ondestroy()

	er("S2C_MailModules_GetDataByID",MailPanelItemContentCtrl.HandleMessage);
	er("S2C_MailModules_SetSingleMailStatus",MailPanelItemContentCtrl.HandleMessage);
end

function MailPanelItemContentCtrl:IsReward()
	if(this.Data.rewards==nil or this.Data.rewards=="" or this.Data.rewards=="0" )then
		return false;
	end
	return true;
end

function MailPanelItemContentCtrl:InitRewardNode()
	--if(self:IsReward())then

	MailPanelItemContentView.trans_BottomPanel.gameObject:SetActive(self:IsReward());
	MailPanelItemContentView.btn_getReward.gameObject:SetActive(self:IsReward());
	--else

	--end
end

function MailPanelItemContentCtrl:ChangeMailStatus(mailstatus)
	if EventExist("C2S_MailModules_SetSingleMailStatus") then
		es("C2S_MailModules_SetSingleMailStatus",{["MailID"]=this.Data.id,["MailStatus"]=mailstatus});
	end

end

function MailPanelItemContentCtrl:InitMailStatus()

	if this.Data~=nil then

		if(this.Data.status==Define.E_EmailStatus.ES_RECEIVE)then

			if EventExist("C2S_MailModules_SetSingleMailStatus") then
				es("C2S_MailModules_SetSingleMailStatus",{["MailID"]=this.Data.id,["MailStatus"]="ES_READ"});
			end

		elseif this.Data.status==Define.E_EmailStatus.ES_READ then

		elseif this.Data.status==Define.E_EmailStatus.ES_REWARD then

			MailPanelItemContentView.btn_getReward.gameObject:SetActive(false);

		end

	end
end