LocalDataManager={};

local this=LocalDataManager;

local UserInfo=
{
	["_AccountData"]="",
	["_Password"]="",
};

LocalDataManager.UserInfoData=UserInfo;

function UserInfo:SetAccount(accinfo)
	self["_AccountData"]=accinfo;
end

function UserInfo:SetPassword(pass)
	self["_Password"]=pass;
end

function UserInfo:GetAccount()
	return self["_AccountData"];
end

function UserInfo:GetPassword()
	return self["_Password"];
end

function UserInfo:SaveData()

	CS.UnityEngine.PlayerPrefs.SetString("_AccountData",self["_AccountData"]);
	CS.UnityEngine.PlayerPrefs.SetString("_Password",self["_Password"]);
	
end

function UserInfo:ReadDataFromFile()

	self["_AccountData"]=CS.UnityEngine.PlayerPrefs.GetString("_AccountData","");
	self["_Password"]=CS.UnityEngine.PlayerPrefs.GetString("_Password","");
	
	print("AccountData:"..self["_AccountData"]);
	print("PasswordData:"..self["_Password"]);
	
end


function LocalDataManager.Init()
	UserInfo:ReadDataFromFile();
end

function LocalDataManager:IsExistUserInfo()
	if UserInfo["_AccountData"]=="" or UserInfo["_Password"]=="" then
		return false;
	end
	return true;
end

function LocalDataManager.SaveAccountAndPassword(act,pass)
	LocalDataManager.UserInfoData:SetPassword(pass);
	LocalDataManager.UserInfoData:SetAccount(act);
	LocalDataManager.UserInfoData:SaveData();
end

function LocalDataManager.GetAccountAndPassword()
	return LocalDataManager.UserInfoData:GetAccount(),LocalDataManager.UserInfoData:GetPassword();
end

LocalDataManager.Init();

return LocalDataManager;