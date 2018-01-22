using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DynamicRectThc
{
    public class GraphRun : MonoBehaviour
    {

        public string assetBundlesPath;
        public AssetBundle assetBundle;
        public AssetBundle perfabAssetBundle;
        // Use this for initialization
        void Start()
        {
            //GraphRoot.create(new GameObject("GraphRoot"), 4, 4);
            if (assetBundlesPath != null && !assetBundlesPath.Equals(""))
            {
                StartCoroutine(LoadGameSceneAsync(assetBundlesPath));
            }
        }

        //加载 Scene 
        IEnumerator LoadGameSceneAsync(string assetBundleName)
        {
            WWW download = new WWW(AppContentPath() + assetBundleName);
            yield return download;

            assetBundle = download.assetBundle;
            string[] assetPaths = assetBundle.GetAllAssetNames();

            SceneRootData sceneRootData = assetBundle.LoadAsset<SceneRootData>(assetPaths[0]);

            string goName = assetBundlesPath;
            if (assetBundlesPath.LastIndexOf("/") > -1)
            {
                goName = assetBundlesPath.Substring(assetBundlesPath.LastIndexOf("/") + 1);
            }

            GraphRoot.load(new GameObject("GraphRoot_" + goName),sceneRootData);
            
            //perfabAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath +   "/" + "map001.prefab");
        }

        public static string AppContentPath()
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    path = "file://" + Application.dataPath + "/StreamingAssets/";
                    break;
            }
            return path;
        }
    }
}