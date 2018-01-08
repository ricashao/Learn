
function awake()
	print("LuaBaseBehaviour destroy..."..self.luaPath)
end	

function start()
	--CS.Libs.AM.I:CreateFromCacheByObj ('DemoPanel', self:GetDelegate('on_gameobject_cmp'));
	CS.ZhuYuU3d.UIManager.GetInstance():Load('DemoGameHudPanel','on_cmp');
end	

function update()

end	

function ondestroy()
	print("LuaBaseBehaviour destroy..."..self.luaPath)
	DemoGameHudPanelCtrl = nil
end	

function on_cmp(name)
	print("oncmp >>>>>>>>>>>>>>>>>>> "..name)
	DemoGameHudPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoGameHudPanel','lua/game/modules/DemoGameHudPanel/DemoGameHudPanelCtrl.lua');
end	