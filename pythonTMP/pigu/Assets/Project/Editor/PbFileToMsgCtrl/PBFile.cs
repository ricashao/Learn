using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CodeSymbol{
	public int Index;
	public char Symbol;
	public CodeSymbol(int Indexp, char Symbolp){
		Index = Indexp;
		Symbol = Symbolp;
	}
}

public class CodeKeyword{
	public int Index;
	public string value;
	public CodeKeyword(int Indexp,string valuep){
		Index = Indexp;
		value = valuep;
	}
}

public class PBElement{

	public const int type_PBKeyValue  = 1;
	public const int type_PBAttribute = 2;
	public const int type_PBEnum 	  = 3;
	public const int type_PBMessage   = 4;
	public const int type_PBFile      = 5;
	public const int type_PBImportFile = 6;
	public const int type_PBPackage = 7;

	public int pb_type;
	public int startIndex;
	public int endIndex;
	public string name;
	public string code;
	public List<PBElement> child = new List<PBElement>();

	public List<CodeKeyword> codekeyStack = new List<CodeKeyword>();

	public string Notes = "";
	
	public virtual void read(){
		
	}
	public void addChild(PBElement element){
		child.Add(element);
	}
	public void cut(string fileCode){
		code = fileCode.Substring(startIndex,endIndex - startIndex + 1);

		analysis();
	}

	public virtual void analysis(){
		int startIndex = 0;
		for(int i = 0;i<code.Length ;i++){
			startIndex = i;
			CodeSymbol curSymbol = PBFile.FindNextSymbol(code,startIndex);
			int Index = curSymbol.Index;
			if(Index > 0){
				string key = code.Substring(startIndex,Index - startIndex);
				
				if(key==""){
					
				}else{
					//关键字压入
					codekeyStack.Add(new CodeKeyword(startIndex,key));
				}
			}else if(Index == -1){
				break;
			}
			i = Index;
		}

	}

	public PBElement getChildByName(string name){
		foreach(PBElement ch in child){
			if(ch.name == name){
				return ch;
			}
		}
		return null;
	}

	public virtual string log(){
		return "pb_type = " + pb_type.ToString() ;
	}
}

public class PBImportFile:PBElement {

	public string pbFilePath;

	public PBImportFile(string codep = ""){
		pb_type = type_PBImportFile;
		code = codep;
	}

	public override void analysis(){
		int s = code.IndexOf ("\"")+1;
		int e = code.IndexOf ("\"",s);
		pbFilePath = code.Substring (s,e-s);
	}
}

public class PBPackage:PBElement {
	
	public string pbPackageName;

	public PBPackage(string codep = ""){
		pb_type = type_PBPackage;
		code = codep;
	}

	public override void analysis(){
		int s = code.IndexOf (" ") + 1;
		int e = code.Length - 1;
		pbPackageName = code.Substring (s,e-s);
	}
}

public class PBKeyValue :PBElement {
	public string key;
	public string value;
	public PBKeyValue(string codep = ""){
		pb_type = type_PBKeyValue;
		code = codep;
	}
	public override void analysis(){
		base.analysis();
		key   = codekeyStack[0].value;
		value = codekeyStack[1].value;
	}
	public override string log(){
		return base.log() + " key = "+ key + ",value= " + value+",Notes = "+ Notes+ "\n";;
	}
}

public class PBAttribute :PBElement {
	public string option;
	public string type;
	//public string name;
	public string index;
	public string Default = "";
	public PBAttribute(string codep = ""){
		pb_type = type_PBAttribute;
		code = codep;
	}
	public override void read(){
	
	}

	public override void analysis(){
		base.analysis();
		option = codekeyStack[0].value;
		type   = codekeyStack[1].value;
		name   = codekeyStack[2].value;
		index  = codekeyStack[3].value;

		if(child.Count == 1 && child[0].pb_type == PBElement.type_PBKeyValue){
			Default = ((PBKeyValue)child[0]).value;
		}
	}

	public override string log(){
		return base.log() + " option = "+option+",type = "+type+",name = "+name+",index = " + index +",Default = "+Default +",Notes = "+ Notes +  "\n";
	}
}

public class PBEnum:PBElement{
	public PBFile pbFile;
	//string name;
	public Dictionary <string,int> v = new Dictionary <string,int>();

