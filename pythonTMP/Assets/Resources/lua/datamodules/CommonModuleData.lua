--TP_ModuleData.lua
-- 本模块数据层
-- Common
-- $MsgName$
-- $Package$
-- $ID$
--[[
$pbCode
]]
local CommonModuleData = {}

local Cmd = {
	MessageNotify =  sib(10000),
	SC_SetMoney = sib(11012),
	CS_GetUserData = sib(1010),
	SC_SetUserData = sib(11010),
	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function CommonModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
	local luaPath = PbPtah()
	--protobuf.register_file(luaPath..'/proto/ZModel.pb')
	--protobuf.register_file(luaPath..'/proto/ZEnum.pb')
	--protobuf.register_file(luaPath..'/proto/ZCommon.pb')
	protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZEnum.pb'))
	protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZModel.pb'))	
	protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZCommon.pb'))

    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
    --protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    --protobuf.register_file(luaPath..'/proto/ZEnum.pb')
	
	local luaPath = PbPtah()
	--映射协议号 -> protobuf 
	MsgDefine.CommonModuleCmd = Cmd
    local pbtable = tcpClinet.pb
	pbtable [Cmd.MessageNotify] = "ZProto.MessageNotify"
	pbtable [Cmd.SC_SetMoney] = "ZProto.SC_SetMoney"
	pbtable [Cmd.CS_GetUserData] = "ZProto.CS_GetUserData"
	pbtable [Cmd.SC_SetUserData] = "ZProto.SC_SetUserData"
	--pbtable [Cmd.$MsgName$] = "$Package$.$MsgName$"
	--监听处理事件
	--tcpClinet.addlistener(Cmd.$MsgName$,CommonModuleData.on_msg)
	tcpClinet.addlistener(Cmd.MessageNotify,CommonModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetMoney,CommonModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetUserData,CommonModuleData.on_msg)
end

function CommonModuleData.clear(tcpClinet)
 	tcpClinet.removelistener(Cmd.MessageNotify,CommonModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetUserData,ZCommonModuleData.on_msg)
end 	


function CommonModuleData.on_msg(key,decode)
	if key == Cmd.MessageNotify then 

		print("MessageNotify.type ".. decode.type);
		print("MessageNotify.msg ".. decode.msg);
		
	end
	
	if key == Cmd.SC_SetMoney then
		if decode.type == 'GOLD' then
			CommonData.user.gold = decode.value
		elseif decode.type == 'GEM' then
			CommonData.user_info.gem = decode.value
		elseif decode.type == 'TICKET' then
			CommonData.user_info.ticket = decode.value
		end
		es(CommonEventConst.Money_Update);
	end	
	if key == Cmd.SC_SetUserData then
		print("ZCommon >> on_msg >> user_para >>  ".. Cmd.SC_SetUserData)

		ZCommonModuleData.SC_SetUserData = decode
		print("SC_SetUserData.user ".. decode.user)
		print("SC_SetUserData.user_info ".. decode.user_info)
	end
--[[
	if key == Cmd.$MsgName$ then
		print("Common >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end 	

return 	CommonModuleData