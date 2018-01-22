using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace DynamicRectThc
{
    public class Tree4
    {

        static public int depth = 2;//4^2 = 16
        static public float tile_w;
        static public float tile_h;

        public List<Tree4> leavesNode = new List<Tree4>();

        public int curDepth;

        public Tree4 root;
        public Rect rect;

        public Tree4 parent;

        public Tree4 node0;
        public Tree4 node1;
        public Tree4 node2;
        public Tree4 node3;

        public Tree4 node_0000;
        public Tree4 node_0130;
        public Tree4 node_0300;
        public Tree4 node_0430;

        public Tree4 node_0600;
        public Tree4 node_0730;
        public Tree4 node_0900;
        public Tree4 node_1030;

        public GameObject nodeGameObject;

        public Dictionary<int, GameObject> goDic = new Dictionary<int, GameObject>();
        //public List<GameObject> goList = new List<GameObject>();
        public Tree4(Rect rectp, int curDepthp, Tree4 parentp = null, Tree4 rootp = null)
        {
            rect = rectp;
            curDepth = curDepthp;
            parent = parentp;
            root = rootp;
            if (parent == null)
            {
                root = this;
            }

            nodeGameObject = new GameObject(rect.ToString());

        }

        public void build()
        {

            if (curDepth >= depth)
            {

                tile_w = rect.width;
                tile_h = rect.height;

                root.leavesNode.Add(this);

                return;
            }

            int chDepth = curDepth + 1;

            Rect rect0 = new Rect(rect.x, rect.y, rect.width * .5f, rect.height * .5f);
            Rect rect1 = new Rect(rect.x, rect.y + rect.height * .5f, rect.width * .5f, rect.height * .5f);
            Rect rect2 = new Rect(rect.x + rect.width * .5f, rect.y, rect.width * .5f, rect.height * .5f);
            Rect rect3 = new Rect(rect.x + rect.width * .5f, rect.y + rect.height * .5f, rect.width * .5f, rect.height * .5f);

            if (node0 != null)
            {
                node0.destroy();
                node0 = null;
            }
            if (node1 != null)
            {
                node1.destroy();
                node1 = null;
            }
            if (node2 != null)
            {
                node2.destroy();
                node2 = null;
            }
            if (node3 != null)
            {
                node3.destroy();
                node3 = null;
            }

            node0 = new Tree4(rect0, chDepth, this, root);
            node1 = new Tree4(rect1, chDepth, this, root);
            node2 = new Tree4(rect2, chDepth, this, root);
            node3 = new Tree4(rect3, chDepth, this, root);

            node0.build();
            node1.build();
            node2.build();
            node3.build();
        }

        public void buildNode8All()
        {
            for (int i = 0; i < root.leavesNode.Count; i++)
            {
                root.leavesNode[i].build8();
            }
        }

        public void logNode8All()
        {
            for (int i = 0; i < root.leavesNode.Count; i++)
            {
                root.leavesNode[i].log8();
            }
        }

        public void build8()
        {

            if (tile_w == 0 || tile_h == 0)
            {
                Debug.LogError("Error !!! tile_w == 0 || tile_h == 0");
                return;
            }
            if (root == null)
            {
                Debug.LogError("Error !!! root == null");
                return;
            }

            node_0000 = root.findByPot(new Vector2(rect.x + rect.width * .5f, rect.y + rect.height * 1.5f));
            node_0130 = root.findByPot(new Vector2(rect.x + rect.width * 1.5f, rect.y + rect.height * 1.5f));
            node_0300 = root.findByPot(new Vector2(rect.x + rect.width * 1.5f, rect.y + rect.height * .5f));
            node_0430 = root.findByPot(new Vector2(rect.x + rect.width * 1.5f, rect.y - rect.height * .5f));

            node_0600 = root.findByPot(new Vector2(rect.x + rect.width * .5f, rect.y - rect.height * .5f));
            node_0730 = root.findByPot(new Vector2(rect.x - rect.width * .5f, rect.y - rect.height * .5f));
            node_0900 = root.findByPot(new Vector2(rect.x - rect.width * .5f, rect.y + rect.height * .5f));
            node_1030 = root.findByPot(new Vector2(rect.x - rect.width * .5f, rect.y + rect.height * 1.5f));
        }

        public void log8()
        {

            Debug.LogWarning(rect.ToString());

            if (node_0000 != null)
            {
                Debug.LogWarning("\tnode_0000 =>");
                node_0000.log();
            }
            if (node_0130 != null)
            {
                Debug.LogWarning("\tnode_0130 =>");
                node_0130.log();
            }
            if (node_0300 != null)
            {
                Debug.LogWarning("\tnode_0300 =>");
                node_0300.log();
            }
            if (node_0430 != null)
            {
                Debug.LogWarning("\tnode_0430 =>");
                node_0430.log();
            }
            if (node_0600 != null)
            {
                Debug.LogWarning("\tnode_0600 =>");
                node_0600.log();
            }
            if (node_0730 != null)
            {
                Debug.LogWarning("\tnode_0730 =>");
                node_0730.log();
            }
            if (node_0900 != null)
            {
                Debug.LogWarning("\tnode_0900 =>");
                node_0900.log();
            }
            if (node_1030 != null)
            {
                Debug.LogWarning("\tnode_1030 =>");
                node_1030.log();
            }
            //Debug.LogWarning("==================================================>");
        }

        public void log()
        {
            string s = "";
            int i = 0;
            while (i <= curDepth)
            {
                s += "\t";
                i++;
            }
            //Debug.LogWarning(s + rect.ToString());

            foreach (GameObject gameObject in goDic.Values)
            {
                Debug.LogWarning(s + "gameObject = " + gameObject + ", p = " + gameObject.transform.position);
            }

            if (node0 == null) return;

            node0.log();
            node1.log();
            node2.log();
            node3.log();
        }

        public bool isInRect(Transform transform)
        {
            return rect.Contains(new Vector2(transform.position.x, transform.position.z));
        }

        public bool isInRectV3(Vector3 p)
        {
            return rect.Contains(new Vector2(p.x, p.z));
        }

        public bool isInRect(Vector2 p)
        {
            return rect.Contains(p);
        }

        public Tree4 findByPot(Vector2 p)
        {
            if (node0 == null)
            {
                if (isInRect(p))
                {
                    return this;
                }
            }
            else
            {
                Tree4 treeNode = node0.findByPot(p);
                if (treeNode != null)
                {
                    return treeNode;
                }
                treeNode = node1.findByPot(p);
                if (treeNode != null)
                {
                    return treeNode;
                }
                treeNode = node2.findByPot(p);
                if (treeNode != null)
                {
                    return treeNode;
                }
                treeNode = node3.findByPot(p);
                if (treeNode != null)
                {
                    return treeNode;
                }
            }
            return null;
        }

        public Tree4 addGameObject(GameObject gameObject)
        {
            if (node0 == null)
            {
                if (!goDic.ContainsKey(gameObject.GetInstanceID()))
                {
                    goDic.Add(gameObject.GetInstanceID(), gameObject);
                }
                return this;
            }
            else
            {
                if (node0.isInRect(gameObject.transform))
                {
                    return node0.addGameObject(gameObject);
                }
                if (node1.isInRect(gameObject.transform))
                {
                    return node1.addGameObject(gameObject);
                }
                if (node2.isInRect(gameObject.transform))
                {
                    return node2.addGameObject(gameObject);
                }
                if (node3.isInRect(gameObject.transform))
                {
                    return node3.addGameObject(gameObject);
                }
            }
            return null;
        }

        public void remGameObject(GameObject gameObject)
        {
            if (node0 == null)
            {
                goDic.Remove(gameObject.GetInstanceID());
            }
            else
            {
                if (node0.isInRect(gameObject.transform))
                {
                    node0.remGameObject(gameObject);
                    return;
                }
                if (node1.isInRect(gameObject.transform))
                {
                    node1.remGameObject(gameObject);
                    return;
                }
                if (node2.isInRect(gameObject.transform))
                {
                    node2.remGameObject(gameObject);
                    return;
                }
                if (node3.isInRect(gameObject.transform))
                {
                    node3.remGameObject(gameObject);
                    return;
                }
            }
        }

        public Tree4 findTree4Node(GameObject gameObject)
        {
            if (node0 == null)
            {
                if (goDic.ContainsKey(gameObject.GetInstanceID()))
                    return this;// (gameObject.GetInstanceID());
            }
            else
            {
                if (node0.isInRect(gameObject.transform))
                {
                    return node0.findTree4Node(gameObject);
                }
                if (node1.isInRect(gameObject.transform))
                {
                    return node1.findTree4Node(gameObject);
                }
                if (node2.isInRect(gameObject.transform))
                {
                    return node2.findTree4Node(gameObject);
                }
                if (node3.isInRect(gameObject.transform))
                {
                    return node3.findTree4Node(gameObject);
                }
            }
            return null;
        }

        void destroy()
        {
            root = null;
            parent = null;

            goDic.Clear();

            if (node0 == null) return;

            node0.destroy();
            node1.destroy();
            node2.destroy();
            node3.destroy();
        }
    }
}