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

    OnAssetsUpdateCmp onAssetsUpdateCmp;

    OnAssetsUpdateProgress onAssetUpdateProgress;

    void Awake(){
        if (instance == null)
        {
            instance = this;
        }
    }

	// Use this for initialization
	void Start () {
        if (assetsUpdatePath == null || assetsUpdatePath.Equals(""))
        {
            assetsUpdatePath = Application.persistentDataPath ;

            #if UNITY_EDITOR
            assetsUpdatePath = Application.dataPath + "/StreamingAssetsUpdate";
            #endif
        }
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

        StartCoroutine(DownLoad(url,
                               		(fileArr)=>{
												if(fileArr == null||fileArr.Length<=0){

													onAssetsUpdateCmp();
                                   					                 
													Debug.LogWarningFormat("当前不需要更新！");
                                                    return;
                                                }
        
                                                SaveDownLoadListFile(fileArr);
                                                UpdateAssets(fileArr);
                                    }
                                )
                       );
    }

    public IEnumerator DownLoad(string url,Action<string[]> callBack)
    {
        WWW www = new WWW(url);
        yield return www;

		string localMD5BakFilePath = assetsUpdatePath + "/md5filelist.txt.bak";

		string localMD5FilePath = assetsUpdatePath + "/md5filelist.txt";
		//如果本地 bak 文件存在
		if (File.Exists (localMD5BakFilePath)) {
			//还原旧列表
			File.Copy(localMD5BakFilePath,localMD5FilePath, true);
		}
		//如果本地文件存在
		if (!File.Exists(localMD5FilePath)){
			File.WriteAllBytes (localMD5FilePath,ReadRes.ReadByte("/md5filelist.txt"));
		}
        //如果本地文件存在
		if (File.Exists(localMD5FilePath))
        {
            //备份旧列表
			File.Copy(localMD5FilePath, localMD5BakFilePath, true);
			//创建新列表
			//File.WriteAllText(localMD5FilePath, www.text, Encoding.UTF8);

			StreamReader sr = new StreamReader(localMD5FilePath, Encoding.UTF8); 
            StringBuilder stringBuilder = new StringBuilder();
            String line;
            while ((line = sr.ReadLine()) != null) 
            {
                stringBuilder.AppendLine(line);
            }

            string[] fileArr =  CheckStr(www.text,stringBuilder.ToString());
            callBack(fileArr);
        }
        //本地文件不存在
        else
        {
			File.WriteAllText (localMD5BakFilePath, www.text, Encoding.UTF8);

            //Dictionary<string,string> fileMd5Dic = new Dictionary<string, string>();

            string[] severLines = www.text.Split('\n');
            /*
            string line;
            for(int i =0 ; i < severLines.Length; i++){
                line = severLines[i];
                if(line.IndexOf("=") >= 0)
                fileMd5Dic.Add( line.Substring( 0,line.IndexOf("=") ), line.Substring( line.IndexOf("=") + 1 ));
            }
            string[] fileArr = new string[fileMd5Dic.Keys.Count];
            fileMd5Dic.Keys.CopyTo(fileArr,0);
            */
            callBack(severLines);
        }
        //string[] lines = www.text.Split('\n');
    }

    public string[] CheckStr(string severFileText,string localFileText){

        HashSet<string> needUpdate = new HashSet<string>();
        Dictionary<string,string> fileMd5Dic = new Dictionary<string, string>();
		Dictionary<string,string> severFileMd5Dic = new Dictionary<string, string>();

        string[] severLines = severFileText.Split('\n');
        string[] localLines = localFileText.Split('\n');

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
                fileMd5Dic.Add(line.Substring(0, line.IndexOf("=")), line.Substring(line.IndexOf("=") + 1));
        }
		/*
		for(int i = 0; i < severLines.Length; i++){
			line = severLines[i];
			if(line.IndexOf("=") > 0)
				severFileMd5Dic.Add(line.Substring(0, line.IndexOf("=")), line.Substring(line.IndexOf("=") + 1));
		}*/

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

            fileMd5Dic.TryGetValue(path, out localmd5);
            //本地没有
            if (severMd5 == null)
            {
                needUpdate.Add(line);

            }
            //本地与远程不同
            else if (!severMd5.Equals(localmd5))
            {
                needUpdate.Add(line);
            }
			/*
			string strfilepath = assetsUpdatePath + path;
			if (!File.Exists (strfilepath)) 
			{
				needUpdate.Add (line);
			}
			*/
        }

        string[] needUpdateFileArr = new string[needUpdate.Count];
        needUpdate.CopyTo(needUpdateFileArr);

        return needUpdateFileArr;
    }


	string []_arrayNeedUpdate=null;

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

		string localMD5BakFilePath = assetsUpdatePath + "/md5filelist.txt.bak";

		string localMD5FilePath = assetsUpdatePath + "/md5filelist.txt";

			if (!Directory.Exists (assetsUpdatePath)) 
			{
				Directory.CreateDirectory (assetsUpdatePath);
			}

		//如果本地文件存在
		if (File.Exists(localMD5FilePath))
		{
			//备份旧列表
			File.Copy(localMD5FilePath, localMD5BakFilePath, true);
			//创建新列表
			//File.WriteAllText(localMD5FilePath, www.text, Encoding.UTF8);

			StreamReader sr = new StreamReader(localMD5FilePath, Encoding.UTF8); 
			StringBuilder stringBuilder = new StringBuilder();
			String line;
			while ((line = sr.ReadLine()) != null) 
			{
				stringBuilder.AppendLine(line);
			}

			_arrayNeedUpdate =  CheckStr(www.text,stringBuilder.ToString());
		}
		//本地文件不存在
		else
		{
			File.WriteAllText (localMD5BakFilePath, www.text, Encoding.UTF8);

			_arrayNeedUpdate = www.text.Split('\n');
		}

		if (updatelist != null)
			updatelist (_arrayNeedUpdate);
	}

	public void StartCheck(string url,Action<string[]> cb)
	{
		StartCoroutine (CheckServerFileAndLocal (url,cb));
	}


}
