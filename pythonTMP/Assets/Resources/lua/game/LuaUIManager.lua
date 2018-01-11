local LuaUIManager = {}
local res = {}

res.DemoPanel = {
	name = 'DemoPanel',
	layer = 'Canvas',
	path = 'lua/game/modules/DemoPanel/DemoPanelCtrl.lua'
}


local layer = {}

LuaUIManager.res = res

LuaUIManagerCurLoadInfo = {}

function LuaUIManager.open(name,callback,callbackparam)

	local resInfo = res[name] 
	resInfo.callback = callback
	resInfo.callbackparam = callbackparam
	if resInfo.status then
	end

	LuaUIManagerCurLoadInfo = resInfo

	CS.ZhuYuU3d.UIManager.GetInstance():Load(name,'LuaUIManager_oncmp',resInfo.layer);
	-- body
	--CS.ZhuYuU3d.UIManager.GetInstance():Load('DemoPanel','LuaUIManager.oncmp');
end

--isopen 0关 1开 2当前状态取反
function LuaUIManager.toggle(name,isopen)
	isopen = isopen or 2
	local resInfo = res[name]
	if isopen ==2 then
		if resInfo.status == true then
			resInfo.go:SetActive(false)
			resInfo.status = false
		elseif resInfo.status == false then
			resInfo.go:SetActive(true)
			resInfo.status = true
			resInfo.go.transform:SetAsLastSibling()
		else --实例化
			LuaUIManager.open(name,nil,nil)
		end
	elseif isopen == 1 then
		if resInfo.status == true then
			return
		elseif resInfo.status == false then
			resInfo.go:SetActive(true)
			resInfo.status = true
		else
			LuaUIManager.open(name,nil,nil)
		end
	else
		if resInfo.status == false then
			return
		elseif resInfo.status == true then
			resInfo.go:SetActive(false)
			resInfo.status = false
		else
			return
		end
	end
end

function LuaUIManager.clear()
	res = {}
end

function LuaUIManager_oncmp(name)

	print("LuaUIManager_oncmp >>>>>>>>>>>>>>>>>>> "..name)
	--local resInfo = res[name] 
	local resInfo = LuaUIManagerCurLoadInfo
	--if resInfo == nil then
	--then	
	resInfo.status = true
	resInfo.go = CS.UnityEngine.GameObject.Find(name);
	print("LuaUIManager_oncmp >>>>>>>>>>>>>>>>>>> "..resInfo.layer..'/'..resInfo.name)
	CS.ZhuYuU3d.LuaBaseBehaviour.Add(resInfo.layer..'/'..resInfo.name,resInfo.path);

	if resInfo.callback ~= nil then 
		resInfo.callback(resInfo.callbackparam)
		resInfo.callback = nil
		resInfo.callbackparam = nil
	end	
	--DemoPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoPanel','lua/game/modules/DemoPanel/DemoPanelCtrl.lua');
	--DemoPanelCtrl.fun("DemoPanelCtrl.fun Call")
end

function LuaUIManager.layerregister()
	layer['Canvas']        =  CS.UnityEngine.GameObject.Find('Canvas')
	layer['SceneCanvas']   =  CS.UnityEngine.GameObject.Find('SceneCanvas')
	layer['StaticCanvas']  =  CS.UnityEngine.GameObject.Find('StaticCanvas')
	layer['PopupCanvas']   =  CS.UnityEngine.GameObject.Find('PopupCanvas')
	layer['EffectsCanvas'] =  CS.UnityEngine.GameObject.Find('EffectsCanvas')
	layer['GuideCanvas']   =  CS.UnityEngine.GameObject.Find('GuideCanvas')
end

function LuaUIManager.register(name,resInfo)
	-- body
	res[name] = resInfo
end

function LuaUIManager.PopWindow()
	
end

return LuaUIManager