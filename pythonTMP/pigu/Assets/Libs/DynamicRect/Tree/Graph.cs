using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
namespace DynamicRectThc
{
    using System;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    /// <summary>
    /// PrefabInstance 缓存实例
    /// </summary>
    public class PrefabInstance
    {
        /// <summary>
        /// Prefab 游戏对象节点
        /// </summary>
        public GameObject prefabGo;
        /// <summary>
        /// 是否空闲
        /// </summary>
        public bool _isFree;

        public bool isFree
        {
            get { return _isFree; }
            set
            {
                /*
                if (value && prefabGo.name.IndexOf("Tree_D_1") > -1) {
                    Debug.LogWarning(prefabGo.name);
                    System.Diagnostics.Debug.Assert(true);
                }
                */
                _isFree = value;
                if (prefabGo)
                {
                    //prefabGo.transform.localScale = Vector3.zero;
                    prefabGo.SetActive(!_isFree);
                }
            }
        }
    }

    public class PrefabInstanceInfo : MonoBehaviour
    {
        public PrefabInstance prefabInstance;
    }

    /// <summary>
    /// 节点根
    /// </summary>
    [System.Serializable]
    public class GraphRoot : MonoBehaviour
    {

        static public Dictionary<string, AssetBundle> assetBundleDic = new Dictionary<string, AssetBundle>();
        /// <summary>
        /// isLoadBySceneRootData = false 数据获取模式
        /// isLoadBySceneRootData = true 运行加载模式
        /// </summary>
        public bool isLoadBySceneRootData = false;
        /// <summary>
        /// 调试模式
        /// </summary>
        public bool isDebug = false;
        /// <summary>
        /// 预制模板缓存 
        /// key 为 prefab 完整路径
        /// </summary>
        public Dictionary<string, GameObject> prefabNameDic = new Dictionary<string, GameObject>();
        /// <summary>
        /// prefabInstance 缓存 
        /// key 为 prefab 完整路径
        /// </summary>
        public Dictionary<string, List<PrefabInstance>> prefabInstanceDic = new Dictionary<string, List<PrefabInstance>>();
        /// <summary>
        /// 分割尺寸
        /// </summary>
        public float tile_w = 25f;
        public float tile_h = 25f;
        /// <summary>
        /// 节点二维数组
        /// </summary>
        public Graph[,] arr;
        /// <summary>
        /// 当前中心节点
        /// </summary>
        public Graph curCell9Center;
        /// <summary>
        /// 场景数据
        /// </summary>
        public SceneRootData sceneRootData;
        /// <summary>
        /// 数量
        /// </summary>
        public int x_Num;
        public int z_Num;
        /// <summary>
        /// 碰撞层
        /// </summary>
        public LayerMask layer;
        void OnGUI()
        {
            /*
            if (GUI.Button(new Rect(110, 110, 100, 100), " 重置光照贴图"))
            {
                curCell9Center.reSetLightMap();
            }
            */
        }
        /// <summary>
        /// 初始化方法 在工具链中调用
        /// </summary>
        /// <param name="xNum">格子方向x数量</param>
        /// <param name="zNum">格子方向z数量</param>
        /// <param name="tilex">格子方向x尺寸</param>
        /// <param name="tilez">格子方向z尺寸</param>
        public void init(int xNum, int zNum, float tilex = 25f, float tilez = 25f)
        {
            x_Num = xNum;
            z_Num = zNum;

            tile_w = tilex;
            tile_h = tilez;

            arr = new Graph[xNum, zNum];

            Graph tNode;
            for (int x = 0; x < xNum; x++)
            {
                for (int z = 0; z < zNum; z++)
                {
                    tNode = new Graph();
                    tNode.root = this;
                    tNode.xIndex = x;
                    tNode.zIndex = z;
                    // if(isDebug)
                    tNode.create();
                    arr[x, z] = tNode;
                }
            }

            for (int x = 0; x < xNum; x++)
            {
                for (int z = 0; z < zNum; z++)
                {
                    arr[x, z].getTree(arr);
                }
            }
        }
        /// <summary>
        /// 通过反序列化后的 SceneRootData 创建
        /// </summary>
        /// <param name="sceneRootData"></param>
        public void initBySceneRootData(SceneRootData sceneRootData)
        {
            isLoadBySceneRootData = true;

            this.sceneRootData = sceneRootData;

            x_Num = sceneRootData.xNum;
            z_Num = sceneRootData.zNum;

            tile_w = sceneRootData.tilex;
            tile_h = sceneRootData.tilez;
            //创建预制体模板
            initPrefab(sceneRootData);

            arr = new Graph[x_Num, z_Num];
            SceneRectData sceneRectData;
            Graph tNode;

            for (int i = 0; i < sceneRootData.sceneRectDataArr.Length; i++)
            {
                sceneRectData = sceneRootData.sceneRectDataArr[i];
                tNode = new Graph();
                tNode.root = this;
                arr[sceneRectData.xIndex, sceneRectData.zIndex] = tNode;
                tNode.xIndex = sceneRectData.xIndex;
                tNode.zIndex = sceneRectData.zIndex;
                for (int j = 0; j < sceneRectData.sceneObjectDataArr.Length; j++)
                {
                    tNode.list.Add(sceneRectData.sceneObjectDataArr[j]);
                }
                tNode.create();
            }

            for (int x = 0; x < x_Num; x++)
            {
                for (int z = 0; z < z_Num; z++)
                {
                    arr[x, z].getTree(arr);
                }
            }
        }
        /// <summary>
        /// 创建 Prefab 模板池
        /// </summary>
        /// <param name="sceneRootData"></param>
        public void initPrefab(SceneRootData sceneRootData)
        {
            GameObject prefabRoot = new GameObject("PrefabRoot");
            prefabRoot.transform.parent = this.transform;

            /*
             *  sceneRootData.prefabPathArr.Length = sceneRootData.prefabAssetBundleNameArr.Length
             */

            //加载光照贴图

            /*
             * 
             * 数组 prefabPathArr  <- index -> 数组 prefabAssetBundleNameArr 
               prefab路径 下标相对应 AssetBundle路径
            */
            for (int i = 0; i < sceneRootData.prefabPathArr.Length; i++)
            {
                AssetBundle assetBundle = null;
                assetBundleDic.TryGetValue(sceneRootData.prefabAssetBundleNameArr[i], out assetBundle);
                if (assetBundle == null)
                {
                    assetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + sceneRootData.prefabAssetBundleNameArr[i]);
                    assetBundleDic.Add(sceneRootData.prefabAssetBundleNameArr[i], assetBundle);
                }

                //string[] allAssetNames = assetBundle.GetAllAssetNames();
                if ( !prefabNameDic.ContainsKey(sceneRootData.prefabPathArr[i]) )
                {
                    GameObject prefab = null;
                    if (assetBundle != null)
                        prefab = assetBundle.LoadAsset<GameObject>(sceneRootData.prefabPathArr[i]);
#if UNITY_EDITOR
                    if (prefab == null)
                        prefab = (GameObject)AssetDatabase.LoadAssetAtPath(sceneRootData.prefabPathArr[i], typeof(GameObject));
#endif

                    GameObject prefabGameObject = GameObject.Instantiate<GameObject>(prefab);
                    prefabGameObject.SetActive(false);
                    prefabGameObject.transform.parent = prefabRoot.transform;

                    prefabNameDic.Add(sceneRootData.prefabPathArr[i], prefabGameObject);
                }
            }
            /*
            for (int i = 0; i < sceneRootData.prefabPathArr.Length; i++)
            {
    #if UNITY_EDITOR
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(sceneRootData.prefabPathArr[i],typeof(GameObject));
                GameObject prefabGameObject = GameObject.Instantiate<GameObject>(prefab);
                prefabGameObject.transform.parent = prefabRoot.transform;

                prefabNameDic.Add(sceneRootData.prefabPathArr[i], prefabGameObject);
    #endif
            }*/
        }
        /// <summary>
        /// 获取一个PrefabInstance缓存
        /// </summary>
        /// <param name="prefabPath"></param>
        /// <returns></returns>
        public PrefabInstance getPrefabInstance(string prefabPath)
        {
            //原始prefab对象映射
            GameObject prefabGameObject = null;
            prefabNameDic.TryGetValue(prefabPath, out prefabGameObject);
            if (prefabGameObject == null)
            {
                Debug.LogError("不存在 prefab 路径=" + prefabPath);
                return null;
            }
            //按路径获取对象
            List<PrefabInstance> prefabInstanceList = null;
            prefabInstanceDic.TryGetValue(prefabPath, out prefabInstanceList);
            //如果没有缓存就创建一个
            if (prefabInstanceList == null)
            {
                prefabInstanceList = new List<PrefabInstance>();
                prefabInstanceDic.Add(prefabPath, prefabInstanceList);
            }
            PrefabInstance prefabInstance = null;
            //从缓存中获取一个空闲的
            for (int i = 0; i < prefabInstanceList.Count; i++)
            {
                prefabInstance = prefabInstanceList[i];
                if (prefabInstance.isFree)
                {
                    prefabInstance.isFree = false;
                    return prefabInstance;
                }
            }
            //如果缓存中没有就创建一个
            prefabInstance = new PrefabInstance();
            prefabInstance.prefabGo = GameObject.Instantiate<GameObject>(prefabGameObject);
            prefabInstance.isFree = false;
            prefabInstanceList.Add(prefabInstance);

            return prefabInstance;
        }

