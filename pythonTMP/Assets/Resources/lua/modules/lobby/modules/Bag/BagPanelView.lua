--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local BagPanelView = {}
local this = BagPanelView
--初始化方法
function BagPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.backbutton = self.transform:Find('BackButton'):GetComponent('Button')
	self.tabgroup = self.transform:Find('TabGroup'):GetComponent("SelectGroup")
	self.goodlistgo = self.transform:Find('GoodList').gameObject
	self.goodlist = self.transform:Find('GoodList/Viewport/Content'):GetComponent("UILoopList")
	self.goodlist_selectgroup = self.transform:Find('GoodList/Viewport/Content'):GetComponent("SelectGroup")
	self.backuplistgo = self.transform:Find('BackupList').gameObject
	self.backuplist = self.transform:Find('BackupList/Viewport/Content'):GetComponent("UILoopList")
	self.backuplist_selectgroup = self.transform:Find('BackupList/Viewport/Content'):GetComponent("SelectGroup")
	self.tipview = self.transform:Find('TipView')
	self.tiptext = self.tipview:Find('TipText'):GetComponent('Text')
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function BagPanelView:on_destroy()
	
	this.transform = nil

	print('BagPanelView:ondestroy!')
end
--视图状态
function BagPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> BagPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		self.tipview.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end	
	if viewState == 'good_state' then
		self.goodlistgo:SetActive(true)
		self.backuplistgo:SetActive(false)
	end
	if viewState == 'backup_state' then
		self.goodlistgo:SetActive(false)
		self.backuplistgo:SetActive(true)
	end
	if viewState == 'help_hide' then
		self.tipview.gameObject:SetActive(false)
	end
	if viewState == 'help_show' then
		self.tipview.gameObject:SetActive(true)
	end
end

return BagPanelView