using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;

public delegate void OnDownLoadProgress(DownLoaderBese downLoader);
public delegate void OnDownLoadComplete(DownLoaderBese downLoader);
public delegate void OnDownLoadError(DownLoaderBese downLoader,Exception e);
//public delegate void OnDownLoadProgress();
/// <summary>
/// Down loader bese.下载器基类
/// </summary>
public class DownLoaderBese{
    
    public string url ;
    public string saveFile;
    public string md5;
    protected bool isFree;
	public long contentLength = 1;
	public long readLength = 0;

	public OnDownLoadProgress onDownLoadProgress;
	public OnDownLoadComplete onDownLoadComplete;
	public OnDownLoadError onDownLoadError;

    public virtual void Execute(){
    
    }

	public virtual void Update(){
		if (onDownLoadProgress != null) {
			onDownLoadProgress (this);
		}
	}

    public virtual bool IsComplete(){

        return true;
    }

    protected virtual void Free(){
        isFree = true;
    }

    public virtual bool IsFree(){

        return isFree;
    }

	public Double progress{
		get { 
			return (Double)readLength / (Double)contentLength;
		}
	}
}
/// <summary>
/// Http web request down loader. HttpWebRequest 下载器
/// </summary>
public class HttpWebRequestDownLoader:DownLoaderBese{

    bool isComplete = false;
    bool executeSync = false;

	public HttpWebRequestDownLoader(string urlp ,string saveFilep, bool executeSyncp = true){
        
        url = urlp;
        saveFile = saveFilep;
        executeSync = executeSyncp;
        isFree = false;
    } 

    public override void Execute(){
       // 异步执行
        if (executeSync)
        {
			Loom.QueueOnMainThread(()=>{
                
                HttpDownload(url, saveFile);
                Loom.QueueOnMainThread(()=>{
					Free();
                });

            });

        }else
        {
            HttpDownload(url, saveFile);
        }
    }

    public override bool IsComplete(){

        return true;
    }

    bool HttpDownload(string url, string path)
    {
        string tempPath = System.IO.Path.GetDirectoryName(path);
        string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
        //System.IO.Directory.CreateDirectory(tempPath);  //创建临时文件目录
        string tempFile = tempPath + "/" + fileName + ".temp"; //临时文件

        if (System.IO.File.Exists(tempFile))
        {
            System.IO.File.Delete(tempFile);    //存在则删除
        }

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);    //存在则删除
        }

		if (!System.IO.Directory.Exists(tempPath))
		{
			System.IO.Directory.CreateDirectory (tempPath);
		}
        /*
        if (!System.IO.Directory.Exists(path))
        {
            System.IO.Directory.CreateDirectory (path);
        }*/

        try
        {
            FileStream fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //request.AddRange

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            //如果返回的response头中Content-Range值为空，说明服务器不支持Range属性，不支持断点续传,返回的是所有数据
            if (response.Headers["Content-Range"] == null)
            {
                Debug.LogWarning("不支持断点续传!");
            }
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
			//获取文件大小
			if (response.StatusCode == HttpStatusCode.OK)
			{
				contentLength = response.ContentLength;
			}
            //创建本地文件写入流
            //Stream stream = new FileStream(tempFile, FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                //stream.Write(bArr, 0, size);
                fs.Write(bArr, 0, size);

				readLength += size;

                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            //stream.Close();
            fs.Close();
            responseStream.Close();
            System.IO.File.Move(tempFile, path);

            isComplete = true;

			if(onDownLoadComplete != null) 
				onDownLoadComplete(this);

            return isComplete;
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat (" url {0} , path {1} LogErrorFormat {2} ", url ,path  , ex.ToString() );

            Free();

			if (onDownLoadError != null)
				onDownLoadError (this,ex);

            return false;
        }
        //isComplete = true;
    }

}
/// <summary>
/// Unity WWW request.下载器
/// </summary>
public class UnityWWWRequest:DownLoaderBese{

}

