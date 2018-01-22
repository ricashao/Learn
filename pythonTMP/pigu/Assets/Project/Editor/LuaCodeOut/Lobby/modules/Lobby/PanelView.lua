--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local PanelView = {}
local this = PanelView
--初始化方法
function PanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.Button1112 = self.transform:Find('Button')
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function PanelView:on_destroy()
	
	this.transform = nil

	print('PanelView:ondestroy!')
end
--视图状态
function PanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> Panellview:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
	end	
	if viewState == 'clicked_state' then
		--DemoPanelview.image.gameObject:SetActive(true)
	end
end

return PanelView