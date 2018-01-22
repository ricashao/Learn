using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneObjectData// : ScriptableObject
{
    public string prefabName;
    public string gameObjectPath;
    public Vector3 p;
    public Quaternion r;
    public Vector3 s;
    //节点光照数据
    public SceneObjectLMData[] sceneObjectLMDataArr;
}
