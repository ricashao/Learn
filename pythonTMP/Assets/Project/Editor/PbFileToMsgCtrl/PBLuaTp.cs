using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
/*
and	 break	do	else	elseif	
end	 false	for	function	if	
in	 local	nil	not	or type
repeat	return	then true until	while
以上为Lua语言关键字
 */
public class PBLuaTp  {

	static public string sendFunTp = "--$Notes$\n " +
									"function $luaModularCtrlName$.send$MsgName$($Params$)\n " +
								    "$InitMsg$ \n"+
								    "\tlocal buffer = ByteBuffer.New();\n"+
								    "\tbuffer.cmdId   = msg.cmd ;\n"+
								    "\tbuffer:WriteStringBytes(msg:SerializeToString());\n"+
								    "\tioo.networkManager:SendMessage(buffer);\n"+
								    "end\n";

	static public string initMsgTp = "\tlocal msg = $pbModularName$.$msgName$();\n";
	static public string toLuaMsgAttributeSetCMDTp = "\tmsg.cmd = $pbModularName$.$Default$;\n";
	static public string toLuaMsgAttributeSetINTTp = "\tmsg.$attributeName$ = toInt($attributeName$);--$Notes$\n";
	static public string toLuaMsgEnumSetINTTp = "\tmsg.$attributeName$ = $attributeName$;--$Notes$\n";//"$MessageName$_$EnumTypeName$_$EnumAttributeName$_ENUM";//S_MSGUSRPLANTCOMBO_TYPE_E_WATER_ENUM
	static public string onPBMsgFunTp = "function $luaModularCtrlName$.OnPBMsg(key, buffer)\n"+
										"warn('$luaModularCtrlName$.OnPBMsg ---->>>'..key);\n"+
										"$handleMsg$ \n"+
										"end\n";
	static public string handleMsgTp = "\tif key ==  $pbModularName$.$Default$ then\n" +
										"\t\tlocal msg = $pbModularName$.$msgName$();\n" +
										"\t\tmsg:ParseFromString(buffer);\n"+
										"$warnMsgAttribute$\n"+
										"$checkResult$\n"+
										"\tend\n";

	static public string warnMsgAttributeTp = "\t\twarn('$msgName$.$attributeName$'..msg.$attributeName$);\n";
	
	static public string checkResultTp     = "\t\tif msg.result ~= nil and msg.result > 0  then\n"+
											 "$checkResultItem$\n"+	
											 "\t\tend\n";
	static public string checkResultItemTp = "\t\t\tif msg.result == $enumValue$ then\n"+
											 "\t\t\t\twarn('$Notes$');\n"+
											"\t\t\treturn;\n"+
											"\t\t\tend;\n";

	static public string luaMsgCtrlCode;

	static public string luaTpPath = "/Project/Editor/PbFileToMsgCtrl/TpMsgCtrl.lua";

	static public void init(){
		string protoPath = Application.dataPath + luaTpPath;
		string protoTxt;
		using (StreamReader readerTemplentPath = new StreamReader(protoPath, Encoding.GetEncoding("utf-8")))
		{
			protoTxt = readerTemplentPath.ReadToEnd();
			readerTemplentPath.Close();
		}
		luaMsgCtrlCode = protoTxt;
	}

