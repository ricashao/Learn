local MailPanelView = require 'lua/modules/Lobby/modules/Mail/MailPanelView'

local MailPanelItemView=require 'lua/modules/Lobby/modules/Mail/MailPanelItemView'

local MessageBoxManager=require 'lua/game/MessageBoxManager'

local MailPanelCtrl=
{
    ["tblMailPanelItemView"]={},

    ["CurrentSelection"]=Define.E_EmailType.ET_SYSTEM,
	
};

local this=MailPanelCtrl;

LuaUIManager = require 'lua/game/LuaUIManager'



function awake()

	MailPanelView:init(self.transform);



	-- Register Message
	el("Global2Ctrl_MailModules_GetMailList",MailPanelCtrl.HandleMessage);

	el("S2C_MailModules_GetMailList",MailPanelCtrl.HandleMessage);

	el("Global2Ctrl_MailModules_SetMailItemSelected",MailPanelCtrl.HandleMessage);

	el("Global2Ctrl_MailModules_SetMailItemContent",MailPanelCtrl.HandleMessage);

	el("S2C_MailModules_GetMailTableDataByTabType",MailPanelCtrl.HandleMessage);

    el("S2C_MailModules_SetSingleMailStatus",MailPanelCtrl.HandleMessage);

	--el("S2C_MailModules_SetSingleMailStatus",MailPanelCtrl.HandleMessage);



	MailPanelView.tabGroupControllWidget:SelectByIndex(0);

	MailPanelView.btn_Back.onClick:AddListener(function()
		print("Back Click");
		LuaUIManager:CloseTopPage();
	end
	);

	MailPanelView.btn_Quick_Del.onClick:AddListener(
			function()
				print("Clk Quick Delete Button");
				this.OnQuickDelMail();
 			end
	)

	MailPanelView.btn_Quick_GetReward.onClick:AddListener(
			function()
				print("Clk Quick Quick GetReward Button");
				this.OnQuickGetReward();
			end
	)

	es("Global2Ctrl_MailModules_GetMailList");

	el("Global2Ctrl_MailModules_TheTabSelected",MailPanelCtrl.HandleMessage);

	el("S2C_MailModules_DeleteAllOpenMailButNoReward",MailPanelCtrl.HandleMessage);

	el("S2C_MailModules_GetAllRewardMail",MailPanelCtrl.HandleMessage);

	--er("S2C_MailModules_GetAllRewardMail",MailPanelCtrl.HandleMessage);
	--
	--er("S2C_MailModules_DeleteAllOpenMailButNoReward",MailPanelCtrl.HandleMessage);

--	EventDispatcher:addEvent("S2C_MailModules_GetMailTableDataByTabType",MailPanelCtrl.HandleMessage);


end

function MailPanelCtrl.HandleMessage(strMessageName,objParam)

	if(strMessageName=="Global2Ctrl_MailModules_GetMailList")then

		print("MailControl:GetMailList");
		es("C2S_MailModules_GetMailList");

	elseif "S2C_MailModules_GetMailTableDataByTabType"==strMessageName then
		this.OnHandleGetTableDataByTabType(objParam);
	elseif (strMessageName=="S2C_MailModules_GetMailList") then

		print("MailControl:GetMailList-BackMessage");

		this.OnHandleGetMailList(objParam);
	elseif "Global2Ctrl_MailModules_SetMailItemContent"==strMessageName then
		print("Global2Ctrl_MailModules_SetMailItemContent");
		this.OnHandleSetMailItemContent(objParam);

	elseif "Global2Ctrl_MailModules_SetMailItemSelected"==strMessageName then
		--print("OnDataRefresh");
		this.OnHandleSetSelectedState(objParam.Transform,objParam.BS);
	elseif "Global2Ctrl_MailModules_TheTabSelected"==strMessageName then
		this.OnHandleTabSelected();
	elseif "S2C_MailModules_SetSingleMailStatus"==strMessageName then
        print("S2C_MailModules_SetSingleMailStatus:"..objParam.MailStatus);
		this.SetMultiMailStatus(objParam.MailID,objParam.MailStatus);
	elseif "S2C_MailModules_DeleteAllOpenMailButNoReward"==strMessageName then

		--this.GetAllOpenMailButNoReward(objParam);
		this.OnHandleGetMailList(objParam);

		LuaUIManager.ToastTip("快速删除成功",2,30);

	elseif "S2C_MailModules_GetAllRewardMail"==strMessageName then
		this.GetAllRewardButNoGet(objParam);
	end

end



