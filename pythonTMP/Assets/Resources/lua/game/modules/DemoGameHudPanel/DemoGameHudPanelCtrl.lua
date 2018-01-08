local DemoGameHudPanelView = require 'lua/game/modules/DemoGameHudPanel/DemoGameHudPanelView'

function awake()
	--DemoListPanelView.transform = self.transform 
	DemoGameHudPanelView:init(self.transform)
	--DemoPanelview.setstate('init_state')
	DemoGameHudPanelView:set_state('init_state')
	
	DemoGameHudPanelView.returnButton:GetComponent("Button").onClick:AddListener(function() 

		print(">>>>>> clicked "..DemoGameHudPanelView.returnButton:ToString())
		--跳转回到游戏
		game_state_jump_to_scene(SceneName.Lobby)
	end)
end	

function start()

end

function ondestroy()

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	DemoGameHudPanelView:on_destroy()
	DemoGameHudPanelView = nil
	
end