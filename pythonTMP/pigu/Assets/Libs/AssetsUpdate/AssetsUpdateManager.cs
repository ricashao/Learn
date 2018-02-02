using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public delegate void OnAssetsUpdateCmp();

public delegate void OnAssetsUpdateProgress(DownLoadBatch dbl);


public class AssetsUpdateManager : MonoBehaviour {

    private static AssetsUpdateManager instance;
    public static AssetsUpdateManager getInstance()
    {
        if (instance == null)
        {
            GameObject gameObject = new GameObject("AssetsUpdateManager");
            DontDestroyOnLoad(gameObject);
            instance = gameObject.AddComponent<AssetsUpdateManager>();
        }
        return instance;
    }
    public static AssetsUpdateManager initForGameObject(GameObject dontDestroyOnLoadGameObject) {

        if (instance == null) {
            instance = dontDestroyOnLoadGameObject.AddComponent<AssetsUpdateManager>();
        }
        return instance;
    }
    public static AssetsUpdateManager I
    {
        get
        {
            return getInstance();
        }
    }

    /// <summary>
    /// The assets sever URL. 资源服务器 root
    /// </summary>
	static public string assetsSeverUrl = "http://127.0.0.1";
    /// <summary>
    /// The assets update path. 资源下载路径 
    /// </summary>
	static public string assetsUpdatePath ;
    /// <summary>
    /// The name of the update assets file. 本次需要更新的文件列表文件
    /// </summary>
    public string updateAssetsFileName = "assetsupdatelist.txt" ;

	string []_arrayNeedUpdate=null;

    OnAssetsUpdateCmp onAssetsUpdateCmp;

    OnAssetsUpdateProgress onAssetUpdateProgress;

    void Awake(){
        if (instance == null)
        {
            instance = this;
        }
    }

	// Use this for initialization
	static void SetPath () {
		if (assetsUpdatePath == null || assetsUpdatePath.Equals(""))
		{
			assetsUpdatePath = Application.persistentDataPath ;

			#if UNITY_EDITOR
			assetsUpdatePath = Application.dataPath + "/StreamingAssetsUpdate";
			#endif
		}
	}
	/// <summary>
	/// 初始化
	/// </summary>
	static public void Init () {

		SetPath ();

		string md5filelist = "md5filelist.txt";

		string dataPath = assetsUpdatePath;//Util.DataPath;  //数据目录
		string resPath = PathTools.AppContentPath();//Util.AppContentPath(); //游戏包资源目录

		string localMD5BakFilePath = PathTools.Combine(dataPath,"/md5filelist.txt.bak");

		string infile = PathTools.Combine( resPath , "md5filelist.txt");
		string outfile = PathTools.Combine( dataPath ,"md5filelist.txt");

		Debug.Log(infile);

		if (PathTools.ExistsPersistentPath ("md5filelist.txt")) {
			Debug.LogWarning ("非首次启动！");
		} else {
			Debug.LogWarning ("首次启动！");

			if (!Directory.Exists(dataPath))
				Directory.CreateDirectory(dataPath);

			Debug.LogErrorFormat ("localMD5BakFilePath {0} {1}",localMD5BakFilePath,File.Exists (localMD5BakFilePath));

			if (File.Exists (localMD5BakFilePath)) {
				Debug.LogErrorFormat ("本地列表文件 bak 存在！{0}", localMD5BakFilePath);
				File.Delete (localMD5BakFilePath);
			}
			if (File.Exists (localMD5BakFilePath)) {
				Debug.LogErrorFormat ("本地列表文件 bak 删除成功！{0}", localMD5BakFilePath);
			}

			string message = "正在解包文件:>md5filelist.txt";

			if (Application.platform == RuntimePlatform.Android) {
				WWW www = new WWW(infile);

				while (true){
					if (www.isDone || !string.IsNullOrEmpty(www.error)){
						System.Threading.Thread.Sleep(50); 
						if (!string.IsNullOrEmpty(www.error)){
							Debug.LogError(www.error);
						}else{
							File.WriteAllBytes(outfile, www.bytes);
							Debug.LogWarning(">>" + outfile);
						}
						break;
					}
				}

			} else File.Copy(infile, outfile, true);
		}

		string path = PathTools.Combine (PathTools. PersistentDataPath(), md5filelist);

		Debug.LogWarningFormat ("热更新步骤 1 【初始化拷贝】完成！{0} {1}",path,System.IO.File.Exists(path));
	}
	