--	快速删除：删除已读邮件
function MailPanelCtrl.OnQuickDelMail()

	if(EventExist("C2S_MailModules_DeleteAllOpenMailButNoReward"))then

		es("C2S_MailModules_DeleteAllOpenMailButNoReward");

	end

end

--	快速领取：快速领取所有带有奖励内容的邮件
function MailPanelCtrl.OnQuickGetReward()


	if(EventExist("C2S_MailModules_GetAllRewardMail"))then

		es("C2S_MailModules_GetAllRewardMail");

	end


end



function MailPanelCtrl.GetAllRewardButNoGet(objParam,mailStatus)

end

function MailPanelCtrl.SetSingleMailStatus(mailID,mailStatus)

    itemview= this:GetItemViewByID(mailID);
    if itemview~=nil then

        if(mailStatus==Define.E_EmailStatus.ES_READ)then
            itemview:InitMailStatus(true);
        elseif mailStatus==Define.E_EmailStatus.ES_RECEIVE then
            itemview:InitMailStatus(false);
		elseif mailStatus==Define.E_EmailStatus.ES_REWARD then
			itemview:InitMailStatus(true);
			--MessageBoxManager
			--MessageBoxManager:ShowModalBox("OneMessageOneButtonPanel",{
			--	["Content"]={PrefabPath="txt_Info",Content="你已经领取奖励！" },
			--	["Button1Param"]=
			--	{
			--		["Desc"]="",
			--		["Callback"]=function()
			--		end
			--	, ["PrefabPath"]="btn_OK",
			--	}
			--}
			--, "PopupCanvas");
		elseif mailStatus==Define.E_EmailStatus.ES_DISCARD then
			--itemview
		end

    end

end



function MailPanelCtrl.SetMultiMailStatus(tblmailID,mailStatus)

    --error("SetMultiMailStatus"..mailStatus);

    if tblmailID==nil then
        return;
    end

	for k,v in pairs(tblmailID) do
		local mailID=v;
		local itemview= this:GetItemViewByID(mailID);
		if itemview~=nil then
			if(mailStatus==Define.E_EmailStatus.ES_READ)then
				itemview:InitMailStatus(true);
			elseif mailStatus==Define.E_EmailStatus.ES_RECEIVE then
				itemview:InitMailStatus(false);
			elseif mailStatus==Define.E_EmailStatus.ES_REWARD then
				itemview:InitMailStatus(true);
			elseif mailStatus==Define.E_EmailStatus.ES_DISCARD then
				--itemview
			end
		end
	end

--	MessageBoxManager
	if(mailStatus==Define.E_EmailStatus.ES_REWARD) then
		MessageBoxManager:ShowModalBox("OneMessageOneButtonPanel",{
			["Content"]={PrefabPath="txt_Info",Content="你已经领取奖励！" },
			["Button1Param"]=
			{
				["Desc"]="",
				["Callback"]=function()
				end
			, ["PrefabPath"]="btn_OK",
			}
		}
		, "PopupCanvas");

	end



end



