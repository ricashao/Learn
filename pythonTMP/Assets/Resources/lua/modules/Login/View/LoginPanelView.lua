--[[
视图层模板
1.保持视图子对象引用
2.
]]--
local LoginPanelview = {}
local this = LoginPanelview

LuaUIManager = require 'lua/game/LuaUIManager'
--初始化方法
function LoginPanelview:init(trans)
	this.transform=trans;
	this.AccountInputField=GetComponentInPath(this.transform,"content/HotFixTipsContent/account_panel/account_InputField",typeof(CS.UnityEngine.UI.InputField));
	if(this.AccountInputField~=nil)
	then
		print("Account:"..this.AccountInputField.text);
	end
	
	this.PasswordInputField=GetComponentInPath(this.transform,"content/HotFixTipsContent/pass_panel/account_InputField",typeof(CS.UnityEngine.UI.InputField));
	if(this.PasswordInputField~=nil)
	then
		print("Password:"..this.PasswordInputField.text);
	end
	
	this.BtnGuestLogin=GetComponentInPath(this.transform,"content/HotFixTipsContent/btn_guest_login",typeof(CS.UnityEngine.UI.Button));
	if (this.BtnGuestLogin~=nil) then
		this.BtnGuestLogin.onClick:AddListener
		(
			function()
				print("Click Guest Login Button");
				local guestLoginData={};
				uniqueToken=CS.ZhuYuU3d.Platform.CallManager.GetDeviceUnqueID ();
				print("Token:"..uniqueToken);
				guestLoginData.Token=uniqueToken;
				es("TryGuestLogin",guestLoginData);
			end
		);
	end
	
	this.BtnWechatLogin=GetComponentInPath(this.transform,"content/HotFixTipsContent/btn_wechat_login",typeof(CS.UnityEngine.UI.Button));
	if (this.BtnWechatLogin~=nil) then
		this.BtnWechatLogin.onClick:AddListener
		(
			function()
				print("Click wechat Login Button");
			end
		);
	end
	
	this.BtnQQLogin=GetComponentInPath(this.transform,"content/HotFixTipsContent/btn_qq_login",typeof(CS.UnityEngine.UI.Button));
	if (this.BtnQQLogin~=nil) then
		this.BtnQQLogin.onClick:AddListener
		(
			function()
				print("Click QQ Login Button");
			end
		);
	end
	
	this.BtnBackGame=GetComponentInPath(this.transform,"content/HotFixTipsContent/btn_back",typeof(CS.UnityEngine.UI.Button));
	if (this.BtnBackGame~=nil) then
		this.BtnBackGame.onClick:AddListener
		(
			function()
				LuaUIManager.CloseWindow("LoginPanel");
				print("Click Back Game Button");
			end
		);
	end
	
	this.BtnLogin=GetComponentInPath(this.transform,"content/HotFixTipsContent/btn_login",typeof(CS.UnityEngine.UI.Button));
	if (this.BtnLogin~=nil) then
		this.BtnLogin.onClick:AddListener
		(
			function()

				if this.AccountInputField.text=="" then
					print("Account is nil.");
					return;
				end
				
				if this.PasswordInputField.text=="" then
					print("Password is nil.");
					return;
				end
				
				print("Click Login Button");
				
				es("V2C_LoginModule_LoginByPhone",{["phoneNumber"]=this.AccountInputField.text , ["password"]=this.PasswordInputField.text});
				
				
			end
		);
	end

	this.BtnRegister=GetComponentInPath(this.transform,"content/HotFixTipsContent/account_panel/btn_checkcode",typeof(CS.UnityEngine.UI.Button));
	if (this.BtnRegister~=nil) then
		this.BtnRegister.onClick:AddListener
		(
			function()
				print("Click Register Button");
				LuaUIManager.register('RegisterPanel',{
											name = 'RegisterPanel',
											layer = 'PopupCanvas',
											path = 'lua/modules/Register/Ctrl/RegisterPanelCtrl.lua' 
											})

				LuaUIManager.open('RegisterPanel',nil,nil);
			end
		);
	end	
	
	this.BtnGuestService=GetComponentInPath(this.transform,"content/btn_guest_service",typeof(CS.UnityEngine.UI.Button));
	if (this.BtnGuestService~=nil) then
		this.BtnGuestService.onClick:AddListener
		(
			function()
				print("Click BtnGuestService Button");
			end
		);
	end	
	
	
end	

--销毁方法
function LoginPanelview:on_destroy()
	
	
end
--视图状态
function LoginPanelview:set_state(viewState)
	
end

function LoginPanelview.setAccountLabel(strValue)
	this.AccountInputField.text=strValue;
end

function LoginPanelview.setPassLabel(strValue)
	this.PasswordInputField.text=strValue;
end

return LoginPanelview