--TP_ModuleData.lua
-- 本模块数据层
-- $ModuleName$
-- $MsgName$
-- $Package$
-- $ID$
--[[
$pbCode
]]
local $ModuleName$ModuleData = {}

local Cmd = {
$CmdKeyList$
	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function $ModuleName$ModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
    --protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    --protobuf.register_file(luaPath..'/proto/ZEnum.pb')
	
	local luaPath = PbPtah()
	--映射协议号 -> protobuf 
	MsgDefine.$ModuleName$ModuleCmd = Cmd
    local pbtable = tcpClinet.pb
$CmdMappingList$
	--pbtable [Cmd.$MsgName$] = "$Package$.$MsgName$"
	--监听处理事件
$CmdAddlistenerList$
	--tcpClinet.addlistener(Cmd.$MsgName$,$ModuleName$ModuleData.on_msg)
end

function $ModuleName$ModuleData.clear(tcpClinet)

$CmdRemlistenerList$
 	--tcpClinet.removelistener(Cmd.$MsgName$,$ModuleName$ModuleData.on_msg)
end 	

$CmdSendFunList$

function $ModuleName$ModuleData.on_msg(key,decode)

$CmdReceiveList$	
--[[
	if key == Cmd.$MsgName$ then
		print("$ModuleName$ >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end 	

return 	$ModuleName$ModuleData