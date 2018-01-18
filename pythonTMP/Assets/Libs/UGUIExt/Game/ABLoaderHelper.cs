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
			//PathTools.PersistentOrStreamingAssetsPath (strFileName);


			string strLoadPath = PathTools.GetAssetPath(strFileName);

			Debug.Log("Load Assetbundle."+strLoadPath);

			StartCoroutine(loadresbywww(strLoadPath,(AssetBundle ab)=>
				{
					Debug.Log ("Load over");

					AssetBundle abLaunch = ab;
					if (abLaunch != null)
					{
						GameObject goret = LoadAssetFromAB(abLaunch,strassetname, goparent);
						if (onInsOver != null)
							onInsOver(goret);
						abLaunch.Unload(false);
					}
				}
			)
			);
                    

        }

		IEnumerator loadresbywww(string sname,System.Action<AssetBundle> oncb)
		{
			WWW w = new WWW(sname);
			yield return w;
			if (w.error == null) {
				oncb (w.assetBundle);
				w.Dispose ();
			} else {
				Debug.Log ("Error:" + w.error);
				w.Dispose ();
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