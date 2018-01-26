--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local SafeBoxPanelView = {}
local this = SafeBoxPanelView
--初始化方法
function SafeBoxPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.backbutton = self.transform:Find('BackButton'):GetComponent('Button')
	self.tabgroup = self.transform:Find('TabGroup'):GetComponent("SelectGroup")
	self.depositgo = self.transform:Find('DepositList').gameObject
	self.depositlist = self.transform:Find('DepositList/Viewport/Content'):GetComponent("UILoopList")
	self.depositlist_selectgroup = self.transform:Find('DepositList/Viewport/Content'):GetComponent("SelectGroup")
	self.withdrawlistgo = self.transform:Find('WithdrawList').gameObject
	self.withdrawlist = self.transform:Find('WithdrawList/Viewport/Content'):GetComponent("UILoopList")
	self.withdrawlistgo_selectgroup = self.transform:Find('WithdrawList/Viewport/Content'):GetComponent("SelectGroup")
	self.depositview = self.transform:Find('DepositView').gameObject
	self.withdrawview = self.transform:Find('WithdrawView').gameObject
	self.depositbutton = self.transform:Find('DepositView/DepositButton'):GetComponent('Button')
	self.depositinput = self.transform:Find('DepositView/DepositInputField'):GetComponent('InputField')
	self.withdrawbutton = self.transform:Find('WithdrawView/WithdrawButton'):GetComponent('Button')
	self.depositinput = self.transform:Find('WithdrawView/WithdrawInputField'):GetComponent('InputField')
	self.pwdinput = self.transform:Find('WithdrawView/PwdInputField'):GetComponent('InputField')
	self.goldtext = self.transform:Find('NumberText0'):GetComponent("Text")
	self.gemtext = self.transform:Find('NumberText1'):GetComponent("Text")
	self.tickettext = self.transform:Find('NumberText2'):GetComponent("Text")
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function SafeBoxPanelView:on_destroy()
	
	this.transform = nil

	print('SafeBoxPanelView:ondestroy!')
end
--视图状态
function SafeBoxPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> SafeBoxPanelView:setstate'..viewState)
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

return SafeBoxPanelView