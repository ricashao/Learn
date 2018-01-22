using System;

public class BaseCmd{
	
	public byte Cmd;			//协议号
	public byte Para;			//协议号-小
    public uint dwTime;
	public bool Stackable = true;
	public BytesBuffer sendBuffer;

	public BytesBuffer getBuffer {
		get{
			return sendBuffer;
		}
	}

	public BaseCmd(){

	}

	byte [] k = new byte[2]; 
	ushort key ;

	public void setKey(){
		k [0] = Cmd;
		k [1] = Para;
		key = BitConverter.ToUInt16( k,0);
	}

	public ushort getKey(){
		//byte[] bs =  BitConverter.GetBytes (key);
		return key;
	}

	public virtual int getSize(){
		//return sizeof(byte) + sizeof(byte) + sizeof(uint);
		return sendBuffer.position;
	}

	public virtual void serialize(){

		sendBuffer.resetPosition ();
		sendBuffer.position = 4;
        sendBuffer.writeByte(Cmd);
        sendBuffer.writeByte(Para);
        //sendBuffer.writeUint(dwTime);
	}

    public virtual void unserialize()
    {
		//dwTime = sendBuffer.readUInt32 ();
        //dwTime = sendBuffer.readUInt32();
    }

    public void Send()
    {
		if (sendBuffer == null) {
			//typeof(this);// this.GetType ();
			sendBuffer = new BytesBuffer (512);
		}

		serialize ();

        NetSystem.getInstance().sendCmd(this);
    }

    public void Send(int _lock_cmd)
    {
        NetSystem.getInstance().sendCmd_And_Lock(this, _lock_cmd);
    }

	public virtual BaseCmd Clone(){

		return null;
	}

}