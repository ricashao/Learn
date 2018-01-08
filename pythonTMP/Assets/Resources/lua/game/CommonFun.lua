
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

