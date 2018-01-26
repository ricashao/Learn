--当前视图
local view = require 'lua/modules/Lobby/modules/UserInfo/UserInfoPanelView'
--本模块消息号
--local msgCmd = GameState.curRunState.MsgDefine.ZLobbyModuleCmd
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
--本模块数据层
local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.backbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.CloseWindow("UserInfoPanel")
	end)
	view.changepwdbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.open("ModifyPasswordPanel",nil,nil)
	end)
	view.bindphonebutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.open("RegisterPanel",nil,2)
	end)
	view.changenamebutton:GetComponent("Button").onClick:AddListener(function()
		--TODO判断修改的名字和原本的名字
		local newname = view.nameinput.text
		ZLobbyModuleData.send_CS_ChangeUserNick(CommonData.user.id,newname)
	end)
	view.changeheadbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.open("ChangeHeadPanel",nil,nil)
	end)
	
	--AddEventCode 追加事件标志

	--PanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 
		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

	--消息监听
	ml(msgCmd.MessageNotify,on_msg)
	--事件监听
	--el(event.name,on_event)
	el(LobbyEventConst.Change_Head,on_event)
end	

function updateinfo()
	if CommonData.user.nick_name and CommonData.user.nick_name ~='' then
		view.nameinput.text = CommonData.user.nick_name 
	elseif CommonData.user.user_name and CommonData.user.user_name ~='' then
		view.nameinput.text = CommonData.user.user_name	
	elseif CommonData.user_info.phone and CommonData.user_info.phone ~='' then
		view.nameinput.text = CommonData.user_info.phone	
	else
		view.nameinput.text = CommonData.user.id
	end
	view.gemtext.text = CommonData.user.gold
	view.goldbanktext.text = CommonData.user_info.gold_bank
	view.uidtext.text = CommonData.user.id
end

function updateface()
	if CommonData.user.face == '' then CommonData.user.face = 'ui/icon/defaulthead/default_head_0.jpg' end
	view.headimg:Url(CommonData.user.face)
end

function start()
	updateinfo()
	checkState()
	updateface()
end

function checkState()
	--error(CommonData.Platform)
	if CommonData.Platform == 'QQ_Platform' then
		view:set_state('qq_state')
	elseif CommonData.Platform == 'Wechat_Platform' then
		view:set_state('wechat_state')
	elseif CommonData.Platform == 'Visitor_Platform' then
		view:set_state('visitor_state')
	else
		view:set_state('official_state')
	end
end

function ondestroy()

	--移除消息监听
	--mr(msgCmd.user_para,on_msg)
	mr(msgCmd.MessageNotify,on_msg)
	--移除事件监听
	--er(event.name,on_event)
	er(LobbyEventConst.Change_Head,on_event)

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end


--消息处理函数
function on_msg(key,decode)
	print(" userinfoctrl on_msg >> "..key)
	if(key == msgCmd.MessageNotify)then
		if decode.pid == 'CS_ChangeUserNickId' then
		CommonData.user.nick_name = view.nameinput.text;
			--TODO 如果服务器不推更新信息 自己派发
			uimanager.ToastTip("修改成功!",3,30);
			es(LobbyEventConst.UserInfo_ChangeName_Success,view.nameinput.text);
		end
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)
	if event == LobbyEventConst.Change_Head then
		updateface()
	end
end		