using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Text;
using System.Linq;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Threading;
using zlib;
using UnityEngine.SceneManagement;
/*
 * 基于 TcpClient
 * https://msdn.microsoft.com/zh-cn/library/system.net.sockets.tcpclient(v=vs.110).aspx
*/
//连接状态
public enum ConnectState
{
	STATE_NONE,         //默认
	STATE_CONNECT,      //连接中
	STATE_ERROR,        //连接出错
	STATE_CONNECTED,    //已连接
	STATE_OUTLINE       //掉线
}

public class NetSystem : MonoBehaviour
{
	static int DF_PORT = 30000;
	static int MAX_RECV_BUF_LENGTH = 512;
	//static int MAX_IOBUFFER_LENGTH = 8 * 1024 * 16;

    ConnectState NetState = ConnectState.STATE_NONE;

    TcpClient m_ClientSocket = null;

	int m_recvBufferIndex = 0;
	Byte[] m_recvData = new byte[MAX_RECV_BUF_LENGTH];

	int m_curPacketLength = 0;

	//int m_streamReadStartIndex = 0;
	int m_available = 0;

	int m_sendUnLockKey = 0;			//发送消息锁的解锁协议号
	bool m_sendLock = false;			//发送消息锁

	public string m_ipAddr = "";			//当前连接的服务器ip
	public int    m_port = 0;				//当前连接的服务器端口
	public bool m_isConnected = false;

	public Int32 m_head_length = 4;

	NetworkStream m_netStream;

	private static NetSystem instance;
	public static NetSystem getInstance()
	{
		if (instance == null)
		{
			GameObject gameObject = new GameObject("NetSystem");
			DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<NetSystem>();
		}
		return instance;
	}

    public void Awake()
    {
        //instance = this;
    }

    public bool GetSendLock {
		get{ return m_sendLock;}
	}

	public int GetUnLockKey {
		get { return m_sendUnLockKey;}
	}

	public void sendUnLock(){
		//解除发送消息锁
		m_sendLock = false;
		m_sendUnLockKey = 0;
	}

	//接收函数线程
	Thread m_receiviceThread = null;
    public delegate void CommonEvent(IAsyncResult iar);
	public delegate void ConnectCallBack(ConnectState state);
	public delegate void OnCmd(BaseCmd cmd );

	ConnectCallBack _connectCallBack;
	OnCmd _onCmd;

	public void SetOnCmdCallBack(OnCmd onCmd){
		_onCmd += onCmd;
	}

	public void Connect(string ipAddr,int port, ConnectCallBack connectCallBack){
		_connectCallBack = null;
		_connectCallBack += connectCallBack;
		Connect (ipAddr,port);
	}

	public void ReConnect(){
		Connect (m_ipAddr,m_port);
	}

	public void Connect(string ipAddr){
		Connect (ipAddr,DF_PORT);
	}

    public void Connect(string ipAddr,int port)
	{
		m_ipAddr = ipAddr;
		m_port = port;

		try
		{
			Debug.LogFormat("开始连接服务器>>> ipAddr:{0} port:{1}",ipAddr,port);

            m_ClientSocket = new TcpClient(AddressFamily.InterNetwork);

			//IPAddress ipAddress = IPAddress.Parse(ipAddr);
			//IPEndPoint ipEndPoint = new IPEndPoint(ipAddress,port);
			IAsyncResult result = m_ClientSocket.BeginConnect(ipAddr, port, new AsyncCallback(_onConnect_Sucess),m_ClientSocket);
            NetState = ConnectState.STATE_CONNECT;
            bool sucess = result.AsyncWaitHandle.WaitOne(5000,true);
            m_ClientSocket.NoDelay = true;
            m_ClientSocket.SendTimeout = 5000;
            m_ClientSocket.ReceiveTimeout = 5000;

			if(!sucess){
				//连接超时  
				_onConnect_outTime(null);
            }
		}
		catch(System.Exception _e){
			Debug.Log ("_onConnect catch " + _e.ToString ());
			_onConnect_Fail ();
		}
	}

    public bool IsConnected { get { return m_ClientSocket != null  && m_ClientSocket.Connected; } }

