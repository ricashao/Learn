--local ZLobbyModuleData = require 'lua/datamodules/ZShopModuleData'

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
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local tasktext = transform:Find('TaskText'):GetComponent('Text')
	counttext.text = data[1]
	tasktext.text = data[2]
	local hadComplete = transform:Find('HadComplete')
	local notComplete = transform:Find('NotComplete')
	if data[3] == 0 or data[3] == nil then
		hadComplete.gameObject:SetActive(false)
		notComplete.gameObject:SetActive(true)
	else
		hadComplete.gameObject:SetActive(true)
		notComplete.gameObject:SetActive(false)
	end
end

function task_daily_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local gotobutton = transform:Find('GotoButton'):GetComponent('Button')
	gotobutton.onClick:AddListener(function()
		print('select>>>>>>>>> '..selectitem.itemIndex)
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

function task_backup_setdata(index,transform,data)
	local counttext = transform:Find('CountText'):GetComponent('Text')
	local tasktext = transform:Find('TaskText'):GetComponent('Text')
	counttext.text = data[1]
	tasktext.text = data[2]
	local hadComplete = transform:Find('HadComplete')
	local notComplete = transform:Find('NotComplete')
	if data[3] == 0 or data[3] == nil then
		hadComplete.gameObject:SetActive(false)
		notComplete.gameObject:SetActive(true)
	else
		hadComplete.gameObject:SetActive(true)
		notComplete.gameObject:SetActive(false)
	end
end

function task_backup_awake(index,transform,data)
	local selectitem = transform:GetComponent('LuaSelectLoopNewItem')
	local gotobutton = transform:Find('GotoButton'):GetComponent('Button')
	gotobutton.onClick:AddListener(function()
		print('select>>>>>>>>> '..selectitem.itemIndex)
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
