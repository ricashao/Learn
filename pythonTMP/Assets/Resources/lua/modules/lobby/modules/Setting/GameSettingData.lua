
GameSettingData=
{
	 
	["MusicEnable"]=0,
	["VolumeSize"]=0,
	["SoundEnable"]=0,
	
};

local this=GameSettingData;

function GameSettingData:IsMusicOn()
	if self.MusicEnable==1 then
		return true;
	else
		return false;
	end
end

function GameSettingData:SetMusicState(bOn)
	if(bOn==true) then
		self.MusicEnable=1;
	elseif bOn==false then
		self.MusicEnable=0;
	end
	self:Write();
end

function GameSettingData:GetVolumeSize()
	return self.VolumeSize;
end

function GameSettingData:SetVolumeSize(nVoluemeSize)
	self.VolumeSize=nVoluemeSize;
	
end

function GameSettingData:IsSoundEffectEnable()
	if self.SoundEnable==1 then
		return true;
	else
		return false;
	end
end

function GameSettingData:SetSoundEffectState(bon)
	print("SetSoundState:"..tostring(bon));
	if bon==true then
		
		self.SoundEnable=1;
	elseif bon==false then
		
		self.SoundEnable=0;
	end
	
	self:Write();
end

function GameSettingData:Init()
	self.MusicEnable=CS.UnityEngine.PlayerPrefs.GetInt("_GameMusicEnable",1);
	self.SoundEnable=CS.UnityEngine.PlayerPrefs.GetInt("_GameSoundEnable",1);
	self.VolumeSize=CS.UnityEngine.PlayerPrefs.GetFloat("_GameVolumeSize",50);
end

function GameSettingData:Write()
	CS.UnityEngine.PlayerPrefs.SetInt("_GameMusicEnable",self.MusicEnable);
	CS.UnityEngine.PlayerPrefs.SetInt("_GameSoundEnable",self.SoundEnable);
	CS.UnityEngine.PlayerPrefs.SetFloat("_GameVolumeSize",self.VolumeSize);
	CS.UnityEngine.PlayerPrefs.Save();
	self:Print();
end

function GameSettingData:Print()
	print("Music Enable:"..self.MusicEnable);
	print("Sound Enable:"..self.SoundEnable);
	print("Volume Size:"..self.VolumeSize);

end

GameSettingData:Init();
GameSettingData:Print();

return (GameSettingData);