--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local ExchangePanelView = {}
local this = ExchangePanelView
--初始化方法
function ExchangePanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.closebutton = self.transform:Find('CloseButton'):GetComponent('Button')
	self.codebutton = self.transform:Find('GetCodeButton'):GetComponent('Button')
	self.okbutton = self.transform:Find('OKButton'):GetComponent('Button')
	self.nameinput = self.transform:Find('NameInputField'):GetComponent('InputField')
	self.telinput = self.transform:Find('TelInputField'):GetComponent('InputField')
	self.codeinput = self.transform:Find('CodeInputField'):GetComponent('InputField')
	self.mailinput = self.transform:Find('MailInputField'):GetComponent('InputField')
	self.addressinput = self.transform:Find('AddressInputField'):GetComponent('InputField')
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function ExchangePanelView:on_destroy()
	
	this.transform = nil

	print('ExchangePanelView:ondestroy!')
end
--视图状态
function ExchangePanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> ExchangePanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end
end

return ExchangePanelView