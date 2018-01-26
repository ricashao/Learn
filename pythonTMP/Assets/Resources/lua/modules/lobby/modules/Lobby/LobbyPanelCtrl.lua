--当前视图
local view = require 'lua/modules/Lobby/modules/Lobby/LobbyPanelView'
--数据层
local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'

--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.ZLobbyModuleCmd
local lobbyModuleData = GameState.curRunState.Data.ZLobbyModuleData
local shopModuleData = GameState.curRunState.Data.ZShopModuleData
--本模块数据层
--local data = GameState.curRunState.Data.LobbyData

local uimanager = require 'lua/game/LuaUIManager'
local timer = CS.ZhuYuU3d.LuaTimerManager.getInstance()

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
		uimanager.open('UserInfoPanel',nil,nil)
	end)
	view.NumberAddButton0:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'NumberAddButton0')
	end)
	view.NumberAddButton1:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'NumberAddButton1')
	end)
	view.NumberAddButton2:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'NumberAddButton2')
	end)
	view.SettingButton:GetComponent("Button").onClick:AddListener(function()
		print("Setting Click");
		uimanager.open('SettingPanel',nil,nil);--,"","");
	end)
	view.KefuButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'KefuButton')
	end)
	view.BottomLeftButton:GetComponent("Button").onClick:AddListener(function()
		uimanager.open('ShopPanel')
	end)
	view.BottomRightButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomRightButton')
	end)
	view.BottomButton0:GetComponent("Button").onClick:AddListener(function()
		print('onClick:'..'BottomButton0-->MailPanelOpen');
		uimanager.open("MailPanel",nil,nil);
	end)
	view.BottomButton1:GetComponent("Button").onClick:AddListener(function()
		uimanager.open('TaskPanel')
	end)
	view.BottomButton2:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton2')
	end)
	view.BottomButton3:GetComponent("Button").onClick:AddListener(function()
		uimanager.open('BagPanel')
	end)
	view.BottomButton4:GetComponent("Button").onClick:AddListener(function()
		uimanager.open('RankPanel',nil,nil);
	end)
	view.BottomButton5:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton5')
	end)
	--AddEventCode 追加事件标志

	--消息监听
	ml(msgCmd.SC_SetMarquee,on_msg)
	--事件监听
	--el(event.name,on_event)
	el(LobbyEventConst.UserInfo_ChangeName_Success,on_event)
	el(CommonEventConst.Money_Update,on_event)
	el(LobbyEventConst.Change_Head,on_event)
	el("LoginSuccess",on_event)

	timer:Add(6,scriptEnv,'timerUpdateMarqueeText','timerUpdateMarqueeText')
	timer:Add(18,scriptEnv,'timerSendMarquee','timerSendMarquee')
end	

local curMarqueeIndex = 1
--跑马灯 更新
function timerUpdateMarqueeText(timerInfo)

	print("Call timerUpdateMarqueeText()")

	if lobbyModuleData.SC_SetMarquee == nil then
		return
	end	

	view.MarqueeText.text = lobbyModuleData.SC_SetMarquee.msgs[curMarqueeIndex] 

	curMarqueeIndex = curMarqueeIndex+1

	if curMarqueeIndex  > #lobbyModuleData.SC_SetMarquee then
		curMarqueeIndex = 1
	end 	

	print(" timerUpdate_MarqueeText  >> "..timerInfo.className.." curMarqueeIndex  >> "..curMarqueeIndex)
end	

function timerSendMarquee(timerInfo)

	print("请求跑马灯！")
	ZLobbyModuleData.send_CS_GetMarquee()
end	

function start()
	updateCommonInfo()
	if CommonData.user ~= nil then
		shopModuleData.send_CS_GetBag(CommonData.user.id)
	end
	
	if(GameState.curRunState.Data.ZLobbyModuleData.SC_SetTask == nil) then
		lobbyModuleData.send_CS_GetTask()
	end
	--跑马灯 
	ZLobbyModuleData.send_CS_GetMarquee()
end

function updateface()
	if CommonData.user.face == '' then CommonData.user.face = 'ui/icon/defaulthead/default_head_0.jpg' end
	view.headimg:Url(CommonData.user.face)
end

function updateCommonInfo()
	if CommonData.user == nil then return end
	if CommonData.user.nick_name and CommonData.user.nick_name ~='' then
		view.UserNameText.text = CommonData.user.nick_name 
	elseif CommonData.user.user_name and CommonData.user.user_name~='' then
		view.UserNameText.text = CommonData.user.user_name	
	elseif CommonData.user_info.phone and CommonData.user_info.phone~='' then
		view.UserNameText.text = CommonData.user_info.phone	
	else
		view.UserNameText.text = CommonData.user.id
	end
	view.NumberText0.text = CommonData.user.gold
	view.NumberText1.text = CommonData.user_info.gem
	view.NumberText2.text = CommonData.user_info.ticket

	--跑马灯
	if lobbyModuleData.SC_SetMarquee ~= nil then
		view.MarqueeText.text = lobbyModuleData.SC_SetMarquee.msgs[1] 
	end
	updateface()
end

function ondestroy()

	timer:Rem('timerSendMarquee')
	timer:Rem('timerUpdateMarqueeText')
	timer = nil
	--消息监听
	mr(msgCmd.SC_SetMarquee,on_msg)
	--事件监听
	--el(event.name,on_event)
	er(LobbyEventConst.UserInfo_ChangeName_Success,on_event)
	er("LoginSuccess",on_event)
	er(CommonEventConst.Money_Update,on_event)
	er(LobbyEventConst.Change_Head,on_event)

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

	if key == msgCmd.SC_SetMarquee then
		--跑马灯
		view.MarqueeText.text = decode.msgs[1]
		--ZLobbyModuleData.SC_SetMarquee = decode
		--[[
		for k,v in pairs(ZLobbyModuleData.SC_SetMarquee.msgs) do
			print("SC_SetMarquee.msgs ".. v)
			view.MarqueeText.text = v
		end ]]
	end	

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)
	
	if event == LobbyEventConst.UserInfo_ChangeName_Success then
		view.UserNameText.text = param
	elseif event == "LoginSuccess" then
		updateCommonInfo()
		shopModuleData.send_CS_GetBag(CommonData.user.id)
		lobbyModuleData.send_CS_GetTask()
	end
	if(event == CommonEventConst.Money_Update) then
		updateCommonInfo()
	end
	if event == LobbyEventConst.Change_Head then	
		updateface()
	end

end		