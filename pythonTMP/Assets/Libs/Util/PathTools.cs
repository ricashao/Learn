using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;

[LuaCallCSharp]
public class PathTools {
	
	static public string UpdateRoot = "";
    /// <summary>
	/// 先检查 PersistentDataPath 热更新目录如果没有返回 streamingAssetsPath
    /// Gets the asset path for load path.
    /// AssetBundle.LoadFromFile(filePath);
    /// 方法不能加 "file://" 
    /// </summary>
    /// <returns>The asset path for load path.</returns>
    /// <param name="assetName">Asset name.</param>
    static public string GetAssetPathForLoadPath1(string assetName){
		
        string path = System.IO.Path.Combine (PersistentDataPath(), assetName);
        if (System.IO.File.Exists (path))
            return path;
        else
		#if UNITY_5
			return System.IO.Path.Combine (Application.streamingAssetsPath,assetName);
		#else 
			return System.IO.Path.Combine (AppContentPath ().Replace("file://",""), assetName);
		#endif
    }
	/// <summary>
	/// Combine the specified path0 and path1.
	/// 路径合并方法
	/// </summary>
	/// <param name="path0">Path0.</param>
	/// <param name="path1">Path1.</param>
	static public string Combine(string path0,string path1){
		if (path0.Trim ().EndsWith ("/") && path1.Trim ().StartsWith ("/")) {
			return string.Format ("{0}{1}",path0.Substring(0,path0.Length - 1),path1);
		}
		if (!path0.Trim ().EndsWith ("/") && !path1.Trim ().StartsWith ("/")) {
			return string.Format ("{0}/{1}",path0,path1);
		}

		return string.Format ("{0}{1}",path0,path1);
	}

	static public string GetStreamingAssetsPath(string assetName){
		return Combine (Application.streamingAssetsPath, assetName);
	}

	static public string GetPersistentPath(string assetName){
		return Combine (PersistentDataPath(), assetName);
	}

	static public string GetAppContentPath(string assetName){
		return Combine (AppContentPath(),assetName);
	}

	static public bool ExistsPersistentPath(string assetName){
		string path = Combine (PersistentDataPath(), assetName);

		return File.Exists (path);
	}

	static public bool ExistsStreamingAssetsPath(string assetName){
		string path = Combine (Application.streamingAssetsPath, assetName);

		return File.Exists (path);
	}

	/// <summary>
	/// Persistents the or streaming assets path.
	/// </summary>
	/// <returns>The or streaming assets path.</returns>
	/// <param name="assetName">Asset name.</param>
	static public string PersistentOrStreamingAssetsPath(string assetName){
		
		string path = Combine (PersistentDataPath(), assetName);
		if (System.IO.File.Exists (path))
			return path;
		
		return System.IO.Path.Combine (Application.streamingAssetsPath,assetName);
	}

    /// <summary>
    /// 针对 WWW www = new WWW(PathTools.GetAssetPath(name));
    /// </summary>
    /// <returns>The asset path.</returns>
    /// <param name="assetName">Asset name.</param>
	static public string GetAssetPath(string assetName){
		string path = System.IO.Path.Combine (PersistentDataPath(), assetName);
		if (System.IO.File.Exists (path))
            return "file://" + path;
		else
			return Combine (AppContentPath (), assetName);
	}
        
	// Use this for initialization
	static public string PersistentDataPath(){
		#if UNITY_EDITOR
        return Application.streamingAssetsPath;
		#else
		return Application.persistentDataPath;
		#endif
	}

	static public string AppContentPath()
	{
		string path = string.Empty;
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			path = "jar:file://" + Application.dataPath + "!/assets/";
			break;
		case RuntimePlatform.IPhonePlayer:
			path = Application.dataPath + "/Raw/";
			break;
		case RuntimePlatform.OSXEditor:
			path = Application.streamingAssetsPath+ "/";
			break;
		case RuntimePlatform.WindowsEditor:
			path = Application.streamingAssetsPath+ "/";
			break;
		default:
			path = "file://"+Application.dataPath + "/StreamingAssets/";
			break;
		}
		return path;
	}

}
