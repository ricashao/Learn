MessageBoxManager=
{
	["Btn_Close"]=nil, -- 主面板的关闭按钮
	["ContentParent"]=nil, -- 子面板的父节点
	["CurOpenPage"]=nil, -- 打开的子面板引用
	["ContainerGameObject"]=nil, --主面板的游戏物体引用
	["WillOpenPanel"]="",--保存需要打开子面板的名字
	["WillOpenPanelParam"]="",--保存需要打开子面板的传入参数
	["Layer"]=nil,--主面板将实例化的层次
};

local this =MessageBoxManager;
local strMessageBoxContainerPath="MessageBoxContainer";
--strOpenPanel 将要打开子面板的名字
--tabParam 传递给子面板的参数
--strLayerName 主面板将实例化的层次

function MessageBoxManager:ShowModalBox(strOpenPanel,tabParam,strLayerName)
	--保存子面板的prefab名字和参数表
	self.WillOpenPanel=strOpenPanel;

	self.WillOpenPanelParam=tabParam;


	
	
	--查找相应的父节点
	self.Layer=CS.UnityEngine.GameObject.Find(strLayerName).transform;
	
	print("Layer:"..self.Layer.name);

	-- 首先加载消息面板本身容器

	--首先判断是否存在主面板的游戏物体，没有则加载
	
	if(this.ContainerGameObject~=nil)then
		this.ContainerGameObject:SetActive(true);
	else
		CS.ZhuYuU3d.MessageBoxManager.Instance:Load
		(strMessageBoxContainerPath,
		this.OnLoadContainSuccess,
		this.OnLoadContainFailed
		);
	end
	
end

--实例化在主面板的子物体抽象
MessageBoxContent=
{
	["Title"]="",-- 显示的title 字符串
	["Content"]="",--显示的内容字符串
	["Button1Param"]={},-- 按钮1的表单  Tag：（代表按钮的类型，eg：close代表这个按钮是关闭，yes代表是确定，cancel代表是取消） 
						--Callback:(按钮回调函数)
						--CallbackParam（按钮回调参数）
						--PrefabPath（按钮在Prefab的路径）
	["Button2Param"]={},-- 按钮2的表单  如上
	["Button3Param"]={},-- 按钮3的表单  如上
	["GameObjectIns"]=nil, --实例化的游戏物体引用
	["TransParent"]=nil,--实例化的transform引用
	["PanelName"]=nil;--子物体prefab名字
};

--初始化参数
function MessageBoxContent:New(transPa,strPanelName,title,con,tabButton1,tabButton2,tabButton3)
	local rettab=self;
	rettab.Title=title;
	rettab.Content=con;
	rettab.Button1Param=tabButton1;
	rettab.Button2Param=tabButton2;
	rettab.Button3Param=tabButton3;
	rettab.PanelName=strPanelName;
	rettab.TransParent=transPa;
	return rettab;
end
-- 重置为nil
function MessageBoxContent:Reset()

	self.Title=nil;
	self.Content=nil;
	self.Button1Param=nil;
	self.Button2Param=nil;
	self.Button3Param=nil;
	self.PanelName=nil;
	self.TransParent=nil;

end

--加载相应的子物体面板
function MessageBoxContent:Load()
	CS.ZhuYuU3d.MessageBoxManager.Instance:Load(self.PanelName,
	self.OnLoadSuc,
	self.OnLoadFailed
	);
end

--成功加载回调
function MessageBoxContent.OnLoadSuc(goIns)
	print("Load Res Suc:"..this.CurOpenPage.PanelName);
	this.CurOpenPage.GameObjectIns=goIns;
	this.CurOpenPage.GameObjectIns:SetActive(true);
	this.CurOpenPage.transform=goIns.transform;
	this.CurOpenPage.transform:SetParent(this.CurOpenPage.TransParent,false);

	if(this.CurOpenPage.title~=nil)then

		this.CurOpenPage:SetTitle(this.CurOpenPage.Title);

	end

	if(this.CurOpenPage.Content~=nil)then

		this.CurOpenPage:SetContent(this.CurOpenPage.Content);

	end

	this.CurOpenPage:ParseButtonEvent(this.CurOpenPage.Button1Param);
	this.CurOpenPage:ParseButtonEvent(this.CurOpenPage.Button2Param);
	this.CurOpenPage:ParseButtonEvent(this.CurOpenPage.Button3Param);
