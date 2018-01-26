--游戏定义
local define =
{
	--host = "127.0.0.1",
	--port = 9888,
	--host = "192.168.8.243",
	--port = 3015,
	host = "192.168.8.228",
	port = 10005,

	E_EmailStatus=
	{
		ES_RECEIVE = "ES_RECEIVE",
		ES_READ    = "ES_READ",
		ES_REWARD  = "ES_REWARD",
		ES_DISCARD = "ES_DISCARD",
	},
	E_EmailType=
	{
	ET_SYSTEM  = "ET_SYSTEM",-- 系统
	ET_SERVICE = "ET_SERVICE", -- 客服
	},

}



return define