	static public void toLuaMsgCtrl(string protoPath,PBFile pbFile){

		init();

		//string 	pbModularName  = protoPath.Replace(Application.dataPath +"/Lua/pblua/","").Replace(".proto","").ToLower() + "_pb";
		string 	pbModularName  = Path.GetFileName(protoPath).ToLower() + "_pb";
		string 	luaModularName = pbModularName.Substring(0,1).ToUpper() + pbModularName.Substring(1,pbModularName.Length-1).Replace("_pb","");
		string 	luaModularCtrlName = luaModularName + "MsgCtrl";
		string 	luaModularCtrlFileName = luaModularName + "MsgCtrl";

		string  sendFunList = "";
		string  handleMsg = "";

		foreach (PBElement chlidPBElement in pbFile.child){

			//1.
			if(chlidPBElement.pb_type == PBElement.type_PBMessage && chlidPBElement.name.IndexOf("Usr") != -1){

				string Params = "";
				string InitMsg = initMsgTp.Replace("$pbModularName$",pbModularName).Replace("$msgName$",chlidPBElement.name);

				foreach (PBElement pElement in chlidPBElement.child){
					if(pElement.pb_type == PBElement.type_PBAttribute){
						PBAttribute attribute = (PBAttribute)pElement;
						if(attribute.name != "cmd")
						Params += ","+ attribute.name ;
						InitMsg+= toLuaMsgAttributeSet(attribute,pbModularName);
					}
					/*
					if(pElement.pb_type == PBElement.type_PBEnum){
						PBEnum pbEnum = (PBEnum)pElement;
						Params += ","+pbEnum.name ;
						toLuaMsgEnumSet(pbEnum,pbModularName);
					}*/
				}
				if(Params.Length > 0)
				Params = Params.Substring(1,Params.Length-1);

				string funCode = sendFunTp.Replace("$Notes$",chlidPBElement.Notes).Replace("$luaModularCtrlName$",luaModularCtrlName).Replace("$MsgName$",chlidPBElement.name).Replace("$Params$",Params).Replace("$InitMsg$",InitMsg);

				Debug.LogWarning(funCode);

				if(funCode.StartsWith("--//申请收获")){
					Debug.Log("xxx");
				}

				sendFunList += funCode;
			}
			//2.
			if(chlidPBElement.pb_type == PBElement.type_PBMessage && chlidPBElement.name.IndexOf("Svr") != -1){

				string checkResult      = toLuaMsgCheckResult((PBMessage)chlidPBElement);
				string warnMsgAttribute = toLuaWarnMsgAttribute((PBMessage)chlidPBElement);
			
				if(chlidPBElement.name == "S_MsgSvrCollectionEnterFailed"){
					Debug.LogWarning(chlidPBElement.name);
				}

				handleMsg += handleMsgTp.Replace("$pbModularName$",pbModularName).Replace("$Default$",((PBAttribute)chlidPBElement.getChildByName("cmd")).Default).Replace("$msgName$",chlidPBElement.name).Replace("$warnMsgAttribute$",warnMsgAttribute).Replace("$checkResult$",checkResult);
			}
		}

		string onPBMsgFunCode = onPBMsgFunTp.Replace("$luaModularCtrlName$",luaModularCtrlName).Replace("$handleMsg$",handleMsg);
		Debug.LogWarning(onPBMsgFunCode);

		newFileMsgCtrl(luaModularCtrlFileName,
		               luaMsgCtrlCode.Replace("$proto$",pbFile.code).Replace("$pbModularName$",pbModularName).Replace("$luaModularCtrlName$",luaModularCtrlName).Replace("$sendFunList$",sendFunList).Replace("$onPBMsgFunCode$",onPBMsgFunCode)
		               );
	}

	static public string toLuaMsgAttributeSet(PBAttribute attribute,string pbModularName){
		if(attribute.name == "cmd"){
			return  toLuaMsgAttributeSetCMDTp.Replace("$pbModularName$",pbModularName).Replace("$Default$",attribute.Default) ;
		}else
		if(attribute.type == "sint32"){
			return  toLuaMsgAttributeSetINTTp.Replace("$attributeName$",attribute.name).Replace("$Notes$",attribute.Notes) ;
		}
		return toLuaMsgEnumSetINTTp.Replace ("$attributeName$", attribute.name).Replace ("$Notes$", attribute.Notes);
		//return "err";
	}

	static public string toLuaMsgEnumSet(PBEnum pbEnum,string pbModularName){
		// toLuaMsgEnumSetINTTp = "$MessageName$_$EnumTypeName$_$EnumAttributeName$_ENUM"
		return toLuaMsgEnumSetINTTp.Replace ("$attributeName$", pbEnum.name).Replace ("$Notes$", pbEnum.Notes);
	}

	static public string toLuaMsgCheckResult(PBMessage pbMessage){

		string checkResult = checkResultTp;
		string checkResultItem = "";
		foreach (PBElement pElement in pbMessage.child){
			if(pElement.pb_type != PBElement.type_PBAttribute){
				continue;
			}
			PBAttribute attribute = (PBAttribute)pElement;
			if(attribute.name == "result"){
				PBElement pbEnum = pbMessage.getChildByName(attribute.type);
				if(pbEnum == null){
					Debug.LogError("not find !!!! = > " + attribute.type);
				}else{
					/*
					static public string checkResultItemTp = "\tif msg.result == $enumValue$ then\n"+
						"\t\twarn('$Notes$');\n"+
							"\tend\n";
					*/
					foreach (PBElement pbKeyValue in pbEnum.child){
						PBKeyValue keyValue = (PBKeyValue)pbKeyValue;
						checkResultItem +=  checkResultItemTp.Replace("$enumValue$",keyValue.value).Replace("$Notes$",keyValue.Notes);
					}

				}
			}
		}
		return checkResult.Replace("$checkResultItem$",checkResultItem);
	}

