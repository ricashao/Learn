using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsUpdateTest : MonoBehaviour {
    
	public string url = "file:///Users/zhuyuu3d/Documents/svn/U3D/u3d_xlua_project/Assets/StreamingAssets/md5filelist.txt";
	// Use this for initialization
	void Start () {
        //AssetsUpdateManager.I.Check("file://"+Application.streamingAssetsPath + "/" + "md5filelist.txt",OnAssetsUpdateCmp);
        AssetsUpdateManager.I.Check(url,OnAssetsUpdateCmp);
	}
	
	// Update is called once per frame
    void OnAssetsUpdateCmp () {
        Debug.LogWarning("更新成功！");
	}
}
