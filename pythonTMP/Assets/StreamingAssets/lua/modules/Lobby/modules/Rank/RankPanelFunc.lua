local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'
local E_MoneyType = 
{
    GOLD = 'GOLD';           --金币
    GEM = 'GEM';            --钻石
    TICKET = 'TICKET';         --点卷
}

function rank_tabselect(index,transform,data)
	if index == 0 then
		ZLobbyModuleData.send_CS_GetRankList(CommonData.user.id,E_MoneyType.GEM)
	elseif index == 1 then
		ZLobbyModuleData.send_CS_GetRankList(CommonData.user.id,E_MoneyType.TICKET)
	elseif index == 2 then
		ZLobbyModuleData.send_CS_GetRankList(CommonData.user.id,E_MoneyType.GOLD)
	else 
	end
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(true)
	select1.gameObject:SetActive(false)
end

function rank_tabunselect(index,transform,data)
	
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(false)
	select1.gameObject:SetActive(true)
end

function rank_setdata(index,transform,data)
	local ranktext = transform:Find('RankText'):GetComponent('Text')
	ranktext.text = index+4
	local nametext = transform:Find('NameText'):GetComponent('Text')
	nametext.text = data.nick_name
	local counttext = transform:Find('CountText'):GetComponent('Text')
	counttext.text = data.value
end