        static public void create(GameObject goRootp, int xNum, int zNum, LayerMask layer, float tilex = 25f, float tilez = 25f)
        {
            GraphRoot GraphRoot = goRootp.AddComponent<GraphRoot>();
            goRootp.transform.position = new Vector3(0, 0, 0);
            GraphRoot.layer = layer;
            GraphRoot.init(xNum, zNum, tilex, tilez);
        }

        static public void load(GameObject goRootp, SceneRootData sceneRootData)
        {
            GraphRoot GraphRoot = goRootp.AddComponent<GraphRoot>();
            goRootp.transform.position = new Vector3(0, 0, 0);
            GraphRoot.initBySceneRootData(sceneRootData);
        }
#if UNITY_EDITOR
        static public void setPrefabByDataAsset(string sceneRectDataAssetPath, string[] prefabSearchPathArr,bool isSetPrefabAbPath = false) {

            if (sceneRectDataAssetPath.StartsWith("Assets/"))
            {
                Debug.LogWarningFormat("sceneRectDataAssetPath 完整路径=>{0} ", sceneRectDataAssetPath);
            }
            else
            {
                sceneRectDataAssetPath = string.Format("Assets/Resources/SceneRectData/{0}", sceneRectDataAssetPath);
            }
            if (sceneRectDataAssetPath.EndsWith(".asset"))
            {
      
                Debug.LogWarningFormat("sceneRectDataAssetPath 完整后缀=>{0} ", sceneRectDataAssetPath);
            }
            else
            {
                sceneRectDataAssetPath += ".asset";
            }

            Debug.LogWarningFormat("sceneRectDataAssetPath 读取路径=>{0} ", sceneRectDataAssetPath);

            string sceneRectDataAssetFileName = sceneRectDataAssetPath;
            //获取当前打开场景名称
            sceneRectDataAssetFileName = sceneRectDataAssetFileName.Substring(sceneRectDataAssetFileName.LastIndexOf("/") + 1);
            sceneRectDataAssetFileName = sceneRectDataAssetFileName.Replace(".asset", "");

            SceneRootData sceneRootData = (SceneRootData)AssetDatabase.LoadAssetAtPath(sceneRectDataAssetPath,
                                           typeof(SceneRootData));

            Dictionary<string, UnityEngine.Object> prefabNameDic = new Dictionary<string, UnityEngine.Object>();

            Dictionary<string, string> prefabPathDic = new Dictionary<string, string>();

            for (int i = 0;i < prefabSearchPathArr.Length; i++) {

                string prefabSearchPath = prefabSearchPathArr[i];
                if (prefabSearchPath.StartsWith("Assets/")) {
                    prefabSearchPath = prefabSearchPath.Replace("Assets/","");
                }
          
                string[] files = System.IO.Directory.GetFiles(Application.dataPath + (prefabSearchPath.StartsWith("/")?"":"/") + prefabSearchPathArr[i]);

                for (int j = 0; j < files.Length ; j++)
                {
                    string file = files[j];

                    file = file.Replace( "\\" , "/" );
                    
                    if (file.EndsWith( ".prefab" )) {
                        string prefabNameKey = file.Substring(file.LastIndexOf("/") + 1 ,
                                                              file.LastIndexOf(".") - (file.LastIndexOf("/") + 1) );
                        string prefabPath = file.Replace(Application.dataPath, "Assets");
                               //prefabPath += file.Substring( file.LastIndexOf("/") + 1);
                        if (prefabPathDic.ContainsKey( prefabNameKey ))
                        {
                            Debug.LogWarningFormat("已经存在！ prefab {0} => file {1}", prefabNameKey , files[j]);
                        }
                        else {
                            prefabPathDic.Add(prefabNameKey, prefabPath);
                        }
                    }
                }
            }

            foreach (SceneRectData sceneRectData in sceneRootData.sceneRectDataArr)
            {
                foreach (SceneObjectData sceneObjectData in sceneRectData.sceneObjectDataArr)
                {
                    //检查物件是否存在
                    GameObject gameObject = GameObject.Find(sceneObjectData.gameObjectPath);
                    if (gameObject == null)
                    {
                        //EditorUtility.DisplayDialog("error", "无效路径=>" + sceneObjectData.gameObjectPath, "ok");
                        Debug.LogErrorFormat("无效路径 => {0}",sceneObjectData.gameObjectPath);
                        continue;
                    }
                    //是个完整关联 Prefab
                    if (PrefabUtility.GetPrefabType(gameObject) == PrefabType.PrefabInstance)
                    {
                        UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(gameObject);
                        string assetPath = AssetDatabase.GetAssetPath(parentObject);
                        //直接设置数据会更新到 Assets 文件
                        sceneObjectData.prefabName = assetPath;

                        if (!prefabNameDic.ContainsKey(assetPath))
                        {
                            //GameObject prefabGo = null;
                            //prefabNameDic.TryGetValue(assetPath, out prefabGo);
                            prefabNameDic.Add(assetPath, parentObject);
                        }
                        Debug.LogWarning(gameObject + "  =  " + assetPath + "," + gameObject.GetInstanceID());
                    }
                    else
                    {
                        string gameObjectPath;

                        if (sceneObjectData.gameObjectPath.LastIndexOf("(") > sceneObjectData.gameObjectPath.LastIndexOf("/") &&
                            sceneObjectData.gameObjectPath.LastIndexOf(")") + 1 == sceneObjectData.gameObjectPath.Length)
                        {
                            gameObjectPath = sceneObjectData.gameObjectPath.Substring(sceneObjectData.gameObjectPath.LastIndexOf("/") + 1);
                            gameObjectPath = gameObjectPath.Substring(0, gameObjectPath.IndexOf("("));
                            gameObjectPath = gameObjectPath.Trim();
                        }
                        else
                        {
                            gameObjectPath = sceneObjectData.gameObjectPath.Substring(sceneObjectData.gameObjectPath.LastIndexOf("/") + 1);
                        }

                        if (prefabPathDic.ContainsKey(gameObjectPath))
                        {
                            sceneObjectData.prefabName = prefabPathDic[gameObjectPath];
                            Debug.LogWarning(gameObject + "  =  " + prefabPathDic[gameObjectPath] + "," + gameObject.GetInstanceID());
                        }
                        else
                        {
                            Debug.LogErrorFormat("{0} 没有找到对应的 prefab", gameObjectPath);
                        }
                        //if (prefabPathDic.ContainsKey(gameObjectPath))
                    }
                    // string gameObjectPath;
                }
            }

            string[] prefabNameDicArr = new string[prefabNameDic.Keys.Count];
            prefabNameDic.Keys.CopyTo(prefabNameDicArr, 0);

            string[] prefabPathDicArr = new string[prefabPathDic.Values.Count];
            prefabPathDic.Values.CopyTo(prefabPathDicArr, 0);

            sceneRootData.prefabPathArr = ArrTools.MergerArray(prefabNameDicArr,prefabPathDicArr);

            sceneRootData.prefabAssetBundleNameArr = new string[sceneRootData.prefabPathArr.Length];
            //设置依赖 prefab 打包
            for (int j = 0; j < sceneRootData.prefabPathArr.Length; j++)
            {
                //设置打包路径
                AssetImporter assetImporter = AssetImporter.GetAtPath(sceneRootData.prefabPathArr[j]);
                //在prefab没有设置打包路径的情况下才设置 打包ab名
                if (isSetPrefabAbPath || assetImporter.assetBundleName != null && assetImporter.assetBundleName.Equals(string.Empty))
                {
                    assetImporter.assetBundleName = sceneRectDataAssetFileName + ".prefab";
                }

                sceneRootData.prefabAssetBundleNameArr[j] = assetImporter.assetBundleName;
                /*
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(sceneRootData.prefabPathArr[i], typeof(GameObject));
                GameObject prefabGameObject = GameObject.Instantiate<GameObject>(prefab);
                prefabGameObject.transform.parent = prefabRoot.transform;

                prefabNameDic.Add(sceneRootData.prefabPathArr[i], prefabGameObject);
                */
            }

            EditorUtility.DisplayDialog("LOG", "prefab 设置完成 " + sceneRectDataAssetPath, "ok");
        }
#endif