public delegate void OnDownLoadBatchProgress(DownLoadBatch downLoadBatch,DownLoaderBese downLoaderBese);
public delegate void OnDownLoadBatchOneComplete(DownLoadBatch downLoadBatch,DownLoaderBese downLoaderBese);
public delegate void OnDownLoadBatchComplete(DownLoadBatch downLoadBatch);
public delegate void OnDownLoadBatchError(DownLoadBatch downLoadBatch,DownLoaderBese downLoaderBese);
/// <summary>
/// 下载批
/// </summary>
public class DownLoadBatch{
	
	public string name;
	public List<DownLoaderBese> downLoaderArr = new List<DownLoaderBese> ();
	public OnDownLoadBatchProgress onDownLoadBatchProgress;
	public OnDownLoadBatchOneComplete onDownLoadBatchOneComplete;
	public OnDownLoadBatchComplete onDownLoadBatchComplete;
    public OnDownLoadBatchError onDownLoadBatchError;
	public int completeCount = 0;

    public string exception;

	public DownLoadBatch(string namep , OnDownLoadBatchComplete onDownLoadBatchCompletep , OnDownLoadBatchError onDownLoadBatchErrorp,OnDownLoadBatchProgress onDownLoadBatchProgressp){
		name = namep;

		onDownLoadBatchComplete = onDownLoadBatchCompletep;
        onDownLoadBatchError = onDownLoadBatchErrorp;
		onDownLoadBatchProgress = onDownLoadBatchProgressp;
	}

	public void Add(string url ,string saveFile,string md5 = null,OnDownLoadComplete onDownLoadComplete = null,OnDownLoadError onDownLoadError = null,OnDownLoadProgress onDownLoadProgress = null){

		HttpWebRequestDownLoader downLoader = new HttpWebRequestDownLoader (url, saveFile, true);
        downLoader.md5 = md5;

		downLoader.onDownLoadComplete = OnDownLoadComplete;
		if (onDownLoadComplete != null) {
			downLoader.onDownLoadComplete += onDownLoadComplete;
		}
        
		downLoader.onDownLoadError = OnDownLoadError;
		if (onDownLoadError != null) {
			downLoader.onDownLoadError += onDownLoadError;
		}

		downLoader.onDownLoadProgress = OnDownLoadProgress;
		if (onDownLoadProgress != null) {
			downLoader.onDownLoadProgress += onDownLoadProgress;
		}

		downLoaderArr.Add (downLoader);
	}

	public Double progress{
		get { 
			return downLoaderArr.Count == 0 ? 0 : (Double)completeCount / (Double)downLoaderArr.Count;
		}
	}

	void OnDownLoadProgress(DownLoaderBese downLoaderBese){
		
		if (onDownLoadBatchProgress != null) {
			onDownLoadBatchProgress (this,downLoaderBese);
		}
	}

	void OnDownLoadComplete(DownLoaderBese downLoaderBese){

		if (onDownLoadBatchOneComplete != null) {
			onDownLoadBatchOneComplete (this,downLoaderBese);
		}

		completeCount++;

		if (completeCount == downLoaderArr.Count) {
			if (onDownLoadBatchComplete != null) {
				onDownLoadBatchComplete (this);

				DownLoadManager.I.RemDownLoadBatch(this);
			}
		}
	}

    void OnDownLoadError(DownLoaderBese downLoaderBese,Exception ex){

        exception = ex.ToString();

        Debug.LogErrorFormat(" 批下载错误 {0} " ,name);

        if (onDownLoadBatchError != null){
			onDownLoadBatchError(this,downLoaderBese);
        }
    }

	public virtual void Execute(){
		DownLoadManager.I.AddDownLoadBatch (this);
	}
}
/*
	string url = "https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo_top_ca79a146.png";
    string saveFile = Application.streamingAssetsPath + "/" + System.IO.Path.GetFileName(url);//url.Substring(url.LastIndexOf("/") + 1);
    DownLoadManager.I.Add(url,saveFile);
*/
public class DownLoadManager : MonoBehaviour {

