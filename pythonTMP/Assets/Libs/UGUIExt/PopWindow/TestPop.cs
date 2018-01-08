using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhuYuU3d;

public class TestPop : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Libs.AssetManager.getInstance().InitAssetName2abPathDic("StreamingAssets_u3d_xlua_project");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		if(GUILayout.Button("Popup a OK Btn MessageBox"))
		{
			ZhuYuU3d.MessageBox.ShowOK ("TestOK-Title", "TestOK-Message", ZhuYuU3d.MessageBox.Type.MESSAGE, (ZhuYuU3d.MessageBox.Result res) => {
				print("res:"+res.ToString());
			});
		}

		if(GUILayout.Button("Popup a OKCancel Btn MessageBox"))
		{
            ZhuYuU3d.MessageBox.ShowOKCancel ("TestOK-Title", "TestOK-Message", ZhuYuU3d.MessageBox.Type.WARNING, (ZhuYuU3d.MessageBox.Result res) => {
				print("res:"+res.ToString());
			});
		}

		 

		if(GUILayout.Button("Popup a YesNo Btn MessageBox with test"))
		{
            ZhuYuU3d.MessageBox.ShowYesNo("TestOK-Title", "TestOK-Message", ZhuYuU3d.MessageBox.Type.WARNING, (ZhuYuU3d.MessageBox.Result res) => {
				print("res:"+res.ToString());
			});
		}

		 

		if(GUILayout.Button("Popup a Totast in Bottom"))
		{
            Toast.Show (this, "Hello World", 2, Toast.Type.WARNING, Toast.Gravity.BOTTOM, 30);
		}

		if(GUILayout.Button("Popup a Totast in Top"))
		{
            Toast.Show (this, "Hello World", 2, Toast.Type.WARNING, Toast.Gravity.TOP,30);
		}

		if(GUILayout.Button("Popup a Totast in Center"))
		{
			Toast.Show (this, "Hello World", 2, Toast.Type.WARNING, Toast.Gravity.CENTER,30);
		}

	}
}
