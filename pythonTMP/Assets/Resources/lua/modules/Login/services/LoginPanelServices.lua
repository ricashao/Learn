LoginPanelServices={};

local this = LoginPanelServices;



function LoginPanelServices.Init(MsgCmd,DataObj)
	
	this.MsgCmd=MsgCmd;
	
	this.DataObj=DataObj;
	
	el("C2S_LoginModule_LoginByPhoneID",LoginPanelServices.OnEvent);
	
	el("C2S_LoginModule_LoginByGuest",LoginPanelServices.OnEvent);

	ml(this.MsgCmd.SC_LoginSuccess,LoginPanelServices.OnNetMessage)
	
end	

function LoginPanelServices.OnEvent(msgName,param)

	if msgName=="C2S_LoginModule_LoginByPhoneID" then
		if param~=nil then
			
			print("Account :"..param["phoneNumber"]);
			
			print("Password :"..param["password"]);
			
			newmd5password=CS.Md5Tools.md5(param["password"]);
			
			LoginPanelServices.send_CS_LoginByAccount(param["phoneNumber"],newmd5password);
			
		end
	elseif msgName=="C2S_LoginModule_LoginByGuest" then
		if param~=nil then
			
			LoginPanelServices.send_CS_Login(param);
			
		end
	end
end

function LoginPanelServices.send_CS_Login(token)
	local CS_Login = {}
	CS_Login.token =CS.Md5Tools.md5(token);
	GameState.tcpClinet.sendmsg(this.MsgCmd.CS_Login,CS_Login)
end

function LoginPanelServices.send_CS_LoginByAccount(account,password)
	local CS_LoginByAccount = {}
	CS_LoginByAccount.account = account
	CS_LoginByAccount.password = password
	GameState.tcpClinet.sendmsg(this.MsgCmd.CS_LoginByAccount,CS_LoginByAccount)
end

function LoginPanelServices.OnNetMessage(key,decode)
	
	if key == this.MsgCmd.SC_LoginSuccess then

		this.DataObj.SC_LoginSuccess = decode
		
		es("S2C_LoginModule_LoginSuccess",decode);

	end
	
	
end

return LoginPanelServices

