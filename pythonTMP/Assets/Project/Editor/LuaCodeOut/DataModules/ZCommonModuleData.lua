--TP_ModuleData.lua
-- 本模块数据层
-- ZCommon
-- $MsgName$
-- ZProto
-- $ID$
--[[
$pbCode
]]
local ZCommonModuleData = {}

local Cmd = {
	MessageNotify = sib(10000),
	CS_GetUserData = sib(1010),
	SC_SetUserData = sib(11010),
	SC_SetMoney = sib(11012),

	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function ZCommonModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
    --protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    --protobuf.register_file(luaPath..'/proto/ZEnum.pb')
    protobuf.register_file(PbPtah()..'/proto/ZCommon.pb')
	
	local luaPath = PbPtah()
	--映射协议号 -> protobuf 
	MsgDefine.ZCommonModuleCmd = Cmd
    local pbtable = tcpClinet.pb
	pbtable [Cmd.MessageNotify] = "ZProto.MessageNotify"
	pbtable [Cmd.CS_GetUserData] = "ZProto.CS_GetUserData"
	pbtable [Cmd.SC_SetUserData] = "ZProto.SC_SetUserData"
	pbtable [Cmd.SC_SetMoney] = "ZProto.SC_SetMoney"

	--pbtable [Cmd.$MsgName$] = "ZProto.$MsgName$"
	--监听处理事件
	tcpClinet.addlistener(Cmd.SC_SetUserData,ZCommonModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetMoney,ZCommonModuleData.on_msg)

	--tcpClinet.addlistener(Cmd.$MsgName$,ZCommonModuleData.on_msg)
end

function ZCommonModuleData.clear(tcpClinet)

	tcpClinet.removelistener(Cmd.SC_SetUserData,ZCommonModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetMoney,ZCommonModuleData.on_msg)

 	--tcpClinet.removelistener(Cmd.$MsgName$,ZCommonModuleData.on_msg)
end 	

function ZCommonModuleData.send_CS_GetUserData(uid)
	local CS_GetUserData = {}
	CS_GetUserData.uid = uid
	GameState.tcpClinet.sendmsg(Cmd.CS_GetUserData,CS_GetUserData)
end


function ZCommonModuleData.on_msg(key,decode)

	if key == Cmd.SC_SetUserData then
		print("ZCommon >> on_msg >> user_para >>  ".. Cmd.SC_SetUserData)

		ZCommonModuleData.SC_SetUserData = decode
		print("SC_SetUserData.user ".. decode.user)
		print("SC_SetUserData.user_info ".. decode.user_info)
	end
	if key == Cmd.SC_SetMoney then
		print("ZCommon >> on_msg >> user_para >>  ".. Cmd.SC_SetMoney)

		ZCommonModuleData.SC_SetMoney = decode
		print("SC_SetMoney.type ".. decode.type)
		print("SC_SetMoney.value ".. decode.value)
	end
	
--[[
	if key == Cmd.$MsgName$ then
		print("ZCommon >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end 	

return 	ZCommonModuleData