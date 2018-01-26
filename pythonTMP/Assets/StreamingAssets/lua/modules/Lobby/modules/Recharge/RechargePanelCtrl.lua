--当前视图
local view = require 'lua/modules/Lobby/modules/Recharge/RechargePanelView'
--本模块消息号
-- local msgCmd = GameState.curRunState.MsgDefine.LobbyCmd
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local ZEntryModuleData = require 'lua/datamodules/ZEntryModuleData'
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	view.closebutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.CloseWindow('RechargePanel')
	end)
	view.alipaybutton.onClick:AddListener(function()
	end)
	view.wechatbutton.onClick:AddListener(function()
	end)
	view.qqpursebutton.onClick:AddListener(function()
	end)
	view.bankbutton.onClick:AddListener(function()
	end)
	--AddEventCode 追加事件标志

	--PanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 
		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

	--消息监听
	--ml(msgCmd.user_para,on_msg)
	ml(msgCmd.MessageNotify,on_msg)
	--事件监听
	--el(event.name,on_event)
end	
function start()

end

function ondestroy()

	--移除消息监听
	--mr(msgCmd.user_para,on_msg)
	mr(msgCmd.MessageNotify,on_msg)
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
	print(" recharge on_msg >> "..key)
	if(key == msgCmd.MessageNotify)then
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)

end		