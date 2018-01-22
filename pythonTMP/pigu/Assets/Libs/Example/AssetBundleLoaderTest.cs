using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoaderTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Libs.ManifestFileTools.GetManifestAssetsNames("cube");
        //Libs.ManifestFileTools.GetManifestAssetsNames("cubemt");

        //Libs.ManifestFileTools.CreateAssetsame2AssetBundle( "StreamingAssets_loadAb" );
        Dictionary<string,string> abDic = new Dictionary<string, string>();
        Libs.ManifestFileTools.ReadAssetsName2AssetBundleInDic("StreamingAssets_loadAb"+"_AssetsName2AssetBundleAll.txt" ,abDic);

        string ab = abDic["Cube"];
        Debug.Log(ab);
        //Libs.ABM.I.LoadAssetBundleManifestAdd( "StreamingAssets_loadAb" );
        //Libs.ABM.I.Load ("cube",OnABCmp);
        //GameObject.DestroyImmediate(this);

        Libs.ABM.I.Load ("scenerectdata_demo2_tree_plant_.prefab",delegate (string name,AssetBundle assetBundle){
            GameObject go = assetBundle.LoadAsset<GameObject>("Grass_A_3");
            Instantiate (go);
        });
        Libs.ABM.I.Load ("scenerectdata_demo2_tree_plant_.prefab",OnABCmp);
	}

	void OnABCmp(string name,AssetBundle ab){
		string [] ns = ab.GetAllAssetNames ();
		GameObject go = ab.LoadAsset<GameObject>(ns[0]);
		Instantiate (go);
        //Resources.UnloadAsset(go);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI(){

        if (GUI.Button(new Rect(0,0,120,50)," Load cube"))
        {
            Libs.ABM.I.Load ("cube",OnABCmp);
        }

        if (GUI.Button(new Rect(0,50,120,50)," Release cube"))
        {
            Libs.ABM.I.Release("cube");
        }

        if (GUI.Button(new Rect(0,50 * 2,120,50)," Load Sphere"))
        {
            Libs.ABM.I.Load ("Sphere",OnABCmp);
        }

        if (GUI.Button(new Rect(0,50 * 3,120,50)," Release Sphere"))
        {
            Libs.ABM.I.Release("Sphere");
        }
    }
}
