UserInfoPanelServices={};

local this = UserInfoPanelServices;

MsgDefine = require 'lua/game/Define'

function UserInfoPanelServices.Init()
	
	-- this.MsgCmd=MsgCmd;
	-- this.DataObj=DataObj;
	
	-- ml(MsgCmd.SC_LoginSuccess,UserInfoPanelServices.on_msg)
	
end	



function UserInfoPanelServices.on_msg(key,decode)
	
	-- if key == this.MsgCmd.SC_LoginSuccess then

		-- this.DataObj.SC_LoginSuccess = decode;
		
		-- print("Login Success"..decode.user.id);
	
	-- elseif key==sib(10000) then
		-- print("Back Message"..decode.type..decode.pid);
		-- if (decode.type=="INF0_REGIST_SUCCESS") then
			
		-- end
	-- end
	
end 





return UserInfoPanelServices

