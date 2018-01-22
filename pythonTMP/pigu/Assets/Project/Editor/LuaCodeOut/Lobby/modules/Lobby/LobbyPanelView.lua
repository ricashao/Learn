--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local LobbyPanelView = {}
local this = LobbyPanelView
--初始化方法
function LobbyPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.HaedImageButton = self.transform:Find('Button')
	self.UserNameText = self.transform:Find('Text')
	self.NumberAddButton0 = self.transform:Find('Button')
	self.NumberAddButton1 = self.transform:Find('Button')
	self.NumberAddButton2 = self.transform:Find('Button')
	self.NumberText0 = self.transform:Find('Text')
	self.NumberText1 = self.transform:Find('Text')
	self.NumberText2 = self.transform:Find('Text')
	self.SettingButton = self.transform:Find('Button')
	self.KefuButton = self.transform:Find('Button')
	self.BottomLeftButton = self.transform:Find('Button')
	self.BottomRightButton = self.transform:Find('Button')
	self.BottomButton0 = self.transform:Find('Button')
	self.BottomButton1 = self.transform:Find('Button')
	self.BottomButton2 = self.transform:Find('Button')
	self.BottomButton3 = self.transform:Find('Button')
	self.BottomButton4 = self.transform:Find('Button')
	self.BottomButton5 = self.transform:Find('Button')
	self.IntoGameButton = self.transform:Find('Button')
	--AddRefCode 追加引用标志
	
	self:set_state('init_state')
end	

--销毁方法
function LobbyPanelView:on_destroy()
	
	this.transform = nil

	print('LobbyPanelView:ondestroy!')
end
--视图状态
function LobbyPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> LobbyPanellview:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
	end	
	if viewState == 'clicked_state' then
		--DemoPanelview.image.gameObject:SetActive(true)
	end
end

return LobbyPanelView