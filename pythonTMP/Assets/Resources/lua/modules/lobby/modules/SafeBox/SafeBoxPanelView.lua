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
	self.resetbutton = self.transform:Find('ResetButton'):GetComponent('Button')
	self.tabgroup = self.transform:Find('TabGroup'):GetComponent("SelectGroup")
	self.depositgo = self.transform:Find('DepositList').gameObject
	self.depositlist = self.transform:Find('DepositList/Viewport/Content'):GetComponent("UILoopList")
	self.depositlist_selectgroup = self.transform:Find('DepositList/Viewport/Content'):GetComponent("SelectGroup")
	self.withdrawlistgo = self.transform:Find('WithdrawList').gameObject
	self.withdrawlist = self.transform:Find('WithdrawList/Viewport/Content'):GetComponent("UILoopList")
	self.withdrawlist_selectgroup = self.transform:Find('WithdrawList/Viewport/Content'):GetComponent("SelectGroup")
	self.depositview = self.transform:Find('DepositView').gameObject
	self.withdrawview = self.transform:Find('WithdrawView').gameObject
	self.depositbutton = self.transform:Find('DepositView/DepositButton'):GetComponent('Button')
	self.depositinput = self.transform:Find('DepositView/DepositInputField'):GetComponent('InputField')
	self.withdrawbutton = self.transform:Find('WithdrawView/WithdrawButton'):GetComponent('Button')
	self.withdrawinput = self.transform:Find('WithdrawView/WithdrawInputField'):GetComponent('InputField')
	self.withdrawpwdinput = self.transform:Find('WithdrawView/WithdrawPwdInputField'):GetComponent('InputField')
	self.goldtext = self.transform:Find('NumberText0'):GetComponent("Text")
	self.gemtext = self.transform:Find('NumberText1'):GetComponent("Text")
	self.tickettext = self.transform:Find('NumberText2'):GetComponent("Text")
	self.resetviewgo = self.transform:Find('ResetView').gameObject
	self.resetview_closebutton = self.resetviewgo.transform:Find('CloseButton'):GetComponent('Button')
	self.resetview_codebutton = self.resetviewgo.transform:Find('GetCodeButton'):GetComponent('Button')
	self.resetview_resetbutton = self.resetviewgo.transform:Find('ResetButton'):GetComponent('Button')
	self.resetview_pwd1input = self.resetviewgo.transform:Find('ResetPwd1InputField'):GetComponent('InputField')
	self.resetview_pwd2input = self.resetviewgo.transform:Find('ResetPwd2InputField'):GetComponent('InputField')
	self.resetview_codeinput = self.resetviewgo.transform:Find('CodeInputField'):GetComponent('InputField')
	self.createviewgo = self.transform:Find('CreateView').gameObject
	self.createview_pwd1input = self.createviewgo.transform:Find('CreatePwd1InputField'):GetComponent('InputField')
	self.createview_pwd2input = self.createviewgo.transform:Find('CreatePwd2InputField'):GetComponent('InputField')
	self.createview_closebutton = self.createviewgo.transform:Find('CloseButton'):GetComponent('Button')
	self.createview_exitbutton = self.createviewgo.transform:Find('ExitButton'):GetComponent('Button')
	self.createview_createbutton = self.createviewgo.transform:Find('CreateButton'):GetComponent('Button')
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
		self.resetviewgo:SetActive(false)
		self.createviewgo:SetActive(false)
	end	
	if viewState == 'deposit_state' then
		self.depositgo:SetActive(true)
		self.withdrawlistgo:SetActive(false)
		self.depositview:SetActive(true)
		self.withdrawview:SetActive(false)
	end
	if viewState == 'withdraw_state' then
		self.depositgo:SetActive(false)
		self.withdrawlistgo:SetActive(true)
		self.depositview:SetActive(false)
		self.withdrawview:SetActive(true)
	end
	
	if viewState == 'reset_open' then
		self.resetviewgo:SetActive(true)
	end
	if viewState == 'reset_hide' then
		self.resetviewgo:SetActive(false)
	end
	if viewState == 'create_open' then
		self.createviewgo:SetActive(true)
	end
	if viewState == 'create_hide' then
		self.createviewgo:SetActive(false)
	end
end

return SafeBoxPanelView