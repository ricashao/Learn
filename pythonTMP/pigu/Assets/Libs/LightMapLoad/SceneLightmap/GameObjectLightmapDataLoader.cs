using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DynamicRectThc;
/// <summary>
/// 加载测试
/// </summary>
/*
public class GameObjectLightmapDataLoaderTest : MonoBehaviour
{
    public string prefabPath;

    void Start()
    {
        GameObjectLightmapDataLoader gameObjectLightmapDataLoader = gameObject.AddComponent<GameObjectLightmapDataLoader>();
        gameObjectLightmapDataLoader.prefabPath = prefabPath;
    }
}
*/

public class GameObjectLightmapDataLoader : MonoBehaviour {
    /// <summary>
    /// prefab path for data
    /// prefab 路径映射 prefab 光照信息
    /// </summary>
    public static Dictionary<string, GameObjectLightmapData> gameObjectLightmapDataDic = new Dictionary<string, GameObjectLightmapData>();

    public LightmapDataMappingType lightmapDataMappingType = LightmapDataMappingType.Prefab;
    public string goPath;
    public string prefabPath;
    public AssetBundle assetBundle;
    // Use this for initialization
    void Start () {
        switch (lightmapDataMappingType) {
            case LightmapDataMappingType.Path:
                LoadByPath();
                break;
            case LightmapDataMappingType.Prefab:
                LoadByPrefab();
                SetLightmapData();
                break;
        }
	}

    void LoadByPath ()
    {
    }

    void LoadByPrefab() {

        if (gameObjectLightmapDataDic.ContainsKey(prefabPath)) {
            return;
        }

        string prefabLightmapDataABPath = SceneLightmapUtil.GetPrefabLightmapDataABPath(prefabPath);

        AssetBundle assetBundle;

        if (SceneLightmapAssetBundleLoader.abPathDic.ContainsKey(prefabLightmapDataABPath))
        {
            assetBundle = SceneLightmapAssetBundleLoader.abPathDic[prefabLightmapDataABPath];
        }
        else
        {
            assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/"  + prefabLightmapDataABPath);
            SceneLightmapAssetBundleLoader.abPathDic.Add(prefabLightmapDataABPath, assetBundle);
        }

        //assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + prefabPath + "lmd");
        string[] assetNames = assetBundle.GetAllAssetNames();

        GameObjectLightmapData gameObjectLightmapData = assetBundle.LoadAsset<GameObjectLightmapData>(assetNames[0]);
        if (gameObjectLightmapDataDic.ContainsKey(gameObjectLightmapData.prefabName)){

        } else {
            gameObjectLightmapDataDic.Add(gameObjectLightmapData.prefabName, gameObjectLightmapData);
        }
    }

    public void SetLightmapData() {

        GameObjectLightmapData gameObjectLightmapData = gameObjectLightmapDataDic[prefabPath];
       
        for (int i = 0;i< gameObjectLightmapData.renderersLightmapDataList.Count;i++ ) {

            RenderersLightmapData renderersLightmapData = gameObjectLightmapData.renderersLightmapDataList[i];

            if (renderersLightmapData.m_name.Equals(gameObject.name))
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                renderer.lightmapIndex = renderersLightmapData.m_lightmapIndex;
                renderer.lightmapScaleOffset = renderersLightmapData.m_lightmapScaleOffset;
            }
            else {
                Transform transformCh = transform.Find(renderersLightmapData.m_name.Replace(gameObject.name+"/",""));
                if (transformCh == null) {
                    Debug.LogError(gameObject.name + " 光照路径不存在 " + renderersLightmapData.m_name.Replace(gameObject.name, ""));
                    continue;
                }
                Renderer renderer = transformCh.GetComponent<Renderer>();
                renderer.lightmapIndex = renderersLightmapData.m_lightmapIndex;
                renderer.lightmapScaleOffset = renderersLightmapData.m_lightmapScaleOffset;
            }
        }

    }
    /*
    private void OnGUI()
    {
        if ( GUI.Button (new Rect(0,0,  300f,26f), "SetLightmapData") ) {
            SetLightmapData();
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
    */
    private void OnDestroy()
    {
        //if(assetBundle)
        //assetBundle.Unload(true);
        gameObjectLightmapDataDic.Remove(prefabPath);
    }
}
