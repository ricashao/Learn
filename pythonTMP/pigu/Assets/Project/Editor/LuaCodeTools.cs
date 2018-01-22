using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.EventSystems;
using System;

[InitializeOnLoad]
public class LuaCodeTools {

	static string LuaCodeOutPath = "/Project/Editor/LuaCodeOut/";

	[MenuItem("Assets/Create/Lua/Module",false, 11)]
	static public void CreateBaseCodeAndDir(){

		Dictionary<string,string> repkey = new Dictionary<string, string> ();
		repkey["$ModuleMame$"] = "Lobby";//"Test";

		CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/config"  , LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/models"  , LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/modules" , LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/proto"   , LuaCodeOutPath ,repkey["$ModuleMame$"] ));
		CreateDirectory (Application.dataPath + string.Format( "{0}/{1}/services", LuaCodeOutPath ,repkey["$ModuleMame$"] ));

		CreateLuaCodeByTp (repkey, Application.dataPath + "/Project/Editor/LuaCodeTp/TP_State.lua",
			Application.dataPath + string.Format( "{0}/{1}/{1}State.lua", LuaCodeOutPath ,repkey["$ModuleMame$"] ) );
		//CreatePlugins();
	}
	/* 子模块 */
	[MenuItem("LuaCode/CreateSubModuleViewAndCtrl")]
	static public void CreateSubModuleBaseCodeAndDir(){
		
		GameObject gameObject = Selection.activeGameObject;

		if (gameObject.transform.parent == null || gameObject.transform.parent.parent == null) {
			Debug.LogError (gameObject.name + "父节点为 null 不是ui控件！");
			return;
		}
		//取父节点名作为模块名
		string moduleMame = gameObject.transform.parent.parent.name;
		string subModuleMame = gameObject.transform.parent.name;

		Dictionary<string,string> repkey = new Dictionary<string, string> ();
		repkey["$PanelName$"] = gameObject.name;
		repkey["$SubModuleMame$"] = subModuleMame;
		repkey["$ModuleMame$"] = moduleMame;

		Image imageComp = gameObject.GetComponent<Image> ();
		if (imageComp == null) {
			Debug.LogError (gameObject.name + "不是ui控件！");
			return;
		}
		//视图层	
		CreateLuaCodeByTp (repkey, Application.dataPath + "/Project/Editor/LuaCodeTp/TP_View.lua",
						Application.dataPath + 
						string.Format( "{0}/{1}/modules/{2}/{3}View.lua",
						LuaCodeOutPath ,repkey["$ModuleMame$"],repkey["$SubModuleMame$"],repkey["$PanelName$"] ) );
		//控制层
		CreateLuaCodeByTp (repkey, Application.dataPath + "/Project/Editor/LuaCodeTp/TP_Ctrl.lua",
						Application.dataPath + 
						string.Format( "{0}/{1}/modules/{2}/{3}Ctrl.lua",
						LuaCodeOutPath ,repkey["$ModuleMame$"],repkey["$SubModuleMame$"],repkey["$PanelName$"] ) );
		
		//Debug.Log (GetParentPath(null,gameObject.transform));
	}

	[MenuItem("LuaCode/Panel/AddPanelItem")]
	static public void AddPanelItem(){
		
		GameObject gameObject = Selection.activeGameObject;

		if (gameObject.transform.parent == null || gameObject.transform.parent.parent == null) {
			Debug.LogError (gameObject.name + "父节点为 null 不是ui控件！");
			return;
		}
		if (gameObject.name.EndsWith("Panel")) {
			Debug.LogError (gameObject.name + "Panel Root");
			return;
		}

		UIBehaviour uiBehaviour = gameObject.GetComponent<Button>();
		if(uiBehaviour == null)
			uiBehaviour = gameObject.GetComponent<UIBehaviour>();

		Type t = uiBehaviour.GetType ();

		string addRefCode = "";
		string addEventCode = "";
		//判断选中组件类型
		if (t.Equals (typeof(UnityEngine.UI.Image))) {

			addRefCode = string.Format ("self.{0} = self.transform:Find('{0}')", gameObject.name);
			Debug.LogWarning ("UnityEngine.UI.Image");
		} else if (t.Equals (typeof(UnityEngine.UI.Button))) {

			addRefCode = string.Format ("self.{0} = self.transform:Find('Button')", gameObject.name);
			addEventCode = string.Format ("view.{0}:GetComponent(\"Button\").onClick:AddListener(function()\n\tprint('onClick:{0}')\n\tend)", gameObject.name);
			Debug.LogWarning ("UnityEngine.UI.Button");
		} else if (t.Equals (typeof(UnityEngine.UI.Text))) {

			addRefCode = string.Format ("self.{0} = self.transform:Find('Text')", gameObject.name);
			Debug.LogWarning ("UnityEngine.UI.Text");
		} else if (t.Equals (typeof(UnityEngine.UI.InputField))) {
			
			addRefCode = string.Format ("self.{0} = self.transform:Find('InputField')", gameObject.name);
			Debug.LogWarning ("UnityEngine.UI.InputField");
		} else {

			addRefCode = string.Format ("self.{0} = self.transform:Find('{1}')", gameObject.name,t.ToString().Replace("UnityEngine.UI.",""));
			Debug.LogWarning (t.ToString());
		}

		Stack<Transform> pathStack = new Stack<Transform> (); 

		Transform curTransform = gameObject.transform;

		while(curTransform != null){

			if (curTransform.name.EndsWith("Canvas")) {
				break;
			}
			pathStack.Push (curTransform);

			curTransform = curTransform.parent;
		}

		string moduleMame = pathStack.Pop ().name;
		string subModuleMame = pathStack.Pop ().name;
		string panelName = pathStack.Pop ().name;

		Dictionary<string,string> repkey = new Dictionary<string, string> ();
		repkey["$PanelName$"] = panelName;
		repkey["$SubModuleMame$"] = subModuleMame;
		repkey["$ModuleMame$"] = moduleMame;

		Debug.LogWarning ("moduleMame:" + moduleMame + " > subModuleMame:" + subModuleMame);
		/* 视图层追加 */
		string viewLuaFile = Application.dataPath +
		                     string.Format ("{0}/{1}/modules/{2}/{3}View.lua",
			                 				LuaCodeOutPath, repkey ["$ModuleMame$"], repkey ["$SubModuleMame$"], repkey ["$PanelName$"]);

		string appendFalgText = "--AddRefCode 追加引用标志";

		string luaCode = File.ReadAllText (viewLuaFile);

		if (luaCode.IndexOf (appendFalgText) == -1) {
			Debug.LogErrorFormat ("{0} 缺少追加标志行！ ",viewLuaFile);
			return;
		}
		if (luaCode.IndexOf (addRefCode) == -1) {
			
			luaCode = luaCode.Replace (appendFalgText, string.Format ("{0}\n\t{1}", addRefCode, appendFalgText));
			File.WriteAllText (viewLuaFile, luaCode);
		} else {
			Debug.LogWarningFormat (" {0} 代码已经存在 {1} ",addRefCode,viewLuaFile);
		}
		/* 控制层追加 */
		if(addEventCode.Equals("")){
			//没有要追加的
			return;
		}
		string ctrLuaFile = Application.dataPath +
							string.Format ("{0}/{1}/modules/{2}/{3}Ctrl.lua",
										   LuaCodeOutPath, repkey ["$ModuleMame$"], repkey ["$SubModuleMame$"], repkey ["$PanelName$"]);

		appendFalgText = "--AddEventCode 追加事件标志";	

		luaCode = File.ReadAllText (ctrLuaFile);

		if (luaCode.IndexOf (appendFalgText) == -1) {
			Debug.LogErrorFormat ("{0} 缺少追加标志行！ ",ctrLuaFile);
			return;
		}

		if (luaCode.IndexOf (addEventCode) == -1) {

			luaCode = luaCode.Replace (appendFalgText, string.Format ("{0}\n\t{1}", addEventCode, appendFalgText));
			File.WriteAllText (ctrLuaFile, luaCode);
		} else {
			Debug.LogWarningFormat (" {0} 代码已经存在 {1} ",addEventCode,ctrLuaFile);
		}

	}

	[InitializeOnLoadMethod]
	static void StartInitializeOnLoadMethod()
	{
		//EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
	}

	static void OnHierarchyGUI(int instanceID, Rect selectionRect)
	{
		if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
			&& Event.current.button == 1 && Event.current.type <= EventType.mouseUp){

			GameObject selectedGameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

			UIBehaviour UIComp = selectedGameObject.GetComponent<UIBehaviour> ();
			if (UIComp == null) {
				//Debug.LogError (selectedGameObject.name + "不是ui控件！");
				return;
			}

			if (selectedGameObject) {

				Vector2 mousePosition = Event.current.mousePosition;
				//EditorUtility.DisplayPopupMenu(new Rect(mousePosition.x, mousePosition.y, 0, 0), "LuaCode/CreateSubModuleBaseCodeAndDir",null);
				EditorUtility.DisplayPopupMenu (new Rect (mousePosition.x,mousePosition.y,0,0), "LuaCode/",null);
				//Event.current.Use ();
				//Debug.Log (selectedGameObject);
			}
		}
	}

	static public string GetParentPath(Transform root,Transform go){
		string p = go.name;

		if(go.parent == root )
			return p;
		
		if ( go.parent != null) {
			return (GetParentPath (root,go.parent) +"/"+ p);
		}
	
		return p;
	}

	static public void CreateDirectory(string path){
		
		if (!Directory.Exists( path ) )
		{
			Directory.CreateDirectory(path );
		}
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
