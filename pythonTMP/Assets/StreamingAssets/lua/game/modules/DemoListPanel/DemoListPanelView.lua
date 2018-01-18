DemoListPanelView = {}
local this = DemoListPanelView

--初始化方法
function DemoListPanelView:init(transform)
	
	DemoListPanelView.transform = transform

	self.button = self.transform:Find('Button')
	self.text = self.transform:Find('Text')
	this.image = this.transform:Find('Image')

	self:set_state('init_state')
end	

--销毁方法
function DemoListPanelView:on_destroy()
	
	this.transform = nil
	this.button = nil
	this.image = nil

	print('DemoListPanelView:ondestroy!')
end
--视图状态
function DemoListPanelView:set_state(viewState)

	print('>>>3 DemoListPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		this.image.gameObject:SetActive(false)
	end	
	if viewState == 'clicked_state' then
		this.image.gameObject:SetActive(true)
	end
end

return DemoListPanelView