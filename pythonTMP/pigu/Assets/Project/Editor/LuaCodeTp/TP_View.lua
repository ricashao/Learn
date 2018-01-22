--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local $PanelName$View = {}
local this = $PanelName$View
--初始化方法
function $PanelName$View:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self:set_state('init_state')
end	

--销毁方法
function $PanelName$View:on_destroy()
	
	this.transform = nil

	print('$PanelName$View:ondestroy!')
end
--视图状态
function $PanelName$View:set_state(viewState)
	
	self.viewState = viewState

	print('>>> $PanelName$lview:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
	end	
	if viewState == 'clicked_state' then
		--DemoPanelview.image.gameObject:SetActive(true)
	end
end

return $PanelName$View