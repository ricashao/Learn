local this = scriptEnv
function OnPointerClick()
	local itemIndex = self:GetComponentInParent(typeof(CS.LuaSelectLoopNewItem)):GetIndex()
	local selectgroup = self:GetComponentInParent(typeof(CS.SelectGroup))
	selectgroup:SelectByIndex(itemIndex)
end