function MailPanelCtrl.OnHandleGetTableDataByTabType(objParam)

	print("OnHandleGetTableDataTabType");

	local tbldata=objParam;
	if(tbldata~=nil)then
		tbldata=this.StrpDiscardMail(tbldata);
		print("Init List:"..tostring(#tbldata));--[1].title);
		MailPanelView.LoopListObj:Data(tbldata);
	end

end

function MailPanelCtrl.OnHandleTabSelected()

	print("OnHandleTabSelected");

	strTabType=this.CurrentMailEntry();

	if strTabType==Define.E_EmailType.ET_SYSTEM then-- "System" then
		es("C2S_MailModules_GetMailTableDataByTabType",1);
	elseif strTabType== Define.E_EmailType.ET_SERVICE then-- "Service"then
		es("C2S_MailModules_GetMailTableDataByTabType",2);
	end

end


function MailPanelCtrl.OnHandleSetSelectedState(trans,bselected)
	if(trans~=nil)then
	local loopItemInUnity=trans:GetComponent(typeof(CS.LuaLoopItem));
	if(loopItemInUnity~=nil) then

		local tblItemView=loopItemInUnity:GetLTItem();
		if(tblItemView==nil)then
			return;
		end

		tblItemView:InitSelectState(bselected);

	end
	end
end

function MailPanelCtrl.OnHandleSetMailItemContent(tblParam)
	print("OnHandleSetMailItemContent");

	local loopItemInUnity=tblParam.Transform:GetComponent(typeof(CS.LuaLoopItem));
	if(loopItemInUnity~=nil) then
		print("LoopItemInUnity is exist");
		local tblItemView=loopItemInUnity:GetLTItem();
		if(tblItemView==nil)then

			tblItemView=MailPanelItemView:New(tblParam.Transform,tblParam.Data.id);
			table.insert(this.tblMailPanelItemView, tblItemView);
			loopItemInUnity:SetLTItem(tblItemView);

			if(tblItemView.Btn_Item~=nil)then
				tblItemView.Btn_Item.onClick:AddListener
				(
						function()
							print("Clk Mail:"..tblItemView.ID);
							this:OnMailItemClk(tblItemView.ID);
						end
				)
			end

		end
		print("item title:"..tblParam.Data.title);

		tblItemView:InitTitle(tblParam.Data.title);

		tblItemView:SetID(tblParam.Data.id);

		print("item time:"..tblParam.Data.time);
		tblItemView:InitTime(tblParam.Data.time);
        print("item status:"..tblParam.Data.status);

		if( tblParam.Data.status==Define.E_EmailStatus.ES_READ)then
			tblItemView:InitMailStatus(true);
		elseif tblParam.Data.status==Define.E_EmailStatus.ES_RECEIVE then
			tblItemView:InitMailStatus(false);
		elseif tblParam.Data.status==Define.E_EmailStatus.ES_REWARD then
			tblItemView:InitMailStatus(true);
		elseif tblParam.Data.status==Define.E_EmailStatus.ES_DISCARD then
			tblItemView:InitMailStatus(true);
		end

	end

end

function MailPanelCtrl:OnMailItemClk(nID)
	local itemview=self:GetItemViewByID(nID);
	if itemview~=nil then

		print("ItemView is Exist."..itemview.ID);

		LuaUIManager.register('MailContentPanel',{
			name = 'MailContentPanel',
			layer = 'PopupCanvas',
			path = 'lua/modules/Lobby/modules/Mail/MailPanelItemContentCtrl.lua'
		})

		LuaUIManager.open("MailContentPanel",nil,nID);

	end
end

function MailPanelCtrl:GetItemViewByID(nID)
	print("ID is:"..nID);

	for k,v in pairs(self.tblMailPanelItemView) do

		--print("Table id:"..v.ID)
		if(v.ID==nID)then
			return v;
		end

	end

	return nil;

end

function MailPanelCtrl.OnHandleGetMailList(tabMailList)

	local strCurMailType=this.CurrentMailEntry();
	local tbldata={};
	if strCurMailType==Define.E_EmailType.ET_SYSTEM then-- "System" then
		print("Is System Mail Type");
		tbldata=tabMailList.SystemMailList;
	else
		print("Is Service Mail Type");
		tbldata=tabMailList.ServiceMailList;
	end

	if(tbldata~=nil)then

		tbldata=this.StrpDiscardMail(tbldata);
		print("Init List:"..tostring(#tbldata));
		MailPanelView.LoopListObj:Data(tbldata);

	end

end

function MailPanelCtrl.StrpDiscardMail(tbldata)
	local rettbl={};
	for k,v in pairs(tbldata) do
		if(v.status~=Define.E_EmailStatus.ES_DISCARD)then
			table.insert(rettbl,v);
		end
	end
	return rettbl;
end

function MailPanelCtrl.CurrentMailEntry()

	if(MailPanelView.TabEntry_System_Service.gameObject.activeSelf==true)then
		return Define.E_EmailType.ET_SERVICE;-- "Service";
	else
		return Define.E_EmailType.ET_SYSTEM;--"System";
	end

end

function ondestroy()

--	EventManager.HandlerInfo();

--	print("MailPanelCtrl:Ondestroy:".. tostring(MailPanelCtrl.HandleMessage));

	er("Global2Ctrl_MailModules_GetMailList",MailPanelCtrl.HandleMessage);

	er("S2C_MailModules_GetMailList",MailPanelCtrl.HandleMessage);

	er("Global2Ctrl_MailModules_SetMailItemSelected",MailPanelCtrl.HandleMessage);

	er("Global2Ctrl_MailModules_SetMailItemContent",MailPanelCtrl.HandleMessage);

	er("S2C_MailModules_GetMailTableDataByTabType",MailPanelCtrl.HandleMessage);

	er("Global2Ctrl_MailModules_TheTabSelected",MailPanelCtrl.HandleMessage);

    er("S2C_MailModules_SetSingleMailStatus",MailPanelCtrl.HandleMessage);

	er("S2C_MailModules_DeleteAllOpenMailButNoReward",MailPanelCtrl.HandleMessage);

	er("S2C_MailModules_GetAllRewardMail",MailPanelCtrl.HandleMessage);


	MailPanelCtrl=nil;

--	EventManager.HandlerInfo();

end

 return MailPanelCtrl;