--当前视图
local view = require 'lua/modules/Lobby/modules/Lobby/LobbyPanelView'
--本模块消息号
--local msgCmd = GameState.curRunState.MsgDefine.LobbyCmd
--本模块数据层
--local data = GameState.curRunState.Data.LobbyData

local uimanager = require 'lua/game/LuaUIManager'
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
		uimanager.toggle('UserInfoPanel',1)
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
		-- uimanager.open('LoginPanel');--,"","");
		uimanager.toggle('LoginPanel',1)
	end)
	view.KefuButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'KefuButton')
	end)
	view.BottomLeftButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomLeftButton')
	end)
	view.BottomRightButton:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomRightButton')
	end)
	view.BottomButton0:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton0')
	end)
	view.BottomButton1:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton1')
	end)
	view.BottomButton2:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton2')
	end)
	view.BottomButton3:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton3')
	end)
	view.BottomButton4:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton4')
	end)
	view.BottomButton5:GetComponent("Button").onClick:AddListener(function()
	print('onClick:'..'BottomButton5')
	end)
	--AddEventCode 追加事件标志

	--消息监听
	--ml(msgCmd.user_para,on_msg)
	--事件监听
	--el(event.name,on_event)
	el(LobbyEventConst.UserInfo_ChangeName_Success,on_event)
	el("LoginSuccess",on_event)
end	

function start()
	updateCommonInfo()
end

function updateCommonInfo()
	if CommonData.user == nil then return end
	view.UserNameText.text = CommonData.user.nick_name
	view.NumberText0.text = CommonData.user.gold
	view.NumberText1.text = CommonData.user_info.gem
	view.NumberText2.text = CommonData.user_info.ticket
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
	
	if event == LobbyEventConst.UserInfo_ChangeName_Success then
		view.UserNameText.text = param
	elseif event == "LoginSuccess" then
		updateCommonInfo()
	end

end		