
RewardServices=
{
["TblReward"]={},

};

local this=RewardServices;

local Iteminfo=require("lua/config/Iteminfo");
local reward=require('lua/modules/Lobby/modules/Mail/Reward');


function RewardServices.Init()

	--el("C2S_RewardModules_AddReward",this.HandleMessage);
	el("C2S_RewradModules_GetReward",this.HandleMessage);
end

function RewardServices.Clear()
	--er("C2S_RewardModules_AddReward",this.HandleMessage);
	er("C2S_RewradModules_GetReward",this.HandleMessage);
end

function RewardServices.HandleMessage(strMessageName,param)

	if strMessageName=="C2S_RewradModules_GetReward"then
		this.GetReward(param);
	end

end

function RewardServices.ParseReward(strReward)

	local straryret=String_Split(strReward,",");
	if straryret~=nil and #straryret>=3 then

		local argType=straryret[1];
		local arg2=straryret[2];
		local argCount=straryret[3];

		local EqualmentInfo=GoodsConfigs.getItemByID(tonumber(argType));
		if(EqualmentInfo~=nil)then
			local bInTable,objreward=this.IsInTable(argType);
			if bInTable==false then
				objreward=reward:New();
				table.insert(this.TblReward, objreward);
			end
			--objreward.ID=argType;
			objreward.ID=argType;
			objreward.Name=EqualmentInfo.name;
			objreward.Arg_1=argType;
			objreward.Arg_2=arg2;
			objreward.Count=argCount;
			objreward.Path=this.GetRewardPath(argType);
			return objreward;
		end
	end
	return nil;
end

function RewardServices.GetRewardPath(argType)
	if argType=="1" then
		return "icon/gold";
	elseif argType=="2" then
		return "icon/diamond";
	else
		return nil;
	end

end

function RewardServices.AddReward(strReward)
	local objreward= this.ParseReward(strReward);
	if objreward~=nil then

		print("Reward is exist."..objreward.Name);

		return objreward;--objreward.ID;

	end

end

function RewardServices.GetReward(strReward)

	local objreward=this.AddReward(strReward);

	if objreward~=nil then
		if EventExist("S2C_RewradModules_GetReward") then
			es("S2C_RewradModules_GetReward",objreward);
		end
	end


end

function RewardServices.IsInTable(nid)
	for k,v in pairs(this.TblReward) do
		if v.mid==nid then
			return true,v
		end
	end
	return false,nil
end



return RewardServices;

