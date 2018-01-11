using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhuYuU3d;
using System.IO;
using Libs;

namespace ZhuYuU3d.Game
{ 
    public class ABLoaderHelper : DDOLSingleton<ABLoaderHelper>
    {

	    // Use this for initialization
	    void Start () {
		
	    }

        public void LoadAB(string strFileName,GameObject goparent,string strassetname,System.Action<GameObject> onInsOver)
        {
            string strLoadPath = Application.persistentDataPath + strFileName;
            if (File.Exists(strLoadPath))
            {
                AssetBundleManagar.getInstance().LoadOne(strFileName, (string path, AssetBundle assetBundle) =>
                {
                    Debug.Log("Load Over");
                    GameObject goret=LoadAssetFromAB(assetBundle,strassetname, goparent);
                    if (onInsOver != null)
                        onInsOver(goret);
                }
                );
            }
            else
            {
                strLoadPath = Application.streamingAssetsPath + strFileName;
                if (File.Exists(strLoadPath))
                {
                    AssetBundle abLaunch = AssetBundle.LoadFromFile(strLoadPath);
                    if (abLaunch != null)
                    {
                        GameObject goret = LoadAssetFromAB(abLaunch,strassetname, goparent);
                        if (onInsOver != null)
                            onInsOver(goret);
                        abLaunch.Unload(false);
                    }
                }

            }
        }

        GameObject LoadAssetFromAB(AssetBundle assetBundle,string strassetname, GameObject goparent)
        {
            UnityEngine.Object obj = assetBundle.LoadAsset(strassetname);
            if (obj != null)
            {
                GameObject goCanvas = goparent;
                if (goCanvas != null)
                {
                    GameObject goLaunchPanel = (GameObject)GameObject.Instantiate<UnityEngine.Object>(obj);
                    goLaunchPanel.transform.SetParent(goCanvas.transform, false);
                    return goLaunchPanel;
                }
                else
                {
                    GameObject goLaunchPanel = (GameObject)GameObject.Instantiate<UnityEngine.Object>(obj);
                    goLaunchPanel.transform.SetAsLastSibling();
                    return goLaunchPanel;
                }
            }
            return null;
        }

    }
}