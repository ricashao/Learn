
function awake()
	print("LuaBaseBehaviour destroy..."..self.luaPath)
end	

function start()
	CS.ZhuYuU3d.UIManager.GetInstance():Load('DemoListPanel','on_cmp');
end	

function update()

end	

function ondestroy()
	print("LuaBaseBehaviour destroy..."..self.luaPath)
	DemoListPanelCtrl = nil
end	

function on_cmp(name)
	print("oncmp >>>>>>>>>>>>>>>>>>> "..name)

	DemoListPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoListPanel','lua/game/modules/DemoListPanel/DemoListPanelCtrl.lua');
end	