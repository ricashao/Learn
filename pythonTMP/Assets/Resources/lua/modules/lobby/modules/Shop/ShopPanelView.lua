--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local ShopkPanelView = {}
local this = ShopkPanelView
--初始化方法
function ShopkPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.backbutton = self.transform:Find('BackButton'):GetComponent('Button')
	self.tabgroup = self.transform:Find('TabGroup'):GetComponent("SelectGroup")
	self.list1go = self.transform:Find('List1').gameObject
	self.list1 = self.transform:Find('List1/Viewport/Content'):GetComponent("UILoopList")
	self.list1_selectgroup = self.transform:Find('List1/Viewport/Content'):GetComponent("SelectGroup")
	self.list2go = self.transform:Find('List2').gameObject
	self.list2 = self.transform:Find('List2/Viewport/Content'):GetComponent("UILoopList")
	self.list2_selectgroup = self.transform:Find('List2/Viewport/Content'):GetComponent("SelectGroup")
	self.list3go = self.transform:Find('List3').gameObject
	self.list3 = self.transform:Find('List3/Viewport/Content'):GetComponent("UILoopList")
	self.list3_selectgroup = self.transform:Find('List3/Viewport/Content'):GetComponent("SelectGroup")
	self.list4go = self.transform:Find('List4').gameObject
	self.list4 = self.transform:Find('List4/Viewport/Content'):GetComponent("UILoopList")
	self.list4_selectgroup = self.transform:Find('List4/Viewport/Content'):GetComponent("SelectGroup")	
	self.goldtext = self.transform:Find('NumberText0'):GetComponent("Text")
	self.gemtext = self.transform:Find('NumberText1'):GetComponent("Text")
	self.tickettext = self.transform:Find('NumberText2'):GetComponent("Text")
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function ShopkPanelView:on_destroy()
	
	this.transform = nil

	print('ShopkPanelView:ondestroy!')
end
--视图状态
function ShopkPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> ShopkPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end	
	if viewState == 'buygem_state' then
		self.list1go:SetActive(true)
		self.list2go:SetActive(false)
		self.list3go:SetActive(false)
		self.list4go:SetActive(false)
	end
	if viewState == 'buygold_state' then
		self.list1go:SetActive(false)
		self.list2go:SetActive(true)
		self.list3go:SetActive(false)
		self.list4go:SetActive(false)
	end
	if viewState == 'buygood_state' then
		self.list1go:SetActive(false)
		self.list2go:SetActive(false)
		self.list3go:SetActive(true)
		self.list4go:SetActive(false)
	end
	
	if viewState == 'buyreal_state' then
		self.list1go:SetActive(false)
		self.list2go:SetActive(false)
		self.list3go:SetActive(false)
		self.list4go:SetActive(true)
	end
end

return ShopkPanelView