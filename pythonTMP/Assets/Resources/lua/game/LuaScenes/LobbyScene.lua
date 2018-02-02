
LuaUIManager = require 'lua/game/LuaUIManager'
--本模块消息号
local commonModuleCmd = GameState.curRunState.MsgDefine.CommonModuleCmd

function awake()
	print("LuaBaseBehaviour destroy..."..self.luaPath)

--	CS.Libs.ABM.I:LoadAssetBundleManifestAdd ("StreamingAssets_u3d_res_project");
--	CS.Libs.AM.I:InitAssetName2abPathDic ("StreamingAssets_u3d_xlua_project");

	CS.Libs.AM.I:ClearABDic ();
	CS.Libs.AM.I:InitAssetName2abPathDic ("StreamingAssets_u3d_res_project");

	--加载资源
	CS.Libs.AM.I:CreateFromCacheByObj ('UIRoot', self:GetDelegate('on_asset_cmp'),'UIRoot');
	CS.Libs.AM.I:CreateFromCacheByObj ('LobbyRoot', self:GetDelegate('on_asset_cmp'),'LobbyRoot');
end	

function on_asset_cmp(name,gotp)
	if name == 'LobbyRoot' then 
		local objInstantiate = CS.UnityEngine.GameObject.Instantiate(gotp)
		objInstantiate.name = name
	end	
	if name == 'UIRoot' then 
		local objInstantiate = CS.UnityEngine.GameObject.Instantiate(gotp)
		objInstantiate.name = name
		LuaUIManager.layerregister()
		LuaUIManager.register('LobbyPanel',{
											name = 'LobbyPanel',
											layer = 'StaticCanvas',
											path = 'lua/modules/Lobby/modules/Lobby/LobbyPanelCtrl.lua' 
											})
		LuaUIManager.register('ModifyPasswordPanel',{
											name = 'ModifyPasswordPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/ModifyPassword/ModifyPasswordPanelCtrl.lua' 
											})
		LuaUIManager.register('UserInfoPanel',{
											name = 'UserInfoPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/UserInfo/UserInfoPanelCtrl.lua' 
											})
		LuaUIManager.register('RegisterPanel',{
											name = 'RegisterPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Register/Ctrl/RegisterPanelCtrl.lua'
											})
											
		LuaUIManager.register('LoginPanel',{
											name = 'LoginPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Login/Ctrl/LoginPanelCtrl.lua'
											})
		LuaUIManager.register('ChangeHeadPanel',{
											name = 'ChangeHeadPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/ChangeHead/ChangeHeadPanelCtrl.lua' 
											})
		LuaUIManager.register('RankPanel',{
											name = 'RankPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Rank/RankPanelCtrl.lua' 
											})
											
		LuaUIManager.register('SettingPanel',{
											name = 'SettingPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Setting/SettingPanelCtrl.lua' 
											})	
											
		LuaUIManager.register('ShopPanel',{
											name = 'ShopPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Shop/ShopPanelCtrl.lua' 
											})	
		LuaUIManager.register('BagPanel',{
											name = 'BagPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Bag/BagPanelCtrl.lua' 
											})	
		LuaUIManager.register('TaskPanel',{
											name = 'TaskPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Task/TaskPanelCtrl.lua' 
											})
		LuaUIManager.register('ExchangePanel',{
											name = 'ExchangePanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Exchange/ExchangePanelCtrl.lua' 
											})	
		LuaUIManager.register('RechargePanel',{
											name = 'RechargePanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/Recharge/RechargePanelCtrl.lua' 
											})
		LuaUIManager.register('SafeBoxPanel',{
											name = 'SafeBoxPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Lobby/modules/SafeBox/SafeBoxPanelCtrl.lua' 
											})

		LuaUIManager.register('MailPanel',{
			name = 'MailPanel',
			layer = 'PopupCanvas',
			path = 'lua/modules/Lobby/modules/Mail/MailPanelCtrl.lua'
		})
									
								

		LuaUIManager.open('LobbyPanel',on_panel_open,'LobbyPanel')

		
		--el(,on_event)
		ml(commonModuleCmd.MessageNotify,on_msg)

	end
end	

function on_event(name,data)

end

function on_msg(key,decode)

	if key == commonModuleCmd.MessageNotify then 

		--if LuaUIManager ~= nil then
		--	LuaUIManager.ToastTip(decode.type,2,30);
		--end	
	end	
end

function on_panel_open( name )
	-- body
end

function start()
	--CS.ZhuYuU3d.UIManager.GetInstance():Load('DemoListPanel','on_cmp');
end	

function update()

end	

function ondestroy()
	print("LuaBaseBehaviour destroy..."..self.luaPath)

	mr(commonModuleCmd.MessageNotify,on_msg)

	DemoListPanelCtrl = nil

	CS.Libs.AM.Destroy()
	CS.Libs.ABM.Destroy()
end	

function on_cmp(name)
	print("oncmp >>>>>>>>>>>>>>>>>>> "..name)

	DemoListPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoListPanel','lua/game/modules/DemoListPanel/DemoListPanelCtrl.lua');
end	