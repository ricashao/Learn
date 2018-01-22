local LuaUIManager = {}
local res = {}

res.DemoPanel = {
	name = 'DemoPanel',
	layer = 'Canvas',
	path = 'lua/game/modules/DemoPanel/DemoPanelCtrl.lua'
}

UIWindowTypeDefine=
{
	["Normal"]=0,
	["Fixed"]=1,
	["PopUp"]=2,
	["None"]=3,
}

UIWindowModeDefine=
{
	["DoNothing"]=0,
	["HideOther"]=1,
	["NeedBack"]=2, 
	["NoNeedBack"]=3,
}

UIWindowColliderDefine=
{
    ["None"]=0,
    ["Normal"]=1,
    ["WithBg"]=2,
}

UIPage=
{
	["name"]="",
	["id"]=-1,
	["UIWindowType"]=0,
	["UIWindowMode"]=0,
	["UIWindowCollider"]=0,
	["uiPath"]="",
	["gameObject"]=nil,
	["transform"]=nil,
	["IsAsync"]=false,
	["IsActive"]=false,
	["IsLoading"]=false,
	["PageController"]={}
};

function UIPage:Active()
	if self.gameObject~=nil then
		self.gameObject:SetActive(true);
		self.IsActive=true;
	end
end

function UIPage:SetController(Controller,nameofcon)

	self.PageController=Controller[nameofcon];
	
end

function UIPage:SendMessage(para)
	if self.PageController~=nil then
		print(self.PageController.TestVar);
		if self.PageController.OnMessage~=nil then
			self.PageController.OnMessage(para);
			print("Send Message Over");
		end
	end
end

function UIPage:Hide(IsDestroy)
	
	if self.gameObject~=nil then
		self.gameObject:SetActive(false);
		self.IsActive=false;
		if IsDestroy then
			CS.UnityEngine.GameObject.Destroy (self.gameObject);
		end
	end
	
end

function UIPage:NewPage(strName,strPath,nWindowType,nWindowMode,nWindowCol)
	
	local self = {}   

	setmetatable( self , {__index = UIPage})  
	
	self["name"]=strName;
	self["UIWindowType"]=nWindowType;
	self["UIWindowMode"]=nWindowMode;
	self["UIWindowCollider"]=nWindowCol;
	self["uiPath"]=strPath;
	self["IsLoading"]=false;
	return self        

end


function UIPage:ShowMessageBox(
	strDisplayTitle,
	strDisplayContent,
	nShowType,
	funOnOKCallback,
	funOnCancelCallback
	)
	
	if self.gameObject==nil and self[uiPath]==nil then
		
		CS.ZhuYuU3d.UIManager.GetInstance():PopWindow(
		self["name"],
		strDisplayTitle,
		strDisplayContent,
		nShowType,
		"PopupCanvas"
		,		
		function()
				
			
			if(funOnOKCallback~=nil)then
				funOnOKCallback();
			end

			LuaUIManager:ClosePageWithName(self.name);
			
		end
		,
		function()

			if(funOnCancelCallback~=nil)then
				funOnCancelCallback();
			end
			
			LuaUIManager:ClosePageWithName(self.name);
			
		end
		,
		function(insobj)
			if insobj~=nil then
				print("PopWindow Name:"..insobj.name);
				if(insobj~=nil)then
					self:SetGameObject(insobj);
					self:Active();
					LuaUIManager:PopNode(self);
				end
			end
		end
		);
		
	else
		
		self:Active();

		LuaUIManager:PopNode(self);
		
	end
	

end

function UIPage:Show( callback,callbackparam)

	if self.gameObject==nil and self.IsLoading == false then
			
		local resInfo=LuaUIManager.GetResInfo(self["name"]);
		
		if(resInfo~=nil)then
			resInfo.callback = callback;
			resInfo.callbackparam = callbackparam;
			LuaUIManager.setCurrentLoadInfo(resInfo);
			CS.ZhuYuU3d.UIManager.GetInstance():Load(self["name"],'LuaUIManager_oncmp',resInfo.layer);
			self["IsLoading"]=true;
		end
		
	elseif self.gameObject~=nil then
		
		self:Active();

		LuaUIManager:PopNode(self);
		
	end
end

function UIPage:CheckIfNeedBack()
	if self["UIWindowType"]==UIWindowTypeDefine["Fixed"] then
		return false;
	elseif self["UIWindowMode"]==UIWindowModeDefine["NoNeedBack"] then
		return false;
	end
	return true;
end

function UIPage:SetGameObject(obj)
	self["gameObject"]=obj;
	self["transform"]=obj.transform;
end

