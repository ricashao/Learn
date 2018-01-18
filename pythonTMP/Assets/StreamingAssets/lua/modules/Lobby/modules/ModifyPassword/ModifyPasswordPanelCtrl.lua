--当前视图
local view = require 'lua/modules/Lobby/modules/ModifyPassword/ModifyPasswordPanelView'
--本模块消息号
-- local msgCmd = GameState.curRunState.MsgDefine.LobbyCmd
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local ZEntryModuleData = require 'lua/datamodules/ZEntryModuleData'
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.cancelbutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.CloseWindow('ModifyPasswordPanel')
	end)
	view.closebutton:GetComponent("Button").onClick:AddListener(function()
		uimanager.CloseWindow('ModifyPasswordPanel')
	end)
	view.surebutton:GetComponent("Button").onClick:AddListener(function()
		local err = this.checkpwd()
		if err then uimanager.ToastTip(err,3,30); return end
		if (#view.codeinput.text ~= 6) then
			print("输入六位有效验证码")
			return
		end
		print("发送修改协议")
		--TODO 发送修改密码协议
		local md5str = CS.Md5Tools.md5(view.pwd1input.text)
		print('md5str>>>>'..md5str)
		ZEntryModuleData.send_CS_EditPassword(CommonData.user.id,view.codeinput.text,md5str)
	end)
	view.getcodebutton:GetComponent("Button").onClick:AddListener(function()
		if(this.checkPhone()==true) then
			print("获取验证码")
			--TODO获取验证码
		else
			uimanager.ToastTip("手机号码格式不正确!",3,30);
		end
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

function checkPhone()
	local phone = view.accountinput.text;
	if(#phone==11 and string.match(phone,"[1][3,4,5,7,8]%d%d%d%d%d%d%d%d%d") == phone) then
		return true
	else
		return false
	end
end

function checkpwd()
	local pwd1 = view.pwd1input.text
	local pwd2 = view.pwd2input.text
	if((#pwd1<6) or (#pwd2<6)) then
		return "新密码长度不符合系统设定"
	elseif(pwd1~=pwd2) then
		return "新密码输入不一致"
	else
		return
	end
end 

function start()

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
		if decode.pid == 'CS_EditPasswordId' then
			if decode.type ~= 'INF0_EDIT_SUCCESS' and decode.msg then
				uimanager.ToastTip(decode.msg,3,30)
			elseif decode.type == 'INF0_EDIT_SUCCESS' then
				uimanager.ToastTip('修改成功',3,30)
			end
		end
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)

end		