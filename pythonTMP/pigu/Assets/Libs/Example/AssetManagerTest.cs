using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManagerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Libs.AssetManager.getInstance().InitAssetName2abPathDic("StreamingAssets_loadAb");
        //Libs.AssetManager.getInstance().CreateAsync("Cube",OnCreate);
        //transform.
        //Libs.AssetManager.getInstance().CreateAsync("Grass_A_3",OnCreate);
	}

    public void OnCreate(string eventName, Object go)
    {
        GameObject obj = go as GameObject;
        GameObject objInstantiate = Instantiate(obj);
        InitGo(objInstantiate);
    }

    public void InitGo(GameObject objInstantiate){
        if (objInstantiate == null)
            return;
        objInstantiate.transform.position = new Vector3(0,2f,0);
    }

    public void InitGo2(GameObject objInstantiate){
        if (objInstantiate == null)
            return;
        objInstantiate.transform.position = new Vector3(0,2f,0);
    }
	// Update is called once per frame
	void TestLoad () {

        Libs.AM.I.CreateFromCache("Grass_A_3", delegate (string eventName, Object objInstantiateTp){
            GameObject objInstantiate = Instantiate(objInstantiateTp as GameObject);
            InitGo(objInstantiate);
        });
        
        Libs.AM.I.CreateFromCache("Cube", delegate (string eventName, Object objInstantiateTp){
            GameObject objInstantiate = Instantiate(objInstantiateTp as GameObject);
            InitGo(objInstantiate);
        });
        
		Libs.AM.I.CreateFromCache ("Cube", delegate (string eventName, Object objInstantiateTp) {
			GameObject objInstantiate = Instantiate (objInstantiateTp as GameObject);
			InitGo (objInstantiate);
		});
        
	}

    //Material material ;
   
    void OnGUI(){

        if (GUI.Button(new Rect(0, 0, 120, 50), " Load cube "))
        {
            //Libs.ABM.I.Load("cube", OnABCmp);
            TestLoad();
        }

        if (GUI.Button(new Rect(0, 50, 120, 50), " Load CubeMT Push "))
        {
            Libs.AM.I.Push();
            //TestLoad();
            //Material material = Libs.AM.I.CreateFromCache( "Assets/Libs/Example/AssetBundleLoaderTest/CubeMT.mat", delegate (string eventName, Object objInstantiateTp){
			Libs.AM.I.CreateFromCache ("CubeMT", delegate (string eventName, Object objInstantiateTp) { 
				//material = Instantiate(objInstantiateTp as Material);
				//objInstantiateTp as Material;
				GameObject.Find ("Cube").GetComponent<MeshRenderer> ().sharedMaterial = objInstantiateTp as Material;
			});
        }
            
        if (GUI.Button(new Rect(0, 50 * 2, 120, 50), " Load Pop "))
        {
            Libs.AM.I.Pop();
            //GameObject.Find("Cube").GetComponent<MeshRenderer>().sharedMaterial = null;

           
           
            //Resources.UnloadAsset(material);
            //material = null;
            //Libs.AM.I.Pop();
        }

        if (GUI.Button(new Rect(0, 50 * 3, 120, 50), " Load Cube Push "))
        {
            Libs.AM.I.Push();

            //GameObject.DestroyImmediate(GameObject.Find("Cube"));
			Libs.AM.I.CreateFromCache ("Cube", delegate (string eventName, Object objInstantiateTp) {
				GameObject objInstantiate = Instantiate (objInstantiateTp as GameObject);
				InitGo (objInstantiate);
			});
        }

    }
}
