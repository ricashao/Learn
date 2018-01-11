--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local RegisterPanelView = {}
local this = RegisterPanelView
--初始化方法
function RegisterPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.cancelbutton = self.transform:Find('CancelButton')
	self.closebutton = self.transform:Find('CloseButton')
	self.surebutton = self.transform:Find('SureButton')
	self.getcodebutton = self.transform:Find('GetCodeButton')
	self.accountinput = self.transform:Find('AccountInputField'):GetComponent("InputField")
	self.pwd1input = self.transform:Find('PasswordInputField'):GetComponent("InputField")
	self.pwd2input = self.transform:Find('ConfirmInputField'):GetComponent("InputField")
	self.codeinput = self.transform:Find('CodeInputField'):GetComponent("InputField")
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function RegisterPanelView:on_destroy()
	
	this.transform = nil

	print('RegisterPanelView:ondestroy!')
end
--视图状态
function RegisterPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> RegisterPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end	
	if viewState == 'clicked_state' then
		--DemoPanelview.image.gameObject:SetActive(true)
	end
	if viewState == 'close_state' then
		this.transform.gameObject:SetActive(false)
	end
end

return RegisterPanelView