    public void _onBeginReceive(){
		//m_receiviceThread = new Thread (new ThreadStart (_onReceiveSocket));
		//m_receiviceThread.Start ();

		_onReceiveSocket ();
	}

    public long Ping()
    {
        System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping();
        System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send(m_ipAddr);
        return temPingReply.RoundtripTime;
    }

	public bool isConnected(){
		return NetState == ConnectState.STATE_CONNECTED;
	}

    //连接成功
    public void _onConnect_Sucess(IAsyncResult iar){
        TcpClient so = (TcpClient)iar.AsyncState;

		if (so.Connected) {
			m_isConnected = true;
			Debug.LogFormat("连接服务器成功 {0}",DateTime.Now);

			NetState = ConnectState.STATE_CONNECTED;

			_onBeginReceive ();	//启动接收消息线程
			//return true;

		} else {
			Debug.Log ("连接服务器失败");
			NetState = ConnectState.STATE_ERROR;
		}

		if (_connectCallBack != null)
			_connectCallBack (NetState);
       // return false;
	}

	//连接超时
	public void _onConnect_outTime(IAsyncResult iar){
		Debug.LogFormat ("连接服务器超时 {0}",DateTime.Now);
        NetState = ConnectState.STATE_ERROR;
		if (_connectCallBack!= null)
			_connectCallBack (NetState);
		
    }

	//连接失败
	public void _onConnect_Fail(){
		Debug.LogFormat ("连接服务器错误 {0}",DateTime.Now);
        NetState = ConnectState.STATE_ERROR;

		if (_connectCallBack!= null)
			_connectCallBack (NetState);
    }

	//服务器断开连接
	public void _onServerClose(){
		Debug.LogFormat ("连接关闭！{0}",DateTime.Now);
		Close ();
        NetState = ConnectState.STATE_OUTLINE;

		if (_connectCallBack!= null)
			_connectCallBack (NetState);
	}

	//关闭连接
	public void Close(){
		if (!m_isConnected) {
			return;
		}

		m_isConnected = false;  
        NetState = ConnectState.STATE_OUTLINE;
        if (m_ClientSocket != null && m_ClientSocket.Connected) {
			m_ClientSocket.Close ();
			m_ClientSocket = null;
		}

		if (m_receiviceThread != null) {
			m_receiviceThread.Abort ();
			m_receiviceThread = null;
		}
		//清空消息队列
		m_recvBufferIndex = 0;

		Debug.LogFormat ("连接关闭成功！{0}",DateTime.Now);
	
		if (_connectCallBack!= null)
			_connectCallBack (NetState);
	}

	AsyncCallback m_ReceiveAsyncCallback;
	//接收线程函数
 	void _onReceiveSocket(){
		if(m_ReceiveAsyncCallback == null)
			m_ReceiveAsyncCallback = new AsyncCallback (Receive);
		m_ClientSocket.Client.BeginReceive( m_recvData,
			m_recvBufferIndex, m_recvData.Length - m_recvBufferIndex,
											SocketFlags.None,m_ReceiveAsyncCallback , m_ClientSocket);
	}

