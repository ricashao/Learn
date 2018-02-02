--local ZLobbyModuleData = require 'lua/datamodules/ZShopModuleData'

local uimanager = require 'lua/game/LuaUIManager'
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
	local icon = transform:Find('IconImage'):GetComponent('WebImg')
	local cfg = GoodsConfigs.getItemByID(data.mid)
	counttext.text = cfg.name..'X'..data.amount
	icon:Url('ui/icon/item/item_'..data.mid..'.png')
	if cfg.price_type == 4 then
		pricetext.text = cfg.price/100 ..'元'
	elseif cfg.price_type == 3 then
		pricetext.text = cfg.price ..'点券'
	elseif cfg.price_type == 2 then
		pricetext.text = cfg.price ..'钻石'
	elseif cfg.price_type == 1 then
		pricetext.text = cfg.price ..'金币'
	else
		pricetext.text = ''
	end
end

function bag_good_awake(index,transform,data)
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		GameState.curRunState:setstate_byname('bagtoshop',selectitem:GetData())
		uimanager.open('ShopPanel')
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