function UIPage:Fun_Is_Active()
	local isact=false;
	if (self["gameObject"]~=nil) and self["gameObject"].activeSelf then
		isact=true;
	end
	if isact==true or self["IsActive"] then
		return true;
	end
	return false;
end

function UIPage:ActivePage()
	self:Active();
	LuaUIManager:PopNode(self);
end

function UIPage:SetLoadingFlag(bLoad)
	self["IsLoading"]=bLoad;
end


local layer = {}

LuaUIManager.res = res

LuaUIManagerCurLoadInfo = {}

LuaUIManager.AllPages={};

LuaUIManager.CurrentOpenPage={};

function LuaUIManager.GetResInfo(strkey)
	return	LuaUIManager.res[strkey];
end

function LuaUIManager.setCurrentLoadInfo(curLoadInfo)
	LuaUIManagerCurLoadInfo=curLoadInfo;
end


function LuaUIManager.open(panelName,callback,callbackparam)
	
	print("Open Panel is:"..panelName);
	if panelName==nil then
		print("Name is Nil");
		return;

	end
	
	local UIPageCurrent=nil;
	IsInAllPage=IsInTable(panelName,LuaUIManager.AllPages);
	print("Is In Table".. tostring(IsInAllPage));
	if(IsInAllPage)then
		UIPageCurrent=LuaUIManager.AllPages[panelName];
	else
		local nwt=0;
		if LuaUIManager.res[panelName].layer=="StaticCanvas" then
			nwt=1;
		elseif LuaUIManager.res[panelName].layer=="PopupCanvas" then
			nwt=2;
		end
		UIPageCurrent=UIPage:NewPage(panelName,panelName,nwt,0,0);
		LuaUIManager.AllPages[panelName]=UIPageCurrent;
	end
	
	UIPageCurrent:Show(callback,callbackparam);

	

	--[[local resInfo = res[name] 
	resInfo.callback = callback
	resInfo.callbackparam = callbackparam

	LuaUIManagerCurLoadInfo = resInfo

	CS.ZhuYuU3d.UIManager.GetInstance():Load(name,'LuaUIManager_oncmp',resInfo.layer);
--]]	
end





function LuaUIManager_oncmp(name)

	print("LuaUIManager_oncmp >>>>>>>>>>>>>>>>>>> "..name)
	--local resInfo = res[name] 
	local resInfo = LuaUIManagerCurLoadInfo
	CurOpenPage=LuaUIManager.AllPages[resInfo.name];
	if CurOpenPage~=nil then
		local curGameobjectInstance=CS.UnityEngine.GameObject.Find(name);
		if(curGameobjectInstance~=nil)then
			CurOpenPage:SetGameObject(curGameobjectInstance);
			CurOpenPage:Active();
			LuaUIManager:PopNode(CurOpenPage);
		end
		CurOpenPage:SetLoadingFlag(false);
	end
	--if resInfo == nil then
	--then	
	print("LuaUIManager_oncmp >>>>>>>>>>>>>>>>>>> "..resInfo.layer..'/'..resInfo.name)
	
	ResControlObj=CS.ZhuYuU3d.LuaBaseBehaviour.Add(resInfo.layer..'/'..resInfo.name,resInfo.path);
	--print("Path:"..);
	if ResControlObj~=nil and resInfo.callbackparam~=nil then
		CurOpenPage:SetController(ResControlObj,stripextension(strippath(resInfo.path)));
		CurOpenPage:SendMessage(resInfo.callbackparam);
	end

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

function LuaUIManager.getlayer(layername)
	return layer[layername]
end

function LuaUIManager.register(name,resInfo)
	-- body
	res[name] = resInfo
end
--[[{
"btnOK" = {fun , param }
"btn" = {fun , param }
}
--]]
function LuaUIManager.PopMessageWindow(strPanelName,strDisplayTitle,strDisplayContent,nShowType,funOnOKCallback,funOnCancelCallback)
	if(strPanelName==nil) then
		print("Panel Name is nil");
		strPanelName="MessageBoxPanel";
	end
	
	if strDisplayContent==nil or strDisplayTitle==nil then
		print("Title or Content is nil");
		return;
	end
	
