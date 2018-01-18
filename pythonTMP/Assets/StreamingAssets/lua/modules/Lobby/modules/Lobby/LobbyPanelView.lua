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

	self.HaedImageButton = self.transform:Find('HaedImageButton')
	self.UserNameText = self.transform:Find('UserNameText'):GetComponent("Text")
	self.NumberAddButton0 = self.transform:Find('NumberAddButton0')
	self.NumberAddButton1 = self.transform:Find('NumberAddButton1')
	self.NumberAddButton2 = self.transform:Find('NumberAddButton2')
	self.NumberText0 = self.transform:Find('NumberText0'):GetComponent("Text")
	self.NumberText1 = self.transform:Find('NumberText1'):GetComponent("Text")
	self.NumberText2 = self.transform:Find('NumberText2'):GetComponent("Text")
	self.SettingButton = self.transform:Find('SettingButton')
	self.KefuButton = self.transform:Find('KefuButton')
	self.BottomLeftButton = self.transform:Find('BottomLeftButton')
	self.BottomRightButton = self.transform:Find('BottomRightButton')
	self.BottomButton0 = self.transform:Find('BottomButton0')
	self.BottomButton1 = self.transform:Find('BottomButton1')
	self.BottomButton2 = self.transform:Find('BottomButton2')
	self.BottomButton3 = self.transform:Find('BottomButton3')
	self.BottomButton4 = self.transform:Find('BottomButton4')
	self.BottomButton5 = self.transform:Find('BottomButton5')
	self.MarqueeText = self.transform:Find('Marquee/bg/Text0'):GetComponent("Text")
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