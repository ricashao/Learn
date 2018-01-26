--当前视图
local view = require 'lua/modules/Lobby/modules/Shop/ShopPanelView'
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
		uimanager.CloseWindow('ShopPanel')
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
	el(LobbyEventConst.Shop_TabChange,on_event)
	el(CommonEventConst.Money_Update,on_event)
end	


function start()
	local state = GameState.curRunState:getstate_byname('bagtoshop')
	if state == nil then
		view.tabgroup:SelectByIndex(0)
	else
		goto_dataindex(state)
	end
	update_money()
end

function ondestroy()

	--移除消息监听
	-- mr(msgCmd2.SC_SetRankList,on_msg)
	-- mr(msgCmd.MessageNotify,on_msg)
	--移除事件监听
	--er(event.name,on_event)
	er(LobbyEventConst.Shop_TabChange,on_event)
	er(CommonEventConst.Money_Update,on_event)

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end

function initdata()

end

function update_money()
	view.goldtext.text = CommonData.user.gold
	view.gemtext.text = CommonData.user_info.gem
	view.tickettext.text = CommonData.user_info.ticket
end


function update_info(index)
	local data;
	if index == 0 then
		view:set_state('buygem_state')
		data = GoodsConfigs.getItemsByType(4)
		table.sort(data,sortFunc)
		view.list1:Data(data)
		view.list1_selectgroup.Index = 0
	elseif index == 1 then
		view:set_state('buygold_state')
		data = GoodsConfigs.getItemsByType(2)
		table.sort(data,sortFunc)
		view.list2:Data(data)
		view.list2_selectgroup.Index = 0
	elseif index == 2 then
		view:set_state('buygood_state')
		data = GoodsConfigs.getItemsByType(1)
		table.sort(data,sortFunc)
		view.list3:Data(data)
		view.list3_selectgroup.Index = 0
	else
		view:set_state('buyreal_state')
		data = GoodsConfigs.getItemsByType(3)
		table.sort(data,sortFunc)
		view.list4:Data(data)
		view.list4_selectgroup.Index = 0
	end
	local state = GameState.curRunState:getstate_byname('bagtoshop')
	if state then	
		local cfg = GoodsConfigs.getItemByID(state.mid)
		if cfg.price_type == 2 then
			list = view.list2
			group = view.list2_selectgroup
		end
		index = list:GetIndexByData(state,'mid')
		if(index ~= -1) then
			list:MovoToIndex(index)
			group.Index = index
			GameState.curRunState:setstate_byname('bagtoshop',nil)
		end
	end
end

function goto_dataindex(data)
	local cfg = GoodsConfigs.getItemByID(data.mid)
	local listdata
	local list
	local group 
	if cfg.price_type == 2 then
		view.tabgroup:SelectByIndex(1)
		list = view.list2
		group = view.list2_selectgroup
	end
	
	index = list:GetIndexByData(data,'mid')
	if(index ~= -1) then
		list:MovoToIndex(index)
		group.Index = index
		GameState.curRunState:setstate_byname('bagtoshop',nil)
	end
end

function sortFunc(a,b)
	if a.sort<b.sort then
		return true
	else
		return false
	end
end


--消息处理函数
function on_msg(key,decode)
	print(" shop on_msg >> "..key)
	if key == msgCmd.MessageNotify then
		if decode.pid == 'CS_GetItemId' then
			if decode.type == 'INF0_BUY_SUCCESS' then
				uimanager.ToastTip('购买成功',3,30)
			end
		end
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)
	if(event == LobbyEventConst.Shop_TabChange) then
		update_info(param)
	end
	if(event == CommonEventConst.Money_Update) then
		update_money()
	end
end		