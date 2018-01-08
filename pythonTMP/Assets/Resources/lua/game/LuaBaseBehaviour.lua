
function awake()

end	

function table_leng(t)
  local leng=0
  for k, v in pairs(t) do
    leng=leng+1
  end
  return leng;
end

function start()

	print(table_leng(scriptEnv).." LuaBaseBehaviour start... "..self.luaPath)
  --[[
	for k, v in pairs(scriptEnv) do
    	print(k,v);
  	end]]--
end

function update()
	local r = CS.UnityEngine.Vector3.up * CS.UnityEngine.Time.deltaTime
	self.transform:Rotate(r)
end

function ondestroy()
    print("LuaBaseBehaviour destroy..."..self.luaPath)
end