    private void Receive(IAsyncResult ar)
    {
		if (!m_ClientSocket.Connected) {
			//重新连接
			_onServerClose ();
		} 
        try
        {
			//m_ClientSocket.Client.Available
			m_available = m_recvBufferIndex + m_ClientSocket.GetStream().EndRead(ar);
			//m_available = m_ClientSocket.Available;
            //TcpClient tempTcpClient = (TcpClient)ar.AsyncState;
			if(m_available == 0){
				_onServerClose();
				return;
			}
			//Buffer.BlockCopy(m_break,0,m_recvData,0,m_streamReadStartIndex);
			//m_available += m_streamReadStartIndex;

			m_recvBufferIndex = 0;

			int i=0;

			do{
				//获取已经从网络接收且可供读取的数据量 如果小于包头字节长度 本次跳过 m_available < 4
				if(m_available < m_head_length){

					if(i > 0){
						for(int j = 0;i<m_available;j++){
							m_recvData[j] = m_recvData[m_recvBufferIndex+j];
						}
						m_recvBufferIndex = 0;
					}else{
						//Buffer.BlockCopy(m_recvData,m_streamReadStartIndex, m_break ,0,m_available);
						m_recvBufferIndex = m_available;
					}
					//m_ClientSocket.Client.BeginReceive(m_recvData, 0, m_recvData.Length, SocketFlags.None, m_ReceiveAsyncCallback, m_ClientSocket);
					_onReceiveSocket ();

					return;
				}else{
				//if(m_curPacketLength == 0){
					//m_streamRead 字节数组的当前下标 m_streamReadStartIndex 
					//m_head_length 包头长度 4 字节 
					//m_netStream.Read(m_streamRead, m_streamReadStartIndex, m_head_length);
					m_curPacketLength = (int) BitConverter.ToUInt32 (m_recvData, m_recvBufferIndex);
					//移动下标
					//m_streamReadStartIndex += m_head_length;
					//m_available -= m_head_length;

					if(m_available < m_head_length + m_curPacketLength){

						m_recvBufferIndex = m_available;

						_onReceiveSocket ();
						return;
					}else{

						m_recvBufferIndex += m_head_length;

						m_available -= m_head_length;

						//m_netStream.Read(m_streamRead, m_streamReadStartIndex, m_curPacketLength);
						OnPacket();

						m_recvBufferIndex += m_curPacketLength;
						//m_streamReadStartIndex = 0;
						m_available -= m_curPacketLength;

						m_curPacketLength = 0;
						//return;
					}
				}
			
				i++;
			
			}while(true);

        }
        catch (Exception e)
        {
			m_netStream.Close ();
            _onServerClose();
            //Debug.LogError(e.ToString());
        }
    }

	ushort m_cmdkey;
	BaseCmd m_Cur_BaseCmd;
	byte [] keyByteArr = new byte[2];
	BytesBuffer bytesBuffer = new BytesBuffer();

	void OnPacket(){

		bytesBuffer.bytes = m_recvData;

		keyByteArr[0] = m_recvData[m_recvBufferIndex];
		keyByteArr[1] = m_recvData[m_recvBufferIndex+1];

		if (keyByteArr [0] == 1 && keyByteArr [1] == 2) {
			CheckTimeDelayReturn ();
		}

		m_cmdkey = BitConverter.ToUInt16(keyByteArr,0);

		m_Cur_BaseCmd = null;
		m_Cmd_Dic.TryGetValue (m_cmdkey,out m_Cur_BaseCmd);

		if (m_Cur_BaseCmd != null) {
			
			bytesBuffer.position = m_recvBufferIndex+2;
			m_Cur_BaseCmd.sendBuffer = bytesBuffer;

			m_Cur_BaseCmd.unserialize ();

			if (m_Cur_BaseCmd.Stackable)
				m_BaseCmd_Queue.Enqueue (m_Cur_BaseCmd.Clone ());
			else if (_onCmd != null) {
				_onCmd (m_Cur_BaseCmd);
			}
			//Debug.LogWarningFormat ("recv cmd:{0} para:{1} Stackable:{2}",keyByteArr[0],keyByteArr[1],m_Cur_BaseCmd.Stackable);	
		}
		//Debug.LogError(m_Cur_BaseCmd.ToString());
	}

	Queue<BaseCmd> m_BaseCmd_Queue = new Queue<BaseCmd>();
	Dictionary<ushort,BaseCmd> m_Cmd_Dic = new  Dictionary<ushort,BaseCmd>(); 

