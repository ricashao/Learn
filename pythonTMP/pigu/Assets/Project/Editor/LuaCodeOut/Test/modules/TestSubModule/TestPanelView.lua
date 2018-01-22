--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local TestPanelView = {}
local this = TestPanelView
--初始化方法
function TestPanelView:init()

	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.RawImage = self.transform:Find('RawImage')
	self.Button = self.transform:Find('Button')
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function TestPanelView:on_destroy()
	
	this.transform = nil

	print('TestPanelView:ondestroy!')
end
--视图状态
function TestPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> TestPanellview:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
	end	
	if viewState == 'clicked_state' then
		--DemoPanelview.image.gameObject:SetActive(true)
	end
end

return TestPanelView