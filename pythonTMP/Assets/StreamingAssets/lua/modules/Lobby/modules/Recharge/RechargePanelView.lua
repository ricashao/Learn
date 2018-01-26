--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local RechargePanelView = {}
local this = RechargePanelView
--初始化方法
function RechargePanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.closebutton = self.transform:Find('CloseButton'):GetComponent('Button')
	self.alipaybutton = self.transform:Find('AlipayButton'):GetComponent('Button')
	self.wechatbutton = self.transform:Find('WechatButton'):GetComponent('Button')
	self.qqpursebutton = self.transform:Find('QQPurseButton'):GetComponent('Button')
	self.bankbutton = self.transform:Find('BankButton'):GetComponent('Button')
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function RechargePanelView:on_destroy()
	
	this.transform = nil

	print('RechargePanelView:ondestroy!')
end
--视图状态
function RechargePanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> RechargePanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end
end

return RechargePanelView