	public PBEnum(string codep = ""){
		pb_type = type_PBEnum;
		code = codep;
	}
	public void read(){
		/*
		name = code.Substring(0,code.IndexOf("{")).Trim();
		Debug.LogWarning(name +" >> PBEnum   -------->  " + code); 
		int s = code.IndexOf("{");
		int e = code.IndexOf("}");
		string[] line = code.Substring(s+ 1,(e-s)-1).Trim().Split( new char[]{'\n','\r'});
		
		for(int i = 0;i<line.Length ;i++){
			string lineStr = line[i];
			int equalsSignIndex = lineStr.IndexOf("=");
			int semicolonIndex  = lineStr.IndexOf(";");
			string key = lineStr.Substring(0,equalsSignIndex).Trim();
			string value = lineStr.Substring(equalsSignIndex+1, semicolonIndex -  equalsSignIndex -1).Trim();
			Debug.LogWarning("key --------> "+key+", value --------> " + value);
			//v[key] = int.Parse(value);
		}*/
	}
	public override void analysis(){
		base.analysis();
		name   = codekeyStack[0].value;

		//pbFile.addPBElementDic(name,this);
	}

	public override string log(){
		string baseLog = base.log() +",Notes = "+ Notes ;

		foreach(PBElement ch in child){
			baseLog += ch.log();
		}

		return baseLog ;
	}
}

public class PBMessage:PBElement{
	public PBFile pbFile;
	//string name;
	Dictionary <string,PBEnum> PBEnumDic = new Dictionary <string,PBEnum>();

	public PBMessage(string code = ""){
		pb_type = type_PBMessage;

	}
	
	public override void analysis(){
		base.analysis();
		name   = codekeyStack[0].value;

		//pbFile.addPBElementDic(name,this);
	}

	public override string log(){
		string baseLog = base.log()+",Notes = "+ Notes ;
		
		foreach(PBElement ch in child){
			baseLog += ch.log();
		}
		
		return baseLog ;
	}
}

public class PBFile :PBElement{

	public static string curPath = "";

	public string name;
	public string package;

	Dictionary <string,PBFile> PBFileImportDic = new Dictionary<string, PBFile>();
	Dictionary <string,PBEnum> PBEnumDic = new Dictionary <string,PBEnum>();
	Dictionary <string,PBMessage> PBMessageDic = new Dictionary <string,PBMessage>();
	Dictionary <string,PBElement> PBElementDic = new Dictionary <string,PBElement>();

	public PBFile(string codep = ""){
		pb_type = type_PBFile;
		code = codep;
	}

	public static PBFile readByPath(string pbFilePath){
		PBFile pbFile = new PBFile (getCodeTextByPath( Path.Combine( curPath,pbFilePath) ));
		pbFile.read ();
		return pbFile;
	}

	public static string getCodeTextByPath(string pbFilePath){
		return System.IO.File.ReadAllText(pbFilePath);
	}

	public void readFile(string pbFilePath){
		code = getCodeTextByPath (Path.Combine (curPath, pbFilePath));
	}

	public void addPBElementDic(string key , PBElement pbElement){
		PBElementDic.Add(key,pbElement);
	}

	public void debug(){

		foreach(PBFile importPBFile in PBFileImportDic.Values){
			importPBFile.debug ();
		}

		foreach (PBElement chlidPBElement in child) {
			Debug.LogFormat (">> name = {0},pb_type = {1}",chlidPBElement.name,chlidPBElement.pb_type);
		}
	}

