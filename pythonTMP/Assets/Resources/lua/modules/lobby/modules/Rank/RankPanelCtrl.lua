--当前视图
local view = require 'lua/modules/Lobby/modules/Rank/RankPanelView'
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
	
	view.backbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.CloseWindow('RankPanel')
	end)
	--AddEventCode 追加事件标志

	--PanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 
		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

	--消息监听
	--ml(msgCmd.user_para,on_msg)
	ml(msgCmd2.SC_SetRankList,on_msg)
	--事件监听
	--el(event.name,on_event)
end	


function start()
	view.tabgroup:SelectByIndex(0)
	-- local decode = {}
	-- decode.type = 'ZProto.E_MoneyType.GEM'
	-- decode.mine = 30
	-- decode.datas = {
	-- {111,'woshi1',111},
	-- {112,'woshi1',112},
	-- {113,'woshi1',113},
	-- {114,'woshi1',114},
	-- {115,'woshi1',115},
	-- {116,'woshi1',116},
	-- {117,'woshi1',117},
	-- {118,'woshi1',118},
	-- {119,'woshi1',119},
	-- {1110,'woshi1',1110},
	-- {1110,'woshi1',1110},
	-- }
	on_msg('ZProto.E_MoneyType.GEM',decode)
end

function ondestroy()

	--移除消息监听
	mr(msgCmd.MessageNotify,on_msg)
	--移除事件监听
	--er(event.name,on_event)

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end

function initdata()

end

function update_first(data)
	if data == nil then
		view.ranknametext1.text = '虚伪以待'
		view.rankgoldtext1.text = ''
	else
		view.ranknametext1.text = data[2]
		view.rankgoldtext1.text = data[3]
	end
end

function update_second(data)
	if data == nil then
		view.ranknametext2.text = '虚伪以待'
		view.rankgoldtext2.text = ''
	else
		view.ranknametext2.text = data[2]
		view.rankgoldtext2.text = data[3]
	end
end

function update_third(data)
	if data == nil then
		view.ranknametext3.text = '虚伪以待'
		view.rankgoldtext3.text = ''
	else
		view.ranknametext3.text = data[2]
		view.rankgoldtext3.text = data[3]
	end
end

function update_info(decode)
	local data = table.remove(decode.datas,1);
	update_first(data)
	data = table.remove(decode.datas,1);
	update_second(data)
	data = table.remove(decode.datas,1);
	update_third(data)
	if(decode.type == 'ZProto.E_MoneyType.GOLD') then
		view.goldlist:Data(decode.datas)
	elseif(decode.type == 'ZProto.E_MoneyType.GEM') then
		view.gemlist:Data(decode.datas)
	elseif (decode.type == 'ZProto.E_MoneyType.TICKET') then
		view.ticketlist:Data(decode.datas)
	else
	end
	update_myrank(decode.mine)

end

function update_myrank(rank)
	if rank and rank<=100 then
		view.myranktext.text = rank
	else
		view.myranktext.text = '你未进入排名，请再接再厉！'
	end
end

--消息处理函数
function on_msg(key,decode)
	print(" modifypwd on_msg >> "..key)
	if(key == msgCmd2.SC_SetRankList) then
		if(decode.type == 'ZProto.E_MoneyType.GOLD') then
			view:set_state('gold_state')
		elseif(decode.type == 'ZProto.E_MoneyType.GEM') then
			view:set_state('gem_state')
		elseif (decode.type == 'ZProto.E_MoneyType.TICKET') then
			view:set_state('ticket_state')
		else
		end
		update_info(decode)
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)

end		