local DemoPanelview = require 'lua/game/modules/DemoPanel/DemoPanelview'
--本模块消息号
local msgCmd = GameState.curRunState.MsgDefine.DemoModuleCmd
--本模块数据层
local data = GameState.curRunState.Data.DemoModuleData

function awake()

	DemoPanelview.transform = self.transform 
	DemoPanelview:init()
	--DemoPanelview.setstate('init_state')
	DemoPanelview:set_state('init_state')
	--print( DemoPanelview.transform)
	print('>>>1'..self.gameObject:ToString())
	print('>>>1'..self.transform:ToString())
	print('>>>2'..DemoPanelview.transform:ToString())
	print('>>>3'..DemoPanelview.button:ToString())

	DemoPanelview.fun('>>>> DemoPanelview.fun Call')
	--事件监听
	EventManager.AddListener("OnButtonClicked",on_click)
	--EventManager.RemoveListener("OnButtonClicked",on_click)
	DemoPanelview.button:GetComponent("Button").onClick:AddListener(function()
		--print("clicked, you input is '" ..input:GetComponent("InputField").text .."'")
		DemoPanelview.text:GetComponent("Text").text = "clicked"

		print(">>>>>> clicked "..DemoPanelview.button:ToString())
		DemoPanelview:set_state('clicked_state')
 		--事件发送
 		--EventManager.Brocast("OnButtonClicked",DemoPanelview.text)
 		--EventManager.Send("OnButtonClicked",DemoPanelview.text)
 		es("OnButtonClicked",DemoPanelview.text)
 		--EventManager.Update()
	end)

	--GameState.tcpClinet.msm.AddListener(msgCmd.user_para,on_msg)
	ml(msgCmd.user_para,on_msg)

	--GameData.DemoData = { viewstate = 'init_state' }
end	
--事件处理函数
function on_click(event,textp)

	textp:GetComponent("Text").text = "on_click set clicked !"
	--DemoPanelview.text:GetComponent("Text").text = "on_click set clicked !"
	print("on_click >>>>>> clicked "..textp:ToString())
	--GameState.curState = 'LoginToLobby' 
	--CS.ZhuYuU3d.LuaCallCsFun.JumpScene(0)
	--跳转到大厅
	--game_state_jump_to_scene(SceneName.Lobby)
    --发测试消息
	GameState.tcpClinet.testPbc2()

	local addressbook = {
        name = "Alice",
        id = 12345,
        phone = {
            { number = "1301234567" },
            { number = "87654321", type = "WORK" },
        }
    }

    --local code = protobuf.encode("tutorial.Person", addressbook)
    --GameState.tcpClinet.sendmsg(msgCmd.user_para,addressbook)
    ms(msgCmd.user_para,addressbook)
end	
--消息处理函数
function on_msg(key,decode)

	print(" on_msg >> "..key .. "  user_para >>  ".. msgCmd.user_para)

	if key == msgCmd.user_para then

		--GameData.DemoData.person = decode

		print(data.person.name)
    	print(data.person.id)
    	print(data.person.email)

    	DemoPanelview.text:GetComponent("Text").text = data.person.name..decode.email
    	--跳转到大厅
		--game_state_jump_to_scene(SceneName.Lobby)

	end

end		

function fun(param)
	print(param);
end 

function start()

end

function update()

end

function ondestroy()
	--移除事件监听
	EventManager.RemoveListener("OnButtonClicked",on_click)
	--移除消息监听
	GameState.tcpClinet.msm.RemoveListener(msgCmd.user_para,on_msg)

	DemoPanelview:on_destroy()
end