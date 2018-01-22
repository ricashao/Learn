using System.Collections;
using System.Collections.Generic;

public class Msg_CS_Data : BaseCmd {

	public uint _dataUint;
	public int _dataInt;
	public string _dataString;

	public Msg_CS_Data(uint dataUint = 0,int dataInt =0,string dataString =""){
		
		Cmd = 0;
		Para = 1;

		_dataUint = dataUint;
		_dataInt = dataInt;
		_dataString = dataString;
	}
		
	public override void serialize(){
		
		base.serialize ();

		sendBuffer.writeUint (_dataUint);
		sendBuffer.writeInt32 (_dataInt);
		sendBuffer.writeString (_dataString);
	}
}

public class Msg_SC_Data : BaseCmd {

	public uint _dataUint;
	public int _dataInt;
	public string _dataString;

	public Msg_SC_Data(){

		Stackable = false;

		Cmd = 0;
		Para = 1;
		setKey ();
	}

	public override void unserialize(){
		base.unserialize ();
		_dataUint = sendBuffer.readUInt32();
		_dataInt = sendBuffer.readInt32();
		_dataString = sendBuffer.readString();
	}

	public override BaseCmd Clone(){
		Msg_SC_Data msg_SC_Data = new Msg_SC_Data ();
		msg_SC_Data._dataUint = _dataUint;
		msg_SC_Data._dataInt = _dataInt;
		msg_SC_Data._dataString = _dataString;
		return msg_SC_Data;
	}
}
