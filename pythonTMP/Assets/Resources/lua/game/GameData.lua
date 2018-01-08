
local GameData = {}
--local demoData = 

function GameData:register()

 	self.DemoData = require 'lua/game/models/DemoData'
 	self.DemoData.register()
 	--批量注册
 	--[[
 	for k, model in pairs(self) do
		model.register()
    	--print('注册数据层 >>>> '..k.." "..model);
    end 
 	--table.insert(t,i,d)
 	]]
end 

function GameData:clear()
 	self.DemoData.clear()
 	--[[
 	local t = self
   	for i=#t, 1, -1 do 
   		t[i].clear()
        table.remove(t,i)   
    end ]]
end 

return GameData