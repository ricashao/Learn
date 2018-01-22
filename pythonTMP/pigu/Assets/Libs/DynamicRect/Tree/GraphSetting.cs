using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DynamicRectThc
{
    public class GraphSetting : MonoBehaviour
    {
        /// <summary>
        /// 尺寸
        /// </summary>
        public int xNum = 8;
        public int zNum = 8;
        public int tilex = 20;
        public int tilez = 20;
        /// <summary>
        /// 设置要碰撞的物件层
        /// </summary>
        public LayerMask layerMask;
        /// <summary>
        /// Asset文件路径
        /// </summary>
        public string sceneRectDataAssetPath;
        /// <summary>
        /// prefab搜寻路径
        /// </summary>
        public string[] prefabSearchPathArr;
        // Use this for initialization
        void Start()
        {

        }
    }
}