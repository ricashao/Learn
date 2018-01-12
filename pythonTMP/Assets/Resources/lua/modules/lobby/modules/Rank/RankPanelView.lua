--[[
视图层模板
1.保持视图子对象引用
2.管理视图状态
]]--
local RankPanelView = {}
local this = RankPanelView
--初始化方法
function RankPanelView:init(transform)
	
	this.transform = transform
	--self.button = self.transform:Find('Button')
	--self.text = self.transform:Find('Text')
	--this.image = DemoPanelview.transform:Find('Image')

	self.backbutton = self.transform:Find('BackButton')
	self.headimage1 = self.transform:Find('HeadImage1'):GetComponent("Image")
	self.headimage2 = self.transform:Find('HeadImage2'):GetComponent("Image")
	self.headimage3 = self.transform:Find('HeadImage3'):GetComponent("Image")
	self.goldimage1 = self.transform:Find('GoldImage1'):GetComponent("Image")
	self.goldimage2 = self.transform:Find('GoldImage2'):GetComponent("Image")
	self.goldimage3 = self.transform:Find('GoldImage3'):GetComponent("Image")
	self.myranktext = self.transform:Find('MyRankText'):GetComponent("Text")
	self.ranknametext1 = self.transform:Find('RankNameText1'):GetComponent("Text")
	self.ranknametext2 = self.transform:Find('RankNameText2'):GetComponent("Text")
	self.ranknametext3 = self.transform:Find('RankNameText3'):GetComponent("Text")
	self.rankgoldtext1 = self.transform:Find('RankGoldText1'):GetComponent("Text")
	self.rankgoldtext2 = self.transform:Find('RankGoldText2'):GetComponent("Text")
	self.rankgoldtext3 = self.transform:Find('RankGoldText3'):GetComponent("Text")
	self.tabgroup = self.transform:Find('TabGroup'):GetComponent("SelectGroup")
	self.gemlistgo = self.transform:Find('GemList')
	self.goldlistgo = self.transform:Find('GoldList')
	self.ticketlistgo = self.transform:Find('TicketList')
	self.gemlist = self.gemlistgo.transform:Find('Viewport/Content'):GetComponent("UILoopList");
	self.goldlist = self.goldlistgo.transform:Find('Viewport/Content'):GetComponent("UILoopList");
	self.ticketlist = self.ticketlistgo.transform:Find('Viewport/Content'):GetComponent("UILoopList");
	--AddRefCode 追加引用标志

	self:set_state('init_state')
end	

--销毁方法
function RankPanelView:on_destroy()
	
	this.transform = nil

	print('RankPanelView:ondestroy!')
end
--视图状态
function RankPanelView:set_state(viewState)
	
	self.viewState = viewState

	print('>>> RankPanelView:setstate'..viewState)
	-- body
	if viewState == 'init_state' then
		--DemoPanelview.image.gameObject:SetActive(false)
		this.transform.gameObject:SetActive(true)
	end	
	if viewState == 'gem_state' then
		self.gemlistgo.gameObject:SetActive(true)
		self.goldlistgo.gameObject:SetActive(false)
		self.ticketlistgo.gameObject:SetActive(false)
	end
	if viewState == 'gold_state' then
		self.gemlistgo.gameObject:SetActive(false)
		self.goldlistgo.gameObject:SetActive(true)
		self.ticketlistgo.gameObject:SetActive(false)
	end
	if viewState == 'ticket_state' then
		self.gemlistgo.gameObject:SetActive(false)
		self.goldlistgo.gameObject:SetActive(false)
		self.ticketlistgo.gameObject:SetActive(true)
	end
end

return RankPanelView