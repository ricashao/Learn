local ZShopModuleData = require 'lua/datamodules/ZShopModuleData'

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
	counttext.text = data[1]
	pricetext.text = data[2]
end

function shop_buygem_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		ZShopModuleData.send_CS_GetItem(CommonData.user.id,2,10)
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
	counttext.text = data[1]
	pricetext.text = data[2]
end

function shop_buygold_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		print('select>>>>>>>>> '..selectitem.itemIndex)
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
	counttext.text = data[1]
	pricetext.text = data[2]
end

function shop_buygood_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		print('select>>>>>>>>> '..selectitem.itemIndex)
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
	counttext.text = data[1]
	pricetext.text = data[2]
end

function shop_buyreal_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local buybutton = transform:Find('BuyButton'):GetComponent('Button')
	buybutton.onClick:AddListener(function()
		print('select>>>>>>>>> '..selectitem.itemIndex)
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