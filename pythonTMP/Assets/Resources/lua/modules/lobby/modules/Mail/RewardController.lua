local RewardItemControllerClass=require("lua/modules/Lobby/modules/Mail/RewardItemController");

local rewardService=require 'lua/modules/Lobby/modules/Mail/RewardServices'



local RewardController=
{
["tblRewardItemController"]={},
["transParent"]=nil,
["transTemplate"]=nil,
};

local this=RewardController;

function RewardController.HandleMessage(strMessageName,param)

	if "S2C_RewradModules_GetReward"==strMessageName then

		objItemController=this.FindItemControllerByID( tonumber(param.ID));
		if objItemController~=nil then
			objItemController:InitTitle( tostring(param.Count));
			objItemController:LoadImage(tostring(param.Path));
		end

	end
end

function RewardController.FindItemControllerByID(nid)
	for k,v in pairs(this.tblRewardItemController) do
		if v.ID == nid then
			return v;
		end
	end
	return nil;
end

function RewardController.Init(tblRewards,transParent,transTemplate)

	rewardService.Init();

	this.tblRewardItemController={};

	this.transParent=transParent;

	this.transTemplate=transTemplate;

	this.transTemplate.gameObject:SetActive(false);

	for k,v in pairs(tblRewards) do

		local goObj=CS.UnityEngine.GameObject.Instantiate(this.transTemplate,this.transParent,false);

		if goObj~=nil then
			goObj=goObj.gameObject;
			goObj.name=v;
			goObj:SetActive(true);
			local RewardItemControllerObj=RewardItemControllerClass:New(goObj.transform,v);
			table.insert(this.tblRewardItemController,RewardItemControllerObj);
		end

	end

	el("S2C_RewradModules_GetReward",this.HandleMessage);

end

function RewardController.Clear()
	er("S2C_RewradModules_GetReward",this.HandleMessage);
	rewardService.Clear();
	--this=nil;
end


return RewardController;

