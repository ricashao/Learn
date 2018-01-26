local ZShopModuleData = require 'lua/datamodules/ZShopModuleData'
local uimanager = require 'lua/game/LuaUIManager'
function shop_tabselect(index,transform,data)
	es(LobbyEventConst.Shop_TabChange,index)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(true)
	select1.gameObject:SetActive(false)
end

function shop_tabunselect(index,transform,data)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(false)
	select1.gameObject:SetActive(true)
end

function shop_buygem_setdata(index,transform,data)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local pricetext = transform:Find('PriceText'):GetComponent('Text')
	local cfg = GoodsConfigs.getItemByID(data.mid)
	counttext.text = cfg.name
	pricetext.text = cfg.price/100 .. '元'
end

function shop_buygem_awake(index,transform,data)
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		--[[
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		local itemdata = selectitem:GetData()
		ZShopModuleData.send_CS_GetItem(CommonData.user.id,itemdata.mid,1)
		]]
		
		uimanager.open('RechargePanel')
	end)
end

function shop_buygem_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function shop_buygem_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end

function shop_buygold_setdata(index,transform,data)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local pricetext = transform:Find('PriceText'):GetComponent('Text')
	local cfg = GoodsConfigs.getItemByID(data.mid)
	counttext.text = cfg.name
	pricetext.text = cfg.price .. '钻石'
end

function shop_buygold_awake(index,transform,data)
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		local itemdata = selectitem:GetData()
		local cfg = GoodsConfigs.getItemByID(itemdata.mid)
		if cfg.price>CommonData.user_info.gem then
			uimanager.ToastTip('钻石不足',3,30)
			return
		end
		ZShopModuleData.send_CS_GetItem(CommonData.user.id,itemdata.mid,1)
	end)
end

function shop_buygold_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function shop_buygold_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end

function shop_buygood_setdata(index,transform,data)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local pricetext = transform:Find('PriceText'):GetComponent('Text')
	local cfg = GoodsConfigs.getItemByID(data.mid)
	counttext.text = cfg.name
	pricetext.text = cfg.price .. '金币'
end

function shop_buygood_awake(index,transform,data)
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		local itemdata = selectitem:GetData()
		ZShopModuleData.send_CS_GetItem(CommonData.user.id,itemdata.mid,1)
	end)
end

function shop_buygood_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function shop_buygood_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end

function shop_buyreal_setdata(index,transform,data)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local pricetext = transform:Find('PriceText'):GetComponent('Text')
	local cfg = GoodsConfigs.getItemByID(data.mid)
	counttext.text = cfg.name
	pricetext.text = cfg.price .. '点券'
end

function shop_buyreal_awake(index,transform,data)
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		uimanager.open('RechargePanel')
	end)
end

function shop_buyreal_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function shop_buyreal_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end