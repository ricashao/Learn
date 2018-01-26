--TP_ModuleData.lua
-- 本模块数据层
-- ZEntry
-- $MsgName$
-- ZProto
-- $ID$
--[[
$pbCode
]]
local ZEntryModuleData = {}
local uimanager = require 'lua/game/LuaUIManager'
local Cmd =
{
	CS_Login = sib(1000),
	CS_LoginByAccount = sib(1001),
	CS_LoginByPlatform = sib(1002),
	SC_LoginSuccess = sib(11000),
	CS_RegistByPhone = sib(1003),
	CS_EditPassword = sib(1004),
	
	--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
}

function ZEntryModuleData.register(tcpClinet,MsgDefine)
	--protobuf 注册
	local protobuf = tcpClinet.proto
    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
	local luaPath = PbPtah()
	--protobuf.register_file(luaPath..'/proto/ZCommon.pb')
    --protobuf.register_file(luaPath..'/proto/ZEnum.pb')
    --protobuf.register_file(luaPath..'/proto/ZModel.pb')
    --protobuf.register_file(luaPath..'/proto/ZLobby.pb')
    --protobuf.register_file(luaPath..'/proto/ZEntry.pb')

    protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZLobby.pb'))
   	protobuf.register(CS.ZhuYuU3d.LuaCallCsFun.ReadByteForLua('/proto/ZEntry.pb'))

	--映射协议号 -> protobuf 
	MsgDefine.ZEntryModuleCmd = Cmd
    local pbtable = tcpClinet.pb
	pbtable [Cmd.CS_Login] = "ZProto.CS_Login"
	pbtable [Cmd.CS_LoginByAccount] = "ZProto.CS_LoginByAccount"
	pbtable [Cmd.CS_LoginByPlatform] = "ZProto.CS_LoginByPlatform"
	pbtable [Cmd.SC_LoginSuccess] = "ZProto.SC_LoginSuccess"
	pbtable [Cmd.CS_RegistByPhone] = "ZProto.CS_RegistByPhone"
	pbtable [Cmd.CS_EditPassword] = "ZProto.CS_EditPassword"
	--pbtable [Cmd.$MsgName$] = "ZProto.$MsgName$"
	--监听处理事件
	tcpClinet.addlistener(Cmd.SC_LoginSuccess,ZEntryModuleData.on_msg)
	
	RegisterPanelServices= require 'lua/modules/Lobby/modules/Register/services/RegisterPanelServices';
	RegisterPanelServices.Init(MsgDefine.ZEntryModuleCmd,ZEntryModuleData);
	
	LoginPanelServices= require 'lua/modules/Lobby/modules/Login/services/LoginPanelServices';
	LoginPanelServices.Init(MsgDefine.ZEntryModuleCmd,ZEntryModuleData);
	

	--EventManager.HandlerInfo()
	--GameState.tcpClinet.msm.HandlerInfo()
--[[    
	optional string phone = 1;          //手机号
    optional uint32 code = 2;           //验证码
    optional string password = 3;       //登录密码（md5加密的32位字符串）
    optional uint32 uid = 4;            //用户ID（如果为空，则代表是用手机号注册账号，如果存在，则表示用手机号绑定这个用户）
    ]]
	--ZEntryModuleData.send_CS_RegistByPhone("13585550291",123456,"712621",7)
	--ZEntryModuleData.send_CS_RegistByPhone("13585550291",123456,nil,nil)
	--ZEntryModuleData.send_CS_RegistByPhone("13585550291",nil,"712621",nil)
	--tcpClinet.addlistener(Cmd.$MsgName$,ZEntryModuleData.on_msg)
end

function ZEntryModuleData.clear(tcpClinet)
	--移除监听事件
	tcpClinet.removelistener(Cmd.SC_LoginSuccess,ZEntryModuleData.on_msg)
	tcpClinet.removelistener(Cmd.MessageNotify,ZEntryModuleData.on_msg)
 	--tcpClinet.removelistener(Cmd.$MsgName$,ZEntryModuleData.on_msg)
	RegisterPanelServices.Clear();

	LoginPanelServices.Clear();

end 	

function ZEntryModuleData.send_CS_Login(token,platform)
	local CS_Login = {}
	CS_Login.token = token
	CS_Login.device = platform
	print(CS_Login.device)
	GameState.tcpClinet.sendmsg(Cmd.CS_Login,CS_Login)
end
function ZEntryModuleData.send_CS_LoginByAccount(account,password,platform)
	local CS_LoginByAccount = {}
	CS_LoginByAccount.account = account
	CS_LoginByAccount.password = password
	CS_LoginByAccount.device = platform
	GameState.tcpClinet.sendmsg(Cmd.CS_LoginByAccount,CS_LoginByAccount)
end
function ZEntryModuleData.send_CS_LoginByPlatform(platform,token)
	local CS_LoginByPlatform = {}
	CS_LoginByPlatform.platform = platform
	CS_LoginByPlatform.token = token
	CS_LoginByPlatform.device = Platform
	GameState.tcpClinet.sendmsg(Cmd.CS_LoginByPlatform,CS_LoginByPlatform)
end
function ZEntryModuleData.send_CS_RegistByPhone(phone,code,password,uid,platform)
	local CS_RegistByPhone = {}
	CS_RegistByPhone.phone = phone
	CS_RegistByPhone.code = code
	CS_RegistByPhone.password = password
	CS_RegistByPhone.uid = uid
	CS_RegistByPhone.device = platform
	GameState.tcpClinet.sendmsg(Cmd.CS_RegistByPhone,CS_RegistByPhone)
end
function ZEntryModuleData.send_CS_EditPassword(uid,code,password)
	local CS_EditPassword = {}
	CS_EditPassword.uid = uid
	CS_EditPassword.code = code
	CS_EditPassword.password = password
	GameState.tcpClinet.sendmsg(Cmd.CS_EditPassword,CS_EditPassword)
end


function ZEntryModuleData.on_msg(key,decode)

	if key == Cmd.SC_LoginSuccess then
		--print("ZEntry >> on_msg >> user_para >>  ".. Cmd.SC_LoginSuccess)

		ZEntryModuleData.SC_LoginSuccess = decode
		CommonData.user = decode.user;
		CommonData.user_info = decode.user_info
		print('face============'..decode.user.face)
		
		if(EventExist("LoginSuccess")) then
			es("LoginSuccess",nil)
		end
		
--[[
		print("SC_LoginSuccess.user ".. decode.user.id);
		
		print("SC_LoginSuccess.user ".. decode.user.gold);

		print("SC_LoginSuccess.user_info ".. decode.user_info.gold_bank);
		]]
	end
	

--[[
	if key == Cmd.$MsgName$ then
		print("ZEntry >> on_msg >> "..key .. " user_para >>  ".. Cmd.$MsgName$)

		DemoModuleData.person = decode
    	--跳转到大厅
		game_state_jump_to_scene(SceneName.Lobby)
		return
	end
]]
end 	


function ZEntryModuleData.GetUserID()
		if ZEntryModuleData.SC_LoginSuccess~=nil then
			return ZEntryModuleData.SC_LoginSuccess.user.id;
		end
		return 0;
end


	

return ZEntryModuleData
