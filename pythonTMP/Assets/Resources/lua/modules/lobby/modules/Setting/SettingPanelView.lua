--[[
视图层模板
1.保持视图子对象引用
2.
]]--
local SettingPanelview = 
{
	 
}

local this = SettingPanelview

local LuaUIManager = require 'lua/game/LuaUIManager'
 

function SettingPanelview:init(trans)
	this.transform=trans;
	
	this.ImgMusicOff=GetComponentInPath(trans,"content/img_Music_Toggle_OFF",typeof(CS.UnityEngine.UI.Image));
	
	this.ImgMusicOn=GetComponentInPath(trans,"content/img_Music_Toggle_ON",typeof(CS.UnityEngine.UI.Image));
	
	this.btnMusicOff=GetComponentInPath(trans,"content/img_Music_Toggle_OFF",typeof(CS.UnityEngine.UI.Button));
	
	this.btnMusicOn=GetComponentInPath(trans,"content/img_Music_Toggle_ON",typeof(CS.UnityEngine.UI.Button));
	
	if(this.ImgMusicOn~=nil)then
		this.ImgMusicOn.gameObject:SetActive(false);
	end
	
	this.Slider_Volume_Left=GetComponentInPath(trans,"content/Slider_Volume_Left",typeof(CS.UnityEngine.UI.Slider));
	
	if(this.Slider_Volume_Left~=nil)then
		this.Slider_Volume_Left.interactable=false;
		this.Slider_Volume_Left.value=0;
	end
	
	this.Slider_Volume_Right=GetComponentInPath(trans,"content/Slider_Volume_Right",typeof(CS.UnityEngine.UI.Slider));
	
	
	this.Toggle_SoundEffect=GetComponentInPath(trans,"content/SoundEffect_Tog",typeof(CS.UnityEngine.UI.Toggle));
	
	this.Btn_SwitchAccount=GetComponentInPath(trans,"btn_SwitchAccount",typeof(CS.UnityEngine.UI.Button));
	
	if(this.Btn_SwitchAccount~=nil)then
		this.Btn_SwitchAccount.onClick:AddListener
		(
			function()
				print("Switch Account Button Clk");
				LuaUIManager.open('LoginPanel',nil,nil);
			end
		);
	end
	
	this.Btn_Back=GetComponentInPath(trans,"BackButton",typeof(CS.UnityEngine.UI.Button));
	
	if(this.Btn_Back~=nil)then
		this.Btn_Back.onClick:AddListener
		(
			function()
				print("BtnBack	Button Clk");
				LuaUIManager.CloseWindow("SettingPanel");
			end
		);
	end
	
	
end	

function SettingPanelview:MusicSprState(bMusicOn)
	
	if bMusicOn==true then
		self.ImgMusicOff.gameObject:SetActive(false);
		self.ImgMusicOn.gameObject:SetActive(true);
	else
		self.ImgMusicOff.gameObject:SetActive(true);
		self.ImgMusicOn.gameObject:SetActive(false);
	end
	
end

function SettingPanelview:VolumeSize(nSize)
	
	if(this.Slider_Volume_Right~=nil)then
		this.Slider_Volume_Right.value = nSize;
	end
	
	
	if(this.Toggle_SoundEffect~=nil)then
		this.Slider_Volume_Left.interactable=false;
		this.Slider_Volume_Left.value=nSize;
	end
	
end

function SettingPanelview:SoundToggleState(bON)
	
	this.Toggle_SoundEffect.isOn=bON;
	
end
 

return SettingPanelview