	public void read(){

		List<CodeKeyword> codekeyStack = new List<CodeKeyword>();
		List<CodeSymbol>  symbolStack  = new List<CodeSymbol>();
		List<PBElement>   elementStack = new List<PBElement>();

		elementStack.Add(this);

		if(charArr == null)
			charArr = code.ToCharArray();

		PBElement curPBElement = null;
		
		int startIndex = 0;
		for(int i = 0;i<code.Length ;i++){
			
			startIndex = i;
			CodeSymbol curSymbol = FindNextSymbol(code,startIndex);
			int Index = curSymbol.Index;

			if(Index > 0){
				//跳过注释
				if (curSymbol.Symbol == '/') {

					if (charArr [curSymbol.Index + 1] == '*') {

						while (Index < charArr.Length) {
							
							curSymbol = FindNextSymbol (code, Index + 1 , '/');
							Index = curSymbol.Index;

							if (Index == -1) {
								Debug.LogErrorFormat ("{0} not find '/' end",name);
								return;
							}
							if (charArr [curSymbol.Index - 1] == '*') {
								break;
							}
						}
							
						i = Index;

						Debug.LogWarningFormat ("{0} jump /**/ index {1} ",name,Index);

						continue;
					}
				}

				string key = code.Substring(startIndex,Index - startIndex);

				if(curPBElement != null && key.StartsWith("//")){
					//	curPBElement.Notes = key;
				}
					
				if(key==""){

				}
				else{
					//关键字压入
					codekeyStack.Add(new CodeKeyword(startIndex,key));
				}

				if(key.Equals("import")){
					//PBFile pbFile = new PBFile ();
					PBImportFile pushPBImportFile = new PBImportFile();
					pushPBImportFile.startIndex = startIndex;

					elementStack.Add(pushPBImportFile);
					curPBElement = pushPBImportFile;
				}
				else
				if (key.Equals ("package")) {
					PBPackage pushPBPackage = new PBPackage ();
					pushPBPackage.startIndex = startIndex;

					elementStack.Add(pushPBPackage);
					curPBElement = pushPBPackage;
				}
				else
				if(key.Equals("required") || key.Equals("optional") || key.Equals("repeated")){
					PBAttribute pushPBAttribute = new PBAttribute();
					pushPBAttribute.startIndex = startIndex;
					
					elementStack.Add(pushPBAttribute);
					curPBElement = pushPBAttribute;
				}else
				//enum压入
				if(key == "enum"){
					PBEnum pushPBElement = new PBEnum();
					pushPBElement.pbFile = this;
					pushPBElement.name = key;
					pushPBElement.startIndex = Index;
					
					elementStack.Add(pushPBElement);
					
					curPBElement = pushPBElement;
				}else
				//message压入
				if(key == "message"){
					PBMessage pushPBMessage = new PBMessage();
					pushPBMessage.pbFile    = this;
					pushPBMessage.name = key;
					pushPBMessage.startIndex = Index;
					
					elementStack.Add(pushPBMessage);
					
					curPBElement = pushPBMessage;

					if(codekeyStack.Count >=2 && codekeyStack[codekeyStack.Count-2].value.StartsWith("//")){
						pushPBMessage.Notes = codekeyStack[codekeyStack.Count-2].value;
					}
				}
				/*
				if(curSymbol.Symbol == '='){
					string pname  =  (string)codekeyStack[codekeyStack.Count-1];
					Debug.LogWarning(" >> pname :)))))   -------->  " + pname);
				}else
				*/
		
				if(curSymbol.Symbol == '{'){
					CodeKeyword ptype  =  codekeyStack[codekeyStack.Count-2];
					CodeKeyword pname  =  codekeyStack[codekeyStack.Count-1];
					//Debug.LogWarning(" >> Index :)))))   -------->  " + pname.Index + ", >> pname :)))))   -------->  " + pname.value );
					//压入符号栈
					symbolStack.Add(curSymbol);
				}
				else
				if(curSymbol.Symbol == '}'){
					//块结束
					CodeSymbol pushSymbol = symbolStack[symbolStack.Count-1];
					if(pushSymbol.Symbol == '{'){
						symbolStack.RemoveAt(symbolStack.Count-1);
						//弹出符号栈
						PBElement popUpPBElement = elementStack[elementStack.Count-1];
						popUpPBElement.endIndex = Index;
						popUpPBElement.cut(code);

						elementStack.RemoveAt(elementStack.Count-1);

						if(elementStack.Count > 0){
							elementStack[elementStack.Count-1].addChild(popUpPBElement);
						}
					}else{
						Debug.LogError("Symbol Error { ");
					}
				}
				else
				if(curSymbol.Symbol == '['){
					symbolStack.Add(curSymbol);
					
					PBKeyValue pushPBKeyValue = new PBKeyValue();
					pushPBKeyValue.startIndex = Index;
					elementStack.Add(pushPBKeyValue);
					
				}
				else
				if(curSymbol.Symbol == ']'){
					CodeSymbol pushSymbol = symbolStack[symbolStack.Count-1];
					if(pushSymbol.Symbol == '['){
						symbolStack.RemoveAt(symbolStack.Count-1);
						//弹出
						PBElement popUpPBElement = elementStack[elementStack.Count-1];
						popUpPBElement.endIndex = Index;
						popUpPBElement.cut(code);

						elementStack.RemoveAt(elementStack.Count-1);
						
						if(elementStack.Count > 0){
							elementStack[elementStack.Count-1].addChild(popUpPBElement);
						}
					}else{
						Debug.LogError("Symbol Error [ ");
					}
				}
				else
				if(curSymbol.Symbol == ';'){
					//弹出
					PBElement popUpPBElement = elementStack[elementStack.Count-1];

						if (popUpPBElement.pb_type == PBElement.type_PBPackage) {

						popUpPBElement.endIndex = Index;
						popUpPBElement.cut (code);

						this.package = (popUpPBElement as PBPackage).pbPackageName;

						elementStack.RemoveAt(elementStack.Count-1);

					}else
					if (popUpPBElement.pb_type == PBElement.type_PBImportFile) {
						
						popUpPBElement.endIndex = Index;
						popUpPBElement.cut (code);

						PBFile pbFile = new PBFile ();
						pbFile.name = (popUpPBElement as PBImportFile).pbFilePath;
						pbFile.readFile (pbFile.name);
						pbFile.read ();

						PBFileImportDic.Add (pbFile.name,pbFile);

						elementStack.RemoveAt(elementStack.Count-1);
						
					}else
					//枚举 属性
					if(popUpPBElement.pb_type == PBElement.type_PBEnum){
						PBKeyValue PBKeyValue = new PBKeyValue();
						PBKeyValue.key   = codekeyStack[codekeyStack.Count-2].value;
						PBKeyValue.value = codekeyStack[codekeyStack.Count-1].value;
						PBKeyValue.startIndex = codekeyStack[codekeyStack.Count-2].Index;
						PBKeyValue.endIndex = Index;
						PBKeyValue.cut(code);

						popUpPBElement.addChild(PBKeyValue);

						curPBElement = PBKeyValue;

						codekeyStack.RemoveAt(codekeyStack.Count-1);
						codekeyStack.RemoveAt(codekeyStack.Count-1);

						CodeSymbol nextSymbol = FindNextSymbol(code,Index+1 );
						string nextKey = code.Substring(Index+1 ,nextSymbol.Index - ( Index+1) );
						
						if(curPBElement != null && nextKey.StartsWith("//")){
							curPBElement.Notes = nextKey;
						}
						
					}else
					//message 属性
					if(popUpPBElement.pb_type == PBElement.type_PBAttribute){
						
						popUpPBElement.endIndex = Index;
						popUpPBElement.cut(code);

						elementStack.RemoveAt(elementStack.Count-1);

						curPBElement = popUpPBElement;

						if(elementStack.Count > 0){
							elementStack[elementStack.Count-1].addChild(popUpPBElement);
						}

						CodeSymbol nextSymbol = PBFile.FindNextSymbol(code,Index+1 );
						string nextKey = code.Substring(Index+1 ,nextSymbol.Index - ( Index+1) );
						
						if(curPBElement != null && nextKey.StartsWith("//")){
							curPBElement.Notes = nextKey;
						}
					}
				}
			}
			i = Index;
		}
		//getAllChild(this);

	}

