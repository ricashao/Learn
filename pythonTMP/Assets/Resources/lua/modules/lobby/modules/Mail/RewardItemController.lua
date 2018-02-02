
local RewardItemView= require 'lua/modules/Lobby/modules/Mail/RewardItemView'


local RewardItemController=
{
	["ID"]=-1,

	["Count"]=-1,

	["RewardItemView"]=nil,

	["RewardDesc"]=nil,
};

local this=RewardItemController;




function  RewardItemController:New(trans,strRewardContent)

	local self = {}

	setmetatable( self , {__index = RewardItemController })

	self.RewardItemView=RewardItemView:New(trans)

	self.RewardDesc=strRewardContent;

	local tblreward=String_Split(strRewardContent,",");
	if tblreward~=nil and #tblreward>=3 then
		self.ID=tonumber(tblreward[1]);
		print("Reward ID is"..self.ID);
	end

	self:Init(trans);

	return self;

end

function RewardItemController:LoadImage(strPath)
	print("load path:"..strPath);

	if strPath~=nil and strPath~="" then

		CS.LoadUIImg.Instance:LoadImgFromResource(strPath,self.RewardItemView.Img_Icon.transform);

	end
end

function RewardItemController:InitTitle(strTitle)
	self.RewardItemView.Txt_Count.text="x"..strTitle;
end






function RewardItemController:Init(transRoot)


	--self.RewardItemView:Init(transRoot);

	es("C2S_RewradModules_GetReward",self.RewardDesc);


end


function RewardItemController:Clear()

end


return RewardItemController;