    void UpdateAssets(string[] needUpdate){

        DownLoadBatch downLoadBatch = new DownLoadBatch("AssetsUpdateManager.UpdateAssets",
                                                                         OnUpdateAssetsCmp,
                                                                         OnDownLoadBatchError,
																		 OnDownLoadBatchProgress); 

        string line;
        for(int i = 0; i < needUpdate.Length; i++){
            line = needUpdate[i];
            if (line.IndexOf("=") >= 0)
            {
                string path = line.Substring(0, line.IndexOf("="));
                string md5 = line.Substring(line.IndexOf("=") + 1);

                //downLoadBatch.Add(Path.Combine(assetsSeverUrl,path),Path.Combine(assetsUpdatePath,path),md5);
                downLoadBatch.Add(assetsSeverUrl + path , assetsUpdatePath + path ,md5);
            }
        }

        downLoadBatch.Execute();

    }

	public void OnDownLoadBatchProgress(DownLoadBatch downLoadBatch,DownLoaderBese downLoaderBese){

        if (onAssetUpdateProgress != null)
            onAssetUpdateProgress(downLoadBatch);
	}

    void OnUpdateAssetsCmp( DownLoadBatch downLoadBatch ){

        File.AppendAllText(assetsUpdatePath+ "/" + updateAssetsFileName, string.Format("已完成！{0} ", DateTime.Now.ToString("F", new System.Globalization.CultureInfo("zh-cn")) ));

		string localMD5BakFilePath = assetsUpdatePath + "/md5filelist.txt.bak";

		string localMD5FilePath = assetsUpdatePath + "/md5filelist.txt";

        //删除临时备份文件 
		if (File.Exists (localMD5BakFilePath)) {
			if (!File.Exists (localMD5FilePath)) {
				File.Move (localMD5BakFilePath, localMD5FilePath);
				Debug.LogWarningFormat ("OnUpdateAssetsCmp add {0} ", localMD5FilePath);
			} else {
				File.Delete (localMD5BakFilePath);
				Debug.LogWarningFormat ("OnUpdateAssetsCmp delete {0} ", localMD5BakFilePath);
			}
		} else {
			Debug.LogErrorFormat ("OnUpdateAssetsCmp md5filelist.txt.bak not exists! in {0}",assetsUpdatePath);
		}

        if (onAssetsUpdateCmp != null)
        {   
            //更新完成回调
            onAssetsUpdateCmp();
        }

		if (_arrayNeedUpdate != null && _arrayNeedUpdate.Length > 0) {
			_arrayNeedUpdate = null;
		}

    }

	void OnDownLoadBatchError(DownLoadBatch downLoadBatch,DownLoaderBese downLoaderBese){
       /*
        * 如果在本次下载批次中出现错误，则还原备份的 md5filelist.txt.bak
        * 当程序再次加载时忽略本次下载结果
        */
        string localFilePath = assetsUpdatePath + "/md5filelist.txt";
        string localFilePathbak = assetsUpdatePath + "/md5filelist.txt.bak";
        //如果本地文件存在
        if (File.Exists(localFilePath) && File.Exists(localFilePathbak))
        {
            File.Delete(localFilePath);
            //还原旧列表
            File.Move(localFilePathbak, localFilePath );
        }

		if (_arrayNeedUpdate != null && _arrayNeedUpdate.Length > 0) {
			_arrayNeedUpdate = null;
		}

    }

