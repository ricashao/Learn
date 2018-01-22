using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class BytesBuffer
{
    /// <summary>
    /// 游标
    /// </summary>
	public int position ;//{ get; private set; }

    private byte[] _byteArray;
    public BytesBuffer(byte[] buf,int capacity)
    {
        _byteArray = new Byte[capacity];
		Array.Copy (buf, _byteArray, capacity);
    }

	public BytesBuffer(int capacity){
		_byteArray = new Byte[capacity];
	}

	public BytesBuffer(byte[] byteArray){
		_byteArray = byteArray;
	}
	public BytesBuffer(){
	}
    public byte[] bytes
    {
		set { _byteArray = value;}
        get { return _byteArray; }
    }

    public void resetPosition()
    {//准备被读取
        position = 0;
    }
    public int capacity
    {//容量
        get { return _byteArray.Length; }
    }
    public int readableLength
    {//可读长度
        get { return position; }
    }
    private void Add(byte value)
    {
        if (position == _byteArray.Length)
            throw new Exception("out byte buffer.capacity" + capacity);
        else
            _byteArray[position] = value;
        position++;
    }
    private void AddRange(byte[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            Add(values[i]);
        }
    }
    public void writeByte(byte value)
    {
        Add(value);
    }
    public void writeBool(bool value)
    {
        AddRange(BitConverter.GetBytes(value));
    }
    public void writeShort(short value)
    {
        AddRange(BitConverter.GetBytes(value));
    }
    public void writeInt32(int value)
    {
        AddRange(BitConverter.GetBytes(value));
    }
    public void writeUint(uint value)
    {
        AddRange(BitConverter.GetBytes(value));
    }
    public void writeUint64(ulong value)
    {
        AddRange(BitConverter.GetBytes(value));
    }
    public void writeLong(long value)
    {
        AddRange(BitConverter.GetBytes(value));
    }
    public void writeFloat(float value)
    {
        AddRange(BitConverter.GetBytes(value));
    }
    public void writeString(string value)
    {
        byte[] temp = Encoding.UTF8.GetBytes(value);
        writeShort((short)temp.Length);//字符长度
        if (temp.Length > 0)
            AddRange(temp);
    }
    public void writeString(string value, int size)
    {
        if (size == 0)
            return;
        byte[] temp = Encoding.UTF8.GetBytes(value);
        if (temp.Length > size)
            throw new Exception("字符从长度和size不匹配");
        AddRange(temp);
        //填充不足
        int fillNum = size - temp.Length;
        if (fillNum > 0)
        {
            AddRange(new byte[fillNum]);
        }
    }
    public void writeBytes(byte[] byteArray, int startIndex, int length)
    {
        for (int i = startIndex; i < length; i++)
            Add(byteArray[i]);
    }
    public void read(byte[] bytes, int startIndex, int length)
    {
        int index = 0;
        for (int i = startIndex; i < length; i++)
        {
            bytes[i] = _byteArray[position + index++];
        }
        position += length - startIndex;
    }
    public byte readByte()
    {
        byte value = _byteArray[position];
        position += 1;
        return value;
    }
    public short readShort()
    {
        short value = BitConverter.ToInt16(_byteArray, position);
        position += 2;
        return value;
    }
    public int readInt32()
    {
        int value = BitConverter.ToInt32(_byteArray, position);
        position += 4;
        //GWDebug.Log("readInt32:" + value);
        return value;
    }
    public float readFloat()
    {
        float value = BitConverter.ToSingle(_byteArray, position);
        position += 4;
        return value;
    }

    public UInt32 readUInt32()
    {
        UInt32 value = BitConverter.ToUInt32(_byteArray, position);
        position += 4;
        return value;
    }
    public long readLong()
    {
        long value = BitConverter.ToInt64(_byteArray, position);
        position += 8;
        return value;
    }

    public ulong readQWORD()
    {
        ulong value = BitConverter.ToUInt64(_byteArray, position);
        position += 8;
        return value;
    }

    public bool readBool()
    {
        bool value = BitConverter.ToBoolean(_byteArray, position);
        position += 1;
        return value;
    }

    public string readString(int len)
    {
        string str = Encoding.UTF8.GetString(_byteArray, position, len);
        position += len;
        //GWDebug.Log("readString:" + str);
        return str.Trim('\0');
    }
    public String readString()
    {
        short len = readShort();
        //GWDebug.Log("readString len:"+len);
        if (len == 0)
            return "";
        else
            return readString(len);
    }
    public String readString16()
    {
        return readString(16);
    }
    public String readString32()
    {
        return readString(32);
    }
    public String readString64()
    {
        return readString(64);
    }
    public String readString128()
    {
        return readString(128);
    }
    public String readString256()
    {
        return readString(256);
    }
    public String readString512()
    {
        return readString(512);
    }
    public String readString1024()
    {
        return readString(1024);
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void reset()
    {
        position = 0;
    }
    /// <summary>
    /// 清除数据
    /// </summary>
    public void clear()
    {
        Array.Clear(_byteArray, 0, _byteArray.Length);
        position = 0;
    }
}

