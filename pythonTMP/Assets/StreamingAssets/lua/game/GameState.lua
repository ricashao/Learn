--gameinit.lua
--[[
游戏全局状态对象
]]
CommonFun = require 'lua/game/CommonFun'

Define = require 'lua/game/Define'

--MsgDefine = require 'lua/game/MsgDefine'
--GameData = require "lua/game/GameData"

local events = require "lua/utils/events"

Platform = 'UNKNOW'

-- 初始化游戏全局状态对象
function game_state_init()
 	-- body
	if GameState ~= nil then

		print("gamestate already created do gameinit.lua return !")

		game_state_run()

		return
	end

	if CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.Android then
		Platform = 'ANDROID'
	elseif CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.IPhonePlayer then
		Platform = 'IOS'
	elseif CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.WebGLPlayer then 
		Platform = 'WEB'
	elseif CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.WindowsPlayer then
		Platform = 'WIN'
	elseif CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.OSXPlayer then
		Platform = 'MAC'
	elseif CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.WindowsEditor then
		Platform = 'WIN'
	elseif CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.OSXEditor then
		Platform = 'MAC'
	end		
	
	print('>>当前平台 = '..Platform)
		
	--游戏状态表
	GameState = {curState = 'InitToLobby'}
	--初始化 update 回调方法列表
	GameState.fiexdUpdateFunList = {}
	GameState.updateFunList = {}
	GameState.lateUpdateFunList = {}
	--注册lua层全局回调
	CS.ZhuYuU3d.LuaManager.SetFixedUpdateFun('game_state_fiexd_update')
	CS.ZhuYuU3d.LuaManager.SetUpdateFun('game_state_update')
	CS.ZhuYuU3d.LuaManager.SetLateUpdateFun('game_state_late_update')
	--事件管理器
	EventManager = events
	--添加 事件管理器 update 回调 用于执行事件队列
	game_state_callback_add(GameState.lateUpdateFunList,EventManager.Update)
	--加载 进度显示界面
	--game_state_loading()	
	SceneName = {Login = 'Login' , Lobby = 'Lobby' , Game = 'Game'}
	--模块数据层初始化
	--GameData.DemoData = {}
	--GameData.DemoListData = {}

	--初始化网络层
	--GameState.tcpClinet = CS.ZhuYuU3d.LuaBaseBehaviour.LoadDontDestroy('TcpClinet','lua/game/TcpClient.lua');
	--GameState.tcpClinet = CS.ZhuYuU3d.LBF.LDD('TcpClinet','lua/game/TcpClient.lua');
	GameState.tcpClinet = LoadDontDestroy('TcpClinet','lua/game/TcpClient.lua');
	--pb 注册
	--MsgDefine:register(GameState.tcpClinet)
	--游戏数据层注册
	--GameData:register()
	--GameData:clear()

	--GameState.tcpClinet.register()
	--[[
 	local protobuf = GameState.tcpClinet.proto

    protobuf.register(CS.UnityEngine.Resources.Load('proto/UserInfo.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/User.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/addressbook.pb').bytes)

    local pbtable = GameState.tcpClinet.pb
    pbtable [string.char(1, 2)] = "tutorial.Person"
    ]]
    --end pb 注册

	--连接服务器
    GameState.tcpClinet.connect(Define.host,Define.port)
    --发测试消息
    --GameState.tcpClinet.testPbc2()

	print("do gameinit.lua！");

	--切换状态
	game_state_run()	
end 
-- 清除化游戏全局状态对象
function game_state_clear()

	print(">>>> cg start mem : "..collectgarbage("count") * 1024 )
	--清除数据层监听
	GameData:clear()
	--清除 update 回调
	game_state_callback_rem(GameState.lateUpdateFunList,EventManager.Update)
	--清除事件监听队列
	EventManager.RemoveAll()
	--清除 updatefunList 
	GameState.updatefunList = nil
	--清除 GameState
	GameState = nil
	--清除 GameData
	GameData = nil

	SceneName = nil

	collectgarbage("collect")

	print("<<<< cg end mem : "..collectgarbage("count") * 1024 )
end 	
-- 添加全局 update 回调 
function game_state_callback_add(tb,updatefun)
	--table.insert(GameState.updateFunList,updatefun)
	table.insert(tb,updatefun)
end 
-- 移除全局 update 回调 
function game_state_callback_rem(tb,updatefun)
	-- 从 回调列表 移除 updatefun
	--local updateFunList = GameState.updateFunList
	local updateFunList = tb
   	for i=#updateFunList, 1, -1 do 
        if updateFunList[i] == updatefun then 
            table.remove(updateFunList,i) 
            print("remove updatefun = " .. i)
        end 
    end 
