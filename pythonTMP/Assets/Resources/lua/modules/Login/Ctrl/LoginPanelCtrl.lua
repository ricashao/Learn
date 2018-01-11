local LoginPanelview = require 'lua/modules/Login/View/LoginPanelview'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.ZEntryModuleCmd
--本模块数据层
local data = GameState.curRunState.Data.ZEntryModuleData

function awake()
	
	LoginPanelview:init(self.transform);
	
	el("TryGuestLogin",OnLoginEvent);
	
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
	
end

function OnLoginEvent(event,param)

	
	print(" on_event >> "..event);
	if param~=nil then
		
		print(" data_param >> "..param.Token);
		
		data.send_CS_Login(param.Token);
	elseif event == EventConst.CLOSELOGINPANEL then
		LoginPanelview:set_state('close')
	end
	
end		