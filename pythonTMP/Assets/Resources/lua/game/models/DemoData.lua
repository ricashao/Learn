
local DemoData = {}

local msgCmd = MsgDefine.cmd

function DemoData.register()
	ml(msgCmd.user_para,DemoData.on_msg)
end

function DemoData.clear()
 	mc(msgCmd.user_para,DemoData.on_msg)
end 

function DemoData.on_msg(key,decode)

	print(" DemoData >> on_msg >> "..key .. " user_para >>  ".. msgCmd.user_para)

	if key == msgCmd.user_para then

		GameData.DemoData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)

	end

end	

return DemoData

