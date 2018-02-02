local state = 'init'

function start()

	--if Index == nil then Index = 0 end
	--CardImage3d
	scriptEnv.index = tonumber(string.sub(self.gameObject.name, 12))
	--Index = Index+1

	print(scriptEnv.index )

	--self:GetAni("CardMoveAnimation").speed = 1
	self:Play('CardMoveAnimation')
	--self:GetAni("CardMoveAnimation").time = 1.2
	
	self:GetAni("CardReSetAnimation").speed = 0

	self:AddEvent("CardMoveAnimation",1.2,"function_event_flaot1",1.2)
	self:AddEvent("CardMoveAnimation",1.4,"function_event_flaot2",1.4)
	self:AddEvent("CardMoveAnimation",1.6,"function_event_flaot3",1.6)
	self:AddEvent("CardMoveAnimation",1.8,"function_event_flaot4",1.8)
	self:AddEvent("CardMoveAnimation",2,"function_event_flaot5",2)
	--self:AddEvent("CardMoveAnimation",2,"function_event_flaot6",2)
	self:AddEvent("CardMoveAnimation",2.77,"function_event_flaot7",2.77)

	self:AddEvent("CardReSetAnimation",0.3,"function_event_CardReSetAnimation",0.3)
	--self:AddEvent("CardMoveAnimation",0.5,"function_event",'data')

end	

function function_event_flaot1( data)
	-- body
	print('function_event_flaot1 = '..scriptEnv.index)

	if state == 'init' then

		if scriptEnv.index == 1 then
			self:Stop("CardMoveAnimation")

			state = 'run'
		end	

		
	elseif state == 'toStrat' or state == 'run' then
			self:Stop("CardMoveAnimation")

			state = 'run'
	end	

	self:GetAni("CardMoveAnimation").time = data + 0.01

end

function function_event_flaot2( data)
	-- body
	print('function_event_flaot2 = '..scriptEnv.index..state)
	if state == 'init' then

		if scriptEnv.index == 2 then
			self:Stop("CardMoveAnimation")

			state = 'run'
		end	

	elseif state == 'toStrat' or state == 'run' then
			self:Stop("CardMoveAnimation")

			state = 'run'
		
	end	

	self:GetAni("CardMoveAnimation").time = data + 0.01

end

function function_event_flaot3( data)
	-- body
	print('function_event_flaot3 = '..scriptEnv.index)
	if state == 'init' then

		if scriptEnv.index == 3 then
			self:Stop("CardMoveAnimation")

			state = 'run'
		end

	elseif state == 'toStrat' or state == 'run' then
			self:Stop("CardMoveAnimation")

			state = 'run'
			
	end	

	self:GetAni("CardMoveAnimation").time = data + 0.01

end

function function_event_flaot4( data)
	-- body
	print('function_event_flaot4 = '..scriptEnv.index)
	if state == 'init' then

		if scriptEnv.index == 4 then
			self:Stop("CardMoveAnimation")

			state = 'run'
		end	

	elseif state == 'toStrat' or state == 'run' then
			self:Stop("CardMoveAnimation")

			state = 'run'
		
	end	

	self:GetAni("CardMoveAnimation").time = data + 0.01
end

function function_event_flaot5( data)
	-- body
	print('function_event_flaot5 = '..scriptEnv.index)
	if state == 'init' then

		if scriptEnv.index == 5 then
			self:Stop("CardMoveAnimation")

			state = 'run'
		end	

	elseif state == 'toStrat' or state == 'run' then
			self:Stop("CardMoveAnimation")

			state = 'run'
		
	end	

	self:GetAni("CardMoveAnimation").time = data + 0.01

end

function function_event_flaot6( data)
	-- body
	print('function_event_flaot = '..data)

	self:Stop("CardMoveAnimation")

end

function function_event_flaot7( data)
	-- body
	print('function_event_flaot7 = '..data)


	self:StopToStart("CardMoveAnimation")
	--self:GetAni("CardMoveAnimation").time = 0
	self:Play('CardReSetAnimation')
	self.ani:CrossFade('CardReSetAnimation')
end

function function_event_CardReSetAnimation(data)

	self:StopToStart("CardReSetAnimation")
	self:Play('CardMoveAnimation')
	self.ani:CrossFade('CardMoveAnimation')

	state ='toStrat' 
end

function function_event( data)
	-- body
	print('function_event = '..data)

end