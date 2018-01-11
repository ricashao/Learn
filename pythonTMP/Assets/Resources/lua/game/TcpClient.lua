
local socket = require("socket")

local protobuf = require 'lua/protobuf'
--lua 字节操作
--http://giderosmobile.com/forum/discussion/1083/any-demo-code-for-lua-socket/p1
local bit32 = require "lua/numberlua".bit32

local msgEventManager = require "lua/utils/events"

local pbtable = {}

local this = scriptEnv

this.msm = msgEventManager
this.proto = protobuf
this.pb = pbtable

local client
--///////////////////////////////////////////// byte function /////////////////////////////////////////////
function int32_big_endian(str)
  return string.byte(str, 1) * 16777216 + string.byte(str, 2) * 65536 + string.byte(str, 3) * 256 + string.byte(str, 4)
end 

function int32_little_endian(str)
  return string.byte(str, 4) * 16777216 + string.byte(str, 3) * 65536 + string.byte(str, 2) * 256 + string.byte(str, 1)
end

-- Integer 32 bit serialization (big-endian)
function serialize_int32_big_endian(value)
  local a = bit32.band(bit32.rshift(value, 24), 255)
  local b = bit32.band(bit32.rshift(value, 16), 255)
  local c = bit32.band(bit32.rshift(value, 8), 255)
  local d = bit32.band(value, 255)
  return string.char(a, b, c, d)
end

-- Integer 32 bit serialization (little-endian)
function serialize_int32_little_endian(value)
  local a = bit32.band(bit32.rshift(value, 24), 255)
  local b = bit32.band(bit32.rshift(value, 16), 255)
  local c = bit32.band(bit32.rshift(value, 8), 255)
  local d = bit32.band(value, 255)
  return string.char(d, c, b, a)
end

--///////////////////////////////////////// end byte function ///////////////////////////////////////////// 

--/////////////////////////////////////// tcp socket function /////////////////////////////////////////////
local curHost
local curPort
--连接服务器
function connect(host,port)
    curHost = host
    curPort = port
    --host = "127.0.0.1"
    --port = 9888
    -- 打开一个TCP连接
    -- client = assert (socket.connect (host, port))
    client = socket.connect (host, port)
    if client == nil then 
        print("connect:"..host..port.." fail !")
        return
    end
    -- non-blocking 非阻塞 
    client:settimeout(0);
end
--[[
0,0,0,31,0,0,3,235,10,11,49,51,53,56,53,53,53,48,50,57,49,16,192,196,7,24,7,34,6,55,49,

0,0,0,31,0,0,3,235,10,11,49,51,53,56,53,53,53,48,50,57,49,16,192,196,7,24,7,34,6,55,49,
0 0 0 31 0 0 3 235 10 11 49 51 53 56 53 53 53 48 50 57 49 16 192 196 7 26 6 55 49 50 54 50 49 32 7 
0,0,0,31,0,0,3,235,24,7,34,6,55,49,50,54,50,49,16,192,196,7,10,11,49,51,53,56,53,53,53,48,50,57,49  >> len:35
0,0,0,31,0,0,3,235,32,7,10,11,49,51,53,56,53,53,53,48,50,57,49,26,6,55,49,50,54,50,49,16,192,196,7, >> len:35
]]
--发送数据
function send(msg)
    --client:send ("GET \n")
    if client == nil then 
        print("connect:".. curHost..curPort.." fail !")
        return
    end
    client:send (msg)
end

--本次接受的字节数
local len = 4
-- 0 表示包体长度
-- 1 表示包体数据
local msgtype = 0

