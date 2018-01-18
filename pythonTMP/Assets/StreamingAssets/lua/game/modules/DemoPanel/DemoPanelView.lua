--[[
视图层模板
1.保持视图子对象引用
2.
]]--
local DemoPanelview = {}
local this = DemoPanelview
--初始化方法
function DemoPanelview:init()
	--DemoPanelview.transform = transform
	self.button = self.transform:Find('Button')
	self.text = self.transform:Find('Text')
	this.image = DemoPanelview.transform:Find('Image')

	print('>>>3'..DemoPanelview.button:ToString())
	print('>>>4'..DemoPanelview.text:ToString())
	print('>>>5'..DemoPanelview.image:ToString())
	print('>>>6'..DemoPanelview.transform.gameObject:ToString())

	self:set_state('init_state')
end	
--（静态）fun
function DemoPanelview.fun(param)
	print('param >> '..param)
end	
--销毁方法
function DemoPanelview:on_destroy()
	
	this.transform = nil
	this.button = nil

	print('DemoPanelview:ondestroy!')
end
--视图状态
function DemoPanelview:set_state(viewState)
	print('>>>3 DemoPanelview:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		DemoPanelview.image.gameObject:SetActive(false)
	end	
	if viewState == 'clicked_state' then
		DemoPanelview.image.gameObject:SetActive(true)
	end
end

return DemoPanelview