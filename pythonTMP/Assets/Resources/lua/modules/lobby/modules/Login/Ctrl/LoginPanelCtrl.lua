LoginPanelCtrl={};

this=LoginPanelCtrl;

local LoginPanelview = require 'lua/modules/Lobby/modules/Login/View/LoginPanelView'
							
LuaUIManager = require 'lua/game/LuaUIManager'

--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.ZEntryModuleCmd
--本模块数据层
local data = GameState.curRunState.Data.ZEntryModuleData

function awake()
	
	LoginPanelview:init(self.transform);
	
	el("TryGuestLogin",LoginPanelCtrl.OnEvent);
	
	el("C2C_RegisterModule_RegisterSuccess",LoginPanelCtrl.OnEvent);
	
	el("V2C_LoginModule_LoginByPhone",LoginPanelCtrl.OnEvent);
	
	el("S2C_LoginModule_LoginSuccess",LoginPanelCtrl.OnEvent);
	
end	

function start()

end

function update()

end

function ondestroy()
	
	er("TryGuestLogin",LoginPanelCtrl.OnEvent);
	
	er("C2C_RegisterModule_RegisterSuccess",LoginPanelCtrl.OnEvent);
	
	er("V2C_LoginModule_LoginByPhone",LoginPanelCtrl.OnEvent);
	
	er("S2C_LoginModule_LoginSuccess",LoginPanelCtrl.OnEvent);
	
	
	
end

function LoginPanelCtrl.OnEvent(event,param)

	print(" on_event >> "..event);
	
	if (event=="TryGuestLogin") then
		
		print(" data_param >> "..param.Token);
		
		es("C2S_LoginModule_LoginByGuest",param.Token);
		--data.send_CS_Login(param.Token);
		
	elseif (event=="C2C_RegisterModule_RegisterSuccess") then
		
		if (param~=nil) then
			
			print("Phone Number:"..param["RegisterByPhoneQuest_PhoneNumber"] );
			print("Phone Password:"..param["RegisterByPhoneQuest_Password"]);
			
			LoginPanelview.setAccountLabel(param["RegisterByPhoneQuest_PhoneNumber"]);
			
			LoginPanelview.setPassLabel(param["RegisterByPhoneQuest_Password"]);
			
			LocalDataManager.SaveAccountAndPassword(param["RegisterByPhoneQuest_PhoneNumber"],
			param["RegisterByPhoneQuest_Password"]);
		end
		
	elseif event=="V2C_LoginModule_LoginByPhone" then
		
		if(param~=nil)then
			es("C2S_LoginModule_LoginByPhoneID",param);
		end
		
	elseif event=="S2C_LoginModule_LoginSuccess" then
		
		LuaUIManager.ToastTip("登陆成功!",3,30);
		
		LuaUIManager.CloseWindow("LoginPanel");

		if(EventExist("C2C_LoginModule2SettingModule_Close"))then

			es("C2C_LoginModule2SettingModule_Close")

		end
		
		
		
	end
	
end		