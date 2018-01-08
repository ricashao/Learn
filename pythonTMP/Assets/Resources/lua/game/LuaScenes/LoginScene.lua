
function awake()
	print("LuaBaseBehaviour destroy..."..self.luaPath)

end	

function start()
	--CS.Libs.AM.I:CreateFromCacheByObj ('DemoPanel', self:GetDelegate('on_gameobject_cmp'));
	CS.ZhuYuU3d.UIManager.GetInstance():Load('DemoPanel','on_cmp');
end	

function update()

end	

function ondestroy()
	print("LuaBaseBehaviour destroy..."..self.luaPath)
	DemoPanelCtrl = nil
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

	DemoPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoPanel','lua/game/modules/DemoPanel/DemoPanelCtrl.lua');
	DemoPanelCtrl.fun("DemoPanelCtrl.fun Call")
end		