    private static DownLoadManager instance;
    public static DownLoadManager getInstance()
    {
        if (instance == null){
            GameObject gameObject = new GameObject("DownLoadManager");
            DontDestroyOnLoad(gameObject);
            instance = gameObject.AddComponent<DownLoadManager>();
        }
        return instance;
    }
    public static DownLoadManager initForGameObject(GameObject dontDestroyOnLoadGameObject) {

        if (instance == null) {
            instance = dontDestroyOnLoadGameObject.AddComponent<DownLoadManager>();
        }
        return instance;
    }
    public static DownLoadManager I
    {
        get
        {
			return  getInstance();
        }
    }
        
    Queue<DownLoaderBese> queueWaitting = new Queue<DownLoaderBese>();
    public int maxWorking  = 8;
    DownLoaderBese[] queueWorking  = new DownLoaderBese[32];

	List<DownLoadBatch> downLoadBatchs = new List<DownLoadBatch>();

	// Use this for initialization
	//void Start () {
		/*
		DownLoadBatch downLoadBatch = new DownLoadBatch ("DownLoadBatch", OnDownLoadBatchCmp);
		string url = "https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo_top_ca79a146.png";
		string saveFile = Application.streamingAssetsPath + "/" + System.IO.Path.GetFileName(url);
		downLoadBatch.Add(url, saveFile);
		downLoadBatch.Execute ();
        /*
        string url = "https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo_top_ca79a146.png";
        string saveFile = Application.streamingAssetsPath + "/" + System.IO.Path.GetFileName(url);//url.Substring(url.LastIndexOf("/") + 1);
        Add(url,saveFile);
        */
	//}

    /*
	void OnDownLoadBatchCmp(DownLoadBatch downLoadBatch){
		Debug.Log (downLoadBatch.name);
	}
    */

	public void clear(){
		
		queueWaitting.Clear();
	}
	 
	public void Add(string url ,string saveFile,OnDownLoadComplete onDownLoadComplete = null,OnDownLoadError onDownLoadError = null){
		
		HttpWebRequestDownLoader httpWebRequestDownLoader = new HttpWebRequestDownLoader(url,saveFile,true);
		httpWebRequestDownLoader.onDownLoadComplete = onDownLoadComplete;
		httpWebRequestDownLoader.onDownLoadError = onDownLoadError;

		AddDownLoader (httpWebRequestDownLoader);
		/*
        int freeIndex = GetFreeIndex();

        if (freeIndex != -1)
        {
            queueWorking[freeIndex] = httpWebRequestDownLoader;
            httpWebRequestDownLoader.Execute();
        }
        else
        {
            queueWaitting.Enqueue(httpWebRequestDownLoader);
        }*/
    }

	public void AddDownLoader(DownLoaderBese downLoader){
		
		int freeIndex = GetFreeIndex();

		if (freeIndex != -1)
		{
			queueWorking[freeIndex] = downLoader;
			downLoader.Execute();
		}
		else
		{
			queueWaitting.Enqueue(downLoader);
		}
	} 

	public void AddDownLoadBatch(DownLoadBatch downLoadBatch){
		
		for(int  i =0 ;i < downLoadBatch.downLoaderArr.Count ;i++){
			AddDownLoader (downLoadBatch.downLoaderArr [i]);
		}

		downLoadBatchs.Add(downLoadBatch);
	}

	public void RemDownLoadBatch(DownLoadBatch downLoadBatch){
		
		downLoadBatchs.Remove (downLoadBatch);
	}

    public int GetFreeIndex(){
        for (int i = 0; i < maxWorking && i < queueWorking.Length; i++)
        {
            if (queueWorking[i] == null || queueWorking[i].IsFree())
            {
                return i;
            }
        }
        return -1;
    }

	// Update is called once per frame
	void Update () {
		
        for(int i=0;i< maxWorking && i<queueWorking.Length;i++){
            if (queueWorking[i] != null)
            {
				if (queueWorking [i].IsFree ()) {
					queueWorking [i] = null;
					if (queueWaitting.Count > 0) {
						DownLoaderBese downLoaderBese = queueWaitting.Dequeue ();
						downLoaderBese.Execute ();
						queueWorking [i] = downLoaderBese;
					}
				} else {
					queueWorking [i].Update ();
					Debug.LogFormat ("downLoader progress : "+queueWorking [i].url +" >>>> "+ queueWorking [i].progress);
				}

            }
        }
	}
   
}
