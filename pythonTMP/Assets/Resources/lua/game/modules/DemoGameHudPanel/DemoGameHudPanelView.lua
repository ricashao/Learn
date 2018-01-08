DemoGameHudPanelView = {}
local this = DemoGameHudPanelView

--初始化方法
function DemoGameHudPanelView:init(transform)
	
	DemoGameHudPanelView.transform = transform

	self.returnButton = self.transform:Find('ReturnButton')
	self.button = self.transform:Find('Button')
	self.text = self.transform:Find('Text')
	this.image = this.transform:Find('Image')

	self:set_state('init_state')
end	

--销毁方法
function DemoGameHudPanelView:on_destroy()
	
	this.transform = nil
	this.button = nil
	this.image = nil
	this.returnButton = nil

	print('DemoGameHudPanelView:ondestroy!')
end
--视图状态
function DemoGameHudPanelView:set_state(viewState)

	print('>>>3 DemoGameHudPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		this.image.gameObject:SetActive(false)
	end	
	if viewState == 'clicked_state' then
		this.image.gameObject:SetActive(true)
	end
end

return DemoGameHudPanelView