	/// <summary>
    /// Saves down load list file.
    /// 保存更新列表
    /// </summary>
    /// <param name="needUpdate">Need update.</param>
    void SaveDownLoadListFile (string[] needUpdate) {
        //创建 "assetsupdatelist.txt" 文件
        StringBuilder fileStr = new StringBuilder();

        for(int i = 0; i < needUpdate.Length; i++){
            fileStr.AppendLine(needUpdate[i]);
        }

        string listFilePath = assetsUpdatePath + "/" + updateAssetsFileName;

        if (File.Exists(listFilePath))
        {
            File.Delete(listFilePath);
        }
        if (!Directory.Exists( Path.GetDirectoryName(listFilePath) ) )
        {
            Directory.CreateDirectory(Path.GetDirectoryName(listFilePath) );
        }

        FileStream fs =  File.Create(listFilePath);

        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(fileStr.ToString());
        fs.Write(bytes,0,bytes.Length);
        fs.Flush();
        fs.Close();

		Debug.LogWarningFormat ("本次更新列表保存到>>{0}",listFilePath);
		Debug.LogWarningFormat ("本次更新列内容>>\n{0}\n<<本次更新列内容", fileStr.ToString ());

	}

    public void Check(string url,OnAssetsUpdateCmp onAssetsUpdateCmpFun,OnAssetsUpdateProgress onassetprogress=null){
       
        onAssetsUpdateCmp = onAssetsUpdateCmpFun;

        onAssetUpdateProgress = onassetprogress;

		if (_arrayNeedUpdate != null && _arrayNeedUpdate.Length > 0) {
			DoDownLoad (_arrayNeedUpdate);
		} else {
			StartCoroutine (CheckServerFileAndLocal (url, DoDownLoad));
		}

    }

	public void DoDownLoad(string[] fileArr){
		
		if(fileArr == null||fileArr.Length<=0){

			onAssetsUpdateCmp();

			Debug.LogWarningFormat("当前不需要更新！");
			return;
		}

		SaveDownLoadListFile(fileArr);
		UpdateAssets(fileArr);
	}

	public IEnumerator CheckServerFileAndLocal(string url,Action<string[]> callBack)
    {
        WWW www = new WWW(url);
        yield return www;

		Debug.LogWarningFormat ("热更新步骤 2 【远程MD5列表下载】完成！{0} \n{1}",url,www.text);

		string localMD5BakFilePath = assetsUpdatePath + "/md5filelist.txt.bak";

		string localMD5FilePath = assetsUpdatePath + "/md5filelist.txt";
		//如果本地 bak 文件存在
		/*
		if (File.Exists (localMD5BakFilePath)) {
			Debug.LogErrorFormat ("本地列表文件 bak 存在！{0}", localMD5BakFilePath);
			//还原旧列表
			File.Copy(localMD5BakFilePath,localMD5FilePath, true);
		}
		*/
        //如果本地文件存在
		if (File.Exists(localMD5FilePath))
        {
            //备份旧列表
			File.Copy(localMD5FilePath, localMD5BakFilePath, true);
			//本地列表
			string localMD5FileText = File.ReadAllText(localMD5FilePath);

			Debug.LogWarningFormat ("热更新步骤 3 【本地MD5列表读取】完成！{0} \n{1}",localMD5FilePath,localMD5FileText);
			//创建新列表
			//File.WriteAllText(localMD5FilePath, www.text, Encoding.UTF8);
			/*
			StreamReader sr = new StreamReader(localMD5FilePath, Encoding.UTF8); 
            StringBuilder stringBuilder = new StringBuilder();
            String line;
            while ((line = sr.ReadLine()) != null) 
            {
                stringBuilder.AppendLine(line);
            }
            */
			 _arrayNeedUpdate =  CheckStr(www.text,localMD5FileText);
			StringBuilder stringBuilder = new StringBuilder();
			for(int i = 0;i<_arrayNeedUpdate.Length;i++ )
			{
				stringBuilder.AppendLine(_arrayNeedUpdate[i]);
			}

			Debug.LogWarningFormat ("热更新步骤 4 【列表比对】完成！更新文件数:{0} \n{1}",_arrayNeedUpdate.Length,stringBuilder.ToString());

			callBack(_arrayNeedUpdate);
        }
        //本地文件不存在
        else
        {
			File.WriteAllText (localMD5BakFilePath, www.text, Encoding.UTF8);

			 _arrayNeedUpdate = www.text.Split('\n');
            
			callBack(_arrayNeedUpdate);
        }
        //string[] lines = www.text.Split('\n');
    }

