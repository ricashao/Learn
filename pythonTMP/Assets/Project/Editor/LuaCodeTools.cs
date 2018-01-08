using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LuaCodeTools : MonoBehaviour {

	[MenuItem("Assets/Create/Lua/Module",false, 11)]
	static public void CreateBaseDir(){

		Dictionary<string,string> repkey = new Dictionary<string, string> ();
		repkey["$ModuleMame$"] = "Test";

		string LuaCodeOutPath = "/Project/Editor/LuaCodeOut/";

		CreateLuaCodeByTp (repkey, Application.dataPath + "/Project/Editor/LuaCodeTp/TP_State.lua",
			Application.dataPath + string.Format( "{0}/{1}/{1}State.lua", LuaCodeOutPath ,repkey["$ModuleMame$"] ) );
		
		Directory.CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/config", LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		Directory.CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/models", LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		Directory.CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/modules", LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		Directory.CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/proto"   , LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		Directory.CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/services", LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		//CreatePlugins();
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

		File.WriteAllText (outPutFileFath,tpCode);

	}
}