	static public void getAllChild(PBElement p){
		foreach (PBElement chlidPBElement in p.child){
			Debug.LogWarning("start ----------------------->>>>>>>>>>>>>>>>>>>>>");

			Debug.LogWarning(chlidPBElement.code);
			Debug.LogWarning(chlidPBElement.log());

			Debug.LogWarning("end ----------------------->>>>>>>>>>>>>>>>>>>>>");

			getAllChild(chlidPBElement);
		}
	}

	public void read1(string codep){

		string code = codep.Trim();

		for (int i = 0;i<code.Length;i++){

			Debug.LogWarning("index   -------->  " + i);

			if(i == 1834){
				Debug.LogWarning("index   -------->  " + i);
			}

			int nextPBEnumIndex = code.IndexOf("enum",i);
			int nextPBMessageIndex = code.IndexOf("message",i);

			if(nextPBEnumIndex == -1 && nextPBMessageIndex == -1){
				return;
			}

			if(nextPBEnumIndex == -1 && nextPBMessageIndex > -1){
				int blockStart = code.IndexOf("{",nextPBMessageIndex );
				int messageEndIndex = FindEnd(code,blockStart + 1) ; //code.IndexOf("}",nextPBMessageIndex);
				PBMessage pbMessage = new PBMessage(code.Substring(nextPBMessageIndex,(messageEndIndex - nextPBMessageIndex) +1  ) );
				
				i =  messageEndIndex+1;
				continue;
			}

			if(nextPBEnumIndex > -1  && nextPBMessageIndex == -1){
				int blockStart = code.IndexOf("{",nextPBEnumIndex );
				int enumEndIndex = FindEnd(code,blockStart + 1);//code.IndexOf("}",nextPBEnumIndex);
				PBEnum pbEnum = new PBEnum(code.Substring(nextPBEnumIndex,(enumEndIndex - nextPBEnumIndex)+1)  );
				
				i =  enumEndIndex+1;
				continue;
			}
			
			if(nextPBMessageIndex < nextPBEnumIndex ){
				int blockStart = code.IndexOf("{",nextPBMessageIndex );
				int messageEndIndex = FindEnd(code,blockStart + 1) ; //code.IndexOf("}",nextPBMessageIndex);
				PBMessage pbMessage = new PBMessage(code.Substring(nextPBMessageIndex,(messageEndIndex - nextPBMessageIndex) +1  ) );

				i =  messageEndIndex+1;
			}else{
				int blockStart = code.IndexOf("{",nextPBEnumIndex );
				int enumEndIndex = FindEnd(code,blockStart + 1);//code.IndexOf("}",nextPBEnumIndex);
				PBEnum pbEnum = new PBEnum(code.Substring(nextPBEnumIndex,(enumEndIndex - nextPBEnumIndex)+1)  );

				i =  enumEndIndex+1;
			}

		}
	}

