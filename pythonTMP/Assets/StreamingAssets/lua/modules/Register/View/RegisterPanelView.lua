--[[
视图层模板
1.保持视图子对象引用
2.
]]--
local RegisterPanelview = 
{
	["TypeOfUI"]=1
}

local this = RegisterPanelview

local LuaUIManager = require 'lua/game/LuaUIManager'
function isNil(obj)
	
	if obj == nil then
		print 'true';
	elseif obj ~=nil then
		print 'false';
	end
	
end

function RegisterPanelview:init(trans)
	this.transform=trans;
	print("Registe Name:"..trans.name);
	
	this.InputFieldOfAccount=GetComponentInPath(this.transform,"content/RegisterContent/Account_field/nickname_inputfield",typeof(CS.UnityEngine.UI.InputField));

	isNil(this.InputFieldOfAccount);
	
	this.InputFieldOfPassword=GetComponentInPath(this.transform,"content/RegisterContent/Password_field/pas_inputfield",typeof(CS.UnityEngine.UI.InputField));

	isNil(this.InputFieldOfPassword);	
	
	this.InputFieldOfPasswordConfirm=GetComponentInPath(this.transform,"content/RegisterContent/Password_field_confirm/pas_confirm_inputfield",typeof(CS.UnityEngine.UI.InputField));

	isNil(this.InputFieldOfPasswordConfirm);	
	
	this.InputFieldOfCheckCode=GetComponentInPath(this.transform,"content/RegisterContent/checkcode_field/checkcode_inputfield",typeof(CS.UnityEngine.UI.InputField));
	
	isNil(this.InputFieldOfCheckCode);	
	
	this.btnGetCheckCode=GetComponentInPath(this.transform,"content/RegisterContent/checkcode_field/btn_checkcode",typeof(CS.UnityEngine.UI.Button));
	this.btnQuit=GetComponentInPath(this.transform,"content/RegisterContent/btn_quit",typeof(CS.UnityEngine.UI.Button));
	this.btnLogin=GetComponentInPath(this.transform,"content/RegisterContent/btn_ok",typeof(CS.UnityEngine.UI.Button));
	this.btnClose=GetComponentInPath(this.transform,"btn_Close",typeof(CS.UnityEngine.UI.Button));
	
	if (this.btnGetCheckCode~=nil) then
		
		print("Name:"..this.btnGetCheckCode.gameObject.name);

		this.btnGetCheckCode.onClick:AddListener
		(
			function()			
				this.OnGetCheckCode();
			end
		);
	end
	
	if (this.btnQuit~=nil) then

		this.btnQuit.onClick:AddListener
		(
			function()			
				this.OnBack();
			end
		);
	end
	
	if (this.btnLogin~=nil) then

		this.btnLogin.onClick:AddListener
		(
			function()			
				this.OnLogin();
			end
		);
		
	end
	
	if (this.btnClose~=nil) then

		this.btnClose.onClick:AddListener
		(
			function()			
				this.OnBack();
			end
		);
		
	end
	
	
end	

function RegisterPanelview:OnGetCheckCode()

	print("Click On Get CheckCode Button");
	--[[CS.ZhuYuU3d.UIManager.GetInstance():PopWindow("MessageBoxPanel",
	"Test Tips",
	"Test Content",
	0,
	"PopupCanvas",
	function()
		print("Click OK");
		CS.ZhuYuU3d.UIManager.GetInstance():ToastTips("Test",3,30,function()
			print("Toast over");
		end)
	end,
	function()
		print("Click Cancel");
	end);--]]
	LuaUIManager.PopMessageWindow("MessageBoxPanel",
	"Test Tips",
	"Test Content",
	0,
	function()
		print("Click OK");
			CS.ZhuYuU3d.UIManager.GetInstance():ToastTips("Test",3,30,function()
			print("Toast over");
		end)
	end,
	function()
		print("Click Cancel");
	end);
	
	
end

function RegisterPanelview:OnBack()
	
	print("Click On Back Button");
	
	LuaUIManager.CloseWindow('RegisterPanel');
	
	
end


function RegisterPanelview:OnLogin()
	print("Click On Login Button");
	local isInfoAll=true;
	if this.InputFieldOfAccount.text=="" then
		print("Account is nil");
		isInfoAll=false;
	end
	if this.InputFieldOfPassword.text=="" then
		print("Password is nil");
		isInfoAll=false;
	end
	if this.InputFieldOfPasswordConfirm.text=="" then
		print("ConfirmPassword is nil");
		isInfoAll=false;
	end

	if this.InputFieldOfPassword.text~=this.InputFieldOfPasswordConfirm.text then
		print("Password not right");
		isInfoAll=false;
	end
	
	if this.InputFieldOfCheckCode.text=="" or string.len(this.InputFieldOfCheckCode.text)<6 then
		print("Checkcode is nil");
		isInfoAll=false;
	end
	if isInfoAll==false then
		LuaUIManager.ToastTip("信息填写错误或不全！",3,30);
		return;
	end

	
	
	local info2send={};

	info2send.Account=this.InputFieldOfAccount.text;
	info2send.Password=this.InputFieldOfPassword.text;
	info2send.ConfirmPassword=this.InputFieldOfPasswordConfirm.text;
	info2send.CheckCode=this.InputFieldOfCheckCode.text;
	
	if(this.TypeOfUI==1) then
		es("V2C_RegisterModule_GoRegister",info2send);
	elseif this.TypeOfUI==2 then
		es("V2C_RegisterModule_GoBindUser",info2send);
	end
end


function RegisterPanelview:TypeRegister(ntype)
	self.TypeOfUI=ntype;
	print("CurType of UI is:"..self.TypeOfUI);
end

--销毁方法
function RegisterPanelview:on_destroy()
	
	
end
--视图状态
function RegisterPanelview:set_state(viewState)
	
end

return RegisterPanelview