require 'lua/config/Taskinfo'
TaskPanelService = {}
TaskPanelService.init = function()
	TaskPanelService.dataByType = {}
	TaskPanelService.dataById = {}
	for _,v in pairs(Taskinfo) do 
		if TaskPanelService.dataByType[v.typek] == nil then TaskPanelService.dataByType[v.typek] = {} end
		TaskPanelService.dataByType[v.typek][v.mid] = v
		TaskPanelService.dataById[v.mid] = v
	end
	Taskinfo = nil
end


TaskPanelService.getCfgByMid = function (mid)
	return TaskPanelService.dataById[mid]
end

TaskPanelService.getDataInMDByMid = function (mid,typek)
	return GameState.curRunState.Data.ZLobbyModuleData.SC_SetTask[typek][mid]
end

TaskPanelService.getCfgsByType = function (typek)
	local data = TaskPanelService.dataByType[typek]
	local tb = {}
	for _,v in pairs(data) do
		table.insert(tb,v)
	end
	return tb
end

TaskPanelService.getCurDailyTask = function()
	--Module层处理了任务的删除所以SC_SetTask里面的任务都是可以暂时的
	local data = GameState.curRunState.Data.ZLobbyModuleData.SC_SetTask[2]
	if data == nil then
		return {}
	else
		local tb = {}
		for _,v in pairs(data) do
			table.insert(tb,v)
		end
		local sortFunc = function (a,b)
			local cfga =TaskPanelService.getCfgByMid(a.mid)
			local cfgb =TaskPanelService.getCfgByMid(b.mid)
			if cfga.sort<cfgb.sort then
				return true
			else
				return false
			end
		end
		table.sort(tb,sortFunc)
		return tb
	end
end

--给成就任务用的
TaskPanelService.getCurTaskStatus = function(mid,typek)
	local datas = GameState.curRunState.Data.ZLobbyModuleData.SC_SetTask[typek]
	if datas == nil then
		return 'TS_RECEIVE'
	end
	for k,v in pairs(datas) do
		if k == mid then
			return v.status
		end
	end
	return 'TS_RECEIVE'
end




TaskPanelService.init()

return TaskPanelService