--消息接受方法
function receive() 
    
    if client == nil then return end
    --轮询缓冲区 len 为字节长度
    local msg, status, partial = client:receive(len)

    if msg ~= nil then

        print("lua socket receive len->"..#msg)

        if msgtype == 0 then
            --反序列化包体长度
            --len = int32_little_endian(msg)
            len = int32_big_endian(msg)
            print("数据包长度->"..len)
            --设置读取状态为读取包体数据
            msgtype = 1;
        else
            --print("数据包内容->"..msg)
            --调用解包函数
            --unpacket(msg)
            --unpacketpbc(msg)
            unpbc4b(msg)
            --重置读取状态为读取包头长度
            msgtype = 0; 
            len = 4
        end
    end

    if status == "closed" then 
        --服务器关闭！
        print("server closed!!")
        client:close() 
        client = nil
    end
    
end

--解包
function unpacket(body)
    string.sub(packet, 2)
end

--打包
function packet(body)
  local size = #body + 4
  return serialize_int32_little_endian(size) .. body
end

--关闭当前连接
function close()
    if client == nil then return end
    client:close() 
end

--清理数据
function clear()
    close()
end

--//////////////////////////////////// end tcp socket function /////////////////////////////////////////////

--///////////////////////////////////////////// Behaviour Call ///////////////////////////////////////////// 
function start()

	print("TcpClinet start...")
    --register()

    --connect()

    --testPbc()
    --testPbc2()

    --发送测试消息！
    --send("GET \n")
end

function update()
	--消息接受循环
    receive() 
end

function ondestroy()
    clear()
    print("LuaSocketBehaviour destroy")
end

--///////////////////////////////////////// end Behaviour Call ///////////////////////////////////////////// 

--///////////////////////////////////////////// pbc function ///////////////////////////////////////////////

--发送接口
function sendmsg(msgKey,data)
    --print("send len->".. pbtable[msgKey])
    --编码
    pbbody = protobuf.encode(pbtable[msgKey], data )
    --解包
    local  unpacketdata = protobuf.decode(pbtable[msgKey],pbbody)
    --加id
    packet = msgKey..pbbody
    --包长度
    print("sendmsg len->".. #packet)
    --send( serialize_int32_little_endian(#packet) .. packet )
    --[[
    local msg =  serialize_int32_big_endian(#packet) .. packet 
    local barr = ''
    for i=1,#msg do
        print("byte "..i..' = '..string.byte(msg, i))
        barr = barr..string.byte(msg, i)..","
    end
    ]]
    --print("send:"..barr.." >> len:"..#msg)

    send( serialize_int32_big_endian(#packet) .. packet )
end

--解包方法 pbc 4 字节
function unpbc4b(packet)

    local key0 = string.byte(packet, 1)
    local key1 = string.byte(packet, 2)
    local key2 = string.byte(packet, 3)
    local key3 = string.byte(packet, 4)

    print("key0:"..key0.." >> key1:"..key1.." >> key2:"..key2.." >> key3:"..key3)

    --4字节标识
    local msgKey = string.char(key0,key1,key2,key3)
    -- string 下标从 1 开始
    pbbody = string.sub(packet, 5)

    print(pbtable[msgKey] )

    local decode = protobuf.decode(pbtable[msgKey] , pbbody);
    --分发消息
    this.msm.Send(msgKey,decode)
end

function testPbc2()

    --local protobuf = require 'protobuf' 

    buffer = CS.UnityEngine.Resources.Load('proto/addressbook.pb').bytes

    protobuf.register(buffer)

    t = protobuf.decode("google.protobuf.FileDescriptorSet", buffer)

    proto = t.file[1]

    print(proto.name)
    print(proto.package)

    message = proto.message_type

    for _,v in ipairs(message) do
        print(v.name)
        for _,v in ipairs(v.field) do
            print("\t".. v.name .. " ["..v.number.."] " .. v.label)
        end
    end

    addressbook = {
        name = "Alice",
        id = 12345,
        phone = {
            { number = "1301234567" },
            { number = "87654321", type = "WORK" },
        }
    }

    code = protobuf.encode("tutorial.Person", addressbook)

    decode = protobuf.decode("tutorial.Person" , code)

    print(decode.name)
    print(decode.id)
    
    for _,v in ipairs(decode.phone) do
        print("\t"..v.number, v.type)
    end

    phonebuf = protobuf.pack("tutorial.Person.PhoneNumber number","87654321")
    buffer = protobuf.pack("tutorial.Person name id phone", "Alice", 123, { phonebuf })
    print(protobuf.unpack("tutorial.Person name id phone", buffer))

    --发送测试！
    send(packetpbc(1,2,code));
end

function testPbc()

    local protobuf = require 'protobuf' 
    protobuf.register(CS.UnityEngine.Resources.Load('proto/UserInfo.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/User.pb').bytes)

    local userInfo = {}
    userInfo.name = 'world'
    userInfo.diamond = 998
    userInfo.level = 100

    local user = {}
    user.id = 1
    user.status = {1,0,2,4}
    user.pwdMd5 = 'md5'
    user.regTime = '2017-03-29 12:00:00'
    user.info = userInfo

    -- 序列化
    local encode = protobuf.encode('User', user)

    -- 反序列化
    local user_decode = protobuf.decode('User', encode)

    assert(user.id == user_decode.id and user.info.diamond == user_decode.info.diamond)
        
    print('name', user_decode.info.name)
    print('diamond', user_decode.info.diamond)

end

--pbc 注册方法
function register()
    --[[
    protobuf.register(CS.UnityEngine.Resources.Load('proto/UserInfo.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/User.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/addressbook.pb').bytes)

    pbtable [string.char(1, 2)] = "tutorial.Person"
    ]]
end

--解包方法
function unpacketpbc(packet)

    local key = string.byte(packet, 1)
    local code = string.byte(packet, 2)

    local msgKey = string.char(key,code)

    print("key = "..key..",code = "..code)
    -- string 下标从 1 开始
    pbbody = string.sub(packet, 3)

    --pbbody = packet 
    --print("pbbody = "..pbbody)

    -- 反序列化
    --decode = protobuf.decode("tutorial.Person" , pbbody)
    --decode = protobuf.decode(pbtable[string.char(1, 2)] , pbbody);
    local decode = protobuf.decode(pbtable[msgKey] , pbbody);

    --print(protobuf.unpack("tutorial.Person name id phone", decode))

    --for key, v in pairs(decode) do      
    --    print(v)
    --end  

    --print(decode.name)
    --print(decode.id)
    --print(decode.email)

    --[[
    for _,v in ipairs(decode.phone) do
        print("\t"..v.number, v.type)
    end
    ]]
    --分发消息
    this.msm.Send(msgKey,decode)

end 

--打包方法
function packetpbc(key,code,pbbody)
    
    packet = key..code..pbbody

    print("send len->".. #packet)

    return serialize_int32_little_endian(#packet) .. packet
end 

function addlistener(msgcmd,fun)
    this.msm.AddListener(msgcmd,fun)
end 

function removelistener(msgcmd,fun)
    this.msm.RemoveListener(msgcmd,fun)
end 

--///////////////////////////////////////// end pbc function ///////////////////////////////////////////////
