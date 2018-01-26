local SettingPanelview = require 'lua/modules/Lobby/modules/Setting/SettingPanelView'

SettingData=require 'lua/modules/Lobby/modules/Setting/GameSettingData'
 
SettingPanelCtrl=
{
	 
	
};

local this=SettingPanelCtrl;

LuaUIManager = require 'lua/game/LuaUIManager'

function awake()

	print("Port:"..Define.port);

	SettingPanelview:init(self.transform);
	
	if(SettingPanelview.Slider_Volume_Right~=nil)then
		
		SettingPanelview.Slider_Volume_Right.onValueChanged:AddListener
		(
			function(fPercenterValue)
				--this.VolumeSize(fPercenterValue);
				print("Value:"..tostring(fPercenterValue));
				SettingPanelview.Slider_Volume_Left.value=fPercenterValue;
				SettingData:SetVolumeSize(fPercenterValue);
			end
		);
		
	end
	
	if(SettingPanelview.Toggle_SoundEffect~=nil)then
		SettingPanelview.Toggle_SoundEffect.onValueChanged:AddListener
		(
			function(bOn)
				
				print("Toggle Value:"..tostring(bOn));
				SettingData:SetSoundEffectState(bOn);
			end
		);
		
	end
	
	if(SettingPanelview.btnMusicOff~=nil)then
		SettingPanelview.btnMusicOff.onClick:AddListener
		(
		function()
			this:OnImgMusicClk(SettingPanelview.btnMusicOff);
		end
		);
	end
	
	if(SettingPanelview.btnMusicOn~=nil)then
		SettingPanelview.btnMusicOn.onClick:AddListener
		(
		function()
			this:OnImgMusicClk(SettingPanelview.btnMusicOn);
		end
		);
	end
	
	
	SettingPanelview:MusicSprState(SettingData:IsMusicOn());
	
	SettingPanelview:VolumeSize(SettingData:GetVolumeSize());
	
	SettingPanelview:SoundToggleState(SettingData:IsSoundEffectEnable());

	el("C2C_LoginModule2SettingModule_Close",SettingPanelCtrl.HandleMessage);
	
	
end

function SettingPanelCtrl.HandleMessage(strMessageName,param)
	if strMessageName=="C2C_LoginModule2SettingModule_Close" then
		LuaUIManager:ClosePageWithName("SettingPanel");
	end
end

function SettingPanelCtrl:OnImgMusicClk(UIWidgetSender)
	print("Send Name:"..UIWidgetSender.name);
	
	if(UIWidgetSender==SettingPanelview.btnMusicOff)then
		SettingData:SetMusicState(true);
	elseif(UIWidgetSender==SettingPanelview.btnMusicOn)then
		SettingData:SetMusicState(false);
	end
	
	SettingPanelview:MusicSprState(SettingData:IsMusicOn());
	--SettingPanelview:SoundToggleState(SettingData:IsSoundEffectEnable());
	
end
 
 
function start()

end

function update()

end

function ondestroy()
	
	 SettingData:Write();

	er("C2C_LoginModule2SettingModule_Close",SettingPanelCtrl.HandleMessage);
	
end

 