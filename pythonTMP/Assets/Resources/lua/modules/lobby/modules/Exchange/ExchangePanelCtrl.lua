--当前视图
local view = require 'lua/modules/Lobby/modules/Exchange/ExchangePanelView'
--本模块消息号
-- local msgCmd = GameState.curRunState.MsgDefine.LobbyCmd
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local ZShopModuleData = require 'lua/datamodules/ZShopModuleData'
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')

	view.closebutton.onClick:AddListener(function()
		uimanager.CloseWindow('ExchangePanel')
	end)
	view.codebutton.onClick:AddListener(function()
		if(this.checkPhone()==true) then
			print("获取验证码")
			--TODO获取验证码
		else
			uimanager.ToastTip("手机号码格式不正确!",3,30);
		end
	end)
	
	view.okbutton.onClick:AddListener(function()
		if(view.nameinput.text==''or view.telinput.text==''or view.codeinput.text==''
		or view.mailinput.text==''or view.addressinput.text=='') then
			uimanager.ToastTip('信息填写不完整',3,30)
		end
		if GameState.curRunState.curbuy ~= nil then
			ZShopModuleData.send_CS_ExchangeItem(CommonData.user.id,GameState.curRunState.curbuy.mid,1,
				view.nameinput.text,view.telinput.text,view.codeinput.text,view.mailinput.text,view.addressinput.text)
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
	local phone = view.telinput.text;
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
	print(" exchange on_msg >> "..key)
	if(key == msgCmd.MessageNotify)then
		if decode.pid == 'CS_ExchangeItemId' then
			if decode.type == 'INF0_BUY_SUCCESS' then
				uimanager.ToastTip('兑换成功',3,30)
			end
		end
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)

end		