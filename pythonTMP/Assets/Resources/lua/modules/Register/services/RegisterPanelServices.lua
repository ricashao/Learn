RegisterPanelServices={};

local this = RegisterPanelServices;

MsgDefine = require 'lua/game/Define'

function RegisterPanelServices.Init(MsgCmd,DataObj)
	
	this.MsgCmd=MsgCmd;
	this.DataObj=DataObj;
	el("C2S_RegisterModule_GoRegister",RegisterPanelServices.OnEvent)
	
	ml(MsgCmd.SC_LoginSuccess,RegisterPanelServices.on_msg)
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
	GameState.tcpClinet.sendmsg( this.MsgCmd.CS_RegistByPhone,CS_RegistByPhone)
end

function RegisterPanelServices.OnEvent(eventName,Param)
	print("Services OnEvent");
	if(eventName=="C2S_RegisterModule_GoRegister") then
		
		UserID=this.DataObj.GetUserID();
		
		if UserID~=nil then
			print("User ID:"..UserID);
		else
			print("User ID is nil");
		end
		
		
		RegisterPanelServices.send_CS_RegistByPhone(Param.Account,Param.CheckCode,Param.Password,UserID);
		
	end
	
end


function RegisterPanelServices.on_msg(key,decode)
	
	if key == this.MsgCmd.SC_LoginSuccess then

		this.DataObj.SC_LoginSuccess = decode;
		
		print("Login Success"..decode.user.id);
	elseif key==sib(10000) then
		print("Back Message"..decode.type..decode.pid);
		if (decode.type=="INF0_REGIST_SUCCESS") then
			
		end
	end
	
end 





return RegisterPanelServices

