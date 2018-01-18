--本模块数据层
local DemoModuleData = {}
--本模块消息号
local Cmd = {
	user_para = string.char(1, 2)
}

function DemoModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto

    local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'

	protobuf.register_file(luaPath..'/lua/modules/demo/proto/UserInfo.pb.bytes');
	protobuf.register_file(luaPath..'/lua/modules/demo/proto/User.pb.bytes');
    protobuf.register_file(luaPath..'/lua/modules/demo/proto/addressbook.pb.bytes');
	
	--映射协议号 -> protobuf 
    local pbtable = tcpClinet.pb
    pbtable [Cmd.user_para] = "tutorial.Person"

    --监听处理事件
    MsgDefine.DemoModuleCmd = Cmd

	tcpClinet.addlistener(Cmd.user_para,DemoModuleData.on_msg)

end

function DemoModuleData.clear(tcpClinet)

 	tcpClinet.removelistener(Cmd.user_para,DemoModuleData.on_msg)

end 

function DemoModuleData.on_msg(key,decode)

	print(" DemoData >> on_msg >> "..key .. " user_para >>  ".. Cmd.user_para)

	if key == Cmd.user_para then

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)

	end

end	

return DemoModuleData