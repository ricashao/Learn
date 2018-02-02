local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'
local uimanager = require 'lua/game/LuaUIManager'
function safebox_tabselect(index,transform,data)
	es(LobbyEventConst.SafeBox_TabChange,index)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(true)
	select1.gameObject:SetActive(false)
end

function safebox_tabunselect(index,transform,data)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(false)
	select1.gameObject:SetActive(true)
end

function safebox_deposit_setdata(index,transform,data)
	local webimg = transform:Find('CountImage'):GetComponent('WebImg')
	local url =''
	if data ~= -1 then
		url = string.format('ui/icon/safebox/safebox_%dw.png',data)
	else
		url = 'ui/icon/safebox/safebox_all.png'
	end
	webimg:Url(url)
end

function safebox_deposit_awake(index,transform,data)
end

function safebox_deposit_onselect(index,transform,data)
	local selectgo = transform:Find('IsSelect').gameObject
	selectgo:SetActive(true)
	es(LobbyEventConst.SafeBox_MoneySelect,data)
end

function safebox_deposit_unselect(index,transform,data)
	local selectgo = transform:Find('IsSelect').gameObject
	selectgo:SetActive(false)
end

function safebox_withdraw_setdata(index,transform,data)
	local webimg = transform:Find('CountImage'):GetComponent('WebImg')
	if data ~= -1 then
		url = string.format('ui/icon/safebox/safebox_%dw.png',data)
	else
		url = 'ui/icon/safebox/safebox_all.png'
	end
	webimg:Url(url)
end

function safebox_withdraw_awake(index,transform,data)
end

function safebox_withdraw_onselect(index,transform,data)
	local selectgo = transform:Find('IsSelect').gameObject
	selectgo:SetActive(true)
	es(LobbyEventConst.SafeBox_MoneySelect,data)
end

function safebox_withdraw_unselect(index,transform,data)
	local selectgo = transform:Find('IsSelect').gameObject
	selectgo:SetActive(false)
end
