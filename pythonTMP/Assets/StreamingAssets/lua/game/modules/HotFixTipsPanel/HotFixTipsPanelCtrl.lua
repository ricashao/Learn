local HotFixPanelview = require 'lua/game/modules/HotFixTipsPanel/HotFixTipsPanelView'


function awake()
	HotFixPanelview:init(self.transform);
	
	EventManager.AddListener("OnHotFixedViewEvent",on_click);
end	
--事件处理函数
function on_click(event,textp)

	
	
	print("on_click >>>>>> clicked "..textp.name);
	
	

end	


--消息处理函数
function on_msg(key,decode)

 
end		

function start()

end

function update()

end

function ondestroy()
	
end