	static public string toLuaWarnMsgAttribute(PBMessage pbMessage){
		string warnMsgAttribute = "";
		foreach (PBElement pElement in pbMessage.child){
			if(pElement.pb_type != PBElement.type_PBAttribute){
				continue;
			}
			if(pElement.name == "result"){
				continue;
			}
			//"\t\twarn('$msgName$.$attributeName$'..msg.$attributeName$);";
			PBAttribute attribute = (PBAttribute)pElement;
			warnMsgAttribute += warnMsgAttributeTp.Replace("$msgName$",pbMessage.name).Replace("$attributeName$",attribute.name);
		}
		return warnMsgAttribute;
	}

	public static void newFileMsgCtrl(string luaModularCtrlName,string luaMsgCtrlCode)
	{
		string msgctrlPath = Application.dataPath+"/Lua/msgctrl/"+luaModularCtrlName+".lua";
		// 判断文件是否存在，不存在则创建，否则读取值显示到窗体

		if(File.Exists(msgctrlPath)){
			//File.Delete(msgctrlPath);
			Debug.LogError(msgctrlPath  + "文件存在!!!");
			return;
		}

		string directory = Path.GetDirectoryName(msgctrlPath);

		if (!System.IO.Directory.Exists (directory)) {
			System.IO.Directory.CreateDirectory (directory);
		}

		File.WriteAllText (msgctrlPath,luaMsgCtrlCode);

		if(!File.Exists(msgctrlPath))
		{	
			//File.Create (msgctrlPath);

			/*
			FileStream   fs= new FileStream(msgctrlPath, FileMode.Create, FileAccess.Write);//创建写入文件
			StreamWriter sw = new StreamWriter(fs);
		
			sw.Write(luaMsgCtrlCode);
			sw.Flush();

			sw.Close();
			fs.Close();*/
		}
	
	}

