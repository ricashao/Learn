RegisterPanelServices={};

local this = RegisterPanelServices;

MsgDefine = require 'lua/game/Define'

function RegisterPanelServices.Init(MsgCmd,DataObj)
	
	this.MsgCmd=MsgCmd;
	this.DataObj=DataObj;
	el("C2S_RegisterModule_GoRegister",RegisterPanelServices.OnEvent);
	
	el("C2S_RegisterModule_GoBindUser",RegisterPanelServices.OnEvent);
	
	--ml(MsgCmd.SC_LoginSuccess,RegisterPanelServices.on_msg)
	ml(sib(10000),RegisterPanelServices.on_msg)
	
	
end	

function RegisterPanelServices.send_CS_RegistByPhone(phone,code,password,uid)
	local CS_RegistByPhone = {}
	CS_RegistByPhone.phone = phone
	CS_RegistByPhone.code = code

	newmd5password=CS.Md5Tools.md5(password);
	
	print("MD5 Pass:"..newmd5password);

	CS_RegistByPhone.password = newmd5password;
	CS_RegistByPhone.uid = uid
	CS_RegistByPhone.device = Platform

	this.DataObj.CS_RegisterByPhoneQuest=
	{
		["RegisterByPhoneQuest_PhoneNumber"]=phone,
		["RegisterByPhoneQuest_Code"]=code;
		["RegisterByPhoneQuest_Password"]=password;
		["RegisterByPhoneQuest_UserID"]=uid;
		["RegisterByPhoneQuest_IsRegisterSuccess"]=false;
	};
	
	GameState.tcpClinet.sendmsg( this.MsgCmd.CS_RegistByPhone,CS_RegistByPhone)
end

function RegisterPanelServices.OnEvent(eventName,Param)
	print("Services OnEvent");
	if(eventName=="C2S_RegisterModule_GoRegister") then
		
		UserID=0;
		
		if UserID~=nil then
			print("User ID:"..UserID);
		else
			print("User ID is nil");
		end
		
		
		RegisterPanelServices.send_CS_RegistByPhone(Param.Account,Param.CheckCode,Param.Password,UserID);
		
	elseif "C2S_RegisterModule_GoBindUser"==eventName then
		
		UserID = this.DataObj.GetUserID();
		
		if UserID~=nil then
			print("User ID:"..UserID);
		else
			print("User ID is nil");
		end
		
		
		RegisterPanelServices.send_CS_RegistByPhone(Param.Account,Param.CheckCode,Param.Password,UserID);
		
	end
	
end


function RegisterPanelServices.on_msg(key,decode)
	
	if key==sib(10000) then
	
		print("Back Message"..decode.type..decode.pid);
		
		if (decode.type=="INF0_REGIST_SUCCESS") then
		
			this.DataObj.CS_RegisterByPhoneQuest["RegisterByPhoneQuest_IsRegisterSuccess"]=true;
			
			es("S2C_RegisterModule_RegisterSuccess",this.DataObj.CS_RegisterByPhoneQuest);
			
		elseif (decode.type=="INF0_BIND_SUCCESS") then

--			this.DataObj.CS_RegisterByPhoneQuest["RegisterByPhoneQuest_IsRegisterSuccess"]=true;
			
			es("S2C_RegisterModule_BindUserSuccess",this.DataObj.CS_RegisterByPhoneQuest);
			
		else 
			if(EventExist("S2C_RegisterModule_RegisterFailed"))then
				es("S2C_RegisterModule_RegisterFailed");
			end
		end
		
		
	end
	
end 





return RegisterPanelServices