    /// <summary>
    /// 重新绑定Prefab
    /// </summary>
    /*
    static public void setPrefab()
    {
    #if UNITY_EDITOR
        //获取当前打开场景(path)
        string currSceneName = EditorApplication.currentScene;
        //获取当前打开场景名称
        currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
        currSceneName = currSceneName.Replace(".unity", "");

        string sceneRectDataAssetPath = string.Format("Assets/Resources/SceneRectData/SceneRectData_{0}.asset", currSceneName);

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

            file = file.Substring(file.LastIndexOf("/Assets/Resources/") + 1);
            sceneRectDataAssetPath = file;

            SceneRootData sceneRootData = (SceneRootData)AssetDatabase.LoadAssetAtPath(sceneRectDataAssetPath,
                                           typeof(SceneRootData));

            Dictionary<string, UnityEngine.Object> prefabNameDic = new Dictionary<string, UnityEngine.Object>();

            foreach (SceneRectData sceneRectData in sceneRootData.sceneRectDataArr)
            {
                foreach (SceneObjectData sceneObjectData in sceneRectData.sceneObjectDataArr)
                {
                    GameObject gameObject = GameObject.Find(sceneObjectData.gameObjectPath);
                    if (gameObject == null)
                    {
                        EditorUtility.DisplayDialog("error", "无效路径=>" + sceneObjectData.gameObjectPath, "ok");
                        return;
                    }
                    if (PrefabUtility.GetPrefabType(gameObject) == PrefabType.PrefabInstance)
                    {
                        UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(gameObject);
                        string assetPath = AssetDatabase.GetAssetPath(parentObject);
                        //直接设置数据会更新到 Assets 文件
                        sceneObjectData.prefabName = assetPath;

                        if (!prefabNameDic.ContainsKey(assetPath))
                        {
                            //GameObject prefabGo = null;
                            //prefabNameDic.TryGetValue(assetPath, out prefabGo);
                            prefabNameDic.Add(assetPath, parentObject);
                        }
                        Debug.LogWarning(gameObject + "  =  " + assetPath + "," + gameObject.GetInstanceID());
                    }
                    else
                    {

                    }
                }
            }

            sceneRootData.prefabPathArr = new string[prefabNameDic.Keys.Count];
            prefabNameDic.Keys.CopyTo(sceneRootData.prefabPathArr, 0);

            sceneRootData.prefabAssetBundleNameArr = new string[prefabNameDic.Keys.Count];
            //设置依赖 prefab 打包
            for (int j = 0; j < sceneRootData.prefabPathArr.Length; j++)
            {
                //设置打包路径
                AssetImporter assetImporter = AssetImporter.GetAtPath(sceneRootData.prefabPathArr[j]);
                assetImporter.assetBundleName = currSceneName + ".prefab";

                sceneRootData.prefabAssetBundleNameArr[j] = assetImporter.assetBundleName;
                
                //GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(sceneRootData.prefabPathArr[i], typeof(GameObject));
                //GameObject prefabGameObject = GameObject.Instantiate<GameObject>(prefab);
                //prefabGameObject.transform.parent = prefabRoot.transform;
                //prefabNameDic.Add(sceneRootData.prefabPathArr[i], prefabGameObject);
                
            }
        }
        //不需要删除后重新创建！！！ LoadAssetAtPath 加载数据后【赋值】【直接更新】到 asset 文件！！
        //AssetDatabase.DeleteAsset(sceneRectDataAssetPath);
        //AssetDatabase.CreateAsset(sceneRootData, string.Format("Assets/Resources/SceneRectData/SceneRectData_{0}.asset", currSceneName));
        }
    #endif

    }
    */
        /// <summary>
        /// 保存场景数据到 序列化对象 SceneRootData 中
        /// 由工具链调用
        /// </summary>
        static public void save()
        {
            GraphRoot graphRoot = GameObject.FindObjectOfType<GraphRoot>();

            if (graphRoot == null)
            {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("error", " 场景没有 Root 请先执行 build 操作 ", "ok!");
#endif
                return;
            }

            SceneRootData sceneRootData = new SceneRootData();

            int xNum = graphRoot.x_Num;
            int zNum = graphRoot.z_Num;

            sceneRootData.xNum = xNum;
            sceneRootData.zNum = zNum;
            sceneRootData.tilex = graphRoot.tile_w;
            sceneRootData.tilez = graphRoot.tile_h;

            sceneRootData.sceneRectDataArr = new SceneRectData[graphRoot.transform.GetChildCount()];

            int index = 0;

            for (int x = 0; x < xNum; x++)
            {
                for (int z = 0; z < zNum; z++)
                {
                    Graph t = graphRoot.arr[x, z];
                    SceneRectData sceneRectData = new SceneRectData();
                    sceneRectData.xIndex = x;
                    sceneRectData.zIndex = z;
                    sceneRectData.sceneObjectDataArr = t.list.ToArray();

                    sceneRootData.sceneRectDataArr[index++] = sceneRectData;
                }
            }

            sceneRootData.prefabPathArr = new string[graphRoot.prefabNameDic.Keys.Count];
            graphRoot.prefabNameDic.Keys.CopyTo(sceneRootData.prefabPathArr, 0);

#if UNITY_EDITOR
        //获取当前打开场景(path)
        string currSceneName = EditorApplication.currentScene;
        //获取当前打开场景名称
        currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
        currSceneName = currSceneName.Replace(".unity", "");
        //string resourcesPath = Application.dataPath + string.Format("/Resources/SceneRectData_{0}", currSceneName);
        string resourcesPath = Application.dataPath + "/Resources/SceneRectData";
        if (!Directory.Exists(resourcesPath))
        {
            System.IO.Directory.CreateDirectory(resourcesPath); 
        }
            //AssetDatabase.CreateAsset(sceneRootData, string.Format("Assets/Resources/SceneRectData/SceneRectData_{0}.asset", currSceneName));
            string colliderLayerStr = Convert.ToString(graphRoot.layer.value , 2  );
            string layerStr = string.Empty;
            char[] charArr = colliderLayerStr.ToCharArray();
            Array.Reverse( charArr );
 
            for (int i =0;i < colliderLayerStr.Length; i++ ) {
                char c = charArr[i];
                int layerInt = int.Parse(c.ToString());
                if (layerInt == 1)
                {
                    layerStr += LayerMask.LayerToName(i) +"_";
                }
            }
            AssetDatabase.CreateAsset(sceneRootData, string.Format("Assets/Resources/SceneRectData/SceneRectData_{0}_{1}.asset", currSceneName, layerStr));
            //AssetDatabase.CreateAsset(sceneRootData, string.Format("Assets/Resources/SceneRectData/SceneRectData_{0}_{1}.asset" , currSceneName, colliderLayerStr));
#endif
        }

    }

    [System.Serializable]
    public class Graph
    {

        public GraphRoot root;
        bool _isInCell9;
        public bool isInCell9
        {
            get
            {
                return _isInCell9;
            }
            set
            {
                isInCell9_Old = _isInCell9;
                _isInCell9 = value;

                if (meshRenderer)
                    meshRenderer.enabled = !_isInCell9;
            }
        }

        public bool isInCell9_Old;

        public int xIndex, zIndex;

        public Graph node_0000;
        public Graph node_0130;
        public Graph node_0300;
        public Graph node_0430;

        public Graph node_0600;
        public Graph node_0730;
        public Graph node_0900;
        public Graph node_1030;
        /// <summary>
        /// 当前节点下的场景对象数据列表
        /// </summary>
        public List<SceneObjectData> list = new List<SceneObjectData>();
        /// <summary>
        /// 当前节点下场景缓存对象
        /// </summary>
        public List<PrefabInstance> prefabInstanceList = new List<PrefabInstance>();
        /// <summary>
        /// 网格节点
        /// </summary>
        public GameObject goNode;
        /// <summary>
        /// 调试网格
        /// </summary>
        MeshRenderer meshRenderer;
        /// <summary>
        /// 创建节点矩阵
        /// </summary>
        public void create()
        {

            goNode = new GameObject("" + xIndex + "_" + zIndex);
            goNode.transform.position = new Vector3(xIndex * root.tile_w, 0, zIndex * root.tile_h);
            goNode.transform.parent = root.transform;
            GraphNode graphNode = goNode.AddComponent<GraphNode>();
            graphNode.node = this;

            BoxCollider boxCollider = goNode.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(root.tile_w, 58f, root.tile_h);
            Rigidbody rigidbody = goNode.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;

            if (root.isDebug)
            {
                float y = 2.6f;
                float w = root.tile_w;
                float h = root.tile_h;
                Mesh mesh = new Mesh();
                mesh.vertices = new Vector3[] {
                    new Vector3 (-w * .5f, y, -h * .5f),
                    new Vector3 (-w * .5f, y,  h * .5f),
                    new Vector3 ( w * .5f, y,  h * .5f),
                    new Vector3 ( w * .5f, y, -h * .5f),
                };

                // 所以只能是(0, 1, 2)这个顺序。
                mesh.triangles = new int[6] { 0, 1, 2, 0, 2, 3 };

                mesh.RecalculateNormals();
                mesh.RecalculateBounds();

                MeshFilter meshFilter = goNode.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = mesh;
                meshRenderer = goNode.AddComponent<MeshRenderer>();
                Material material = new Material(Shader.Find("Diffuse"));
                meshRenderer.sharedMaterial = material;
                //meshRenderer.enabled = false;
            }
        }
        /// <summary>
        /// 显示当前区域的 GameObject
        /// </summary>
        public void show()
        {
            for (int i = 0; i < prefabInstanceList.Count; i++)
            {
                prefabInstanceList[i].isFree = true;
            }
            prefabInstanceList.Clear();

            SceneObjectData curSceneObjectData;
            PrefabInstance curPrefabInstance;
            GameObject sceneGameObject = null;
            for (int i = 0; i < list.Count; i++)
            {

                curSceneObjectData = list[i];
                curPrefabInstance = root.getPrefabInstance(curSceneObjectData.prefabName);

                if (curPrefabInstance != null)
                {
                    sceneGameObject = curPrefabInstance.prefabGo;
                    prefabInstanceList.Add(curPrefabInstance);
                }

                if (sceneGameObject == null)
                {
                    sceneGameObject = GameObject.Find(curSceneObjectData.gameObjectPath);
                }
                if (sceneGameObject == null)
                {
                    Debug.LogError("路径不存在 ！！=  " + curSceneObjectData.gameObjectPath);
                }
                sceneGameObject.transform.parent = goNode.transform;
                sceneGameObject.transform.position = curSceneObjectData.p;
                sceneGameObject.transform.rotation = curSceneObjectData.r;
                sceneGameObject.transform.localScale = curSceneObjectData.s;

                Renderer[] rendererList = sceneGameObject.GetComponentsInChildren<Renderer>();

                for (int j = 0; j < curSceneObjectData.sceneObjectLMDataArr.Length && j < rendererList.Length; j++)
                {
                    Renderer renderer = rendererList[j];
                    renderer.lightmapIndex = curSceneObjectData.sceneObjectLMDataArr[j].lightmapIndex;
                    renderer.lightmapScaleOffset = curSceneObjectData.sceneObjectLMDataArr[j].lightmapScaleOffset;

                    renderer.sharedMaterial.shader = Shader.Find(renderer.sharedMaterial.shader.name);
                }

                Collider collider = sceneGameObject.GetComponentInChildren<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }
            }
        }

        public void reSetLightMap()
        {
            GameObject curSceneObjectData = null;
            for (int i = 0; i < prefabInstanceList.Count; i++)
            {
                //prefabInstanceList[i].isFree = true;
                curSceneObjectData = prefabInstanceList[i].prefabGo;
                Renderer[] rendererList = curSceneObjectData.GetComponentsInChildren<Renderer>();

                for (int j = 0; j < rendererList.Length; j++)
                {
                    Renderer renderer = rendererList[j];
                    renderer.lightmapIndex = list[i].sceneObjectLMDataArr[j].lightmapIndex;
                    renderer.lightmapScaleOffset = list[i].sceneObjectLMDataArr[j].lightmapScaleOffset;
                    renderer.sharedMaterial.shader = Shader.Find(renderer.sharedMaterial.shader.name);
                }
            }
        }
        /// <summary>
        /// 释放该节点下所有 prefabInstance 预制体实例 为 free
        /// </summary>
        public void free()
        {
            for (int i = 0; i < prefabInstanceList.Count; i++)
            {
                prefabInstanceList[i].isFree = true;
                //GameObject.Destroy(prefabInstanceList[i].prefabGo);
            }
            prefabInstanceList.Clear();
        }

        public void add(GameObject go)
        {

            if (root.isLoadBySceneRootData)
                return;

            SceneObjectData sceneObjectData = new SceneObjectData();
            sceneObjectData.p = go.transform.position;
            sceneObjectData.s = go.transform.localScale;
            sceneObjectData.r = go.transform.rotation;
            sceneObjectData.gameObjectPath = GetPath(go.transform.name, go.transform);

#if UNITY_EDITOR
      
        //判断GameObject是否为一个Prefab的引用
        /*
        if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
        {
            UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(go);
            string assetPath = AssetDatabase.GetAssetPath(parentObject);
            sceneObjectData.prefabName = assetPath;
            GameObject prefabGo = null;
            root.prefabNameDic.TryGetValue(sceneObjectData.prefabName, out prefabGo);
            if (prefabGo == null)
            {
                root.prefabNameDic.Add(sceneObjectData.prefabName, go);
            }
        }       
        */
#endif

            list.Add(sceneObjectData);

            GameObject goNew = GameObject.Instantiate(go);

            Collider collider = goNew.GetComponentInChildren<Collider>();
            if (collider != null)
                collider.enabled = false;
            collider = goNew.GetComponent<Collider>();
            if (collider != null)
                collider.enabled = false;

            goNew.transform.parent = goNode.transform;
            goNew.transform.position = sceneObjectData.p;
            goNew.transform.localScale = sceneObjectData.s;
            goNew.transform.rotation = sceneObjectData.r;

            Renderer[] rendererList = go.GetComponentsInChildren<Renderer>();
            Renderer[] rendererListNew = goNew.GetComponentsInChildren<Renderer>();

            sceneObjectData.sceneObjectLMDataArr = new SceneObjectLMData[rendererList.Length];

            for (int i = 0; i < rendererList.Length; i++)
            {
                Renderer renderer = rendererList[i];
                Renderer rendererNew = rendererListNew[i];
                rendererNew.lightmapIndex = renderer.lightmapIndex;
                rendererNew.lightmapScaleOffset = renderer.lightmapScaleOffset;
                SceneObjectLMData sceneObjectLMData = new SceneObjectLMData();
                sceneObjectLMData.rendererIndex = i;
                sceneObjectLMData.lightmapIndex = renderer.lightmapIndex;
                sceneObjectLMData.lightmapScaleOffset = renderer.lightmapScaleOffset;
                sceneObjectData.sceneObjectLMDataArr[i] = sceneObjectLMData;
            }

        }

        public bool isJoin(Graph node)
        {
            if (node.node_0000 == this)
                return true;
            if (node.node_0130 == this)
                return true;
            if (node.node_0300 == this)
                return true;
            if (node.node_0430 == this)
                return true;
            if (node.node_0600 == this)
                return true;
            if (node.node_0730 == this)
                return true;
            if (node.node_0900 == this)
                return true;
            if (node.node_1030 == this)
                return true;
            return false;
        }

        public void center()
        {

            if (root.curCell9Center != null && root.curCell9Center != this)
            {

                Graph oldGraphNode = root.curCell9Center;
                //oldGraphNode.isInCell9 = false;

                if (oldGraphNode.node_0000 != null)
                    oldGraphNode.node_0000.isInCell9 = oldGraphNode.node_0000.isJoin(this);
                if (oldGraphNode.node_0130 != null)
                    oldGraphNode.node_0130.isInCell9 = oldGraphNode.node_0130.isJoin(this);
                if (oldGraphNode.node_0300 != null)
                    oldGraphNode.node_0300.isInCell9 = oldGraphNode.node_0300.isJoin(this);
                if (oldGraphNode.node_0430 != null)
                    oldGraphNode.node_0430.isInCell9 = oldGraphNode.node_0430.isJoin(this);
                if (oldGraphNode.node_0600 != null)
                    oldGraphNode.node_0600.isInCell9 = oldGraphNode.node_0600.isJoin(this);
                if (oldGraphNode.node_0730 != null)
                    oldGraphNode.node_0730.isInCell9 = oldGraphNode.node_0730.isJoin(this);
                if (oldGraphNode.node_0900 != null)
                    oldGraphNode.node_0900.isInCell9 = oldGraphNode.node_0900.isJoin(this);
                if (oldGraphNode.node_1030 != null)
                    oldGraphNode.node_1030.isInCell9 = oldGraphNode.node_1030.isJoin(this);

                if (oldGraphNode.node_0000 != null && oldGraphNode.node_0000.isInCell9 == false && oldGraphNode.node_0000.isInCell9_Old == true)
                {
                    oldGraphNode.node_0000.free();
                }
                if (oldGraphNode.node_0130 != null && oldGraphNode.node_0130.isInCell9 == false && oldGraphNode.node_0130.isInCell9_Old == true)
                {
                    oldGraphNode.node_0130.free();
                }
                if (oldGraphNode.node_0300 != null && oldGraphNode.node_0300.isInCell9 == false && oldGraphNode.node_0300.isInCell9_Old == true)
                {
                    oldGraphNode.node_0300.free();
                }
                if (oldGraphNode.node_0430 != null && oldGraphNode.node_0430.isInCell9 == false && oldGraphNode.node_0430.isInCell9_Old == true)
                {
                    oldGraphNode.node_0430.free();
                }
                if (oldGraphNode.node_0600 != null && oldGraphNode.node_0600.isInCell9 == false && oldGraphNode.node_0600.isInCell9_Old == true)
                {
                    oldGraphNode.node_0600.free();
                }
                if (oldGraphNode.node_0730 != null && oldGraphNode.node_0730.isInCell9 == false && oldGraphNode.node_0730.isInCell9_Old == true)
                {
                    oldGraphNode.node_0730.free();
                }
                if (oldGraphNode.node_0900 != null && oldGraphNode.node_0900.isInCell9 == false && oldGraphNode.node_0900.isInCell9_Old == true)
                {
                    oldGraphNode.node_0900.free();
                }
                if (oldGraphNode.node_1030 != null && oldGraphNode.node_1030.isInCell9 == false && oldGraphNode.node_1030.isInCell9_Old == true)
                {
                    oldGraphNode.node_1030.free();
                }
            }
            if (root.curCell9Center != this)
            {
                this.isInCell9 = true;
                this.show();

                root.curCell9Center = this;

                //if (node_0000 != null && !node_0000.isInCell9_Old )
                if (node_0000 != null)
                {
                    this.node_0000.isInCell9 = true;
                    this.node_0000.show();
                }
                //if (node_0130 != null && !node_0130.isInCell9_Old)
                if (node_0130 != null)
                {
                    this.node_0130.isInCell9 = true;
                    this.node_0130.show();
                }
                //if (node_0300 != null && !node_0300.isInCell9_Old)
                if (node_0300 != null)
                {
                    this.node_0300.isInCell9 = true;
                    this.node_0300.show();
                }
                //if (node_0430 != null && !node_0430.isInCell9_Old)
                if (node_0430 != null)
                {
                    this.node_0430.isInCell9 = true;
                    this.node_0430.show();
                }
                //if (node_0600 != null && !node_0600.isInCell9_Old)
                if (node_0600 != null)
                {
                    this.node_0600.isInCell9 = true;
                    this.node_0600.show();
                }
                //if (node_0730 != null && !node_0730.isInCell9_Old)
                if (node_0730 != null)
                {
                    this.node_0730.isInCell9 = true;
                    this.node_0730.show();
                }
                //if (node_0900 != null && !node_0900.isInCell9_Old)
                if (node_0900 != null)
                {
                    this.node_0900.isInCell9 = true;
                    this.node_0900.show();
                }
                //if (node_1030 != null && !node_1030.isInCell9_Old)
                if (node_1030 != null)
                {
                    this.node_1030.isInCell9 = true;
                    this.node_1030.show();
                }

            }
        }

        static string GetPath(Transform transRoot, Transform trans)
        {
            if (trans.parent == transRoot)
            {
                return trans.name;
            }
            else
            {
                trans = trans.parent;
                return GetPath(transRoot, trans);
            }
        }

        static string GetPath(string name, Transform trans)
        {
            if (trans.parent == null)
            {
                return name;
            }
            else
            {
                trans = trans.parent;
                return GetPath(trans.name + "/" + name, trans);
            }
        }

        public void getTree(Graph[,] arr)
        {
            if (zIndex < arr.GetUpperBound(1))
                node_0000 = arr[xIndex, zIndex + 1];
            if (xIndex < arr.GetUpperBound(0) && zIndex < arr.GetUpperBound(1))
                node_0130 = arr[xIndex + 1, zIndex + 1];
            if (xIndex < arr.GetUpperBound(0))
                node_0300 = arr[xIndex + 1, zIndex];
            if (xIndex < arr.GetUpperBound(0) && zIndex > 0)
                node_0430 = arr[xIndex + 1, zIndex - 1];
            if (zIndex > 0)
                node_0600 = arr[xIndex, zIndex - 1];
            if (xIndex > 0 && zIndex > arr.GetLowerBound(1))
                node_0730 = arr[xIndex - 1, zIndex - 1];
            if (xIndex > 0)
                node_0900 = arr[xIndex - 1, zIndex];
            if (xIndex > 0 && zIndex < arr.GetUpperBound(1))
                node_1030 = arr[xIndex - 1, zIndex + 1];
        }

    }
}