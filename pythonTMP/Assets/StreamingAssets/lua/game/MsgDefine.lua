local msgCmd = {
	user_para = string.char(1, 2)
}

local MsgDefine = {
	cmd = msgCmd
}

function MsgDefine:register(tcpClinet)

	local protobuf = tcpClinet.proto

    protobuf.register(CS.UnityEngine.Resources.Load('proto/UserInfo.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/User.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/addressbook.pb').bytes)

    protobuf.register(CS.UnityEngine.Resources.Load('proto/addressbook.pb').bytes)

    local pbtable = tcpClinet.pb
    pbtable [msgCmd.user_para] = "tutorial.Person"

 	-- body
end 

return MsgDefine