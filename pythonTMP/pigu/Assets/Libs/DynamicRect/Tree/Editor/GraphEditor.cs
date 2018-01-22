using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace DynamicRectThc
{

    public class GraphEditor : Editor
    {
        [MenuItem("GraphEditor/tools Add Collider")]
        public static void AddCollider()
        {
            GameObject goRoot = Selection.activeGameObject;
            for (int i =0;i< goRoot.transform.childCount; i++) {
                Transform ch = goRoot.transform.GetChild(i);
                Collider collider = ch.GetComponentInChildren <Collider> ();
                if (collider == null) {
                    SphereCollider sphereCollider = ch.gameObject.AddComponent<SphereCollider>();
                    sphereCollider.center = Vector3.zero;
                    sphereCollider.radius = 1f;
                }
            }
        }

        [MenuItem("GraphEditor/tools Rem Collider")]
        public static void  RemCollider()
        {
            GameObject goRoot = Selection.activeGameObject;
            for (int i = 0; i < goRoot.transform.childCount; i++)
            {
                Transform ch = goRoot.transform.GetChild(i);
                Collider collider = ch.GetComponent<Collider>();
                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }
            }

        }


        /// <summary>
        /// 创建设置对象
        /// </summary>
        [MenuItem("GraphEditor/0 Add Setting")]
        public static void AddSetting()
        {
            GraphSetting graphSetting = FindObjectOfType<GraphSetting>();
            if (graphSetting == null) {
                new GameObject("GraphSetting").AddComponent<GraphSetting>() ;
            }
        }
        /// <summary>
        /// 工具链的第一步 创建场景 阵列图节点  绑定碰撞器
        /// 1.运行场景 
        /// 2.点击 GraphEditor/1 Build
        /// </summary>
        [MenuItem("GraphEditor/1 Build")]
        public static void Build()
        {
            if (!EditorApplication.isPlaying)
            {
                //EditorUtility.DisplayDialog("error", "场景必须在运行状态 ", "ok");
                //return;
            }

            GraphSetting graphSetting = FindObjectOfType<GraphSetting>();
            if (graphSetting == null) {
                EditorUtility.DisplayDialog("error", "找不到 GraphSetting 组件", "ok");
                return;
            }
            //Tree8.isDebug = false;
            //GraphRoot.create(new GameObject("GraphRoot"), 4, 4, LayerMask.NameToLayer("Tree"));
            GraphRoot.create(new GameObject("GraphRoot"), graphSetting.xNum, graphSetting.zNum, graphSetting.layerMask, graphSetting.tilex, graphSetting.tilez);
        }
        /// <summary>
        /// 工具链的第二步 保存场景数据 
        /// </summary>
        [MenuItem("GraphEditor/2 Save")]
        public static void Save()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("error", "场景必须在运行状态 ", "ok");
                return;
            }
            GraphRoot.save();
        }
        /// <summary>
        /// 该步骤必须停止场景运行状态！！！
        /// 在场景数据输出后
        /// 绑定 SetPrefab 
        /// </summary>
        [MenuItem("GraphEditor/3 SetPrefab")]
        public static void SetPrefab()
        {
            /*
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("error", "场景不能在运行状态 ", "ok");
                return;
            }*/
            GraphSetting graphSetting = FindObjectOfType<GraphSetting>();
            if (graphSetting == null)
            {
                EditorUtility.DisplayDialog("error", "找不到 GraphSetting 组件", "ok");
                return;
            }
            
            GraphRoot.setPrefabByDataAsset(graphSetting.sceneRectDataAssetPath, graphSetting.prefabSearchPathArr);
        }
        /// <summary>
        /// 加载 Assets 
        /// </summary>
        [MenuItem("GraphEditor/4 LoadAssets")]
        public static void Load()
        {
            //获取当前打开场景(path)
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");
            SceneRootData sceneRootData = (SceneRootData)AssetDatabase.LoadAssetAtPath(string.Format("Assets/Resources/SceneRectData/SceneRectData_{0}.asset", currSceneName),
                                           typeof(SceneRootData));

            GraphRoot.load(new GameObject("GraphLoadRoot"), sceneRootData);
        }
        /// <summary>
        /// 打包 AssetBundles
        /// </summary>
        [MenuItem("GraphEditor/5 AssetBundles")]
        public static void AssetBundles()
        {
            #region Unity5.x 打包方式  AssetBundles
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");

            string bundleName = "SceneRectData_" + currSceneName + ".srd";

            string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/Resources/SceneRectData/");
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                if (file.EndsWith(".meta"))
                {
                    continue;
                }
                file = file.Substring(file.LastIndexOf("/Assets/Resources/") + 1);
                string assetPath = file;

                bundleName = assetPath.Substring(assetPath.LastIndexOf("/") + 1).Replace("_","");
                //设置打包路径
                AssetImporter assetImporter = AssetImporter.GetAtPath(file);
                assetImporter.assetBundleName = bundleName;
            }
            //打包
            //BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/", 0, BuildTarget.Android);
            #endregion
        }
        /// <summary>
        /// 创建运行时加载对象
        /// </summary>
        [MenuItem("GraphEditor/6 Create GraphRoot")]
        public static void CreateGraphRoot()
        {
            //加载AssetBundles
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");

            string bundleName = "SceneRectData_" + currSceneName + ".srd";

            string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/Resources/SceneRectData/");
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                if (file.EndsWith(".meta"))
                {
                    continue;
                }
                if (file.IndexOf(currSceneName) < 0)
                {
                    continue;
                }

                AssetImporter assetImporter = AssetImporter.GetAtPath(file.Substring(file.LastIndexOf("/Assets") + 1));
                bundleName = assetImporter.assetBundleName;

                if (bundleName == null || bundleName.Equals(""))
                {
                    file.Replace(Application.streamingAssetsPath,"");
                }
                file = file.Substring(file.LastIndexOf("/Assets/Resources/") + 1);
                //string assetPath = file;
                //bundleName = assetPath.Substring(assetPath.LastIndexOf("/") + 1);

                GameObject graphRunGo = new GameObject(bundleName);
                GraphRun graphRun = graphRunGo.AddComponent<GraphRun>();
                graphRun.assetBundlesPath = bundleName;
            }
        }
        /*
        [MenuItem("GraphEditor/PreFabOutPut")]
        public static void preFab()
        {
            UnityEngine.Object go = Selection.activeObject;
            if (go != null)
            {
                if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
                {
                    UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(go);
                    string assetPath = AssetDatabase.GetAssetPath(parentObject);

                    Debug.LogWarning(go + "  =  " + assetPath + "," + go.GetInstanceID());
                }

                EditorApplication.isPlaying = true;

                Debug.LogWarning(go + "  =  " + go.GetInstanceID());
            }
            //GameObject sceneRootDataGo = new GameObject("SceneRootData");
        }
        */
    }
}