end

--失败回调
function MessageBoxContent.OnLoadFailed()

end

function MessageBoxContent:SetTitle(title)
	txtTitleWidget=GetComponentInPath(self.transform,title.PrefabPath,typeof(CS.UnityEngine.UI.Text)); -- "txt_Title"
	if(txtTitleWidget~=nil)then

		txtTitleWidget.text=title.Content;

	end
end

function MessageBoxContent:SetContent(content)
	txtContentWidget=GetComponentInPath(self.transform,content.PrefabPath,typeof(CS.UnityEngine.UI.Text));--"txt_Content"
	if txtContentWidget~=nil then

		txtContentWidget.text=content.Content;

	end
end

function MessageBoxContent:ParseButtonEvent(btnToParse)

	if btnToParse~=nil then
		btnButton1=GetComponentInPath(self.transform,btnToParse.PrefabPath,typeof(CS.UnityEngine.UI.Button));
		if btnButton1~=nil then
			print("Button Exist");
			btnButton1.onClick:AddListener
			(
				function()
				
					if(btnToParse.Callback~=nil)then
						btnToParse.Callback(btnToParse.CallbackParam);
					end
					
					MessageBoxManager:Destroy();
				end
			);
		end
	end
end

function MessageBoxContent:Destroy()

	if(self.GameObjectIns~=nil)then
		CS.UnityEngine.GameObject.Destroy(self.GameObjectIns);
	end
	self:Reset();
	self=nil;

end



function MessageBoxManager.OnLoadContainSuccess(goIns)
	
	print("Load Success "..goIns.name);
	
	this.ContainerGameObject=goIns;
	
	this.ContainerGameObject:SetActive(true);
	
	this.ContainerGameObject.transform:SetParent(this.Layer,false);
	
	this:InitWidget();
	
	this:LoadContent(this.WillOpenPanel,this.WillOpenPanelParam);
	
	
end

function MessageBoxManager.OnLoadContainFailed()
	print("Load Failed");
	
end

function MessageBoxManager:InitWidget()
	
	if(self.ContainerGameObject~=nil)then
		print("Init Widget");
		self.Btn_Close=GetComponentInPath(self.ContainerGameObject.transform,"btn_Close",typeof(CS.UnityEngine.UI.Button));
		print("Get Button Widget");
		if self.Btn_Close~=nil then
			print("if go in");
			self.Btn_Close.onClick:AddListener
			(
			function()
				print("Destroy Window");
				if(self.CurOpenPage~=nil)then
					
					--self.CurOpenPage:OnClose();
					self:Destroy();
					
				end
				
				
			end);
		end
		
		self.ContentParent=GetComponentInPath(this.ContainerGameObject.transform,"content",typeof(CS.UnityEngine.Transform));
	end
	print("Init Widget Over!");
end

function MessageBoxManager:LoadContent(strContentResName,tabParam)

	if(self.CurOpenPage~=nil) then
		self.CurOpenPage:Destroy();
	end
	print("Child Name:"..strContentResName);
	
	ContentObj=MessageBoxContent:New(
	self.ContentParent,
	strContentResName,
	tabParam.Title,
	tabParam.Content,
	tabParam.Button1Param,
	tabParam.Button2Param,
	tabParam.Button3Param);
	
	self.CurOpenPage=ContentObj;
	
	self.CurOpenPage:Load();
	
end



function MessageBoxManager:Destroy()
	
	if(self.CurOpenPage~=nil) then
		self.CurOpenPage:Destroy();
		self.CurOpenPage=nil;
	end
	
	if self.ContainerGameObject~=nil then
		CS.UnityEngine.GameObject.Destroy(self.ContainerGameObject);
	end
	
	self.Btn_Close=nil;
	
	self.ContainerGameObject=nil;
	
	self.ContentParent=nil;
	
end


return MessageBoxManager;