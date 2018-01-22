using UnityEngine;
using System.Collections;
namespace DynamicRectThc
{
    public class Tree4Test : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Tree4 tree4Root = new Tree4(new Rect(-50, -50, 100, 100), 0);
            tree4Root.build();
            tree4Root.buildNode8All();

            GameObject go = new GameObject("go0");
            go.transform.position = new Vector3(20f, 0, 13f);
            tree4Root.addGameObject(go);
            go = new GameObject("go1");
            go.transform.position = new Vector3(-20f, 0, 13f);
            tree4Root.addGameObject(go);

            tree4Root.log();

            Tree4 t = tree4Root.findTree4Node(go);
            Debug.LogWarning("find = " + t.rect);

            Tree4 Treebyp = tree4Root.findByPot(new Vector3(0f, 5f, 0f));
            Treebyp.log8();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}