	public void Register(BaseCmd baseCmd){
		m_Cmd_Dic.Add (baseCmd.getKey(),baseCmd);
	}
	/*
    //分割消息
    public bool TcpDataSplt(Byte[] _data,int _size){
		if (m_recvBufferIndex == 0) {
			//没有剩余的数据
			System.Buffer.BlockCopy(_data,0,m_ioBuffer,0,_size);
			int readLenght = 0;

			readLenght = parsePacket (_size);

			if (readLenght < _size) {
				//消息包不全
				int leftLength = _size - readLenght;
				System.Buffer.BlockCopy (_data, readLenght, m_ioBuffer, 0, leftLength);
				m_recvBufferIndex += leftLength;
			} 
			else if (readLenght > _size) {
				return false;
			}
				
			return true;
		}

		//拼接之前不完整的数据
		System.Buffer.BlockCopy(_data,0,m_ioBuffer,m_recvBufferIndex,_size);
		m_recvBufferIndex += _size;
		int readLength = parsePacket (m_recvBufferIndex);
		if (readLength == 0) {
			return true;
		} 
		else if (readLength > m_recvBufferIndex) {
			return false;
		}
		//移走已经读取的buff
		System.Buffer.BlockCopy(m_ioBuffer,m_recvBufferIndex,m_ioBuffer,0,readLength);
		m_recvBufferIndex -= readLength;

		return true;
	}

	//消息解析
	public int parsePacket(int _recv_length){

		int readLenght = 0;
		while (true) {
			if (readLenght + 4 >= _recv_length) {
				return readLenght;
			}

			UInt32 packetLength = BitConverter.ToUInt32 (m_ioBuffer, readLenght);
			readLenght += 4;
			byte[] tmpBuf = new byte[1024 * 8 * 16];
			bool hasCompress = (packetLength & 0x40000000) > 0;
			int iZipLen = 0;
			if (hasCompress) {
				//数据需要解压缩
				packetLength = packetLength ^ 0x40000000;

                byte[] inBuffer = new byte[packetLength];
				System.Buffer.BlockCopy (m_ioBuffer, readLenght, inBuffer, 0, (int)packetLength);
				byte[] zipData = Decompress (inBuffer);
				iZipLen = zipData.Length;
				System.Buffer.BlockCopy (zipData, 0, tmpBuf, 0, iZipLen);
			} 
			else {
				//int size = packetLength;
				System.Buffer.BlockCopy (m_ioBuffer, readLenght, tmpBuf, 0, (int)packetLength);
				iZipLen = (int)packetLength;
			}

			readLenght += (int)packetLength;
			//分发消息
			BytesBuffer buffer = new BytesBuffer(tmpBuf,iZipLen);
			//MessageEventsMgr.getInstance ().dispatchPacket (buffer);
		}
	}
	*/
	AsyncCallback _sendAsyncCallback;
	//发送协议
	public bool sendCmd(BaseCmd _cmd){

		if (!m_isConnected) {
			Debug.LogErrorFormat ("当前未连接服务器! {0}",DateTime.Now);
		}

		if (_cmd.getSize() <= 0 || !m_isConnected || m_sendLock) {
            Debug.LogWarning("send field");
			return false;
		}

       // Debug.Log("[SEND] cmd : " + _cmd.Cmd.ToString() + " para :" + _cmd.Para.ToString() + " msgLength : " + _cmd.getSize().ToString());

		int _message_length = _cmd.getSize();
		//BytesBuffer _sendBuf = new BytesBuffer (_message_length);
		BytesBuffer _sendBuf = _cmd.sendBuffer;
		_sendBuf.resetPosition ();
		_sendBuf.writeInt32 (_message_length - 4);

		//System.Buffer.BlockCopy (_cmd.getBuffer.bytes, 0, _sendBuf.bytes, 4, _cmd.getSize());

		if(_sendAsyncCallback == null) _sendAsyncCallback = new AsyncCallback (_onSendMsg);
		//m_ClientSocket.Client.BeginSend (_sendBuf.bytes, 0, _sendBuf.bytes.Length, SocketFlags.None, _sendAsyncCallback, m_ClientSocket);

		m_ClientSocket.Client.Send (_sendBuf.bytes, 0, _message_length, SocketFlags.None);

		/* 断包测试 
		int bl = 5;
		m_ClientSocket.Client.Send (_sendBuf.bytes, 0, _sendBuf.bytes.Length - bl, SocketFlags.None);
		System.Threading.Thread.Sleep (1000);
		m_ClientSocket.Client.Send (_sendBuf.bytes, bl, bl, SocketFlags.None);
		///*/
		return true;
	}

