using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace DynamicRectThc
{
    [System.Serializable]
    public class RenderersLightmapData
    {
        public string m_name;

        public string perfabName;
        //在 AssetBundle 中的 Asset 名称 用于实例化
        public string lightMapName;
        //在 AssetBundle 中的 Asset 名称 用于实例化
        public string dirLightMapName;
        //AssetBundle  名称
        public string lmAssetBundleName;

        public Vector3 m_position;

        public Vector3 m_rotation;

        public Vector3 m_scale;

        public int m_lightmapIndex;

        public Vector4 m_lightmapScaleOffset;

        public int m_realtimeLightmapIndex;

        public Vector4 m_realtimeLightmapScaleOffset;

        public void ToString()
        {
            Debug.Log(lightMapName + "|" + m_name + "|" + m_position + "|" + m_rotation + "|" + m_scale + "|" + m_lightmapIndex + "|" + m_lightmapScaleOffset + "|" + m_realtimeLightmapIndex + "|" + m_realtimeLightmapScaleOffset);
        }
    }

}