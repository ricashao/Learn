--当前视图
local view = require 'lua/modules/Lobby/Lobby/LobbyPanelView'
--本模块消息号
--local msgCmd = GameState.curRunState.MsgDefine.LobbyCmd
--本模块数据层
--local data = GameState.curRunState.Data.LobbyData

function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	--LobbyPanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 
		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

	view.HaedImageButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..HaedImageButton)
	end)
	view.NumberAddButton0:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..NumberAddButton0)
	end)
	view.NumberAddButton1:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..NumberAddButton1)
	end)
	view.NumberAddButton2:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..NumberAddButton2)
	end)
	view.SettingButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..SettingButton)
	end)
	view.KefuButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..KefuButton)
	end)
	view.BottomLeftButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomLeftButton)
	end)
	view.BottomRightButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomRightButton)
	end)
	view.BottomButton0:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomButton0)
	end)
	view.BottomButton1:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomButton1)
	end)
	view.BottomButton2:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomButton2)
	end)
	view.BottomButton3:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomButton3)
	end)
	view.BottomButton4:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomButton4)
	end)
	view.BottomButton5:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..BottomButton5)
	end)
	view.IntoGameButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:IntoGameButton')
	end)
	--AddEventCode 追加事件标志

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