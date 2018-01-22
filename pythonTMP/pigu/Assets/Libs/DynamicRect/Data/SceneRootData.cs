using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneRootData : ScriptableObject
{
    public int xNum,zNum;
    public float tilex,tilez;
    public SceneRectData[] sceneRectDataArr;
    /// <summary>
    /// prefab 路径
    /// </summary>
    public string[] prefabPathArr;
    /// <summary>
    ///  prefab 打包名 下标对应路径
    /// </summary>
    public string[] prefabAssetBundleNameArr;
    //public GameObject[] prefabArr; 
}