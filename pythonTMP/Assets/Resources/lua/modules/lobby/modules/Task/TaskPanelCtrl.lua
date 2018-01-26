--当前视图
local view = require 'lua/modules/Lobby/modules/Task/TaskPanelView'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
local msgCmd2 = GameState.curRunState.MsgDefine.ZLobbyModuleCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.backbutton.onClick:AddListener(function()
		uimanager.CloseWindow('TaskPanel')
	end)
	
	view.testbutton.onClick:AddListener(function()
		local data = TaskPanelService.getDataInMDByMid(2000,2)
		ZLobbyModuleData.send_CS_ProcessTask('PT_STEP',data.id)
	end)
	--AddEventCode 追加事件标志

	--PanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 
		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

	--消息监听
	--ml(msgCmd.user_para,on_msg)
	--事件监听
	--el(event.name,on_event)
	el(LobbyEventConst.Task_TabChange,on_event)
	el(LobbyEventConst.Task_Update,on_event)
end	

function start()
	view.tabgroup:SelectByIndex(0)
end

function ondestroy()

	--移除消息监听
	-- mr(msgCmd2.SC_SetRankList,on_msg)
	-- mr(msgCmd.MessageNotify,on_msg)
	--移除事件监听
	--er(event.name,on_event)
	er(LobbyEventConst.Task_TabChange,on_event)
	er(LobbyEventConst.Task_Update,on_event)

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end

function initdata()

end


function update_info(index)
	if index == 0 then
		view:set_state('daily_state')
		local data = TaskPanelService.getCurDailyTask()
		view.dailylist:Data(data)
		view.dailylist_selectgroup.Index = 0
	elseif index == 1 then
		view:set_state('backup_state')
		local data = TaskPanelService.getCfgsByType(3)
		view.backuplist:Data(data)
		view.backuplist_selectgroup.Index = 0
	end
end

function task_update()
	local index = view.tabgroup.Index
	if index == 0 then
		view.dailylist:refreshWithoutPosChange()
	elseif index == 1 then
		view.backuplist:refreshWithoutPosChange()
	end
end


--消息处理函数
function on_msg(key,decode)
	print(" rank on_msg >> "..key)
	-- if(key == msgCmd2.SC_SetRankList) then
		-- if(decode.type == 'GOLD') then
			-- view:set_state('gold_state')
		-- elseif(decode.type == 'GEM') then
			-- view:set_state('gem_state')
		-- elseif (decode.type == 'TICKET') then
			-- view:set_state('ticket_state')
		-- else
		-- end
		-- update_info(decode)
	-- end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)
	if(event == LobbyEventConst.Task_TabChange) then
		update_info(param)
	end
	if(event == LobbyEventConst.Task_Update) then
		task_update()
	end
end		