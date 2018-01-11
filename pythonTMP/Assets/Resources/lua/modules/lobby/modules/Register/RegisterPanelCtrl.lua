--当前视图
local view = require 'lua/modules/Lobby/modules/Register/RegisterPanelView'
--本模块消息号
-- local msgCmd = GameState.curRunState.MsgDefine.LobbyCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local this = scriptEnv
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.cancelbutton:GetComponent("Button").onClick:AddListener(function()
		view:set_state("close_state")
	end)
	view.closebutton:GetComponent("Button").onClick:AddListener(function()
		view:set_state("close_state")
	end)
	view.surebutton:GetComponent("Button").onClick:AddListener(function()
		local err = this.checkpwd()
		if err then print(err) return end
		if (#view.codeinput.text == 0) then
			print("输入验证码")
			return
		end
		print("发送注册协议")
		--TODO 发送修改密码协议
	end)
	view.getcodebutton:GetComponent("Button").onClick:AddListener(function()
		if(this.checkPhone()==true) then
			print("获取验证码")
			--TODO获取验证码
		else
			print("手机号码格式不正确")
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

end		