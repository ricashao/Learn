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
	return CS.UnityEngine.Application.dataPath..'/Resources'
end

--获取在指定路径的组件 eg:GetComponentInPath(trans,"txt_title",typeof(CS.UnityEngine.UI.Text));
function GetComponentInPath(transPar,strPath,typeVar)
	objRet=transPar:Find(strPath);
	if objRet~=nil then
		return objRet:GetComponent(typeVar);
	end
	return nil;
end