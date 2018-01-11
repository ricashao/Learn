
local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'
function changeHead_onSelect(index,transform,data)
	if(index ~=0) then
		ZLobbyModuleData.send_CS_ChangeUserFace(CommonData.user.id,data)
	else
	end
end

function changeHead_unSelect(index,transform,data)
	
end

function changeHead_setData(index,transform,data)
	local type1 = transform:Find('Type1')
	local type2 = transform:Find('Type2')
	local selected = type2:Find('SelectImage')
	if index == 0 then
		type1.gameObject:SetActive(true)
		type2.gameObject:SetActive(false)
	else
		type1.gameObject:SetActive(false)
		type2.gameObject:SetActive(true)
		--TODO换头像
	end
	if CommonData.user.face == data then
		selected.gameObject:SetActive(true)
	else
		selected.gameObject:SetActive(false)
	end
end