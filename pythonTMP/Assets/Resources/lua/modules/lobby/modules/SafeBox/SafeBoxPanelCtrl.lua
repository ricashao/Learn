--当前视图
local view = require 'lua/modules/Lobby/modules/SafeBox/SafeBoxPanelView'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.CommonModuleCmd
local msgCmd2 = GameState.curRunState.MsgDefine.ZBankModuleCmd
--本模块数据层
-- local data = GameState.curRunState.Data.LobbyData
local ZBankModuleData = require 'lua/datamodules/ZBankModuleData'
local this = scriptEnv
local uimanager = require 'lua/game/LuaUIManager'
function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.backbutton.onClick:AddListener(function()
		uimanager.CloseWindow('SafeBoxPanel')
	end)
	
	view.resetbutton.onClick:AddListener(function()
		view.resetview_pwd1input.text = ''
		view.resetview_pwd2input.text = ''
		view.resetview_codeinput.text = ''
		view:set_state('reset_open')
	end)
	
	view.resetview_closebutton.onClick:AddListener(function()
		view:set_state('reset_hide')
	end)
	
	view.resetview_resetbutton.onClick:AddListener(function()
		if view.resetview_pwd1input.text ~= view.resetview_pwd2input.text then
			uimanager.ToastTip('密码不一致',3,30)
			return
		elseif #(view.resetview_pwd1input.text)<6 then
			uimanager.ToastTip('密码不足6位',3,30)
			return
		end
		local code = view.resetview_codeinput.text
		if #code ~=6 then
			uimanager.ToastTip('输入6位验证码',3,30)
			return
		end
		local md5str = CS.Md5Tools.md5(view.resetview_pwd1input.text)
		ZBankModuleData.send_CS_EditBankPassword(CommonData.user.id,code,md5str)
	end)
	
	view.createview_exitbutton.onClick:AddListener(function()
		view:set_state('create_hide')
	end)
	view.createview_closebutton.onClick:AddListener(function()
		view:set_state('create_hide')
	end)
	
	view.createview_createbutton.onClick:AddListener(function()
		if view.createview_pwd1input.text ~= view.createview_pwd2input.text then
			uimanager.ToastTip('密码不一致',3,30)
			return
		elseif #(view.createview_pwd1input.text)<6 then
			uimanager.ToastTip('密码不足6位',3,30)
			return
		end
		local md5str = CS.Md5Tools.md5(view.resetview_pwd1input.text)
		ZBankModuleData.send_CS_EditBankPassword(CommonData.user.id,0,md5str)
	end)
	view.depositbutton.onClick:AddListener(function()
		local depositnum =  tonumber(view.depositinput.text)
		if depositnum>0 then
			if CommonData.user.gold ~= nil and CommonData.user.gold>0 then
				if CommonData.user.gold<depositnum then
					ZBankModuleData.send_CS_SaveIntoBank(CommonData.user.id,CommonData.user.gold)
				else
					ZBankModuleData.send_CS_SaveIntoBank(CommonData.user.id,depositnum)
				end
			else
				uimanager.ToastTip('金币不足',3,30)
			end
		else
			uimanager.ToastTip('无法存款',3,30)
		end		
	end)
	
	view.withdrawbutton.onClick:AddListener(function()
	
		if CommonData.Platform == 'Visitor_Platform' then
			uimanager.open("RegisterPanel",nil,2)
			return
		end
		if #(view.withdrawpwdinput.text) <6 then
			uimanager.ToastTip('密码不足6位',3,30)
			return
		end
		
		local md5str = CS.Md5Tools.md5(view.withdrawpwdinput.text)
		local withdrawnum =  tonumber(view.withdrawinput.text)
		if withdrawnum>0 then
			if CommonData.user_info.gold_bank ~= nil and CommonData.user_info.gold_bank>0 then
				if CommonData.user_info.gold_bank<withdrawnum then
					ZBankModuleData.send_CS_DrawFromBank(CommonData.user.id,md5str,CommonData.user_info.gold_bank)
				else
					ZBankModuleData.send_CS_DrawFromBank(CommonData.user.id,md5str,withdrawnum)
				end
			else
				uimanager.ToastTip('金币不足',3,30)
			end
		else
			uimanager.ToastTip('无法取款',3,30)
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
	el(LobbyEventConst.SafeBox_TabChange,on_event)
	el(CommonEventConst.Money_Update,on_event)
	el(LobbyEventConst.SafeBox_MoneySelect,on_event)
	el(CommonEventConst.User_Update,on_event)
end	


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
	er(LobbyEventConst.SafeBox_TabChange,on_event)
	er(CommonEventConst.Money_Update,on_event)
	er(LobbyEventConst.SafeBox_MoneySelect,on_event)
	er(CommonEventConst.User_Update,on_event)

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
	local data={1,3,5,10,30,50,100,-1};
	if index == 0 then
		view:set_state('deposit_state')
		view.depositlist:Data(data)
		view.depositlist_selectgroup.Index = 0
	elseif index == 1 then
		view.withdrawpwdinput.text = ''
		view:set_state('withdraw_state')
		view.withdrawlist:Data(data)
		view.withdrawlist_selectgroup.Index = 0
	end
end

function update_view(param)
	local index = view.tabgroup.Index
	if index == 0 then
		update_depositview(param)
	elseif index == 1 then
		update_withdrawview(param)
	end
end

function update_depositview(param)
	if param ~=-1 then
		view.depositinput.text = param*10000
	else
		if CommonData.user.gold == nil then
			view.depositinput.text = 0
		else
			view.depositinput.text = CommonData.user.gold
		end
	end
end

function update_withdrawview(param)
	
	if param ~=-1 then
		view.withdrawinput.text = param*10000
	else
		if CommonData.user_info.gold_bank == nil then
			view.withdrawinput.text = 0
		else
			view.withdrawinput.text = CommonData.user_info.gold_bank
		end
	end
end



--消息处理函数
function on_msg(key,decode)
	print(" safebox on_msg >> "..key)
	if key == msgCmd.MessageNotify then
		if decode.pid == 'CS_SaveIntoBankId' then
			if decode.type == 'INF0_BANK_SUCCESS' then
				uimanager.ToastTip('操作成功',3,30)
			elseif decode.type == 'WARN_NO_PASSWORD' then
				view:set_state('create_open')
			end
		elseif decode.pid == 'CS_EditBankPasswordId' then
			if decode.type == 'INF0_BIND_SUCCESS' then
				uimanager.ToastTip('设置成功',3,30)
			elseif decode.type == 'INF0_EDIT_SUCCESS' then
				uimanager.ToastTip('修改成功',3,30)
			end
		elseif decode.pid == 'CS_DrawFromBankId' then
			if decode.type == 'WARN_NO_PASSWORD' then
				uimanager.ToastTip('密码错误',3,30)
			elseif decode.type == 'INF0_BANK_SUCCESS' then
				uimanager.ToastTip('操作成功',3,30)
			end
		end
	end

end	
--事件处理函数
function on_event(event,param)
	print(" on_event >> "..event)
	if(event == LobbyEventConst.SafeBox_TabChange) then
		update_info(param)
	end
	if(event == CommonEventConst.Money_Update) then
		update_money()
	end
	
	if(event == LobbyEventConst.SafeBox_MoneySelect) then
		update_view(param)
	end
	if event == CommonEventConst.User_Update then
		update_money()
	end
	
end		