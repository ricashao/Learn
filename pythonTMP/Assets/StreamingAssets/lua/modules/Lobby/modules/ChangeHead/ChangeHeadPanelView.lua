--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local ChangeHeadPanelView = {}
local this = ChangeHeadPanelView
--初始化方法
function ChangeHeadPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.backbutton = self.transform:Find('BackButton')
	self.headlist = self.transform:Find('HeadList/Viewport/Content')
	-- self.codeinput = self.transform:Find('CodeInputField'):GetComponent("InputField")
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function ChangeHeadPanelView:on_destroy()
	
	this.transform = nil

	print('ChangeHeadPanelView:ondestroy!')
end
--视图状态
function ChangeHeadPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> ChangeHeadPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end	
	if viewState == 'clicked_state' then
		--DemoPanelview.image.gameObject:SetActive(true)
	end
end

return ChangeHeadPanelView