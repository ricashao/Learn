local HotFixTipsview = {}
local this = HotFixTipsview
--初始化方法

function HotFixTipsview:GetComponentInPath(strPath,typeVar)
	objRet=self.transform:FindChild(strPath);
	if objRet~=nil then
		return objRet:GetComponent(typeVar);
	end
	return nil;
end




function HotFixTipsview:init(transformobj)

	self.transform=transformobj;
	
	self.Title=self:GetComponentInPath("txt_title",typeof(CS.UnityEngine.UI.Text));
	
	self.Content=self:GetComponentInPath("bg/txt_Content",typeof(CS.UnityEngine.UI.Text));
	
	self.UpdateSize=self:GetComponentInPath("bg/txt_Size_title/txt_Size",typeof(CS.UnityEngine.UI.Text));
	
	self.BtnConfirm=self:GetComponentInPath("bg/btn_Confirm",typeof(CS.UnityEngine.UI.Button));
	
	if self.BtnConfirm~=nil then
		
		self.BtnConfirm.onClick:AddListener
		(
		function()
			
			print("Hello Button");
			
			self.Title.text="更新啦";
			self.Content.text="有更新啊，快来下载啊！";
			self.UpdateSize.text="100M";
			
			EventManager.Send("OnHotFixedViewEvent",self.BtnConfirm);
			
		end	
		)
	end
	
	
	
end	


return HotFixTipsview;