end
-- 执行全局 fiexdupdate 回调 
function game_state_fiexd_update()
	for i,updatefun in ipairs(GameState.fiexdUpdateFunList) do
		updatefun()
	end
end 	
-- 执行全局 update 回调 
function game_state_update()
	--迭代调用 update
	for i,updatefun in ipairs(GameState.updateFunList) do
		updatefun()
	end
end 	
-- 执行全局 lateupdate 回调 
function game_state_late_update()
	for i,updatefun in ipairs(GameState.lateUpdateFunList) do
		updatefun()
	end
end 

function game_state_loading()	

	if GameState.curState == 'InitToLobby' then

		GameState.curRunState = require 'lua/modules/Lobby/LobbyState'
		GameState.curRunState:init(GameState, GameState.tcpClinet,GameData)
		
		LoginWhenToLobby=require 'lua/modules/Lobby/modules/Login/services/LoginCommand';
		LoginWhenToLobby.Excute(function()
			GameState.curState ='Lobby'
			CS.ZhuYuU3d.LuaCallCsFun.JumpToRun()
		end
		);

		

	elseif GameState.curState == 'InitToLogin' then

		GameState.curRunState = require 'lua/modules/demo/DemoState'
		GameState.curRunState:init(GameState, GameState.tcpClinet,GameData)

		--跳转到运行场景
		GameState.curState ='Login'
		CS.ZhuYuU3d.LuaCallCsFun.JumpToRun()
		
	elseif GameState.curState == 'LoginToLobby' then

		GameState.curRunState:clear()
		GameState.curRunState = nil

		GameState.curRunState = require 'lua/modules/Lobby/LobbyState'
		GameState.curRunState:init(GameState, GameState.tcpClinet,GameData)

		GameState.curState ='Lobby'
		CS.ZhuYuU3d.LuaCallCsFun.JumpToRun()

	elseif GameState.curState == 'LobbyToGame' then

		GameState.curState ='Game'
		CS.ZhuYuU3d.LuaCallCsFun.JumpToRun()

	elseif GameState.curState == 'GameToLobby' then

		GameState.curState ='Lobby'
		CS.ZhuYuU3d.LuaCallCsFun.JumpToRun()

	end	
end

function game_state_run()	

	if GameState.curState == 'Login' then
		--local behaviourLuaTable = CS.ZhuYuU3d.LuaBaseBehaviour.Load("LuaBaseBehaviourTest",'lua/game/LuaBaseBehaviour.lua')
		 GameState.curLuaScene = CS.ZhuYuU3d.LuaBaseBehaviour.Load("LoginScene",'lua/game/LuaScenes/LoginScene.lua');	
		--local leng = behaviourLuaTable.table_leng(behaviourLuaTable);
		--print("behaviourLuaTable >>>> table_leng = "..leng)
		--local demoPane = CS.UnityEngine.GameObject.Find('Canvas/DemoPanel')
		--demoPanelCtrl = CS.ZhuYuU3d.LuaBaseBehaviour.Add('Canvas/DemoPanel','lua/game/modules/DemoPanel/DemoPanelCtrl.lua');
		--CS.ZhuYuU3d.LuaBaseBehaviour.Load("LuaBaseBehaviourTest",'lua/game/modules/DemoPanel/DemoPanelCtrl.lua');
		--print("gamedata.DemoData.viewstate = "..GameData.DemoData.viewstate);LobbyScene.lua
	elseif GameState.curState == 'Lobby' then
			
		GameState.curLuaScene = CS.ZhuYuU3d.LuaBaseBehaviour.Load("LoginScene",'lua/game/LuaScenes/LobbyScene.lua');	
			
	elseif GameState.curState == 'Game' then

		GameState.curLuaScene = CS.ZhuYuU3d.LuaBaseBehaviour.Load("LoginScene",'lua/game/LuaScenes/GameScene.lua');

	end

end 	

function game_state_jump_to_scene(sceneName)

	if GameState.curState == 'Login' and SceneName.Lobby == sceneName then
		GameState.curState = 'LoginToLobby'
		CS.ZhuYuU3d.LuaCallCsFun.JumpToLoading()

	elseif GameState.curState == 'Lobby' and SceneName.Game == sceneName then
		GameState.curState = 'LobbyToGame'
		CS.ZhuYuU3d.LuaCallCsFun.JumpToLoading()

	elseif	GameState.curState == 'Game' and SceneName.Lobby == sceneName then
		GameState.curState = 'GameToLobby'
		CS.ZhuYuU3d.LuaCallCsFun.JumpToLoading()
	end

end	

CommonData={}