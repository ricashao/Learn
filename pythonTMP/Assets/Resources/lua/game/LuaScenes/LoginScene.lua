
LuaUIManager = require 'lua/game/LuaUIManager'

function awake()
	print("LuaBaseBehaviour destroy..."..self.luaPath)

	CS.Libs.ABM.I:LoadAssetBundleManifestAdd ("StreamingAssets_u3d_res_project");
	CS.Libs.AM.I:InitAssetName2abPathDic ("StreamingAssets_u3d_xlua_project");
	CS.Libs.AM.I:InitAssetName2abPathDic ("StreamingAssets_u3d_res_project");

	CS.Libs.AM.I:CreateFromCacheByObj ('UIRoot', self:GetDelegate('on_asset_cmp'),'UIRoot');
	
end	

function on_asset_cmp(name,gotp)
	if name == 'UIRoot' then 
		local objInstantiate = CS.UnityEngine.GameObject.Instantiate(gotp)
		objInstantiate.name = name
		LuaUIManager.layerregister()
		
		--[[
		LuaUIManager.register('LobbyPanel',{
											name = 'LobbyPanel',
											layer = 'StaticCanvas',
											path = 'lua/modules/Lobby/modules/Lobby/LobbyPanelCtrl.lua' 
											})

		LuaUIManager.open('LobbyPanel',on_panel_open,'LobbyPanel')
--]]

		LuaUIManager.register('LoginPanel',{
											name = 'LoginPanel',
											layer = 'StaticCanvas',
											path = 'lua/modules/Login/Ctrl/LoginPanelCtrl.lua' 
											})

		LuaUIManager.open('LoginPanel',on_panel_open,'LoginPanel')
		
	end
end	

function on_panel_open( name )
	-- body
end

function start()
	--CS.Libs.AM.I:CreateFromCacheByObj ('DemoPanel', self:GetDelegate('on_gameobject_cmp'));
	--CS.ZhuYuU3d.UIManager.GetInstance():Load('DemoPanel','on_cmp');
	--CS.ZhuYuU3d.UIManager.GetInstance():Load('HotFixTipsContent','on_cmp');
	
	--CS.ZhuYuU3d.UIManager.GetInstance():Load('LaunchPanel','on_cmp');
	print("LaunchPanel");

end	

function update()
	
end	

function ondestroy()
	print("LuaBaseBehaviour destroy..."..self.luaPath)
	DemoPanelCtrl = nil

	CS.Libs.AM.Destroy()
	CS.Libs.ABM.Destroy()
end	

function on_gameobject_cmp(name,gotp)

	local objInstantiate = CS.UnityEngine.GameObject.Instantiate(gotp)
	objInstantiate.name= string.gsub(objInstantiate.name,'(Clone','')
	objInstantiate.name = name
	objInstantiate.transform:SetParent(CS.UnityEngine.GameObject.Find("Canvas").transform,false);

	DemoPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoPanel','lua/game/modules/DemoPanel/DemoPanelCtrl.lua');
end	

function on_cmp(name)
	print("oncmp >>>>>>>>>>>>>>>>>>> "..name)

	--HotFixTipsPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/HotFixTipsContent','lua/game/modules/HotFixTipsPanel/HotFixTipsPanelCtrl.lua');
	--DemoPanelCtrl.fun("DemoPanelCtrl.fun Call")
end		