--[[	if nShowType~=0 or nShowType~=1 then
		print("The Type is Not Support.")
		return;
	end--]]
	
	print("Open Panel is:"..strPanelName);
	if strPanelName==nil then
		print("Name is Nil");
		return;

	end
	
	local UIPageCurrent=nil;
	IsInAllPage=IsInTable(strPanelName,LuaUIManager.AllPages);
	if(IsInAllPage)then
		UIPageCurrent=LuaUIManager.AllPages[strPanelName];
	else
		--[[local nwt=0;
		if LuaUIManager.res[strPanelName].layer=="StaticCanvas" then
			nwt=1;
		elseif LuaUIManager.res[strPanelName].layer=="PopupCanvas" then
			nwt=2;
		end--]]
		UIPageCurrent=UIPage:NewPage(strPanelName,strPanelName,2,0,0);
		LuaUIManager.AllPages[strPanelName]=UIPageCurrent;
	end
	
	UIPageCurrent:ShowMessageBox
	(
	strDisplayTitle,
	strDisplayContent,
	nShowType,
	funOnOKCallback,
	funOnCancelCallback
	);
	
	--CS.ZhuYuU3d.UIManager.GetInstance():PopWindow
	
end

function LuaUIManager.CloseWindow(strPanelName)
	
	if(strPanelName==nil)then
		LuaUIManager:CloseTopPage();
	else
		LuaUIManager:ClosePageWithName(strPanelName);
	end
	
end

function LuaUIManager.ToastTip(strContent,nTime,nFontSize)
	
	CS.ZhuYuU3d.UIManager.GetInstance():ToastTips
		(
		strContent,
		nTime,
		nFontSize,
		function()
			print("Toast over");
		end
		);
end

function LuaUIManager.CheckNeedBack(pageinstance)
	return pageinstance~=nil and pageinstance:CheckIfNeedBack();
end

function LuaUIManager:PopNode(pageins)
	if pageins==nil then
		print("Page is nil");
		return;
	end
	
	--removebyvalue(self.CurrentOpenPage,pageins,false)
	
	removebyvalue(self.CurrentOpenPage,pageins,false);
	table.insert(self.CurrentOpenPage,pageins);

  
	LuaUIManager:HideOldNodes();
	
end

function LuaUIManager:HideOldNodes()
	local ntabCnt=#self.CurrentOpenPage;
	if(ntabCnt<=1)then
		return;
	end
	
	pageins=self.CurrentOpenPage[ntabCnt];
	if(pageins["UIWindowMode"]==UIWindowModeDefine["HideOther"])then
		for i=ntabCnt-1, 1, -1 do 
             self.CurrentOpenPage[i]:Hide(false);
		end
	end	
end

function LuaUIManager:CloseTopPage()
	
	local ntabCnt=#self.CurrentOpenPage;
	print("Back&Close PageNodes Count:" ..ntabCnt);
	if ntabCnt<=0 then
		return;
	end
	
	local pageins= self.CurrentOpenPage[ntabCnt];
	removebyvalue(self.CurrentOpenPage,pageins);
	pageins:Hide(true);
	--removebyvalue(self.AllPages,pageins);
	self.AllPages=removeElementByKey(self.AllPages,pageins.name);
	pageins=nil;
	
	ntabCnt=#self.CurrentOpenPage;
	if ntabCnt>0 then
		pageins= self.CurrentOpenPage[ntabCnt];	
		pageins:ActivePage();
	end
	
end

function LuaUIManager:ClosePage(pageins)
	
	if pageins==nil then
		return;
	end
	
	if( pageins:Fun_Is_Active()==false)then
		if self.CurrentOpenPage~=nil then
			removebyvalue(self.CurrentOpenPage,pageins);
			--removebyvalue(self.AllPages,pageins);
			self.AllPages=removeElementByKey(self.AllPages,pageins.name);
			pageins:Hide(true);
			pageins=nil;
			return;
		end
	end
	
	ntabCnt=#self.CurrentOpenPage;
	if (ntabCnt>=1)and(self.CurrentOpenPage[ntabCnt]==pageins) then

		removebyvalue(self.CurrentOpenPage,pageins);
		
		self.AllPages=removeElementByKey(self.AllPages,pageins.name);
		
		--removebyvalue(self.AllPages,pageins);
		
		pageins:Hide(true);
		pageins=nil;
		
		ntabCnt=#self.CurrentOpenPage;
		if ntabCnt>0 then
			pageins= self.CurrentOpenPage[ntabCnt];	
			pageins:ActivePage();
		end
	elseif pageins:CheckIfNeedBack() then
		removebyvalue(self.CurrentOpenPage,pageins);
		--removebyvalue(self.AllPages,pageins);
		self.AllPages=removeElementByKey(self.AllPages,pageins.name);
		
		pageins:Hide(true);
		pageins=nil;
	end
	
	

end

function LuaUIManager:ClosePageWithName(pagename)
	if IsInTable(pagename,self.AllPages)then
		LuaUIManager:ClosePage(self.AllPages[pagename]);
	else
		print(pagename.."Have Not Yet");
	end
end




return LuaUIManager