	//发送协议并加锁，防止连续多次发送协议，必须收到指定协议之后才可继续发送协议
	public bool sendCmd_And_Lock(BaseCmd _cmd,int _unLockKey){
		
		if (_cmd.getSize() <= 0 || !m_isConnected || m_sendLock) {
			return false;
		}

		int _message_length = _cmd.getSize() + 4;
		BytesBuffer _sendBuf = new BytesBuffer (_message_length);

		_sendBuf.writeInt32 (_message_length - 4);

		System.Buffer.BlockCopy (_cmd.getBuffer.bytes, 0, _sendBuf.bytes, 4, _cmd.getSize());

		m_ClientSocket.Client.BeginSend (_sendBuf.bytes, 0, _sendBuf.bytes.Length, SocketFlags.None, new AsyncCallback (_onSendMsg), m_ClientSocket);

		m_sendUnLockKey = _unLockKey;
		m_sendLock = true;

		return true;
	}

	public void _onSendMsg(IAsyncResult iar){
		
	}

    private void Update()
    {
		if (m_ClientSocket == null) {
			Debug.LogError("服务器断开！");
		}

        if(NetState == ConnectState.STATE_OUTLINE)
        {
            //处于掉线状态 
            //UIManager.getInstance().m_messageBoxUI.Show(MessageBoxType.EN_OK, "与服务器断开连接，请点击按钮返回登陆界面！", TextAnchor.UpperLeft, "返回登陆", "", ReturnLogin);
            NetState = ConnectState.STATE_NONE;
            //GameMain.getInstance().m_SelfPlayer.joystick.enabledMove = false;
            //GameMain.getInstance().m_SelfPlayer.PLAYER_STIFFEN = true;
            //MessageEventsMgr.getInstance().CloseHeart();
        }

		if (_onCmd != null && m_BaseCmd_Queue.Count > 0) {
			//curBaseCmd = m_BaseCmd_Queue.Dequeue ();
			//_onCmd (curBaseCmd);
			_onCmd (m_BaseCmd_Queue.Dequeue());
			//Libs.PM.I.FreeOne(curBaseCmd);
		}
    }

    byte [] checkTimeDelayMsgBuff = new byte[6];
	int checkTimeDelayMsgBuffIndex = 4;
	int checkTimeDelayMsgLen = 2;
	byte [] checkTimeDelayMsgLenBytes = new byte[4];
	long checkTimeDelaySend;
	/* 样本数组 */
	long []checkTimeDelaySample = new long[12];
	/* 样本数组下标 */
	int checkTimeDelaySampleIndex = 0;

	bool checkTimeDelayLoop = false;

	public delegate void OnCheckTimeDelayCompleteMethod(); 
	public OnCheckTimeDelayCompleteMethod onCheckTimeDelayComplete;

	public void CheckTimeDelayRunOnce(OnCheckTimeDelayCompleteMethod onCheckTimeDelayCompletCallBack = null){
		onCheckTimeDelayComplete += onCheckTimeDelayCompletCallBack; 
		checkTimeDelayLoop = false;
		CheckTimeDelay ();
	}

	public void CheckTimeDelayRun(OnCheckTimeDelayCompleteMethod onCheckTimeDelayCompletCallBack = null){
		onCheckTimeDelayComplete += onCheckTimeDelayCompletCallBack; 
		checkTimeDelaySampleIndex = 0;
		checkTimeDelayLoop = true;
		CheckTimeDelay ();
	}

	public bool IsCheckTimeDelayComplete(){
		return checkTimeDelaySampleIndex == checkTimeDelaySample.Length;
	}

	void CheckTimeDelay(){
		
		if (m_ClientSocket != null){
			/* 数据包长度 **/
			checkTimeDelayMsgLenBytes = BitConverter.GetBytes (checkTimeDelayMsgLen);
			checkTimeDelayMsgBuff [0] = checkTimeDelayMsgLenBytes [0];
			checkTimeDelayMsgBuff [1] = checkTimeDelayMsgLenBytes [1];
			checkTimeDelayMsgBuff [2] = checkTimeDelayMsgLenBytes [2];
			checkTimeDelayMsgBuff [3] = checkTimeDelayMsgLenBytes [3];
			/* 数据类型表示，这个可以自定 **/
			checkTimeDelayMsgBuff [4] = 0; 
			checkTimeDelayMsgBuff [5] = 1; 

			m_ClientSocket.Client.Send (checkTimeDelayMsgBuff, 0, 6, SocketFlags.None);
			/* 保存发送时间戳（毫秒 **/
			checkTimeDelaySend = (DateTime.UtcNow.ToUniversalTime ().Ticks - 621355968000000000) / 10000;
   		}
   	}

