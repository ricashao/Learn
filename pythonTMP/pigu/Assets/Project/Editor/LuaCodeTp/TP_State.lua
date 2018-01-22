local $ModuleMame$State = {}

local MsgDefine = {}

local Data = {}

--local DemoModuleData = require 'lua/modules/demo/models/DemoModuleData'

--导入子模块数据层

local tcpClinet
-- 初始化游戏全局状态对象
function $ModuleMame$State:init(gameState,curTcpClinet,gameData)
	--保持网络层引用
	tcpClinet = curTcpClinet
	--协议号
	$ModuleMame$State.MsgDefine = MsgDefine
	$ModuleMame$State.Data = Data
	--Data.DemoModuleData = DemoModuleData
	--pb 注册
	--DemoModuleData.register(tcpClinet,MsgDefine)
end 
-- 清除化游戏全局状态对象
function $ModuleMame$State:clear()
	--清理消息注册
	--DemoModuleData.clear(tcpClinet)
	
	tcpClinet = nil
	MsgDefine = nil
	$ModuleMame$State.MsgDefine = nil

	--DemoModuleData = nil
	$ModuleMame$State.Data = nil
	--Data.DemoModuleData = nil
	
end 

return $ModuleMame$State 