	[MenuItem("LuaCode/pbFileToMsgCtrl")]
	static void pbFileToMsgCtrl(){ 
		/*
		string templentPath = Application.dataPath +"/Lua/plant.proto";
		string templentTxt;
		using (StreamReader readerTemplentPath = new StreamReader(templentPath, Encoding.Default))
		{
			templentTxt = readerTemplentPath.ReadToEnd();
			readerTemplentPath.Close();
		}
		*/
		//string protoPath = Application.dataPath +"/Lua/pblua/sewing.proto";
		//string protoPath = Application.dataPath +"/Lua/pblua/plant.proto";
		//string protoPath = Application.dataPath +"/Lua/pblua/test.proto";
		//string protoPath = Application.dataPath +"/Lua/pblua/manor.proto";

		//string protoPath = Application.dataPath +"/Lua/pblua/newproduct.proto";
		//string protoPath = Application.dataPath +"/Lua/pblua/animal_combo.proto";

		string protoPath = Application.dataPath + "/Resources/proto/addressbook.proto";

		string protoTxt;
		//using (StreamReader readerTemplentPath = new StreamReader(protoPath, Encoding.GetEncoding("GBK")))
		using (StreamReader readerTemplentPath = new StreamReader(protoPath, Encoding.GetEncoding("utf-8")))
		{
			protoTxt = readerTemplentPath.ReadToEnd();
			readerTemplentPath.Close();
		}

		PBFile pbFile = new PBFile(protoTxt);
		pbFile.read();
		PBLuaTp.toLuaMsgCtrl(protoPath,pbFile);

		AssetDatabase.Refresh();
	}
	/*
	local $ModuleName$ModuleData = {}

	local Cmd = {
	$CmdKeyList$
		--[[AddFlagCmd CodeTP${ $MsgName$ = sib($ID$), }$ ]]--
	}

	function $ModuleName$ModuleData.register(tcpClinet,MsgDefine)
		--protobuf 注册
		local protobuf = tcpClinet.proto
	    --local luaPath = CS.UnityEngine.Application.dataPath..'/Resources'
		local luaPath = PbPtah()
		--映射协议号 -> protobuf 
		MsgDefine.$ModuleName$ModuleCmd = Cmd
	    local pbtable = tcpClinet.pb
	$CmdMappingList$
		--pbtable [Cmd.$MsgName$] = "$Package$.$MsgName$"
		--监听处理事件
	$CmdAddlistenerList$
		--tcpClinet.addlistener(Cmd.$MsgName$,$ModuleName$ModuleData.on_msg)
	end

	function $ModuleName$ModuleData.clear(tcpClinet)

	$CmdRemlistenerList$
	 	--tcpClinet.removelistener(Cmd.$MsgName$,$ModuleName$ModuleData.on_msg)
	end 	

	$CmdSendFunList$

	function $ModuleName$ModuleData.on_msg(key,decode)

	$CmdReceiveList$	

	end 
	*/
	[MenuItem("LuaCode/pbFileInfo")]
	static void pbFileInfo(){
		
		string protoPath = Application.dataPath + "/Resources/proto/addressbook.proto";

		PBFile.curPath = "/Users/zhuyuu3d/Downloads";
		protoPath = PBFile.curPath + "/ZTest.proto";

		PBFile.curPath = "C:/Users/Administrator.PC-20171226GAHY/Desktop/proto";
		protoPath = "C:/Users/Administrator.PC-20171226GAHY/Desktop/proto/ZShop.proto";

		string LuaCodeOutPath = "/Project/Editor/LuaCodeOut/";

		string protoTxt;
		//using (StreamReader readerTemplentPath = new StreamReader(protoPath, Encoding.GetEncoding("GBK")))
		using (StreamReader readerTemplentPath = new StreamReader (protoPath, Encoding.GetEncoding ("utf-8"))) {
			protoTxt = readerTemplentPath.ReadToEnd ();
			readerTemplentPath.Close ();
		}

		string ModuleName = Path.GetFileName( protoPath).Replace(".proto","");

		PBFile pbFile = new PBFile (protoTxt);
		pbFile.read ();

		Debug.Log (">> package = " + pbFile.package);

		StringBuilder CmdKeyList = new StringBuilder();
		StringBuilder CmdMappingList = new StringBuilder();
		StringBuilder CmdSendFunList = new StringBuilder();
		StringBuilder CmdReceiveList = new StringBuilder();
		StringBuilder CmdAddlistenerList = new StringBuilder();
		StringBuilder CmdRemlistenerList = new StringBuilder();
		//pbFile.debug ();
		//循环 poto->Element 文件定义
		foreach (PBElement chlidPBElement in pbFile.child) {
			
			Debug.LogFormat ("消息 >> name = {0},pb_type = {1}", chlidPBElement.name, chlidPBElement.pb_type);
			//循环 poto->Element->Enum 
			foreach (PBElement msgPBElement in chlidPBElement.child) {

				Debug.LogFormat ("消息字段 >> >> name = {0},pb_type = {1}", msgPBElement.name, msgPBElement.pb_type);

				if(msgPBElement.pb_type == PBElement.type_PBEnum && msgPBElement.name.Equals("enumID") ){
					//消息 ID 枚举
					PBEnum PBEnum = (PBEnum)msgPBElement;
					foreach (PBElement EnumPBElement in msgPBElement.child) {

						if(EnumPBElement is PBKeyValue){

							PBKeyValue cmdID = (EnumPBElement as PBKeyValue);

							Debug.LogFormat (">> >> >> ID = {0},value = {1}",
								(EnumPBElement as PBKeyValue).key,  (EnumPBElement as PBKeyValue).value);
							//string tp = "$MsgName$ = sib($ID$),";
							CmdKeyList.AppendLine ("\t$MsgName$ = sib($ID$),".Replace("$MsgName$",chlidPBElement.name).Replace("$ID$",cmdID.value));

						}
					}
				}
			}

			CmdMappingList.AppendLine ("\tpbtable [Cmd.$MsgName$] = \"$Package$.$MsgName$\"".Replace("$MsgName$",chlidPBElement.name).Replace("$Package$",pbFile.package) );

			//循环 poto->Element.name == CS_
			if (chlidPBElement.name.StartsWith ("CS_")) {
				CmdSendFunList.Append("function $ModuleName$ModuleData.send_$MsgName$(".Replace("$MsgName$",chlidPBElement.name).Replace("$ModuleName$",ModuleName));

				//循环 poto->Element->Attribute
				//参数列表
				int i=0;
				foreach (PBElement msgPBElement in chlidPBElement.child) {
					if (msgPBElement.pb_type == PBElement.type_PBAttribute) {
						CmdSendFunList.Append ( i > 0 ? "," + msgPBElement.name : msgPBElement.name);
						i++;
					}
				}
				CmdSendFunList.AppendLine (")");

				CmdSendFunList.AppendLine ("\tlocal " + chlidPBElement.name + " = {}" );
				//循环 poto->Element->Attribute
				foreach (PBElement msgPBElement in chlidPBElement.child) {
					if (msgPBElement.pb_type == PBElement.type_PBAttribute) {
						CmdSendFunList.AppendLine ("\t" + chlidPBElement.name + "." + msgPBElement.name + " = " + msgPBElement.name);
					}
				}
				CmdSendFunList.AppendLine (string.Format("\tGameState.tcpClinet.sendmsg(Cmd.{0},{1})",chlidPBElement.name,chlidPBElement.name));
				CmdSendFunList.AppendLine ("end");
			}

			//循环 poto->Element.name == SC_
			if (chlidPBElement.name.StartsWith ("SC_")) {

				CmdAddlistenerList.AppendLine ("\ttcpClinet.addlistener(Cmd.$MsgName$,$ModuleName$ModuleData.on_msg)".Replace ("$ModuleName$", ModuleName).Replace ("$MsgName$", chlidPBElement.name));

				CmdRemlistenerList.AppendLine ("\ttcpClinet.removelistener(Cmd.$MsgName$,$ModuleName$ModuleData.on_msg)".Replace ("$ModuleName$", ModuleName).Replace ("$MsgName$", chlidPBElement.name));

				CmdReceiveList.AppendLine ("\tif key == Cmd.$MsgName$ then".Replace ("$MsgName$", chlidPBElement.name));
				CmdReceiveList.AppendLine ("\t\tprint(\"$ModuleName$ >> on_msg >> user_para >>  \".. Cmd.$MsgName$)\n".Replace ("$ModuleName$", ModuleName).Replace ("$MsgName$", chlidPBElement.name));
				CmdReceiveList.AppendLine ("\t\t$ModuleName$ModuleData.$MsgName$ = decode".Replace ("$ModuleName$", ModuleName).Replace ("$MsgName$", chlidPBElement.name));

				foreach (PBElement msgPBElement in chlidPBElement.child) {
					if (msgPBElement.pb_type == PBElement.type_PBAttribute) {
						CmdReceiveList.AppendLine ("\t\tprint(\"$MsgName$.$Attribute$ \".. decode.$Attribute$)").Replace ("$MsgName$", chlidPBElement.name).Replace ("$Attribute$", msgPBElement.name);
					}
				}

				CmdReceiveList.AppendLine ("\tend");
			}
		
		}

		/*
		-- $ModuleName$
		-- $MsgName$
		-- $Package$
		-- $ID$
		*/
		Dictionary<string,string> repkey = new Dictionary<string, string> ();
		repkey["$ModuleName$"] = ModuleName;
		repkey ["$CmdKeyList$"] = CmdKeyList.ToString();
		repkey ["$CmdMappingList$"] = CmdMappingList.ToString();
		repkey ["$CmdReceiveList$"] = CmdReceiveList.ToString();
		repkey ["$CmdAddlistenerList$"] = CmdAddlistenerList.ToString ();
		repkey ["$CmdRemlistenerList$"] = CmdRemlistenerList.ToString ();
		repkey ["$CmdSendFunList$"]= CmdSendFunList.ToString ();
		repkey ["$Package$"] = pbFile.package;
		//"$MsgName$ = sib($ID$),"
		//$CmdKeyList$
		string luaFilePath = Application.dataPath + string.Format ("{0}/DataModules/{1}ModuleData.lua", LuaCodeOutPath, repkey ["$ModuleName$"]);

		CreateLuaCodeByTp(repkey, Application.dataPath + "/Project/Editor/LuaCodeTp/TP_ModuleData.lua",luaFilePath );

	}

	static public void CreateLuaCodeByTp(Dictionary<string,string> repkeyDic,string tpFileFath,string outPutFileFath){

		string tpCode = File.ReadAllText (tpFileFath);

		foreach(string key in repkeyDic.Keys ){
			tpCode = tpCode.Replace (key,repkeyDic[key]);
		}

		if (!Directory.Exists( Path.GetDirectoryName(outPutFileFath) ) )
		{
			Directory.CreateDirectory(Path.GetDirectoryName(outPutFileFath) );
		}
		//Directory.Exists (outPutFileFath);

		if (File.Exists (outPutFileFath)) {
			Debug.LogErrorFormat ("文件已经存在！{0}",outPutFileFath);
			return;
		}

		File.WriteAllText (outPutFileFath,tpCode);

	}

}
