local RegisterPanelview = require 'lua/modules/Register/View/RegisterPanelview'
--本模块消息号
--local msgCmd = GameState.curRunState.MsgDefine.ZEntryModuleCmd
--本模块数据层
--local data = GameState.curRunState.Data.ZEntryModuleData

RegisterPanelCtrl={};

local this=RegisterPanelCtrl;

LuaUIManager = require 'lua/game/LuaUIManager'

function awake()
	
	RegisterPanelview:init(self.transform);
	
	el("V2C_RegisterModule_GoRegister",RegisterPanelCtrl.OnEvent);
	
	el("S2C_RegisterModule_RegisterSuccess",RegisterPanelCtrl.OnEvent);
	
	el("S2C_RegisterModule_RegisterFailed",RegisterPanelCtrl.OnEvent);
	
end	
--事件处理函数
function on_button_click(event,textp)
	
end
--事件处理函数
function on_click(event,textp)

	
end	

--消息处理函数
function on_msg(key,decode)

	
end		

function fun(param)
	print(param);
end 

function start()

end

function update()

end

function ondestroy()
	
	er("V2C_RegisterModule_GoRegister",RegisterPanelCtrl.OnEvent);
	
	er("S2C_RegisterModule_RegisterSuccess",RegisterPanelCtrl.OnEvent);
	
	er("S2C_RegisterModule_RegisterFailed",RegisterPanelCtrl.OnEvent);
	
end

function RegisterPanelCtrl.OnEvent(event,param)
	
	if(event=="V2C_RegisterModule_GoRegister") then
		
		if(param~=nil) then
			es("C2S_RegisterModule_GoRegister",param);
		end
		
	elseif event=="S2C_RegisterModule_RegisterSuccess" then
		
		print("S2C_RegisterModule_RegisterSuccess");
		
		LuaUIManager.ToastTip
		(
		"你已经注册成功!",
		3,
		30
		);
		
		LuaUIManager.CloseWindow("RegisterPanel");
		
		es("C2C_RegisterModule_RegisterSuccess",param);
		
		print("Toast over");
		
	elseif event=="S2C_RegisterModule_RegisterFailed" then
		LuaUIManager.ToastTip
		(
		"注册失败!",
		3,
		30
		);
		
	end
	
	
	
	
	
end		