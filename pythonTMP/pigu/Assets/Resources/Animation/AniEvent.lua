

function start()

	self:AddEvent("CubeAnimation",0.5,"function_event_flaot",0.5)
	self:AddEvent("CubeAnimation",0.5,"function_event",'data')

end	

function function_event_flaot( data)
	-- body
	print('function_event_flaot = '..data)

end

function function_event( data)
	-- body
	print('function_event = '..data)

end