	void CheckTimeDelayReturn(){
		/* 延时时间戳样本 当前时间戳（毫秒) - 发送时间戳（毫秒) **/
		checkTimeDelaySample [checkTimeDelaySampleIndex++] = (DateTime.UtcNow.ToUniversalTime ().Ticks - 621355968000000000) / 10000 - checkTimeDelaySend;
		if (checkTimeDelaySampleIndex < checkTimeDelaySample.Length) {
			if(checkTimeDelayLoop) CheckTimeDelay ();
		}else if(onCheckTimeDelayComplete!=null){
			// CheckTimeDelay Complete 
			checkTimeDelayLoop = false;
			onCheckTimeDelayComplete();
		}
	}

	public long GetCurTimeDelay(){
		return checkTimeDelaySample[checkTimeDelaySampleIndex];
	}

	public long GetMeanTimeDelay(){
		
		long temp = 0;
		int size = checkTimeDelaySample.Length;
		/* 排序 */
		for(int i = 0 ; i < size-1; i ++)
		{
			for(int j = 0 ;j < size-1-i ; j++)
			{
				if(checkTimeDelaySample[j] > checkTimeDelaySample[j+1])  //交换两数位置
				{
					temp = checkTimeDelaySample[j];
					checkTimeDelaySample[j] = checkTimeDelaySample[j+1];
					checkTimeDelaySample[j+1] = temp;
				}
			}
		}
		long sum = 0;
		/* 去掉头尾求均值 */
		for (int i = 1; i < size - 1; i++) {
			sum += checkTimeDelaySample [i];
		}

		return(sum / (checkTimeDelaySample.Length - 2));
	}

	public void CheckTimeDelayPrint(){
		string str = "";
		for (int i = 0; i < checkTimeDelaySample.Length; i++) {
			str += checkTimeDelaySample [i] + ",";
		}
		Debug.LogFormat ("{0},GetMeanTimeDelay {1}",str,GetMeanTimeDelay());
	}
		
	/*
	MemoryStream m_outputZipMemoryStream = new MemoryStream();
	ZOutputStream m_outputZipStream; 
	MemoryStream m_inputZipMemoryStream = new MemoryStream();
	ZInputStream m_inputStream;
	*/
	/// <summary>
	/// zip 压缩
	/// </summary>
	/// <param name="buffer">Buffer.</param>
	/// 
	/*
	public byte[] Zip(byte[] buffer){
		m_inputZipMemoryStream.Position = 0;
		//if (m_inputStream == null)
			m_inputStream = new ZInputStream (m_inputZipMemoryStream,zlibConst.Z_BEST_COMPRESSION);
		m_inputZipMemoryStream.Write (buffer, 0, buffer.Length);
		//m_inputStream.Write (buffer, 0, buffer.Length);
		byte[] outBytes = m_inputZipMemoryStream.ToArray ();
		return outBytes;
	}
	*/
    /// <summary>
    /// 解压
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
	/*
    public byte[] Uzip(byte[] buffer)
	{
		m_outputZipMemoryStream.Position = 0;
		//if(m_outputZipStream == null)
		m_outputZipStream = new ZOutputStream(m_outputZipStream,zlibConst.Z_BEST_COMPRESSION);
        //reader.Read(bytes, 0, length);
		//m_outputZipStream.Position = 0;
		m_outputZipMemoryStream.Write(buffer, 0, buffer.Length);
		m_outputZipStream.finish ();
		byte[] outBytes = m_outputZipMemoryStream.ToArray ();
        return outBytes;
        /*string commonString = "";
		MemoryStream ms = new MemoryStream(buffer);
		Stream sm = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(ms);
		//这里要指明要读入的格式，要不就有乱码
		StreamReader reader = new StreamReader(sm, System.Text.Encoding.UTF8);
		try
		{
			commonString = reader.ReadToEnd();
		}
		finally
		{
			sm.Close();
			ms.Close();
		}
		return System.Text.Encoding.Default.GetBytes (commonString);*/
	//}
	public void testZIP(){
		
		byte[] buffer = new byte[64] ;

		for(int i = 0;i< buffer.Length;i++){
			buffer[i] = (byte)2;
		}

		//MemoryStream inStream = new MemoryStream();
		//inStream.Write (buffer,0,buffer.Length);

		//inStream = compressStream (inStream);

		//inZStream.finish ();
		byte[] zipBuffer = compressBytes(buffer);


		for(int i = 0;i< zipBuffer.Length;i++){
			Debug.LogFormat ("zipBuffer {0}",zipBuffer[i]);
		}

		//MemoryStream outStream = new MemoryStream();
		//outStream.Write (zipBuffer,0,zipBuffer.Length);

		//ZOutputStream outZStream = new ZOutputStream(outStream);
		//outZStream.finish ();
		//outStream = deCompressBytes(zipBuffer);
		/*
		MemoryStream outStream = new MemoryStream();
		ZOutputStream outZStream = new ZOutputStream(outStream);

		outStream.Write (zipBuffer,0,zipBuffer.Length);
		outStream.Flush ();
		outZStream.finish ();

		byte[] unZipBuffer = outStream.ToArray();
		*/
		byte[] unZipBuffer = deCompressBytes(zipBuffer);

		//byte[] zipBuffer = Zip (buffer);
		//byte[] unZipBuffer = Uzip (zipBuffer );

		for(int i = 0;i< unZipBuffer.Length;i++){
			Debug.LogFormat ("UnzipBuffer {0}",unZipBuffer[i]);
		}
	}

