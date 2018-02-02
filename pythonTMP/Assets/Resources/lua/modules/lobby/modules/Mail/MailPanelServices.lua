MailPanelServices={};

--local E_EmailStatus=
--{
--	ES_RECEIVE = "ES_RECEIVE",
--	ES_READ    = "ES_READ",
--	ES_REWARD  = "ES_REWARD",
--	ES_DISCARD = "ES_DISCARD",
--}


local this = MailPanelServices;

function MailPanelServices.Init(MsgCmd,DataObj)
	
	this.MsgCmd=MsgCmd;
	
	this.DataObj=DataObj;

	ml(this.MsgCmd.SC_SetEmail,MailPanelServices.OnNetMessageCallback);

	ml(this.MsgCmd.SC_SetEmailStatus,MailPanelServices.OnNetMessageCallback);

	el("C2S_MailModules_GetMailList",MailPanelServices.HandleMessage);

	el("C2S_MailModules_GetMailDataByID",MailPanelServices.HandleMessage);

	el("C2S_MailModules_GetMailTableDataByTabType",MailPanelServices.HandleMessage);

	el("C2S_MailModules_SetSingleMailStatus",MailPanelServices.HandleMessage);

	el("C2S_MailModules_DeleteAllOpenMailButNoReward",MailPanelServices.HandleMessage);

	el("C2S_MailModules_GetAllRewardMail",MailPanelServices.HandleMessage);



end

function MailPanelServices:GetMailLst()

	local mailRequest={};

	ms(this.MsgCmd.CS_GetEmail,mailRequest);

end

function MailPanelServices:SetMailListStatus(tblMailID,mailStatus)
	local sndMailStatus={};
	sndMailStatus.ids=tblMailID;
	sndMailStatus.status=LobbyEventConst.E_EmailStatus[mailStatus];-- E_EmailStatus[mailStatus];
	print("Snd Status:"..sndMailStatus.status);
	ms(this.MsgCmd.CS_SetEmailStatus,sndMailStatus);
end

function MailPanelServices:SetMailStatus(MailID,mailStatus)

	local mailobj=this.GetDataByID(MailID);
	if mailobj~=nil then
		if mailobj.status~=mailStatus then
			--mailobj.status=mailStatus;
			local tblmailID={};
			table.insert(tblmailID,MailID);
			this:SetMailListStatus(tblmailID,mailStatus);
			--es("S2C_MailModules_SetSingleMailStatus",{["MailID"]=MailID,["MailStatus"]=mailStatus});
		end
	end

end


