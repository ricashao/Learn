using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DynamicRectThc;
/*
public class SceneLightmapSceneObjectDataLoaderTest : MonoBehaviour {
    void Start() {
    
    }
}
*/
public class SceneLightmapSceneObjectDataLoader : SceneLightmapAssetBundleLoader
{
    public string sceneName;
    public string lmType = "";
    public string[] prefabPathArr;

    public static SceneLightmapSceneObjectDataLoader Create(string[] prefabPathArrp) {
        SceneLightmapSceneObjectDataLoader sceneLightmapSceneObjectDataLoader = FindObjectOfType<SceneLightmapSceneObjectDataLoader>();
        if (sceneLightmapSceneObjectDataLoader == null) {
            sceneLightmapSceneObjectDataLoader = new GameObject("SceneLightmapSceneObjectDataLoader").AddComponent<SceneLightmapSceneObjectDataLoader>();
        }
        sceneLightmapSceneObjectDataLoader.Init(prefabPathArrp);
        return sceneLightmapSceneObjectDataLoader;
    }

    public void Init(string[] prefabPathArrp)
    {
        prefabPathArr = prefabPathArrp;
    }

    // Use this for initialization
    override public void Start()
    {
        //从 prefabPathArr 列表 获取所有的 prefab 的光照信息
        for (int i = 0;i< prefabPathArr.Length;i++ )
        {
            string prefabLightmapDataABPath = SceneLightmapUtil.GetPrefabLightmapDataABPath(prefabPathArr[i]);

            AssetBundle assetBundle;

            if (abPathDic.ContainsKey(prefabLightmapDataABPath))
            {
                assetBundle = abPathDic[prefabLightmapDataABPath];
            } else {
                assetBundle = AssetBundle.LoadFromFile (Application.streamingAssetsPath + "/" + prefabLightmapDataABPath);
                abPathDic.Add(prefabLightmapDataABPath, assetBundle);
            }

            string[] allAssetNames = assetBundle.GetAllAssetNames();
            GameObjectLightmapData gameObjectLightmapData = assetBundle.LoadAsset<GameObjectLightmapData>(allAssetNames[0]);
            //加入路径列表
            AddLightMap(gameObjectLightmapData, sceneName, lmType);
        }

        LoadLightMap();
    }

    public void AddLightMap(GameObjectLightmapData gameObjectLightmapData, string sceneName , string lmtype = "")
    {
        for (int i = 0;i < gameObjectLightmapData.renderersLightmapDataList.Count;i++)
        {
            RenderersLightmapData renderersLightmapData = gameObjectLightmapData.renderersLightmapDataList[i];
            string texAssetBundlePath = SceneLightmapUtil.GetLightmapTexABPath(sceneName, renderersLightmapData.m_lightmapIndex, lmtype);
            if (!assetBundlePathIndexDic.ContainsKey(renderersLightmapData.m_lightmapIndex))
            {
                assetBundlePathIndexDic.Add(renderersLightmapData.m_lightmapIndex, texAssetBundlePath);
                assetBundlePathArr.Add(texAssetBundlePath);
            }
        }
    }

    public void LoadLightMap()
    {
        //foreach (string assetBundlePath in assetBundlePathArr) {
        for (int i = 0; i < assetBundlePathIndexDic.Keys.Count; i++)
        {
            //按顺序加载
            string assetBundlePath = assetBundlePathIndexDic[i];
            
            AssetBundle assetBundle;

            if (abPathDic.ContainsKey(assetBundlePath))
            {
                assetBundle = abPathDic[assetBundlePath];
            }
            else
            {
                assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + assetBundlePath);
                abPathDic.Add(assetBundlePath, assetBundle);
            }
            //AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + assetBundlePath);
            string[] assetNames = assetBundle.GetAllAssetNames();
            if (assetNames.Length == 1)
            {
                Texture2D lightTexture2D = assetBundle.LoadAsset(assetNames[0]) as Texture2D;
                texture2DlightmapLight = AddArr(texture2DlightmapLight, lightTexture2D);
            }
        }

        SetLightMap();
    }

    // Update is called once per frame
    /*
    void Update () {
		
	}
    */

  
}
