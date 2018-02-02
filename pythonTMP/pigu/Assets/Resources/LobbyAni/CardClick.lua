
local  luaAnimationCtrl 

local  luaAnimationCtrlList = {}

function start()

	luaAnimationCtrl = self.gameObject:GetComponent('LuaAnimationCtrl')

	local i = 0
	while i < self.transform.parent.childCount do

		luaAnimationCtrlList[i+1] = self.transform.parent:GetChild(i):GetComponent('LuaAnimationCtrl')
		i = i+1
	end	

	--luaAnimationCtrlList = self.transform.parent:GetComponentsInChildren('LuaAnimationCtrl')

	print("luaAnimationCtrlList->"..#luaAnimationCtrlList)
end

function OnPointerClick()

	print('',self.curEventData.pointerPressRaycast.gameObject.name);
	local isRun = false
		local speed = 0

		local i = 1
   		while isRun == false and i <= #luaAnimationCtrlList do

   			speed = luaAnimationCtrlList[i]:GetAni("CardMoveAnimation").speed + luaAnimationCtrlList[i]:GetAni("CardReSetAnimation").speed 
   			--if luaAnimationCtrlList[i]:GetAni("CardMoveAnimation").speed > 0 or luaAnimationCtrlList[i]:GetAni("CardReSetAnimation").speed >= 0 then
   			if speed > 0 then

   				print('speed>>'..luaAnimationCtrlList[i]:GetAni("CardMoveAnimation").speed )

   				isRun = true
   			end	

   			i = i+1
		end

	--if luaAnimationCtrl:GetAni("CardMoveAnimation").speed == 0  then 
	if isRun == false then
		--luaAnimationCtrl:GetAni("CardMoveAnimation").time = luaAnimationCtrl:GetAni("CardMoveAnimation").time + 1
		--luaAnimationCtrl:Play('CardMoveAnimation')
--[[]]
		local i = 1
   		while i <= #luaAnimationCtrlList do

   			luaAnimationCtrlList[i]:Play('CardMoveAnimation')

   			i = i+1
		end

	end

end
