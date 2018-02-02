local LobbyState = {}

local MsgDefine = {}

local Data = {}

require 'lua/modules/Lobby/LobbyEventConst'
local ZEntryModuleData = require 'lua/datamodules/ZEntryModuleData'
local ZLobbyModuleData = require 'lua/datamodules/ZLobbyModuleData'
local CommonModuleData = require 'lua/datamodules/CommonModuleData'
local ZShopModuleData = require 'lua/datamodules/ZShopModuleData'
local ZBankModuleData = require 'lua/datamodules/ZBankModuleData'
require 'lua/modules/Lobby/modules/ChangeHead/ChangeHeadPanelFunc'
require 'lua/modules/Lobby/modules/Rank/RankPanelFunc'
require 'lua/modules/Lobby/modules/Shop/ShopPanelFunc'
require 'lua/modules/Lobby/modules/Bag/BagPanelFunc'
require 'lua/modules/Lobby/modules/Task/TaskPanelFunc'
require 'lua/modules/Lobby/modules/SafeBox/SafeBoxPanelFunc'
require 'lua/config/Iteminfo'
require 'lua/modules/Lobby/modules/Task/TaskPanelService'
require 'lua/modules/Lobby/modules/Mail/MailGlobalEntry'
--导入子模块数据层

local tcpClinet
-- 初始化游戏全局状态对象
function LobbyState:init(gameState,curTcpClinet,gameData)
	--保持网络层引用
	tcpClinet = curTcpClinet
	--协议号
	LobbyState.MsgDefine = MsgDefine
	LobbyState.Data = Data
	Data.ZEntryModuleData = ZEntryModuleData
	Data.CommonModuleData = CommonModuleData
	Data.ZLobbyModuleData = ZLobbyModuleData
	Data.ZShopModuleData = ZShopModuleData
	Data.ZBankModuleData = ZBankModuleData
	--pb 注册
	CommonModuleData.register(tcpClinet,MsgDefine)
	ZEntryModuleData.register(tcpClinet,MsgDefine)
	ZLobbyModuleData.register(tcpClinet,MsgDefine)
	ZShopModuleData.register(tcpClinet,MsgDefine)
	ZBankModuleData.register(tcpClinet,MsgDefine)

end 
-- 清除化游戏全局状态对象
function LobbyState:clear()
	--清理消息注册
	ZEntryModuleData.clear(tcpClinet)
	ZLobbyModuleData.clear(tcpClinet)
	ZShopModuleData.clear(tcpClinet)
	ZBankModuleData.clear(tcpClinet)
	
	tcpClinet = nil
	MsgDefine = nil
	LobbyState.MsgDefine = nil

	--DemoModuleData = nil
	LobbyState.Data = nil
	--Data.DemoModuleData = nil
	
end 

function LobbyState:getstate_byname(name)
	return LobbyState[name]
end

function LobbyState:setstate_byname(name,value)
	LobbyState[name] =value
end

return LobbyState 