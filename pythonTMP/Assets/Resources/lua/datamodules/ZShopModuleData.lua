--TP_ModuleData.lua
-- 本模块数据层
-- ZShop
-- $MsgName$
-- ZProto
-- $ID$
--[[
$pbCode
]]
local ZShopModuleData = {}

local Cmd = {
	CS_GetItemList = sib(1100),
	SC_SetItemList = sib(11100),
	CS_GetItem = sib(1101),
	SC_SetItem = sib(11101),
	CS_ExchangeItem = sib(1102),
	CS_UseItem = sib(1103),
	CS_GetBag = sib(1104),
	SC_SetBag = sib(11104),

	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function ZShopModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
    --protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZShop.pb'))
	
	local luaPath = PbPtah()
	--映射协议号 -> protobuf 
	MsgDefine.ZShopModuleCmd = Cmd
    local pbtable = tcpClinet.pb
	pbtable [Cmd.CS_GetItemList] = "ZProto.CS_GetItemList"
	pbtable [Cmd.SC_SetItemList] = "ZProto.SC_SetItemList"
	pbtable [Cmd.CS_GetItem] = "ZProto.CS_GetItem"
	pbtable [Cmd.SC_SetItem] = "ZProto.SC_SetItem"
	pbtable [Cmd.CS_ExchangeItem] = "ZProto.CS_ExchangeItem"
	pbtable [Cmd.CS_UseItem] = "ZProto.CS_UseItem"
	pbtable [Cmd.CS_GetBag] = "ZProto.CS_GetBag"
	pbtable [Cmd.SC_SetBag] = "ZProto.SC_SetBag"

	--pbtable [Cmd.$MsgName$] = "ZProto.$MsgName$"
	--监听处理事件
	tcpClinet.addlistener(Cmd.SC_SetItemList,ZShopModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetItem,ZShopModuleData.on_msg)
	tcpClinet.addlistener(Cmd.SC_SetBag,ZShopModuleData.on_msg)

	--tcpClinet.addlistener(Cmd.$MsgName$,ZShopModuleData.on_msg)
end

function ZShopModuleData.clear(tcpClinet)

	tcpClinet.removelistener(Cmd.SC_SetItemList,ZShopModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetItem,ZShopModuleData.on_msg)
	tcpClinet.removelistener(Cmd.SC_SetBag,ZShopModuleData.on_msg)

 	--tcpClinet.removelistener(Cmd.$MsgName$,ZShopModuleData.on_msg)
end 	

function ZShopModuleData.send_CS_GetItemList(version)
	local CS_GetItemList = {}
	CS_GetItemList.version = version
	GameState.tcpClinet.sendmsg(Cmd.CS_GetItemList,CS_GetItemList)
end
function ZShopModuleData.send_CS_GetItem(uid,mid,amount)
	local CS_GetItem = {}
	CS_GetItem.uid = uid
	CS_GetItem.mid = mid
	CS_GetItem.amount = amount
	GameState.tcpClinet.sendmsg(Cmd.CS_GetItem,CS_GetItem)
end
function ZShopModuleData.send_CS_ExchangeItem(uid,mid,amount,realname,phone,code,postcode,address)
	local CS_ExchangeItem = {}
	CS_ExchangeItem.uid = uid
	CS_ExchangeItem.mid = mid
	CS_ExchangeItem.amount = amount
	CS_ExchangeItem.realname = realname
	CS_ExchangeItem.phone = phone
	CS_ExchangeItem.code = code
	CS_ExchangeItem.postcode = postcode
	CS_ExchangeItem.address = address
	GameState.tcpClinet.sendmsg(Cmd.CS_ExchangeItem,CS_ExchangeItem)
end
function ZShopModuleData.send_CS_UseItem(uid,id,amount)
	local CS_UseItem = {}
	CS_UseItem.uid = uid
	CS_UseItem.id = id
	CS_UseItem.amount = amount
	GameState.tcpClinet.sendmsg(Cmd.CS_UseItem,CS_UseItem)
end
function ZShopModuleData.send_CS_GetBag(uid)
	local CS_GetBag = {}
	CS_GetBag.uid = uid
	GameState.tcpClinet.sendmsg(Cmd.CS_GetBag,CS_GetBag)
end


function ZShopModuleData.on_msg(key,decode)

	if key == Cmd.SC_SetItemList then
		print("ZShop >> on_msg >> user_para >>  ".. Cmd.SC_SetItemList)

		ZShopModuleData.SC_SetItemList = decode
		print("SC_SetItemList.items ".. decode.items)
	end
	if key == Cmd.SC_SetItem then
		print("ZShop >> on_msg >> user_para >>  ".. Cmd.SC_SetItem)

		if CommonData.bags == nil then CommonData.bags = {} end
		for _,v in pairs(decode.items) do
			CommonData.bags[v.id] = v;
		end
		ZShopModuleData.clearBag()
		es(LobbyEventConst.Bag_Update)
		--print("SC_SetItem.items ".. decode.items)
	end
	if key == Cmd.SC_SetBag then
		print("ZShop >> on_msg >> user_para >>  ".. Cmd.SC_SetBag)
		if CommonData.bags == nil then CommonData.bags = {} end
		if decode.items ~= nil then
			for _,v in pairs(decode.items)  do
				CommonData.bags[v.id] = v
			end
		end
		--print("SC_SetBag.items ".. decode.items)
	end
	
--[[
	if key == Cmd.$MsgName$ then
		print("ZShop >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end 	

function ZShopModuleData.clearBag()
	local tmp ={}
	for k,v in pairs(CommonData.bags) do
		if v.amount ~= 0 then
			tmp[k] = v
		end
    end
	CommonData.bags = tmp
end

return 	ZShopModuleData