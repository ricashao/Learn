using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLightmapAssetBundleLoader : SceneLightmapSimpleLoader
{
    //已经加载的
    static public Dictionary<string, AssetBundle> abPathDic = new Dictionary<string, AssetBundle>();
    //lightmapassetbundle_0__map002.lm
    //public List<string> assetBundlePathArr = new List<string>();
    public HashSet<string> assetBundlePathArr = new HashSet<string>();
    public Dictionary<int, string> assetBundlePathIndexDic = new Dictionary<int, string>();
    // Use this for initialization
    override public void Start () {

        int[] keyIndex = new int[assetBundlePathIndexDic.Keys.Count];
        assetBundlePathIndexDic.Keys.CopyTo(keyIndex,0);

        //foreach (string assetBundlePath in assetBundlePathArr) {
        for (int i = 0;i< assetBundlePathIndexDic.Keys.Count;i++) {
            //按顺序加载
            string assetBundlePath = assetBundlePathIndexDic[i];
            AssetBundle assetBundle;
            if (abPathDic.ContainsKey(assetBundlePath))
            {
                assetBundle = abPathDic[assetBundlePath];
            } else {
                assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + assetBundlePath);
                abPathDic.Add(assetBundlePath, assetBundle);
            }
            //AssetBundle assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + assetBundlePath);
            string[] assetNames = assetBundle.GetAllAssetNames();
            if (assetNames.Length == 1) {
                Texture2D lightTexture2D = assetBundle.LoadAsset(assetNames[0]) as Texture2D;
                texture2DlightmapLight =  AddArr(texture2DlightmapLight, lightTexture2D);
            }
        }

        SetLightMap();
    }
	
    private void OnDestroy()
    {
        foreach (AssetBundle assetBundle in abPathDic.Values)
        {
            if(assetBundle != null)
            assetBundle.Unload(false);
        }
    }
}
