local DemoState = {}

local MsgDefine = {}

local Data = {}

local DemoModuleData = require 'lua/modules/demo/models/DemoModuleData'

local tcpClinet
-- 初始化游戏全局状态对象
function DemoState:init(gameState,curTcpClinet,gameData)
	--保持网络层引用
	tcpClinet = curTcpClinet
	--协议号
	DemoState.MsgDefine = MsgDefine
	DemoState.Data = Data
	Data.DemoModuleData = DemoModuleData
	--pb 注册
	DemoModuleData.register(tcpClinet,MsgDefine)
end 
-- 清除化游戏全局状态对象
function DemoState:clear()
	--清理消息注册
	DemoModuleData.clear(tcpClinet)
	
	tcpClinet = nil
	MsgDefine = nil
	DemoState.MsgDefine = nil

	DemoModuleData = nil
	DemoState.Data = nil
	Data.DemoModuleData = nil
	
end 

function DemoState:loading()

end 

function DemoState:run()
	
end 

return DemoState