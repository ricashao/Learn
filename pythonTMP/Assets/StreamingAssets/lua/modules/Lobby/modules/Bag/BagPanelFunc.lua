--local ZLobbyModuleData = require 'lua/datamodules/ZShopModuleData'

function bag_tabselect(index,transform,data)
	es(LobbyEventConst.Bag_TabChange,index)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(true)
	select1.gameObject:SetActive(false)
end

function bag_tabunselect(index,transform,data)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(false)
	select1.gameObject:SetActive(true)
end

function bag_good_setdata(index,transform,data)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local pricetext = transform:Find('PriceText'):GetComponent('Text')
	local cfg = GoodsConfigs.getItemByID(data.mid)
	counttext.text = cfg.name
	pricetext.text = ''
end

function bag_good_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		print('select>>>>>>>>> '..selectitem.itemIndex)
	end)
	
end

function bag_good_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function bag_good_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end

function bag_backup_setdata(index,transform,data)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local pricetext = transform:Find('PriceText'):GetComponent('Text')
	counttext.text = data[1]
	pricetext.text = data[2]
end

function bag_backup_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		print('select>>>>>>>>> '..selectitem.itemIndex)
	end)
end

function bag_backup_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function bag_backup_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end
