--当前视图
local view = require 'lua/modules/Lobby/modules/Bag/BagPanelView'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
local msgCmd2 = GameState.curRunState.MsgDefine.ZLobbyModuleCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
local tipview_rect
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.backbutton.onClick:AddListener(function()
		uimanager.CloseWindow('BagPanel')
	end)
	
	tipview_rect = view.tipview.rect
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
	el(LobbyEventConst.Bag_HelpPress,on_event)
	el(LobbyEventConst.Bag_HelpRelease,on_event)
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
	er(LobbyEventConst.Bag_TabChange,on_event)
	er(LobbyEventConst.Bag_HelpPress,on_event)
	er(LobbyEventConst.Bag_HelpRelease,on_event)

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end

function initdata()

end


function update_info(index)
	if index == 0 then
		view:set_state('good_state')
		view.goodlist:Data(getPairsTable(CommonData.bags or {},sortFunc))
		view.goodlist_selectgroup.Index = 0
	elseif index == 1 then
		view:set_state('backup_state')
		view.backuplist:Data({})
		view.backuplist_selectgroup.Index = 0
	end
end

function sortFunc(a,b)
	local acfg = GoodsConfigs.getItemByID(a.mid)
	local bcfg = GoodsConfigs.getItemByID(b.mid)
	if acfg.sort<bcfg.sort then
		return true
	else
		return false
	end
end


function update_tipview_position(data)
	local press_position = CS.UnityEngine.Input.mousePosition
	local canvas = uimanager.getlayer('PopupCanvas'):GetComponent('Canvas')
	local pos = CS.ZhuYuU3d.LuaCallCsFun.ScreenPointToLocalPointInRectangle(canvas.transform,
                    press_position.x,press_position.y, canvas.worldCamera)
	local movx,movy=0,0
	if(pos.x>0) then
		movx = pos.x - tipview_rect.width/2 - 10
	else
		movx = pos.x + tipview_rect.width/2 + 10
	end
	view.tipview.localPosition = CS.UnityEngine.Vector3(movx,view.tipview.localPosition.y,0)
	local cfg = GoodsConfigs.getItemByID(data.mid)
	view.tiptext.text = cfg.name
end


--消息处理函数
function on_msg(key,decode)
	print(" bag on_msg >> "..key)
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
		view:set_state('help_show')
		update_tipview_position(param)
	elseif(event == LobbyEventConst.Bag_HelpRelease) then
		view:set_state('help_hide')
	end
end		