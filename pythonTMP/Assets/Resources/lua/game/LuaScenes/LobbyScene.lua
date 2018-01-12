
LuaUIManager = require 'lua/game/LuaUIManager'

function awake()
	print("LuaBaseBehaviour destroy..."..self.luaPath)

	CS.Libs.ABM.I:LoadAssetBundleManifestAdd ("StreamingAssets_u3d_res_project");
	CS.Libs.AM.I:InitAssetName2abPathDic ("StreamingAssets_u3d_xlua_project");
	CS.Libs.AM.I:InitAssetName2abPathDic ("StreamingAssets_u3d_res_project");

	--加载资源
	CS.Libs.AM.I:CreateFromCacheByObj ('UIRoot', self:GetDelegate('on_asset_cmp'),'UIRoot');

end	

function on_asset_cmp(name,gotp)
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
											path = 'lua/modules/Lobby/modules/Register/RegisterPanelCtrl.lua' 
											})
											
		LuaUIManager.register('LoginPanel',{
											name = 'LoginPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Login/Ctrl/LoginPanelCtrl.lua' 
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
									
								

		LuaUIManager.open('LobbyPanel',on_panel_open,'LobbyPanel')

		
		
		
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
	DemoListPanelCtrl = nil

	CS.Libs.AM.Destroy()
	CS.Libs.ABM.Destroy()
end	

function on_cmp(name)
	print("oncmp >>>>>>>>>>>>>>>>>>> "..name)

	DemoListPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoListPanel','lua/game/modules/DemoListPanel/DemoListPanelCtrl.lua');
end	