local view = require 'lua/modules/Test/TestSubModule/TestPanelView'

function awake()
	--DemoListPanelView.transform = self.transform 
	view:init(self.transform)
	--DemoPanelview.setstate('init_state')
	view:set_state('init_state')
	
	view.Button:GetComponent("Button").onClick:AddListener(function()
	end)
	--AddEventCode 追加事件标志

	--TestPanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 

		--print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		--game_state_jump_to_scene(SceneName.Lobby)
	--end)

end	

function start()

end

function ondestroy()

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	view:on_destroy()
	view = nil
	
end