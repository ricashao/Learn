using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PureMVC;

public class Launcher : MonoBehaviour {

	public string url = "file:///Users/zhuyuu3d/Documents/svn/U3D/u3d_xlua_project/Assets/StreamingAssets/md5filelist.txt";

	AsyncOperation asyncOperation ;

	void Awake(){
		//热更新url
		AssetsUpdateManager.assetsSeverUrl = "http://127.0.0.1/res/StreamingAssets";
		//保存路径
		AssetsUpdateManager.assetsUpdatePath = Application.dataPath + "/StreamingAssetsUpdate";

		#if UNITY_EDITOR
		//url =  "file://"+ Application.streamingAssetsPath +"/md5filelist.txt";
		#endif
	}

	// Use this for initialization
	void Start () {
		//CheckVersions ();
	}

	/* 检查版本更新 */
	void CheckVersions(){
		AssetsUpdateManager.I.Check(url,OnAssetsUpdateCmp);
	}

    public void BeginUpdateResource()
    {
        
    }

	// Update is called once per frame
	void OnAssetsUpdateCmp () {

		Loom.QueueOnMainThread(()=>
        {
			Debug.LogWarning("更新成功！");
			/* 跳转到 loading */
			asyncOperation = SceneManager.LoadSceneAsync (1, LoadSceneMode.Single);



		});
			
	}

	void Update () {

	}

}
