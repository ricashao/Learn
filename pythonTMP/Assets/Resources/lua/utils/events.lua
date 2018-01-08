--[[
事件管理器

1.注册事件
	EventManager.AddListener("OnButtonClicked",on_click)
	--事件处理函数
	function on_click(textp)
		textp:GetComponent("Text").text = "on_click set clicked !"
		print("on_click >>>>>> clicked "..textp:ToString())
	end	

2.发送事件
	--发送到事件队列在下次 update 回调时执行
	EventManager.Send("OnButtonClicked",DemoPanelview.text) 
	--广播事件直接执行 
	EventManager.Brocast("OnButtonClicked",DemoPanelview.text) 

3.移除事件回调注册
	EventManager.RemoveListener("OnButtonClicked",on_click)
]]

--local EventLib = require "lua/utils/eventlib"
local Event = {}
local events = {}

local eventQueue = {}
--注册监听
function Event.AddListener(event,handler)
	if not event or type(event) ~= "string" then
		error("event parameter in addlistener function has to be string, " .. type(event) .. " not right.")
	end
	if not handler or type(handler) ~= "function" then
		error("handler parameter in addlistener function has to be function, " .. type(handler) .. " not right")
	end

	if not events[event] then
		--create the Event with name
		--events[event] = EventLib:new(event)
		events[event] = {}
	end
	--conn this handler
	table.insert(events[event],handler)
	--events[event]:connect(handler)
end
--发送到事件队列
function Event.Send(event,...)
	table.insert(eventQueue,{event,...})
end 
--update 回调 ,执行事件队列
function Event.Update()
	for i,eventItem in ipairs(eventQueue) do
		for k,handler in ipairs(events[eventItem[1]]) do
    		--print(k,v)
    		handler(eventItem[1],eventItem[2])
		end
    	eventItem[1] = nil
    	eventItem[2] = nil
	end
	eventQueue = {}
end
--直接分发事件
function Event.Brocast(event,...)
	--print("Event.Brocast >>>>>> "...event)
	if not events[event] then
		error("brocast " .. event .. " has no event.")
	else
		--events[event]:fire(...)
		for k,handler in ipairs(events[event]) do
    		--print(k,v)
    		handler(event,...)
		end
	end
end

function Event.RemoveListener(event,handler)
	if not events[event] then
		error("remove " .. event .. " has no event.")
	else
		local handlerList = events[event]

   		for i=#handlerList, 1, -1 do 
            if handlerList[i] == handler then 
                table.remove(handlerList,i) 
                print("remove event = " .. event)
            end 
        end 
		--events[event]:disconnect(handler)
	end
end

function Event.RemoveAll()
	events = {}
	eventQueue = {}
end

return Event