--当前视图
local view = require 'lua/modules/Lobby/modules/UserInfo/UserInfoPanelView'
--本模块消息号
--local msgCmd = GameState.curRunState.MsgDefine.ZLobbyModuleCmd
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
		uimanager.toggle("UserInfoPanel",0)
	end)
	view.changepwdbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.toggle("ModifyPasswordPanel")
	end)
	view.bindphonebutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.open("RegisterPanel",nil,nil)
	end)
	view.changenamebutton:GetComponent("Button").onClick:AddListener(function()
		--TODO判断修改的名字和原本的名字
		local newname = view.nameinput.text
		ZLobbyModuleData.send_CS_ChangeUserNick(CommonData.user.id,newname)
	end)
	view.changeheadbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.toggle("ChangeHeadPanel",1)
	end)
	
	--AddEventCode 追加事件标志

	--PanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 
		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

	--消息监听
	ml(sib(10000),on_msg)
	--事件监听
	--el(event.name,on_event)
end	

function updateinfo()
	view.nameinput.text = CommonData.user.nick_name
	view.gemtext.text = CommonData.user.gold
	view.goldbanktext.text = CommonData.user_info.gold_bank
	view.uidtext.text = CommonData.user.id
end

function start()
	updateinfo()
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


--消息处理函数
function on_msg(key,decode)
	print(" userinfoctrl on_msg >> "..key)
	if(key == sib(10000))then
		if decode.pid == 'CS_ChangeUserNickId' then
		CommonData.user.nick_name = view.nameinput.text;
			--TODO 如果服务器不推更新信息 自己派发
			es(LobbyEventConst.UserInfo_ChangeName_Success,view.nameinput.text);
		end
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)

end		