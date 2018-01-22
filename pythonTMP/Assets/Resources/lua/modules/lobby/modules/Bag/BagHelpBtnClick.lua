local this = scriptEnv
function OnPointerDown()
	local data = self.transform.parent:GetComponent('LuaSelectLoopNewItem'):GetData()
	es(LobbyEventConst.Bag_HelpPress,data)
end

function OnPointerUp()
	es(LobbyEventConst.Bag_HelpRelease)
end