	static public int FindEnd(string code,int startIndex){
		string startFlag = "{";
		string endFlag   = "}";
		int index = startIndex;
		while (index < code.Length )
		{
			int nextStartFlagIndex = code.IndexOf(startFlag,index);
			int nextEndFlagIndex = code.IndexOf(endFlag,index);

			if(nextStartFlagIndex == -1){
				return nextEndFlagIndex;
			}

			string s1 = code.Substring(nextStartFlagIndex,code.Length - nextStartFlagIndex);
			string s2 = code.Substring(nextEndFlagIndex,code.Length - nextEndFlagIndex);

			if(nextStartFlagIndex < nextEndFlagIndex){
				index = nextEndFlagIndex ;
			}
			else{
				return nextEndFlagIndex;
			}
		}
		return -1;
	}

 	public CodeSymbol FindNextSymbol(string code,int startIndex,char charCode){

		int curIndex = startIndex;

		if(charArr == null)
			charArr = code.ToCharArray();
		
		while (curIndex < charArr.Length) {

			char c = charArr [curIndex];

			if (c == charCode) {
				return  new CodeSymbol(curIndex,c);
			}
			curIndex++;
		}

		return new CodeSymbol(-1,' ');
	}

	char[] charArr;

	static public CodeSymbol FindNextSymbol(string code,int startIndex){

		int curIndex = startIndex;

		//if(charArr == null)
		char[] charArr = code.ToCharArray();
		
		while(curIndex < charArr.Length){

			char c = charArr[curIndex];

			if(c == '='){
				return  new CodeSymbol(curIndex,c);
			}else
			if(c == '['){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == ']'){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == '{'){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == '}'){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == ';'){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == ' '){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == '\r'){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == '\n'){
				return new CodeSymbol(curIndex,c);
			}else
			if(c == '\t'){
				return new CodeSymbol(curIndex,c);
			}
			if(c == '/'){
				return new CodeSymbol(curIndex,c);
			}

			curIndex ++;
		}

		return new CodeSymbol(-1,' ');
	}
}
