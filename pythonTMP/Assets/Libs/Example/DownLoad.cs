using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

public class DownLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//DownLoadManager.I.Add();

		DownLoadBatch downLoadBatch = new DownLoadBatch ("DownLoadBatch",
														OnDownLoadBatchCmp,OnDownLoadBatchError,OnDownLoadProgress);
		string url = "https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo_top_ca79a146.png";
		string saveFile = Application.streamingAssetsPath + "/" + System.IO.Path.GetFileName(url);
		downLoadBatch.Add(url, saveFile);
		string url2 = "https://gss0.bdstatic.com/5bVWsj_p_tVS5dKfpU_Y_D3/res/r/image/2017-09-15/ecdbc5929ce5b9e974ea1b876875df6a.png";
		string saveFile2 = Application.streamingAssetsPath + "/" + System.IO.Path.GetFileName(url2);
		downLoadBatch.Add(url2, saveFile2);

		string url3 = "http://127.0.0.1/thunder_mac_3.1.7.3266.dmg";
		string saveFile3 = Application.streamingAssetsPath + "/" + System.IO.Path.GetFileName(url3);
		//downLoadBatch.Add(url3, saveFile3);

		downLoadBatch.Execute();

		UpZip("/Library/WebServer/Documents/data.zip","/Users/zhuyuu3d/Documents/webroot/unzip");
	}

	void OnDownLoadProgress(DownLoadBatch downLoadBatch,DownLoaderBese downLoaderBese){
	
	}

	void OnDownLoadBatchCmp(DownLoadBatch downLoadBatch){
		Debug.Log ("下载完成: " + downLoadBatch.name);
	}

	void OnDownLoadBatchError(DownLoadBatch downLoadBatch,DownLoaderBese downLoaderBese){
		Debug.LogError ("下载错误! "+downLoadBatch.name);
    }

	void UpZip(string filePath,string outPath){
	
		using (ZipInputStream s = new ZipInputStream( File.OpenRead( filePath ) )	
		) {

			ZipEntry theEntry;
			while ((theEntry = s.GetNextEntry()) != null) {

				Debug.LogFormat ("{0} 解压缩 -> {1}",filePath,theEntry.Name);
				//Console.WriteLine(theEntry.Name);

				if (theEntry.Name.IndexOf ("__MACOSX") > -1) {
					continue;
				}

				string directoryName = Path.GetDirectoryName(theEntry.Name);
				//string directoryName = outPath;
				string fileName      = Path.GetFileName(theEntry.Name);

				// create directory
				if ( directoryName.Length > 0 ) {
					Directory.CreateDirectory(Path.Combine(outPath ,directoryName));
				}

				if (fileName != string.Empty) {
					using (FileStream streamWriter = File.Create( Path.Combine(outPath ,theEntry.Name) ) ) {

						int size = 2048;
						byte[] data = new byte[2048];
						while (true) {
							size = s.Read(data, 0, data.Length);
							if (size > 0) {
								streamWriter.Write(data, 0, size);
							} else {
								break;
							}
						}
					}
				}
			}
		}
	
	}

	// Update is called once per frame
	void Update () {
		
	}
}
