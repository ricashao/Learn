using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Libs{

	public class CfgItem{

		public string cmd;
		public string value;

		public CfgItem(string cmdp,string valuep){
			cmd = cmdp;
			value = valuep;
		}
	}

	public class LibsToolsCFG{

		//public Dictionary<string,CfgItem> cfgItemDic = new Dictionary<string, CfgItem>();

		public HashSet<CfgItem> cfgItemArr = new HashSet<CfgItem>();

		public LibsToolsCFG(){

			string fileAddress = System.IO.Path.Combine(Application.dataPath + "/", "Libscfg");

			FileInfo fInfo0 = new FileInfo(fileAddress);
			string cfg = "";
			if (fInfo0.Exists) {
				StreamReader r = new StreamReader (fileAddress);
				cfg = r.ReadToEnd ();
			} else {
				EditorUtility.DisplayDialog("错误！", Application.dataPath + "/" +"Libscfg 不存在！请从libs下copy", "ok");
				return;
			}

			string[] lines = cfg.Split(new char[]{'\n'});

			for(int i = 0; i< lines.Length; i++){
				string line = lines[i].Trim();
				if (line == "" && line.Equals(""))
				{
					continue;
				}
				if (line.StartsWith("//"))
				{
					continue;
				}
				string key = line.Substring(0, line.IndexOf("="));
				string value = line.Substring(line.IndexOf("=")+1);

				CfgItem cfgItem = new CfgItem(key,value);
				cfgItemArr.Add(cfgItem);
				/*
                if (key.Equals("copyTo"))
                {
                    pathArr.Add(value);
                }*/
			}
		}

		public string GetPathList(string cmd){

			string str = "";
			foreach( CfgItem f in cfgItemArr){
				if(cmd.Equals(f.cmd))
					str+= f.value + "\n";
			}
			return str;
		}

	}

	public class LibsTools  {

		static public void CopyOut(string cmd,string file,LibsToolsCFG libsToolsCFG ){

			foreach(CfgItem cfgItem in libsToolsCFG.cfgItemArr){ 
				if (cfgItem.cmd.Equals(cmd))
				{
					FileTools.CopyDir(file, cfgItem.value);
				}
			}
			EditorUtility.DisplayDialog("LOG", "Copy 完成！", "ok");
		}

		[MenuItem("Assets/LibsTools/Copy Files")]
		static public void CopyFiles(){

			LibsToolsCFG libsToolsCFG = new LibsToolsCFG();

			foreach( CfgItem f in libsToolsCFG.cfgItemArr){
				if (f.cmd.Equals ("CopyFiles")) {

					string formPath = f.value.Substring(0, f.value.IndexOf("->"));
					string toPath = f.value.Substring(f.value.IndexOf("->")+2);

					FileTools.CopyDir( formPath ,toPath);

					Debug.Log (f.value);

					Debug.LogWarning ("CopyFiles >> " + toPath );
				}
				if (f.cmd.Equals ("CopyFile"))
				{
					string formPath = f.value.Substring(0, f.value.IndexOf("->"));
					string toPath = f.value.Substring(f.value.IndexOf("->")+2);

					int index = toPath.LastIndexOf ("/");
					string filePath = string.Empty;

					if (index != -1) {
						filePath = toPath.Substring (0, index);
					}

					if (!Directory.Exists(filePath))
					{
						Directory.CreateDirectory(filePath);
					}

					if (File.Exists(toPath))
					{
						File.Delete(toPath);
					}

					File.Copy(formPath, toPath);

					Debug.Log (f.value);

					Debug.LogWarning ("CopyFile >> " + toPath );
				}
			}

		}

		[MenuItem("Assets/LibsTools/Copy Libs To Other")]
		static public void CopyLibsToOther(){

			LibsToolsCFG libsToolsCFG = new LibsToolsCFG();

			if (EditorUtility.DisplayDialog("LOG", "确定将 Libs Copy 到 " + libsToolsCFG.GetPathList("CopyTo"), "ok", "cancel")){
				CopyOut("CopyTo", Application.dataPath + "/Libs" , libsToolsCFG);
			}

		}

		[MenuItem("Assets/LibsTools/Copy Assets To Other")]
		static public void CopyAssetsToOther(){

			LibsToolsCFG libsToolsCFG = new LibsToolsCFG();

			if (EditorUtility.DisplayDialog("LOG", "确定将 Assets Copy 到 " + libsToolsCFG.GetPathList("CopyAssetsTo"), "ok", "cancel")){
				CopyOut("CopyAssetsTo", Application.dataPath  , libsToolsCFG);
			}
			if (EditorUtility.DisplayDialog("LOG", "确定将 ProjectSettings Copy 到 " + libsToolsCFG.GetPathList("CopyProjectSettingsTo"), "ok", "cancel")){
				CopyOut("CopyProjectSettingsTo", Application.dataPath.Replace("Assets","ProjectSettings")  , libsToolsCFG);
			}
		}

		[MenuItem("Assets/LibsTools/Copy ProjectSettings To Other")]
		static public void CopyProjectSettingsToOther(){

			LibsToolsCFG libsToolsCFG = new LibsToolsCFG();

			if (EditorUtility.DisplayDialog("LOG", "确定将 ProjectSettings Copy 到 " + libsToolsCFG.GetPathList("CopyProjectSettingsTo"), "ok", "cancel")){
				CopyOut("CopyProjectSettingsTo", Application.dataPath.Replace("Assets","ProjectSettings")  , libsToolsCFG);
			}

		}

		[MenuItem("Assets/LibsTools/Copy StreamingAssetsPath To Other")]
		static public void CopyStreamingAssetsPathToOther(){

			LibsToolsCFG libsToolsCFG = new LibsToolsCFG();

			if (EditorUtility.DisplayDialog("LOG", "确定 Copy 到 " + libsToolsCFG.GetPathList("StreamingAssetsTo"), "ok", "cancel")){
				CopyOut("StreamingAssetsTo", Application.streamingAssetsPath , libsToolsCFG);
			}

		}

	}

}