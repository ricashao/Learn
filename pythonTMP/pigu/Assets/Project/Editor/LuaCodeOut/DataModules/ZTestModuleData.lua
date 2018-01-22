--TP_ModuleData.lua
-- 本模块数据层
-- ZTest
-- $MsgName$
-- ZProto
-- $ID$
--[[
$pbCode
]]
local ZTestModuleData = {}

local Cmd = {
	CS_Test = sib(1000),
	SC_Test = sib(10010),

	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function ZTestModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
	local luaPath = PbPtah()
	--映射协议号 -> protobuf 
	MsgDefine.ZTestModuleCmd = Cmd
    local pbtable = tcpClinet.pb
	pbtable [Cmd.CS_Test] = "ZProto.CS_Test"
	pbtable [Cmd.SC_Test] = "ZProto.SC_Test"

	--pbtable [Cmd.$MsgName$] = "ZProto.$MsgName$"
	--监听处理事件
	tcpClinet.addlistener(Cmd.SC_Test,ZTestModuleData.on_msg)

	--tcpClinet.addlistener(Cmd.$MsgName$,ZTestModuleData.on_msg)
end

function ZTestModuleData.clear(tcpClinet)

	tcpClinet.removelistener(Cmd.SC_Test,ZTestModuleData.on_msg)

 	--tcpClinet.removelistener(Cmd.$MsgName$,ZTestModuleData.on_msg)
end 	

function ZTestModuleData.send_CS_Test(s_in)
	local CS_Test = {}
	CS_Test.s_in = s_in
	GameState.tcpClinet.sendmsg(Cmd.CS_Test,CS_Test)
end


function ZTestModuleData.on_msg(key,decode)

	if key == Cmd.SC_Test then
		print("ZTest >> on_msg >> user_para >>  ".. Cmd.SC_Test)

		ZTestModuleData.SC_Test = decode
		print("SC_Test.s_out ".. decode.s_out)
	end
	
--[[
	if key == Cmd.$MsgName$ then
		print("ZTest >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end 		