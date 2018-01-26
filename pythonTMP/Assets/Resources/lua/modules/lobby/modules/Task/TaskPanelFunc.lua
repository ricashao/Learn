local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'

function task_tabselect(index,transform,data)
	es(LobbyEventConst.Task_TabChange,index)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(true)
	select1.gameObject:SetActive(false)
end

function task_tabunselect(index,transform,data)
	local select1 = transform:Find('Select1')
	local select2 = transform:Find('Select2')
	select2.gameObject:SetActive(false)
	select1.gameObject:SetActive(true)
end

function task_daily_setdata(index,transform,data)
	local cfg = TaskPanelService.getCfgByMid(data.mid)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local tasktext = transform:Find('TaskText'):GetComponent('Text')
	local hadComplete = transform:Find('HadComplete')
	local notComplete = transform:Find('NotComplete')
	local isTake = transform:Find('IsTake')
	local rewardBtn = transform:Find('RewardButton')
	local gotoBtn = transform:Find('GotoButton')
	tasktext.text = cfg.name
	local rewards = split(cfg.rewards,',')
	counttext.text = 'X'..rewards[3]
	if data.status == 'TS_DONE' then
		isTake.gameObject:SetActive(false)
		gotoBtn.gameObject:SetActive(false)
		rewardBtn.gameObject:SetActive(true)
	elseif data.status == 'TS_REWARD' then
		isTake.gameObject:SetActive(true)
		gotoBtn.gameObject:SetActive(false)
		rewardBtn.gameObject:SetActive(false)
	else
		isTake.gameObject:SetActive(false)
		gotoBtn.gameObject:SetActive(true)
		rewardBtn.gameObject:SetActive(false)
	end
end

function task_daily_awake(index,transform,data)
	local gotobutton = transform:Find('GotoButton'):GetComponent('Button')
	local rewardbutton = transform:Find('RewardButton'):GetComponent('Button')
	gotobutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		print('select>>>>>>>>> '..selectitem.itemIndex)
	end)
	rewardbutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		local data = selectitem:GetData()
		ZLobbyModuleData.send_CS_ProcessTask('PT_REWARD',data.id)
	end)
end

function task_daily_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function task_daily_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end

--下面是成就任务 之前面板说是备用所以名字变成这样了
function task_backup_setdata(index,transform,data)
	local status = TaskPanelService.getCurTaskStatus(data.mid,data.typek)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local tasktext = transform:Find('TaskText'):GetComponent('Text')
	local hadComplete = transform:Find('HadComplete')
	local notComplete = transform:Find('NotComplete')
	local isTake = transform:Find('IsTake')
	local rewardBtn = transform:Find('RewardButton')
	local gotoBtn = transform:Find('GotoButton')
	local rewards = split(data.rewards,',')
	tasktext.text = data.name
	counttext.text = 'X'..rewards[3]
	if status == 'TS_DONE' then
		isTake.gameObject:SetActive(false)
		gotoBtn.gameObject:SetActive(false)
		rewardBtn.gameObject:SetActive(true)
	elseif status == 'TS_REWARD' then
		isTake.gameObject:SetActive(true)
		gotoBtn.gameObject:SetActive(false)
		rewardBtn.gameObject:SetActive(false)
	else
		isTake.gameObject:SetActive(false)
		gotoBtn.gameObject:SetActive(true)
		rewardBtn.gameObject:SetActive(false)
	end
end

function task_backup_awake(index,transform,data)
	local gotobutton = transform:Find('GotoButton'):GetComponent('Button')
	local rewardbutton = transform:Find('RewardButton'):GetComponent('Button')
	gotobutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		print('select>>>>>>>>> '..selectitem.itemIndex)
	end)
	rewardbutton.onClick:AddListener(function()
		local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
		local data = selectitem:GetData()
		ZLobbyModuleData.send_CS_ProcessTask('PT_REWARD',data.id)
	end)
end

function task_backup_onselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(true)
end

function task_backup_unselect(index,transform,data)
	local selectgo = transform:Find('Select').gameObject
	selectgo:SetActive(false)
end
