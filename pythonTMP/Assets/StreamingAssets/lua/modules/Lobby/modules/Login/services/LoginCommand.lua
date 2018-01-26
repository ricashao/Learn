LoginCommand={};

local this = LoginCommand;

LocalDataManager=require 'lua/LocalDataManager'



function LoginCommand.Excute(onOverCallback)
	
	if LocalDataManager:IsExistUserInfo()==false then
		print("UserInfo is Not Exist!");
		uniqueToken=CS.ZhuYuU3d.Platform.CallManager.GetDeviceUnqueID ();
		es("C2S_LoginModule_LoginByGuest",uniqueToken);
	else
		acc,pas=LocalDataManager.GetAccountAndPassword();
		es(
			"C2S_LoginModule_LoginByPhoneID",
			{["phoneNumber"]=acc , ["password"]=pas}
			);
	end

	if(onOverCallback~=nil)then
		onOverCallback();
	end
	
	
	
end	



return LoginCommand

