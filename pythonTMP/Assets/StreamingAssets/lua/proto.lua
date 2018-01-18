local protobuf = require 'protobuf'
--pbc 注册方法
function pbcRegister()

    protobuf.register(CS.UnityEngine.Resources.Load('proto/UserInfo.pb').bytes)
    protobuf.register(CS.UnityEngine.Resources.Load('proto/User.pb').bytes)

end

function onMsg()

end


function getMsg()

end