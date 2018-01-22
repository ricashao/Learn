using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//NetSystem.getInstance ().testZIP ();

		NetSystem.getInstance ().Register (new Msg_SC_Data());
		NetSystem.getInstance ().SetOnCmdCallBack (OnCmd);
	}

	public void OnCmd(BaseCmd cmd ){

		if (cmd.Cmd == 0) {
			if (cmd.Para == 1) {
				Msg_SC_Data msg_SC_Data = (Msg_SC_Data)cmd;
				Debug.LogFormat ("Data Cmd:{0} Para:{1} ->>{2} ,{3}, {4}",cmd.Cmd,cmd.Para, 
					msg_SC_Data._dataUint,msg_SC_Data._dataInt, msg_SC_Data._dataString);
			}
		}

	}

	public void OnConnect(ConnectState state){
		if (state == ConnectState.STATE_CONNECTED) {
			//Debug.Log ("连接服务器成功！");
		}
		else if (state == ConnectState.STATE_OUTLINE) {
			//Debug.Log ("连接关闭成功！");
		}
		else{
			//Debug.LogError("连接服务器超时！");
		}
	}

	Msg_CS_Data msg_CS_Data = new Msg_CS_Data ();

	void SendTest(){
		
		msg_CS_Data._dataUint = 3;
		msg_CS_Data._dataInt = 2;
		msg_CS_Data._dataString = "";
		//for(int i = 0 ;i< 512 / 4;i++){
		for(int i = 0 ;i< 5;i++){
			msg_CS_Data._dataString += "1024";
		}
		if (msg_CS_Data.getBuffer != null) {
			msg_CS_Data.getBuffer.resetPosition ();
			//Debug.Log (msg_CS_Data.getBuffer.position);
		}
		msg_CS_Data.Send();
	}

	bool isSend = false;

	void Update(){
		if (isSend) {
			SendTest ();
		}
	}

	// Update is called once per frame
	void OnGUI () {

		if (GUI.Button (new Rect (0, 0, 78, 24), "connect")) {
			NetSystem.getInstance ().Connect ("127.0.0.1",30000,OnConnect);
		}

		if (GUI.Button (new Rect (78, 0, 78, 24), "close")) {
			NetSystem.getInstance ().Close ();
		}

		if(GUI.Button(new Rect(0,24,78,24),"send->")){

			SendTest ();
		}

		if(GUI.Button(new Rect(0,24 * 2,78,24),"start send->")){

			isSend = true;
		}

		if(GUI.Button(new Rect(0,24 * 3,78,24),"stop send->")){

			isSend = false;
		}
	}
}
