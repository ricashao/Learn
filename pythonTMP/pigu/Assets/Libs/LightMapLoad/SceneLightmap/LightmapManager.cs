using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapManager : MonoBehaviour {

    public static LightmapManager Init() {
        LightmapManager lightmapManager = FindObjectOfType<LightmapManager>();
        if (lightmapManager == null) {
            lightmapManager = new GameObject("LightmapManager").AddComponent<LightmapManager>();
        }
        return lightmapManager;
    }

    public void LoadData(string sceneName,string lmtype="") {

    }

    public void LoadRendererLmData(string sceneName, string scenePath, string lmtype= "") {

    }

    public void LoadPrefabLmData(string sceneName, string prefabPath, string lmtype = "")
    {

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
