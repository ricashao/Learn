--当前视图
local view = require 'lua/modules/$ModuleMame$/modules/$SubModuleMame$/$PanelName$View'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.$ModuleMame$Cmd
--本模块数据层
local data = GameState.curRunState.Data.$SubModuleMame$Data

function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	--$PanelName$View.returnButton:GetComponent("Button").onClick:AddListener(function() 
		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

	--消息监听
	--ml(msgCmd.user_para,on_msg)
	--事件监听
	--el(event.name,on_event)
end	

function start()

end

function ondestroy()

	--移除消息监听
	--mr(msgCmd.user_para,on_msg)
	--移除事件监听
	--er(event.name,on_event)

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end

function initdata()

end

--消息处理函数
function on_msg(key,decode)
	print(" on_msg >> "..key)

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)

end		