	/// <summary>
	/// The buffer.解压缓冲
	/// </summary>
	static byte[] buffer = new byte[1024];
	/// <summary>
	/// 复制流
	/// </summary>
	/// <param name="input">原始流</param>
	/// <param name="output">目标流</param>
	public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
	{
		int len;
		while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
		{
			output.Write(buffer, 0, len);
		}
		output.Flush();
	}
	/// <summary>
	/// 压缩字节数组
	/// </summary>
	/// <param name="sourceByte">需要被压缩的字节数组</param>
	/// <returns>压缩后的字节数组</returns>
	private static byte[] compressBytes(byte[] sourceByte)
	{
		MemoryStream inputStream = new MemoryStream(sourceByte);
		Stream outStream = compressStream(inputStream);
		byte[] outPutByteArray = new byte[outStream.Length];
		outStream.Position = 0;
		outStream.Read(outPutByteArray, 0, outPutByteArray.Length);
		outStream.Close();
		inputStream.Close();
		return outPutByteArray;
	}
	/// <summary>
	/// 解压缩字节数组
	/// </summary>
	/// <param name="sourceByte">需要被解压缩的字节数组</param>
	/// <returns>解压后的字节数组</returns>
	private static byte[] deCompressBytes(byte[] sourceByte)
	{
		MemoryStream inputStream = new MemoryStream(sourceByte);
		Stream outputStream = deCompressStream(inputStream);
		byte[] outputBytes = new byte[outputStream.Length];
		outputStream.Position = 0;
		outputStream.Read(outputBytes, 0, outputBytes.Length);
		outputStream.Close();
		inputStream.Close();
		return outputBytes;
	}
	/// <summary>
	/// 压缩流
	/// </summary>
	/// <param name="sourceStream">需要被压缩的流</param>
	/// <returns>压缩后的流</returns>
	private static Stream compressStream(Stream sourceStream)
	{
		MemoryStream streamOut = new MemoryStream();
		ZOutputStream streamZOut = new ZOutputStream(streamOut, zlibConst.Z_DEFAULT_COMPRESSION);
		CopyStream(sourceStream, streamZOut);
		streamZOut.finish();
		return streamOut;
	}
	/// <summary>
	/// 解压缩流
	/// </summary>
	/// <param name="sourceStream">需要被解压缩的流</param>
	/// <returns>解压后的流</returns>
	private static Stream deCompressStream(Stream sourceStream)
	{
		MemoryStream outStream = new MemoryStream();
		ZOutputStream outZStream = new ZOutputStream(outStream);
		CopyStream(sourceStream, outZStream);
		outZStream.finish();
		return outStream;
	}
}

