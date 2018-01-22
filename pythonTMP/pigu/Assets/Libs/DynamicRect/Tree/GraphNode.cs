using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicRectThc
{
    //[ExecuteInEditMode]
    [System.Serializable]
    public class GraphNode : MonoBehaviour
    {
        public Graph node;
        public LayerMask colliderLayer;
        int playerLayer;

        private void Start()
        {
            //colliderLayer = LayerMask.NameToLayer("Tree");
            colliderLayer = node.root.layer ;

            playerLayer = LayerMask.NameToLayer( "Player" );

        }
        /// <summary>
        /// 几个个栗子：
        /*
        LayerMask mask = 1 << 2; 表示开启Layer2。

        LayerMask mask = 0 << 5; 表示关闭Layer5。

        LayerMask mask = 1 << 2 | 1 << 8; 表示开启Layer2和Layer8。

        LayerMask mask = 0 << 3 | 0 << 7; 表示关闭Layer3和Layer7。

        上面也可以写成：

        LayerMask mask = ~（1<<3|1<<7）; 表示关闭Layer3和Layer7。

        LayerMask mask = 1 << 2 | 0 << 4; 表示开启Layer2并且同时关闭Layer4.

        */
        /// </summary>
        /// <param name="c"></param>
        void OnTriggerEnter(Collider c)
        {
            string layerName = LayerMask.LayerToName(c.gameObject.layer);

            if (layerName.Equals("Tree")) {
                Debug.LogWarning("OnTriggerEnter = " + c.ToString());
            }

            LayerMask mask = 1 << c.gameObject.layer;

            string colliderLayerStr = Convert.ToString(colliderLayer.value, 2);
            
            int layerNum = (c.gameObject.layer & colliderLayer.value);

            layerNum = mask.value & colliderLayer.value;

            if (layerNum >  0) {
                Debug.LogWarning("OnTriggerEnter = " + c.ToString());
            }

            int d = 43254320;
            if ( layerNum > 0 ) {
                Debug.LogWarning("OnTriggerEnter = " + c.ToString());
            }

            //运行时
            if (node.root.isLoadBySceneRootData)
            {
                //Debug.LogError("c.gameObject.tag = " + c.gameObject.tag);

                if (c.gameObject.tag.Equals("Player"))
                {
                    if (node.root.isLoadBySceneRootData)
                    {
                        node.center();
                    }
                }
            }
            else//编辑构建时
            //if (c.gameObject.layer == colliderLayer)
            if (layerNum > 0)
            {
                if (c.gameObject.GetComponent<Renderer>())
                {
                    //碰撞器的和Renderer在平级
                    node.add(c.gameObject);
                }
                else
                {
                    //碰撞器的父级为单个显示对象根
                    node.add(c.transform.parent.gameObject);
                }
            }
        }

        void OnTriggerExit(Collider c)
        {
            Debug.LogWarning("OnTriggerExit = " + c.ToString());
        }

        void OnCollisionEnter(Collision collision)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }

            Debug.LogWarning("OnCollisionEnter = " + collision.ToString());
        }

        void OnCollisionExit(Collision c)
        {
            Debug.LogWarning("OnCollisionExit = " + c.ToString());
        }
    }
}