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
	-- ml(msgCmd2.SC_SetRankList,on_msg)
	--事件监听
	--el(event.name,on_event)
	el(LobbyEventConst.Shop_TabChange,on_event)
end	

local datas = {
	{{'zs1','zs1'},{'zs2','zs2'},{'zs2','zs2'},{'zs3','zs3'},{'zs4','zs4'},{'zs5','zs5'},{'zs6','zs6'},{'zs7','zs7'},{'zs8','zs8'},{'zs9','zs9'},{'zs10','zs10'},{'zs11','zs11'}
	},
	{{'jb1','jb1'},{'jb2','zs2'},{'jb2','zs2'},{'jb3','zs3'},{'jb4','zs4'},{'jb5','zs5'},{'jb6','jb6'},{'jb7','jb7'},{'jb8','jb8'},{'jb9','jb9'},{'jb10','jb10'},{'jb11','jb11'}
	},
	{{'dj1','jb1'},{'dj2','zs2'},{'dj2','zs2'},{'dj3','zs3'},{'dj4','zs4'},{'dj5','zs5'},{'dj6','jb6'},{'dj7','jb7'},{'dj8','jb8'},{'dj9','jb9'},{'dj10','jb10'},{'dj11','jb11'}
	},
	{{'dh1','jb1'},{'dh2','zs2'},{'dh2','zs2'},{'dh3','zs3'},{'dh4','zs4'},{'dh5','zs5'}
	}
}


function start()
	view.tabgroup:SelectByIndex(0)
	update_money()
end

function ondestroy()

	--移除消息监听
	-- mr(msgCmd2.SC_SetRankList,on_msg)
	-- mr(msgCmd.MessageNotify,on_msg)
	--移除事件监听
	--er(event.name,on_event)
	er(LobbyEventConst.Shop_TabChange,on_event)

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
	local data = datas[index+1]
	if index == 0 then
		view:set_state('buygem_state')
		view.list1:Data(data)
		view.list1_selectgroup.Index = 0
	elseif index == 1 then
		view:set_state('buygold_state')
		view.list2:Data(data)
		view.list2_selectgroup.Index = 0
	elseif index == 2 then
		view:set_state('buygood_state')
		view.list3:Data(data)
		view.list3_selectgroup.Index = 0
	else
		view:set_state('buyreal_state')
		view.list4:Data(data)
		view.list4_selectgroup.Index = 0
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
	if(event == LobbyEventConst.Shop_TabChange) then
		update_info(param)
	end
end		