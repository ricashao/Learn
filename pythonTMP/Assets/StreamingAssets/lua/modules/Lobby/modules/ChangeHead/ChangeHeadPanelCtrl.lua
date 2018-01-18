--当前视图
local view = require 'lua/modules/Lobby/modules/ChangeHead/ChangeHeadPanelView'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
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
	
	view.backbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.CloseWindow('ChangeHeadPanel')
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
end	

function start()
	if CommonData.user.face == '' then CommonData.user.face = '0' end
	updateinfo()
end

function updateinfo()
	local data = {'','0','1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17'}
	local list = view.headlist:GetComponent("UILoopList");
	list:Data(data)
end

function ondestroy()

	--移除消息监听
	--mr(msgCmd.user_para,on_msg)
	mr(msgCmd.MessageNotify,on_msg)
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
	print(" modifypwd on_msg >> "..key)
	if(key == msgCmd.MessageNotify)then
		if decode.pid == 'CS_ChangeUserFaceId' then
			local group = view.headlist:GetComponent("SelectGroup");
			if group.Index == 0 then
				--TODO 上传头像
			else
				CommonData.user.face = group.SelectData
				local list = view.headlist:GetComponent("UILoopList");
				list:refreshWithoutPosChange()
			end
			uimanager.ToastTip('修改成功',3,30)
		end
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)

end		