function MailPanelServices.OnNetMessageCallback(strMessageHead,objMessageObj)

	print("Message Head:"..strMessageHead);

	if strMessageHead==this.MsgCmd.SC_SetEmail then
		print("Mail Request Callback,Mail Total Have:"..tostring(#objMessageObj.emails) );

		if(#objMessageObj.emails>=2)then

			--RewardServices= require 'lua/modules/Lobby/modules/Mail/RewardServices';
            --
			--RewardServices.AddReward(objMessageObj.emails[2].rewards);

			print("Email Status:"..objMessageObj.emails[1].rewards);

			print("Email Status:"..objMessageObj.emails[2].rewards);

		end

		this.DataObj.MailListObject=objMessageObj.emails;

		this.DataObj.tblMailSystem,this.DataObj.tblMailService=this.ParseMailType(objMessageObj.emails);


		if(EventExist("S2C_MailModules_GetMailList")) then
			es("S2C_MailModules_GetMailList",
					{
						["SystemMailList"]=this.DataObj.tblMailSystem ,["ServiceMailList"]=this.DataObj.tblMailService
					}
			);
		end
	elseif strMessageHead==this.MsgCmd.SC_SetEmailStatus then

		print("Mail Request Callback,Mail Status Count:"..tostring(#objMessageObj.ids) );

		print("Mail Request Callback,Mail Status:"..tostring(objMessageObj.status) );

		this.S2C_SetMailListStatus(objMessageObj.ids,objMessageObj.status);

	end


end

function MailPanelServices.S2C_SetMailListStatus(tblMail,mailStatus)
	if(mailStatus~=LobbyEventConst.E_EmailStatus.ES_DISCARD)then
		for k,v in pairs(tblMail) do

			local mailobj=this.GetDataByID(v);
			if mailobj~=nil then
				mailobj.status=mailStatus;
			end
		end

		if(EventExist("S2C_MailModules_SetSingleMailStatus")) then
			EventManager.Brocast("S2C_MailModules_SetSingleMailStatus",{["MailID"]=tblMail,["MailStatus"]=mailStatus});
			--es("S2C_MailModules_SetSingleMailStatus",{["MailID"]=tblMail,["MailStatus"]=mailStatus});
		end

	else
		for k,v in pairs(tblMail) do
			mailobj=this.GetDataByID(v);
			if mailobj~=nil then
				mailobj.status=mailStatus;
			end
		end

		this.DataObj.tblMailSystem,this.DataObj.tblMailService=this.ParseMailType(this.DataObj.MailListObject);-- objMessageObj.emails);


		if(EventExist("S2C_MailModules_DeleteAllOpenMailButNoReward")) then
			es("S2C_MailModules_DeleteAllOpenMailButNoReward",
					{
						["SystemMailList"]=this.DataObj.tblMailSystem ,["ServiceMailList"]=this.DataObj.tblMailService
					}
			);
		end



	end

end

function MailPanelServices.GetTableDataByType(ntype)
	print("Get Table Data");

	if ntype==1 then
		return this.DataObj.tblMailSystem;
	elseif ntype==2 then
		return this.DataObj.tblMailService;
	end
	return nil;
end

function MailPanelServices.ParseMailType(tblMailList)

	local tblSystemMail={};
	local tblService={};
	for k,v in pairs(tblMailList) do
		if(v.type==LobbyEventConst.E_EmailType.ET_SYSTEM)then
			table.insert(tblSystemMail,v);
		elseif v.type==LobbyEventConst.E_EmailType.ET_SERVICE then
			table.insert(tblService,v);
		end
	end

	return tblSystemMail,tblService

end

function MailPanelServices.HandleMessage(strMessageName,objMessage)

	if(strMessageName=="C2S_MailModules_GetMailList")then
		print("Services:GetMailList");
		this.GetMailLst();
	elseif strMessageName=="C2S_MailModules_GetMailDataByID" then
		dataobj=this.GetDataByID(objMessage);
		print(dataobj.id);
		es("S2C_MailModules_GetDataByID",dataobj);
	elseif "C2S_MailModules_GetMailTableDataByTabType"==strMessageName then
		dataTableRet=this.GetTableDataByType(objMessage);
		--EventDispatcher:dispatchEvent("S2C_MailModules_GetMailTableDataByTabType","S2C_MailModules_GetMailTableDataByTabType", dataTableRet);
		es("S2C_MailModules_GetMailTableDataByTabType",dataTableRet);
	elseif "C2S_MailModules_SetSingleMailStatus"==strMessageName then
		this:SetMailStatus(objMessage.MailID,objMessage.MailStatus);
		--this.SetMailListStatus(objMessage.MailID,objMessage.MailStatus);
	elseif "C2S_MailModules_DeleteAllOpenMailButNoReward"==strMessageName then
		local tblret=this.GetAllOpenMailButNoReward();

		local tblid={};

		if #tblret >0 then

			for k,v in pairs(tblret) do
				--this.DeleteMail(v);
				table.insert(tblid,v.id);
			end

			this:SetMailListStatus(tblid,LobbyEventConst.E_EmailStatus.ES_DISCARD);

		end

	elseif "C2S_MailModules_GetAllRewardMail"==strMessageName then

		local tblret=this.GetAllNoGetRewardMail();

		local tblid={};

		if #tblret >0 then

			for k,v in pairs(tblret) do
				--this.DeleteMail(v);
				table.insert(tblid,v.id);
			end

			this:SetMailListStatus(tblid,LobbyEventConst.E_EmailStatus.ES_REWARD);

		end

	end

end

function MailPanelServices.DeleteMail(objdata)
	removebyvalue(this.DataObj.MailListObject,objdata)
end

function MailPanelServices.GetAllOpenMailButNoReward()
	local retmail={};
	for k,v in pairs(this.DataObj.MailListObject) do
		if 		v.rewards==nil or
				v.rewards=="" or
				v.rewards=="0"
		then
			if(v.status==LobbyEventConst.E_EmailStatus.ES_READ) then
				table.insert(retmail,v);
			end
		else
			if(v.status==LobbyEventConst.E_EmailStatus.ES_REWARD) then
				table.insert(retmail,v);
			end
		end
	end
	return retmail;
end


function MailPanelServices.GetAllNoGetRewardMail()
	local retmail={};
	for k,v in pairs(this.DataObj.MailListObject) do
		if 		v.rewards~=nil and
				v.rewards~=""  and
				v.rewards~="0"
		then

			if(v.status~=LobbyEventConst.E_EmailStatus.ES_REWARD and v.status~=LobbyEventConst.E_EmailStatus.ES_DISCARD ) then
				table.insert(retmail,v);
			end

		end
	end
	return retmail;
end



function MailPanelServices.Clear()

	mr(this.MsgCmd.SC_SetEmail,MailPanelServices.OnNetMessageCallback);

	mr(this.MsgCmd.SC_SetEmailStatus,MailPanelServices.OnNetMessageCallback);

	er("C2S_MailModules_GetMailList",MailPanelServices.HandleMessage);

	er("C2S_MailModules_GetMailDataByID",MailPanelServices.HandleMessage);

	er("C2S_MailModules_GetMailTableDataByTabType",MailPanelServices.HandleMessage);

	--er("C2S_MailModules_SetMailListStatus",MailPanelServices.HandleMessage);

	er("C2S_MailModules_SetSingleMailStatus",MailPanelServices.HandleMessage);

	er("C2S_MailModules_DeleteAllOpenMailButNoReward",MailPanelServices.HandleMessage);

	er("C2S_MailModules_GetAllRewardMail",MailPanelServices.HandleMessage);


end

function MailPanelServices.GetDataByID(nid)

	for k,v in pairs(this.DataObj.MailListObject) do
		if v.id==nid then
			return v;
		end
	end
	return nil;
end



return MailPanelServices;

