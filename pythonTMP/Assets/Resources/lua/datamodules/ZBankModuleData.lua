--TP_ModuleData.lua
-- 本模块数据层
-- ZBank
-- $MsgName$
-- ZProto
-- $ID$
--[[
$pbCode
]]
local ZBankModuleData = {}

local Cmd = {
	CS_EditBankPassword = sib(1201),
	CS_SaveIntoBank = sib(1202),
	CS_DrawFromBank = sib(1203),

	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function ZBankModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
    --protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    --protobuf.register_file(luaPath..'/proto/ZEnum.pb')
    protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZBank.pb'))
	
	local luaPath = PbPtah()
	--映射协议号 -> protobuf 
	MsgDefine.ZBankModuleCmd = Cmd
    local pbtable = tcpClinet.pb
	pbtable [Cmd.CS_EditBankPassword] = "ZProto.CS_EditBankPassword"
	pbtable [Cmd.CS_SaveIntoBank] = "ZProto.CS_SaveIntoBank"
	pbtable [Cmd.CS_DrawFromBank] = "ZProto.CS_DrawFromBank"

	--pbtable [Cmd.$MsgName$] = "ZProto.$MsgName$"
	--监听处理事件

	--tcpClinet.addlistener(Cmd.$MsgName$,ZBankModuleData.on_msg)
end

function ZBankModuleData.clear(tcpClinet)


 	--tcpClinet.removelistener(Cmd.$MsgName$,ZBankModuleData.on_msg)
end 	

function ZBankModuleData.send_CS_EditBankPassword(uid,code,password)
	local CS_EditBankPassword = {}
	CS_EditBankPassword.uid = uid
	CS_EditBankPassword.code = code
	CS_EditBankPassword.password = password
	GameState.tcpClinet.sendmsg(Cmd.CS_EditBankPassword,CS_EditBankPassword)
end
function ZBankModuleData.send_CS_SaveIntoBank(uid,gold)
	local CS_SaveIntoBank = {}
	CS_SaveIntoBank.uid = uid
	CS_SaveIntoBank.gold = gold
	GameState.tcpClinet.sendmsg(Cmd.CS_SaveIntoBank,CS_SaveIntoBank)
end
function ZBankModuleData.send_CS_DrawFromBank(uid,password,gold)
	local CS_DrawFromBank = {}
	CS_DrawFromBank.uid = uid
	CS_DrawFromBank.password = password
	CS_DrawFromBank.gold = gold
	GameState.tcpClinet.sendmsg(Cmd.CS_DrawFromBank,CS_DrawFromBank)
end


function ZBankModuleData.on_msg(key,decode)

	
--[[
	if key == Cmd.$MsgName$ then
		print("ZBank >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end 	

return 	ZBankModuleData