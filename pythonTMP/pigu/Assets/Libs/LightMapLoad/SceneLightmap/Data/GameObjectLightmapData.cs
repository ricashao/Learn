using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DynamicRectThc
{
    /// <summary>
    /// 一个 prefab 有多个 Renderers（子节点）
    /// </summary>
    [System.Serializable]
    public class GameObjectLightmapData : ScriptableObject
    {
        /// <summary>
        /// 为昼夜系统预留
        /// </summary>
        public string lightType;
        public string prefabName;
        public List<RenderersLightmapData> renderersLightmapDataList;
    }

    public class PrefabLightmapData : GameObjectLightmapData
    {

    }
}