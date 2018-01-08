local DemoListPanelView = require 'lua/game/modules/DemoListPanel/DemoListPanelView'

function awake()

	--DemoListPanelView.transform = self.transform 
	DemoListPanelView:init(self.transform)
	--DemoPanelview.setstate('init_state')
	DemoListPanelView:set_state('init_state')
	
	DemoListPanelView.button:GetComponent("Button").onClick:AddListener(function() 

		print(">>>>>> clicked "..DemoListPanelView.button:ToString())
		--跳转到游戏
		game_state_jump_to_scene(SceneName.Game)
	end)
end	

function start()

end

function ondestroy()

	--EventManager.RemoveListener("OnButtonClicked",on_click)
	DemoListPanelView:on_destroy()
	DemoListPanelView = nil
	
end