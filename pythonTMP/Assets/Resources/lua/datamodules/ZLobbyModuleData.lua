--TP_ModuleData.lua
-- 本模块数据层
-- ZLobby
-- $MsgName$
-- ZProto
-- $ID$
--[[
$pbCode
]]
local ZLobbyModuleData = {}

local Cmd = {
	-- CS_GetUserData = sib(1010),
	-- SC_SetUserData = sib(11010),
	CS_GetUserState = sib(1011),
	SC_SetUserState = sib(11011),
	CS_ChangeUserFace = sib(1020),
	CS_ChangeUserNick = sib(1021),
	CS_GetRankList = sib(1030),
	SC_SetRankList = sib(11030),
	CS_GetMarquee = sib(1040),
	SC_SetMarquee = sib(11040),
	CS_GetActivity = sib(1050),
	SC_SetActivity = sib(11050),
	CS_GetGameList = sib(1060),
	SC_SetGameList = sib(11060),
	CS_GetEmail=sib(1041),
	SC_SetEmail=sib(11041),
	CS_SetEmailStatus=sib(1042),
	SC_SetEmailStatus=sib(11042),
	CS_GetGameList = sib(1060),
	SC_SetGameList = sib(11060),
	CS_GetTask = sib(1070),
	SC_SetTask = sib(11070),
	CS_ProcessTask = sib(1071),

	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function ZLobbyModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
    --protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    --protobuf.register_file(luaPath..'/proto/ZEnum.pb')
	
	local luaPath = PbPtah()
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'

   	protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZCommon.pb'))
   	protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZEnum.pb'))
    protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZModel.pb'))
    protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZLobby.pb'))
    protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZEntry.pb'))
    
	--protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    --protobuf.register_file(luaPath..'/proto/ZEnum.pb')
    --protobuf.register_file(luaPath..'/proto/ZModel.pb')
    --protobuf.register_file(luaPath..'/proto/ZLobby.pb')
    --protobuf.register_file(luaPath..'/proto/ZEntry.pb')
	
	--映射协议号 -> protobuf 
	MsgDefine.ZLobbyModuleCmd = Cmd
    local pbtable = tcpClinet.pb
	-- pbtable [Cmd.CS_GetUserData] = "ZProto.CS_GetUserData"
	-- pbtable [Cmd.SC_SetUserData] = "ZProto.SC_SetUserData"
	pbtable [Cmd.CS_GetUserState] = "ZProto.CS_GetUserState"
	pbtable [Cmd.SC_SetUserState] = "ZProto.SC_SetUserState"
	pbtable [Cmd.CS_ChangeUserFace] = "ZProto.CS_ChangeUserFace"
	pbtable [Cmd.CS_ChangeUserNick] = "ZProto.CS_ChangeUserNick"
	pbtable [Cmd.CS_GetRankList] = "ZProto.CS_GetRankList"
	pbtable [Cmd.SC_SetRankList] = "ZProto.SC_SetRankList"
	pbtable [Cmd.CS_GetMarquee] = "ZProto.CS_GetMarquee"
	pbtable [Cmd.SC_SetMarquee] = "ZProto.SC_SetMarquee"
	pbtable [Cmd.CS_GetActivity] = "ZProto.CS_GetActivity"
	pbtable [Cmd.SC_SetActivity] = "ZProto.SC_SetActivity"
	pbtable [Cmd.CS_GetGameList] = "ZProto.CS_GetGameList"
	pbtable [Cmd.SC_SetGameList] = "ZProto.SC_SetGameList"
	pbtable [Cmd.CS_GetEmail] = "ZProto.CS_GetEmail"
	pbtable [Cmd.SC_SetEmail] = "ZProto.SC_SetEmail"
	pbtable [Cmd.CS_SetEmailStatus] = "ZProto.CS_SetEmailStatus"
	pbtable [Cmd.SC_SetEmailStatus] = "ZProto.SC_SetEmailStatus"
	pbtable [Cmd.CS_GetTask] = "ZProto.CS_GetTask"
	pbtable [Cmd.SC_SetTask] = "ZProto.SC_SetTask"
	pbtable [Cmd.CS_ProcessTask] = "ZProto.CS_ProcessTask"

	--pbtable [Cmd.$MsgName$] = "ZProto.$MsgName$"
	--监听处理事件
	-- tcpClinet.addlistener(Cmd.SC_SetUserData,ZLobbyModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetUserState,ZLobbyModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetRankList,ZLobbyModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetMarquee,ZLobbyModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetActivity,ZLobbyModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetGameList,ZLobbyModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetTask,ZLobbyModuleData.on_msg)
--	tcpClinet.addlistener(Cmd.SC_SetEmail,ZLobbyModuleData.on_msg)
--	tcpClinet.addlistener(Cmd.SC_SetEmailStatus,ZLobbyModuleData.on_msg)

	MailPanelServices= require 'lua/modules/Lobby/modules/Mail/MailPanelServices';
	MailPanelServices.Init(MsgDefine.ZLobbyModuleCmd,ZLobbyModuleData);

	-- UserInfoPanelService = require 'lua/modules/Lobby/modules/UserInfo/UserInfoPanelService'
	-- UserInfoPanelService.Init(MsgDefine.ZEntryModuleCmd,CommonData);
	--tcpClinet.addlistener(Cmd.$MsgName$,ZLobbyModuleData.on_msg)
end

function ZLobbyModuleData.clear(tcpClinet)

	-- tcpClinet.removelistener(Cmd.SC_SetUserData,ZLobbyModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetUserState,ZLobbyModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetRankList,ZLobbyModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetMarquee,ZLobbyModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetActivity,ZLobbyModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetGameList,ZLobbyModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetTask,ZLobbyModuleData.on_msg)
--	tcpClinet.removelistener(Cmd.SC_SetEmail,ZLobbyModuleData.on_msg)
--	tcpClinet.removelistener(Cmd.SC_SetEmailStatus,ZLobbyModuleData.on_msg)
 	--tcpClinet.removelistener(Cmd.$MsgName$,ZLobbyModuleData.on_msg)
	MailPanelServices.Clear();
end 	

-- function ZLobbyModuleData.send_CS_GetUserData(uid)
	-- local CS_GetUserData = {}
	-- CS_GetUserData.uid = uid
	-- GameState.tcpClinet.sendmsg(Cmd.CS_GetUserData,CS_GetUserData)
-- end
function ZLobbyModuleData.send_CS_GetUserState(uid)
	local CS_GetUserState = {}
	CS_GetUserState.uid = uid
	GameState.tcpClinet.sendmsg(Cmd.CS_GetUserState,CS_GetUserState)
end
function ZLobbyModuleData.send_CS_ChangeUserFace(uid,face)
	local CS_ChangeUserFace = {}
	CS_ChangeUserFace.uid = uid
	CS_ChangeUserFace.face = face
	GameState.tcpClinet.sendmsg(Cmd.CS_ChangeUserFace,CS_ChangeUserFace)
end
function ZLobbyModuleData.send_CS_ChangeUserNick(uid,nick_name)
	local CS_ChangeUserNick = {}
	CS_ChangeUserNick.uid = uid
	CS_ChangeUserNick.nick_name = nick_name
	GameState.tcpClinet.sendmsg(Cmd.CS_ChangeUserNick,CS_ChangeUserNick)
end
function ZLobbyModuleData.send_CS_GetRankList(uid,type1)
	local CS_GetRankList = {}
	CS_GetRankList.uid = uid
	CS_GetRankList.type = type1
	GameState.tcpClinet.sendmsg(Cmd.CS_GetRankList,CS_GetRankList)
end
function ZLobbyModuleData.send_CS_GetMarquee()
	local CS_GetMarquee = {}
	GameState.tcpClinet.sendmsg(Cmd.CS_GetMarquee,CS_GetMarquee)
end
function ZLobbyModuleData.send_CS_GetEmail()
	local CS_GetEmail = {}
	GameState.tcpClinet.sendmsg(Cmd.CS_GetEmail,CS_GetEmail)
end
function ZLobbyModuleData.send_CS_SetEmailStatus(ids,status)
	local CS_SetEmailStatus = {}
	CS_SetEmailStatus.ids = ids
	CS_SetEmailStatus.status = status
	GameState.tcpClinet.sendmsg(Cmd.CS_SetEmailStatus,CS_SetEmailStatus)
end
function ZLobbyModuleData.send_CS_GetActivity()
	local CS_GetActivity = {}
	GameState.tcpClinet.sendmsg(Cmd.CS_GetActivity,CS_GetActivity)
end
function ZLobbyModuleData.send_CS_GetGameList()
	local CS_GetGameList = {}
	GameState.tcpClinet.sendmsg(Cmd.CS_GetGameList,CS_GetGameList)
end
function ZLobbyModuleData.send_CS_GetTask()
	local CS_GetTask = {}
	GameState.tcpClinet.sendmsg(Cmd.CS_GetTask,CS_GetTask)
end
function ZLobbyModuleData.send_CS_ProcessTask(op,id)
	local CS_ProcessTask = {}
	CS_ProcessTask.op = op
	CS_ProcessTask.id = id
	GameState.tcpClinet.sendmsg(Cmd.CS_ProcessTask,CS_ProcessTask)
end


function ZLobbyModuleData.on_msg(key,decode)

	-- if key == Cmd.SC_SetUserData then
		-- print("ZLobby >> on_msg >> user_para >>  ".. Cmd.SC_SetUserData)

		-- ZLobbyModuleData.SC_SetUserData = decode
		-- print("SC_SetUserData.user ".. decode.user)
		-- print("SC_SetUserData.user_info ".. decode.user_info)
	-- end
	if key == Cmd.SC_SetUserState then
		print("ZLobby >> on_msg >> user_para >>  ".. Cmd.SC_SetUserState)

		ZLobbyModuleData.SC_SetUserState = decode
		print("SC_SetUserState.msg ".. decode.msg)
		print("SC_SetUserState.task ".. decode.task)
		print("SC_SetUserState.activiy ".. decode.activiy)
	end
	if key == Cmd.SC_SetRankList then
		print("ZLobby >> on_msg >> user_para >>  ".. Cmd.SC_SetRankList)

		ZLobbyModuleData.SC_SetRankList = decode
		print("SC_SetRankList.mine ".. decode.mine)
		print("SC_SetRankList.type ".. decode.type)
		--print("SC_SetRankList.datas ".. decode.datas)
	end
	if key == Cmd.SC_SetMarquee then
		print("ZLobby >> on_msg >> user_para >>  ".. Cmd.SC_SetMarquee)

		ZLobbyModuleData.SC_SetMarquee = decode
		for k,v in pairs(ZLobbyModuleData.SC_SetMarquee.msgs) do
--			print("SC_SetMarquee.msgs ".. v)
		end 	
		--print("SC_SetMarquee.msgs ".. decode.msgs)
	end
	if key == Cmd.SC_SetActivity then
		print("ZLobby >> on_msg >> user_para >>  ".. Cmd.SC_SetActivity)

		ZLobbyModuleData.SC_SetActivity = decode
		print("SC_SetActivity.urls ".. decode.urls)
	end
	if key == Cmd.SC_SetGameList then
		print("ZLobby >> on_msg >> user_para >>  ".. Cmd.SC_SetGameList)

		ZLobbyModuleData.SC_SetGameList = decode
		--print("SC_SetGameList.games ".. decode.games)
	end
	
	if key == Cmd.SC_SetTask then
		print("ZLobby >> on_msg >> user_para >>  ".. Cmd.SC_SetTask)
		if ZLobbyModuleData.SC_SetTask==nil then ZLobbyModuleData.SC_SetTask = {} end
		--1 更新 2 插入
		for _,v in pairs(decode.tasks) do
			print('task middddddddddddddddddd '..v.mid)
			local cfg = TaskPanelService.getCfgByMid(v.mid)
			if ZLobbyModuleData.SC_SetTask[cfg.typek] == nil then ZLobbyModuleData.SC_SetTask[cfg.typek] = {} end
			-- ZLobbyModuleData.SC_SetTask[cfg.typek][v.mid] = v
			local data1 = ZLobbyModuleData.SC_SetTask[cfg.typek][v.mid]
			if v.discard == true then
				ZLobbyModuleData.delete_task(v)
			elseif data1 ~= nil then 
				ZLobbyModuleData.SC_SetTask[cfg.typek][v.mid] = updateTableValue(data1,v)
			else
				ZLobbyModuleData.SC_SetTask[cfg.typek][v.mid] = v
			end
		end
		es(LobbyEventConst.Task_Update,nil)
		--print("SC_SetTask.tasks ".. decode.tasks)
	end
	
--[[
	if key == Cmd.$MsgName$ then
		print("ZLobby >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end
ZLobbyModuleData.delete_task=function (data)
	local cfg = TaskPanelService.getCfgByMid(v.mid)
	local deltable = ZLobbyModuleData.SC_SetTask[cfg.typek]
	local newtb = {}
	for _,v in pairs(deltable) do
		if v.id ~= data.id then
			newtb[v.mid] = data
		end
	end
	ZLobbyModuleData.SC_SetTask[cfg.typek] = newtb
end

return 	ZLobbyModuleData