using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhuYuU3d;
using System;

namespace ZhuYuU3d
{
    public enum LoadResourceWay
    {
        FromResource,
        FromAssetbundle,
    };

    public class ResourceLoader : DDOLSingleton<ResourceLoader> {

        // Use this for initialization
        void Start() {

        }

        public UnityEngine.Object LoadInstanceAsset(string strPath,Action<UnityEngine.Object> onLoadOver=null,LoadResourceWay lrw=LoadResourceWay.FromResource)
        {
            UnityEngine.Object objRet = null;

            switch (lrw)
            {
                case LoadResourceWay.FromResource:
                    if (onLoadOver == null)
                    {
                        objRet=ResManager.Instance.LoadInstance(strPath);
                    }
                    else
                    {
                        ResManager.Instance.LoadCoroutineInstance(strPath,onLoadOver);
                    }
                    break;
                case LoadResourceWay.FromAssetbundle:
                    string sAssetName=System.IO.Path.GetFileName(strPath);
                    
                    Libs.AM.I.CreateFromCache(sAssetName, (string assetName,UnityEngine.Object objInstantiateTp)=>
                    {
                        Debug.Log("AssetName:" + assetName);
                        UnityEngine.Object objInstantiate = Instantiate(objInstantiateTp);
                        objRet = objInstantiate;
                        if (onLoadOver != null)
                        {
                           onLoadOver(objInstantiate);
                        }
                    }
                    );
                    break;
                default:
                    break;
            }

            return objRet;
        }


}

}
