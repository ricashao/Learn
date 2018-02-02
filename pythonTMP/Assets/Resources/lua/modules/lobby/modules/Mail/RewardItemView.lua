local RewardItemView =
{
	["TransRoot"]=nil,
	["Img_Icon"]=nil,
	["Txt_Count"]=nil,
}

local this = RewardItemView

local LuaUIManager = require 'lua/game/LuaUIManager'

function  RewardItemView:New(trans)

	local self = {}

	setmetatable( self , {__index = RewardItemView})

	self:Init(trans);

	return self;
end

 

function RewardItemView:Init(trans)

	self.TransRoot=trans;

	self.Img_Icon=GetComponentInPath(trans,"Img_Select_BG",typeof(CS.UnityEngine.UI.Image));

	self.Txt_Count=GetComponentInPath(trans,"txt_datetime",typeof(CS.UnityEngine.UI.Text));


end	



return RewardItemView