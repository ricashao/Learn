--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local TaskPanelView = {}
local this = TaskPanelView
--初始化方法
function TaskPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.backbutton = self.transform:Find('BackButton'):GetComponent('Button')
	self.tabgroup = self.transform:Find('TabGroup'):GetComponent("SelectGroup")
	self.dailylistgo = self.transform:Find('DailyList').gameObject
	self.dailylist = self.transform:Find('DailyList/Viewport/Content'):GetComponent("UILoopList")
	self.dailylist_selectgroup = self.transform:Find('DailyList/Viewport/Content'):GetComponent("SelectGroup")
	self.backuplistgo = self.transform:Find('BackupList').gameObject
	self.backuplist = self.transform:Find('BackupList/Viewport/Content'):GetComponent("UILoopList")
	self.backuplist_selectgroup = self.transform:Find('BackupList/Viewport/Content'):GetComponent("SelectGroup")
	self.testbutton = self.transform:Find('Button'):GetComponent('Button')
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function TaskPanelView:on_destroy()
	
	this.transform = nil

	print('TaskPanelView:ondestroy!')
end
--视图状态
function TaskPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> TaskPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end	
	if viewState == 'daily_state' then
		self.dailylistgo:SetActive(true)
		self.backuplistgo:SetActive(false)
	end
	if viewState == 'backup_state' then
		self.dailylistgo:SetActive(false)
		self.backuplistgo:SetActive(true)
	end
end

return TaskPanelView