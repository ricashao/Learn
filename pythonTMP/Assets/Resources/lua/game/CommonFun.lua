local bit32 = require "lua/numberlua".bit32
--注册消息
function ml(msgcmd ,fun)
	-- body
	GameState.tcpClinet.addlistener(msgcmd,fun)
end
--移除注册消息
function mr(msgcmd ,fun)
	-- body
	GameState.tcpClinet.removelistener(msgcmd,fun)
end
--发送网络消息
function ms(msgcmd,data)

	 GameState.tcpClinet.sendmsg(msgcmd,data)
end	
--发送事件
function es(event,data)

	EventManager.Send(event,data)
end	

function el(event,fun)
	EventManager.AddListener(event,fun)
end	

function er(event,fun)
	EventManager.RemoveListener(event,fun)
end	
--基础 Behaviour
function LuaBaseBehaviourAdd(goPath,luafile)
	local tb = CS.ZhuYuU3d.LBF.AddBase(goPath,luafile)
	return tb
end	
--带 Update 的 Behaviour
function LuaUpdateBehaviourAdd(goPath,luafile)
	local tb = CS.ZhuYuU3d.LBF.AddUpdate(goPath,luafile)
	return tb
end	
--不销毁的 Behaviour
function LoadDontDestroy(name,luafile)
	--CS.ZhuYuU3d.LBF.LDD('TcpClinet','lua/game/TcpClient.lua');
	return CS.ZhuYuU3d.LBF.LDD(name,luafile)
end	
--function serialize_int32_big_endian(value)
--序列化一个int 大端
function sib(value)
  local a = bit32.band(bit32.rshift(value, 24), 255)
  local b = bit32.band(bit32.rshift(value, 16), 255)
  local c = bit32.band(bit32.rshift(value, 8), 255)
  local d = bit32.band(value, 255)
  return string.char(a, b, c, d)
end

function PbPtah()
		if Platform == 'ANDROID' then	
		return CS.UnityEngine.Application.persistentDataPath
	elseif Platform == 'IOS' then
		return CS.UnityEngine.Application.persistentDataPath
	end
		
	return CS.UnityEngine.Application.dataPath..'/Resources'
	--return CS.UnityEngine.Application.dataPath..'/Res'
end

--获取在指定路径的组件 eg:GetComponentInPath(trans,"txt_title",typeof(CS.UnityEngine.UI.Text));
function GetComponentInPath(transPar,strPath,typeVar)
	objRet=transPar:Find(strPath);
	if objRet~=nil then
		return objRet:GetComponent(typeVar);
	end
	return nil;
end

function EventExist(strkey)
	return EventManager.Contains(strkey);
end

function IsInTable(key, tbl)
	for k,v in pairs(tbl) do
	  if k == key then
		return true;
	  end
	end
	return false;
end


function removebyvalue(array, value)
    for i=#array, 1, -1 do 
             if array[i] == value then 
                 table.remove(array,i) 
             end 
    end 
end

-- 删除table中的元素
function removeElementByKey(tbl,key)
    --新建一个临时的table
    local tmp ={}

    --把每个key做一个下标，保存到临时的table中，转换成{1=a,2=c,3=b} 
    --组成一个有顺序的table，才能在while循环准备时使用#table
    for i in pairs(tbl) do
        table.insert(tmp,i)
    end

    local newTbl = {}
    --使用while循环剔除不需要的元素
    local i = 1
    while i <= #tmp do
        local val = tmp [i]
        if val == key then
            --如果是需要剔除则remove 
            table.remove(tmp,i)
         else
            --如果不是剔除，放入新的tabl中
            newTbl[val] = tbl[val]
            i = i + 1
         end
     end
    return newTbl
end 

-- 删除table中的元素
function removeElementByKeyAttr(tbl,arrtibute,key)
    --新建一个临时的table
    local tmp ={}

    --把每个key做一个下标，保存到临时的table中，转换成{1=a,2=c,3=b} 
    --组成一个有顺序的table，才能在while循环准备时使用#table
    for i in pairs(tbl) do
        table.insert(tmp,i)
    end

    local newTbl = {}
    --使用while循环剔除不需要的元素
    local i = 1
    while i <= #tmp do
        local val = tmp [i]
        if val[arrtibute] == key then
            --如果是需要剔除则remove 
            table.remove(tmp,i)
         else
            --如果不是剔除，放入新的tabl中
            newTbl[val] = tbl[val]
            i = i + 1
         end
     end
    return newTbl
end 

--获取文件名
function strippath(filename)
	return string.match(filename, ".+/([^/]*%.%w+)$") -- *nix system
	--return string.match(filename, “.+\\([^\\]*%.%w+)$”) — *nix system
end

--去除扩展名
function stripextension(filename)
	local idx = filename:match(".+()%.%w+$")
	if(idx) then
		return filename:sub(1, idx-1)
	else
		return filename
	end
end


function getPairsTable(tb,sortFunc)
    local tmp ={}
	for _,v in pairs(tb) do
        table.insert(tmp,v)
    end
	
	if sortFunc~=nil then
		table.sort(tmp,sortFunc)
		return tmp
	else
		return tmp
	end
end

function updateTableValue(t1,t2)
	for k,v in pairs(t2) do
		if v ~= nil then
			if type(v) ~= 'table' then
				t1[k] = v
			else
				t1[k] = updateTableValue(t1.v,t2.v)
			end
		end
	end
	return t1
end

function split(s, sp)  
    local res = {}
    local temp = s  
    local len = 0  
    while true do  
        len = string.find(temp, sp)  
        if len ~= nil then  
            local result = string.sub(temp, 1, len-1)  
            temp = string.sub(temp, len+1)  
            table.insert(res, result)  
        else  
            table.insert(res, temp)  
            break  
        end  
    end  
  
    return res  
end 