    public string[] CheckStr(string severFileText,string localFileText){

        HashSet<string> needUpdate = new HashSet<string>();
		Dictionary<string,string> localfileMd5Dic = new Dictionary<string, string>();
		Dictionary<string,string> severFileMd5Dic = new Dictionary<string, string>();

        string[] severLines = severFileText.Split('\n');
        string[] localLines = localFileText.Split('\n');

		Debug.LogWarningFormat ("本地列表 {0},远程列表 {1}" ,localLines.Length,severLines.Length);

//        if (severLines.Length > 0 && localLines.Length > 0)
//        {
//            if (severLines[0].Equals(localLines[0]))
//            {
//				Debug.LogWarningFormat (" 文件更新时间相等 {0} ，跳过更新！",severLines[0]);
//                return null;
//            }
//        }
//
        string line;

        for (int i = 0; i < localLines.Length; i++)
        {
            line = localLines[i];
            if(line.IndexOf("=") > 0)
				localfileMd5Dic.Add(line.Substring(0, line.IndexOf("=")), line.Substring(line.IndexOf("=") + 1));
        }

        string path;
        string localmd5 = null;
        string severMd5 = null;
        //远程列表与本地列表比较
        for (int i = 0; i < severLines.Length; i++)
        {
            line = severLines[i];

            if (line.IndexOf("=") < 0)
                continue;
            path = line.Substring(0, line.IndexOf("="));
            severMd5 = line.Substring(line.IndexOf("=") + 1);

            localmd5 = null;

			localfileMd5Dic.TryGetValue(path, out localmd5);
            //本地没有
			if (localmd5 == null) {
				needUpdate.Add (line);
				Debug.LogWarningFormat ("新增>>文件 {0}", line);
			}
            //本地与远程不同
            else if (!severMd5.Equals (localmd5)) {
				needUpdate.Add (line);
				Debug.LogWarningFormat ("更新>>本地列表 {0},远程列表 {1}", localmd5, line);
			}
        }

        string[] needUpdateFileArr = new string[needUpdate.Count];
        needUpdate.CopyTo(needUpdateFileArr);

        return needUpdateFileArr;
    }
		
	/*
	public IEnumerator CheckServerFileAndLocal(string url,Action<string[]> updatelist)
	{
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null) 
		{
			updatelist (null);
			_arrayNeedUpdate = null;
			yield break;
		}

		//string localMD5BakFilePath = assetsUpdatePath + "/md5filelist.txt.bak";

		string localMD5FilePath = assetsUpdatePath + "/md5filelist.txt";

			if (!Directory.Exists (assetsUpdatePath)) 
			{
				Directory.CreateDirectory (assetsUpdatePath);
			}

		//如果本地文件存在
		if (File.Exists(localMD5FilePath))
		{
			//备份旧列表
			//File.Copy(localMD5FilePath, localMD5BakFilePath, true);
			//创建新列表
			//File.WriteAllText(localMD5FilePath, www.text, Encoding.UTF8);


			//本地列表
			string localMD5FileText = File.ReadAllText(localMD5FilePath);

			_arrayNeedUpdate =  CheckStr(www.text,localMD5FileText);
		}
		//本地文件不存在
		else
		{
			//File.WriteAllText (localMD5BakFilePath, www.text, Encoding.UTF8);
			//_arrayNeedUpdate = www.text.Split('\n');

			Debug.LogErrorFormat ("localMD5File 文件丢失 {0} 热更新检测错误！",localMD5FilePath);

			File.WriteAllText (localMD5FilePath, www.text, Encoding.UTF8);

			_arrayNeedUpdate = www.text.Split('\n');
		}

		if (updatelist != null)
			updatelist (_arrayNeedUpdate);
		
	}*/

	public void StartCheck(string url,Action<string[]> cb)
	{
		StartCoroutine (CheckServerFileAndLocal (url,cb));
	}
		
}
