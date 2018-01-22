--当前视图
local view = require 'lua/modules/Lobby/modules/Bag/BagPanelView'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
local msgCmd2 = GameState.curRunState.MsgDefine.ZLobbyModuleCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.backbutton.onClick:AddListener(function()
		uimanager.CloseWindow('BagPanel')
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
	el(LobbyEventConst.Bag_TabChange,on_event)
	el(LobbyEventConst.Bag_HelpPress,on_msg)
	el(LobbyEventConst.Bag_HelpRelease,on_msg)
end	

local datas = {
	{{'zs1','zs1'},{'zs2','zs2'},{'zs2','zs2'},{'zs3','zs3'},{'zs4','zs4'},{'zs5','zs5'},{'zs6','zs6'},{'zs7','zs7'},{'zs8','zs8'},{'zs9','zs9'},{'zs10','zs10'},{'zs11','zs11'},
	{'zs1','zs1'},{'zs2','zs2'},{'zs2','zs2'},{'zs3','zs3'},{'zs4','zs4'},{'zs5','zs5'},{'zs6','zs6'},{'zs7','zs7'},{'zs8','zs8'},{'zs9','zs9'},{'zs10','zs10'},{'zs11','zs11'},
	{'zs1','zs1'},{'zs2','zs2'},{'zs2','zs2'},{'zs3','zs3'},{'zs4','zs4'},{'zs5','zs5'},{'zs6','zs6'},{'zs7','zs7'},{'zs8','zs8'},{'zs9','zs9'},{'zs10','zs10'},{'zs11','zs11'}
	},
	{{'jb1','jb1'},{'jb2','zs2'},{'jb2','zs2'},{'jb3','zs3'},{'jb4','zs4'},{'jb5','zs5'},{'jb6','jb6'},{'jb7','jb7'},{'jb8','jb8'},{'jb9','jb9'},{'jb10','jb10'},{'jb11','jb11'}
	}
}


function start()
	view.tabgroup:SelectByIndex(0)
end

function ondestroy()

	--移除消息监听
	-- mr(msgCmd2.SC_SetRankList,on_msg)
	-- mr(msgCmd.MessageNotify,on_msg)
	--移除事件监听
	--er(event.name,on_event)
	er(LobbyEventConst.Bag_TabChange,on_event)
	er(LobbyEventConst.Bag_HelpPress,on_msg)
	er(LobbyEventConst.Bag_HelpRelease,on_msg)

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end

function initdata()

end


function update_info(index)
	local data = datas[index+1]
	if index == 0 then
		view:set_state('good_state')
		view.goodlist:Data(data)
		view.goodlist_selectgroup.Index = 0
	elseif index == 1 then
		view:set_state('backup_state')
		view.backuplist:Data(data)
		view.backuplist_selectgroup.Index = 0
	end
end


function update_tipview_position()
	local press_position = CS.UnityEngine.Input.mouseposition
	print('x '.. press_position.x..' y '..press_position.y)
	view.tipview.position = press_position
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
	if(event == LobbyEventConst.Bag_TabChange) then
		update_info(param)
	elseif(event == LobbyEventConst.Bag_HelpPress) then
		view:set_state('tip_show')
		update_tipview_position()
	elseif(event == LobbyEventConst.Bag_HelpRelease) then
		view:set_state('tip_hide')
	end
end		