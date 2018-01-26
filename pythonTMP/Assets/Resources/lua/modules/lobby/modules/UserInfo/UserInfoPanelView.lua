--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local UserInfoPanelView = {}
local this = UserInfoPanelView
--初始化方法
function UserInfoPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.userview = self.transform:Find('UserView')
	self.backbutton = self.transform:Find('BackButton')
	self.changepwdbutton = self.userview.transform:Find('ChangePwdButton')
	self.bindphonebutton = self.userview.transform:Find('BindPhoneButton')
	self.changenamebutton = self.userview.transform:Find('ChangeNameButton')
	self.changeheadbutton = self.userview.transform:Find('ChangeHeadButton')
	self.nameinput = self.userview.transform:Find('NameInputField'):GetComponent("InputField")
	self.gemtext = self.userview.transform:Find('GemText'):GetComponent("Text")
	self.goldbanktext = self.userview.transform:Find('GoldBankText'):GetComponent("Text")
	self.uidtext = self.userview.transform:Find('UidText'):GetComponent("Text")
	self.headimg = self.userview.transform:Find('HeadImage'):GetComponent("WebImg")
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function UserInfoPanelView:on_destroy()
	
	this.transform = nil

	print('UserInfoPanelView:ondestroy!')
end
--视图状态
function UserInfoPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> UserInfoPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end	
	if viewState == 'clicked_state' then
		--DemoPanelview.image.gameObject:SetActive(true)
	end
	if viewState == 'official_state' then
		this.bindphonebutton.gameObject:SetActive(false)
		this.changepwdbutton.gameObject:SetActive(true)
	elseif viewState == 'visitor_state' then
		this.bindphonebutton.gameObject:SetActive(true)
		this.changepwdbutton.gameObject:SetActive(false)
	elseif viewState == 'qq_state' or viewState == 'wechat_state' then
		this.bindphonebutton.gameObject:SetActive(false)
		this.changepwdbutton.gameObject:SetActive(false)
	end
end

return UserInfoPanelView