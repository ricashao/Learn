//*******************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MessageEventsMgr : MonoBehaviour{

	private static MessageEventsMgr instance;
	public static MessageEventsMgr getInstance()
    {
        return instance;
    }

    public void Awake()
    {
        instance = this;
    }

    public int GET_MESSAGE_KEY(byte cmd,byte para){
		return (int)(cmd * 1000 + para);
	}
     
	public int MAX_MESSAGE_QUEUE_LENGTH = 65535;		//最大的消息队列长度
	public int MESSAGE_COUNT = 0;
	private bool m_isPacketProcessing = false;		//用于保证消息顺序执行的开关

    //公共委托事件
    public delegate void CommonEvent(byte cmd,byte para,object data);

    //事件列表
    public Dictionary<int, CommonEvent> m_dicEvents = new Dictionary<int, CommonEvent>();

	//消息处理队列
	public Queue<BytesBuffer> m_messageQueue = new Queue<BytesBuffer>();

    bool IsSendHeartCmd = false;
    float HeartSendTime = 0;
    float HeartBegin = 0;
    public float NetDelay = 0f;
    int HeartCount = 0;

    public void BeginSendHeart()
    {
        IsSendHeartCmd = true;
        HeartSendTime = Time.time;
        HeartCount = 0;
    }

    void SendHeartCmd()
    {
        if(Time.time - HeartSendTime > 5f)
        {
            if(HeartCount > 3)
            {
                //心跳超过3次没有回应 断线
                NetSystem.getInstance().Close();
                return;
            }
			/*
            userSceneHeartCmd send = new userSceneHeartCmd();
            send.serialize();
            send.SendCmd();
            */
            HeartSendTime = Time.time;
            HeartBegin = Time.time;
            HeartCount++;
        }
    }

    public void CloseHeart()
    {
        IsSendHeartCmd = false;
    }

    void RecvHeartCmd()
    {
        NetDelay = Time.time - HeartBegin;
        HeartCount--;
    }

    public bool dispatchPacket(BytesBuffer _buffer){
		MESSAGE_COUNT = m_messageQueue.Count;
		if (MESSAGE_COUNT > MAX_MESSAGE_QUEUE_LENGTH) {
			Debug.LogError ("超出最大消息队列长度");
            return false;
		}

		//将数据插入消息队列
		m_messageQueue.Enqueue(_buffer);

		//processPacket ();

		return true;
	}

    public void Update()
    {
        if (IsSendHeartCmd)
        {
            SendHeartCmd();
        }
        processPacket();

        /*if (Input.GetKeyDown(KeyCode.P))
        {
            NetSystem.getInstance()._close();
        }*/
    }

    private void processPacket(){
		if (!NetSystem.getInstance().m_isConnected) {
			//如果链接已经关闭，则不处理之后的消息
			return;
		}

		if (m_messageQueue.Count <= 0) {
			return;
		}

		if (m_isPacketProcessing) {
			Debug.LogError ("消息出现阻塞");
			return;
		}

		m_isPacketProcessing = true;
        int _msgCount = m_messageQueue.Count;
        //一次将消息队列中堆积的消息都处理掉
        for(int i = 0; i < _msgCount; ++i)
        {
            DispenseMsg();
        }

        m_isPacketProcessing = false;
        MESSAGE_COUNT = m_messageQueue.Count;
        //回调执行下一条消息
        //processPacket();
    }

    void DispenseMsg()
    {
        //解析消息的协议头
        BytesBuffer _msgBuffer = m_messageQueue.Dequeue();
        if (_msgBuffer.bytes.Length < 4)
        {
            m_isPacketProcessing = false;
            MESSAGE_COUNT = m_messageQueue.Count;

            Debug.LogError("收到错误的协议,抛弃");
            return;
        }
        byte _cmd = _msgBuffer.readByte();
        byte _para = _msgBuffer.readByte();
        uint _dwTime = _msgBuffer.readUInt32();

        Debug.Log("[RECV]cmd :" + _cmd.ToString() + "   para : " + _para.ToString());

        if(_cmd == 2 && _para == 15)
        {
            //心跳协议 不需要处理
            RecvHeartCmd();
            return;
        }


        if (NetSystem.getInstance().GetSendLock)
        {
            //发送加锁 需要判断收到的消息是否可以进行解锁
            if (GET_MESSAGE_KEY(_cmd, _para) == NetSystem.getInstance().GetUnLockKey)
            {
                NetSystem.getInstance().sendUnLock();
            }
        }

        StartCoroutine(TriigerEvent(GET_MESSAGE_KEY(_cmd, _para), _cmd, _para, _msgBuffer));
    }

    /// <summary>
    ///  添加一个回调事件
    /// </summary>
	private void AddDelegate(int key,CommonEvent attachEvent)
    {
        //Debug.Log(" 添加一个回调 : "+ key);
		m_dicEvents.Add(key,attachEvent);
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    public IEnumerator TriigerEvent(int key, byte cmd ,byte para,object param)
    {
        if (m_dicEvents.ContainsKey(key))
        {
            m_dicEvents[key](cmd, para,param);
        }
        else
        {
            Debug.LogError("没有这个定义的事件:" + key);
        }

        yield return null;
    }
    /// <summary>
    /// 事件绑定
    /// </summary>
    /// <param name="cmd">大消息号</param>
    /// <param name="para">小消息号</param>
    /// <param name="attachEvent">回调</param>
    public void AttachEvent(byte cmd, byte para,CommonEvent attachEvent) {
        AttachEvent(GET_MESSAGE_KEY(cmd, para), attachEvent);
    }
    /// <summary>
    /// 事件绑定
    /// </summary>
    public void AttachEvent(int strEventKey, CommonEvent attachEvent)
    {
        if (m_dicEvents.ContainsKey(strEventKey))
        {
            //Debug.Log(" 事件绑定 : " + strEventKey);
            //m_dicEvents[strEventKey] += attachEvent;
			Debug.LogWarning("已经有相同的消息事件:" + strEventKey);
        }
        else
        {
			AddDelegate (strEventKey, attachEvent);
        }
    }

    public void Clear()
    {
        m_messageQueue.Clear();
        m_dicEvents.Clear();
    }

    /// <summary>
    /// 去除事件绑定
    /// </summary>
    public void DetachEvent(int strEventKey, CommonEvent attachEvent)
    {
        if (m_dicEvents.ContainsKey(strEventKey))
        {
            //Debug.Log("去除事件绑定 : " + strEventKey);
            m_dicEvents[strEventKey] -= attachEvent;
        }
        else
        {
            Debug.LogError("没有这个定义的事件:" + strEventKey);
        }
    }
}
