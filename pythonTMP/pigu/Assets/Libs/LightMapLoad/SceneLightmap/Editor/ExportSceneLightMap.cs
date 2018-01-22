using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
namespace DynamicRectThc
{
    /*
    public enum LightmapDataMapping
    {
        Path,
        Prefab
    }
    */
    public class ExportSceneLightMap : Editor
    {
        [MenuItem("LightTools/LightMap2AssetBundles/Export Scene LightMap Data")]
        public static void Export()
        {
            m_transList = new List<Transform>();
            GetSceneLightMapData();
        }

        static string lmType = "";

        static List<Transform> m_transList;
        static SceneLightmapData sceneLightMapData;

        static void GetSceneLightMapData()
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("error", "场景不能在运行状态 ", "ok");
                return;
            }
            #region 保存场景信息到 SceneLightmapData
            //创建场景光照序列化对象
            sceneLightMapData = ScriptableObject.CreateInstance<SceneLightmapData>();
            //获取当前打开场景(path)
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");
            //Debug.Log(currSceneName);
            //设置开场景名称
            sceneLightMapData.m_sceneName = currSceneName;
            //当前的lightmap设置中的 LightmapData 数组
            LightmapData[] currSceneLightMaps = LightmapSettings.lightmaps;
            int length = currSceneLightMaps.Length;
            if (length <= 0)
            {
                EditorUtility.DisplayDialog("error", "场景" + currSceneName + ":没有LightmapData", "ok");
                return;
            }
            //光照贴图名数组
            sceneLightMapData.m_lightMapFarName = new string[length];
            sceneLightMapData.m_lightMapNearName = new string[length];
            sceneLightMapData.m_assetBundleName = new string[length];

            string resourcesPath = Application.dataPath + string.Format("/Resources/Lightmap_{0}", currSceneName);

            if (!Directory.Exists(resourcesPath))
            {
                //AssetDatabase.CreateFolder("/Assets/Resources/",string.Format("Lightmap_{0}", currSceneName));
                System.IO.Directory.CreateDirectory(resourcesPath);
            }

            for (int i = 0; i < length; i++)
            {
                //获取 lm Texture2D
                Texture2D currLightMapColor = currSceneLightMaps[i].lightmapColor;
                Texture2D currLightMapDir = currSceneLightMaps[i].lightmapDir;

                //格式化名称
                /*
                string currLightMapColorName = string.Format("Assets/Resources/Lightmap_{1}/LightmapColor_{0}_{1}", i , currSceneName);
                string currLightMapDirName = string.Format("Assets/Resources/Lightmap_{1}/LightmapDir_{0}_{1}", i , currSceneName);

                sceneLightMapData.m_assetBundleName[i] = string.Format("LightmapAssetBundle_{0}_{1}.lm", i, currSceneName);
                */
                string currLightMapColorName = string.Format("Assets/Resources/Lightmap_{1}/LightmapColor_{0}_{2}_{1}", i, currSceneName, lmType);
                string currLightMapDirName = string.Format("Assets/Resources/Lightmap_{1}/LightmapDir_{0}_{2}_{1}", i, currSceneName, lmType);

                sceneLightMapData.m_assetBundleName[i] = string.Format("LightmapAssetBundle_{0}_{2}_{1}.lm", i, currSceneName, lmType);

                //保存 lm Texture2D 为 Assets
                if (currLightMapColor != null)
                {
                    string currLightMapColorPath = AssetDatabase.GetAssetPath(currLightMapColor);
                    string colorSuffix = currLightMapColorPath.Substring(currLightMapColorPath.LastIndexOf("."), currLightMapColorPath.Length - currLightMapColorPath.LastIndexOf("."));
                    AssetDatabase.CopyAsset(currLightMapColorPath, currLightMapColorName + colorSuffix);

                    AssetImporter importer = AssetImporter.GetAtPath(currLightMapColorName + colorSuffix);
                    importer.assetBundleName = sceneLightMapData.m_assetBundleName[i];
                    //AssetDatabase.CreateAsset(currLightMapColor, currLightMapColorName);
                }
                if (currLightMapDir != null)
                {
                    string currLightMapDirPath = AssetDatabase.GetAssetPath(currLightMapDir);
                    string dirSuffix = currLightMapDirPath.Substring(currLightMapDirPath.LastIndexOf("."), currLightMapDirPath.Length - currLightMapDirPath.LastIndexOf("."));
                    AssetDatabase.CopyAsset(currLightMapDirPath, currLightMapDirName + dirSuffix);

                    AssetImporter importer = AssetImporter.GetAtPath(currLightMapDirName + dirSuffix);
                    importer.assetBundleName = sceneLightMapData.m_assetBundleName[i];
                    //AssetDatabase.CreateAsset(currLightMapDir, currLightMapDirName);
                }
                //设置 lm name 

                sceneLightMapData.m_lightMapFarName[i] = currLightMapColorName;
                sceneLightMapData.m_lightMapNearName[i] = currLightMapDir != null ? currLightMapDirName : null;

            }
            #endregion

            #region 保存物件信息到 GameObjectLightmapData
            sceneLightMapData.m_gameObjectList = new List<RenderersLightmapData>();
            //获取当前属性
            MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                Transform _tempRoot = meshRenderers[i].transform;
                sceneLightMapData.m_gameObjectRoot = _tempRoot.name;
                m_transList.Add(_tempRoot);
                //CalcAllGameObject(_tempRoot); 添加子对象
            }
            AddChidGameObjectData();

            /* 选中节点方式  
            GameObject[] selectionGameObjects;
            if (Selection.activeGameObject)
            {
                selectionGameObjects = Selection.gameObjects;
                int selectedObjsLength = selectionGameObjects.Length;

                if (selectedObjsLength > 0)
                {
                    if (selectedObjsLength > 1)
                    {
                        EditorUtility.DisplayDialog("Error", "你选中了多个", "ok");
                        return;
                    }
                    else
                    {
                        Transform _tempRoot = selectionGameObjects[0].transform;
                        sceneLightMapData.m_gameObjectRoot = _tempRoot.name;
                        m_transList.Add(_tempRoot);
                        CalcAllGameObject(_tempRoot);
                    }

                    AddChidGameObjectData();
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "请在Hierarchy中选中要导出的GameObject", "ok");
            }
            */
            sceneLightMapData.ToString();
            #endregion

            #region 保存 SceneLightmapData.asset 到 Resources
            string path = string.Format("Assets/Resources/Lightmap_{0}/SceneLightmapData_{0}.asset", currSceneName);

            AssetDatabase.CreateAsset(sceneLightMapData, path);

            //编辑器环境下可以通过AssetDatabase.LoadAssetAtPath来加载
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(SceneLightmapData));
            #endregion

            #region Unity5.x 打包方式  AssetBundles
            string bundleName = path;
            bundleName = bundleName.Substring(bundleName.LastIndexOf("/") + 1);
            bundleName = bundleName.Replace(".asset", ".unity3d");
            //打包成.unity3d文件。
            AssetImporter assetImporter = AssetImporter.GetAtPath(path);
            assetImporter.assetBundleName = bundleName;
            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/", 0, BuildTarget.Android);
            #endregion

            AssetDatabase.Refresh();
        }

        //public static LightmapDataMapping curLightmapDataMapping = LightmapDataMapping.Prefab;

        [MenuItem("LightTools/LightMap2AssetBundles/Export LightMap Data Mapping Path")]
        public static void ExportLightMapDataMappingPath()
        {

            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("error", "场景不能在运行状态 ", "ok");
                return;
            }

            #region 获取场景名
            //获取当前打开场景(path)
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");
            #endregion

            #region 贴图打包
            //当前的lightmap设置中的 LightmapData 数组
            LightmapData[] curSceneLightMaps = LightmapSettings.lightmaps;
            int length = curSceneLightMaps.Length;
            if (length <= 0)
            {
                EditorUtility.DisplayDialog("error", "场景" + currSceneName + ":没有LightmapData", "ok");
                return;
            }
            //Lightmap 贴图 Asset 存放目录
            string resourcesPath = Application.dataPath + string.Format("/Resources/Lightmap_{0}", currSceneName);
            if (!Directory.Exists(resourcesPath))
            {
                System.IO.Directory.CreateDirectory(resourcesPath);
            }
            //AssetBundle包体名
            string[] lmAssetBundleNameArr = new string[curSceneLightMaps.Length];
            //AssetBundle 里的 assetName 用来实例化数据
            string[] assetNameLMColorArr = new string[curSceneLightMaps.Length];
            string[] assetNameLMDirArr = new string[curSceneLightMaps.Length];

            for (int i = 0; i < curSceneLightMaps.Length; i++)
            {
                //格式化名称
                string currLightMapColorName = string.Format("Assets/Resources/Lightmap_{1}/LightmapColor_{0}_{2}_{1}", i, currSceneName, lmType);
                string currLightMapDirName = string.Format("Assets/Resources/Lightmap_{1}/LightmapDir_{0}_{2}_{1}", i, currSceneName, lmType);
                string lmAssetBundleName = string.Format("LightmapAssetBundle_{0}_{2}_{1}.lm", i, currSceneName, lmType);

                lmAssetBundleNameArr[i] = lmAssetBundleName;

                //获取 lm Texture2D
                Texture2D currLightMapColor = curSceneLightMaps[i].lightmapColor;
                Texture2D currLightMapDir = curSceneLightMaps[i].lightmapDir;
                //保存 lm Texture2D 为 Assets
                if (currLightMapColor != null)
                {
                    string currLightMapColorPath = AssetDatabase.GetAssetPath(currLightMapColor);
                    string colorSuffix = currLightMapColorPath.Substring(currLightMapColorPath.LastIndexOf("."), currLightMapColorPath.Length - currLightMapColorPath.LastIndexOf("."));

                    string currLightMapColorResourcesPath = currLightMapColorName + colorSuffix;
                    //拷贝光照贴图到 Resources 下
                    AssetDatabase.CopyAsset(currLightMapColorPath, currLightMapColorResourcesPath);
                    //设置打包 assetBundleName
                    AssetImporter importer = AssetImporter.GetAtPath(currLightMapColorResourcesPath);
                    importer.assetBundleName = lmAssetBundleName;

                    assetNameLMColorArr[i] = currLightMapColorResourcesPath.Replace("Assets/", "");
                }
                if (currLightMapDir != null)
                {
                    string currLightMapDirPath = AssetDatabase.GetAssetPath(currLightMapDir);
                    string dirSuffix = currLightMapDirPath.Substring(currLightMapDirPath.LastIndexOf("."), currLightMapDirPath.Length - currLightMapDirPath.LastIndexOf("."));

                    string currLightMapDirResourcesPath = currLightMapDirName + dirSuffix;

                    AssetDatabase.CopyAsset(currLightMapDirPath, currLightMapDirResourcesPath);

                    AssetImporter importer = AssetImporter.GetAtPath(currLightMapDirResourcesPath);
                    importer.assetBundleName = lmAssetBundleName;

                    assetNameLMDirArr[i] = currLightMapDirResourcesPath.Replace("Assets/", "");
                }
            }
            #endregion

            #region 数据打包 
            //RenderersLightmapDataAsset 存放目录
            resourcesPath = string.Format("Assets/Resources/RenderersLightmapData_{0}", currSceneName);
            if (!Directory.Exists(Application.dataPath.Replace("/Assets", "/") + resourcesPath))
            {
                System.IO.Directory.CreateDirectory(resourcesPath);
            }
            //获取当前场景中所有 MeshRenderer
            MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                MeshRenderer meshRenderer = meshRenderers[i];

                //游戏对象场景路径
                string goPath = CalcGoChildName(meshRenderer.name, meshRenderer.transform);
                //RenderersLightmapData goRenderersLightmapData = new RenderersLightmapData();
                RenderersLightmapDataSerializable goRenderersLightmapData = ScriptableObject.CreateInstance<RenderersLightmapDataSerializable>();
                //场景路径信息
                goRenderersLightmapData.m_name = goPath;

                string rldAssetBundleName = string.Format("RenderersLightmapDataAssetBundle_{0}_{1}_{2}.rld", currSceneName, lmType, goPath.Replace("/", "_"));
                //transform 信息
                goRenderersLightmapData.m_position = meshRenderer.transform.position;
                goRenderersLightmapData.m_rotation = meshRenderer.transform.eulerAngles;
                goRenderersLightmapData.m_scale = meshRenderer.transform.lossyScale;
                //光照信息
                goRenderersLightmapData.m_lightmapIndex = meshRenderer.lightmapIndex;
                goRenderersLightmapData.m_lightmapScaleOffset = meshRenderer.lightmapScaleOffset;
                //AssetBundle 信息
                if (meshRenderer.lightmapIndex >= 0)
                {
                    goRenderersLightmapData.lmAssetBundleName = lmAssetBundleNameArr[meshRenderer.lightmapIndex];
                    goRenderersLightmapData.lightMapName = assetNameLMColorArr[meshRenderer.lightmapIndex];
                    goRenderersLightmapData.dirLightMapName = assetNameLMDirArr[meshRenderer.lightmapIndex];
                }
                //是否是预制体
                if (PrefabUtility.GetPrefabType(meshRenderer.gameObject) == PrefabType.PrefabInstance)
                {
                    UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(meshRenderer.gameObject);
                    string assetPath = AssetDatabase.GetAssetPath(parentObject);
                    //直接设置数据会更新到 Assets 文件
                    goRenderersLightmapData.perfabName = assetPath;
                }
                //判断当前映射方式
                //if (curLightmapDataMapping == LightmapDataMapping.Path)
                {
                    //场景 路径
                    string assetResourcesPath = resourcesPath + "/" + goPath.Replace("/", "~") + ".asset";

                    //创建 asset
                    AssetDatabase.CreateAsset(goRenderersLightmapData, assetResourcesPath);
                    //设置打包 assetBundleName
                    AssetImporter importer = AssetImporter.GetAtPath(assetResourcesPath);
                    importer.assetBundleName = goPath.Replace("/", "~") + ".asset";///<<<<<< Scene Path
                }/*
            else
            if (curLightmapDataMapping == LightmapDataMapping.Prefab)
            {
                //场景 路径 默认
                string assetResourcesPath = resourcesPath + "/" + goPath.Replace("/", "~") + ".asset";
                
                if (goRenderersLightmapData.perfabName != null && goRenderersLightmapData.perfabName != "")
                {
                    //预制体 路径
                    assetResourcesPath = resourcesPath + "/" + goRenderersLightmapData.perfabName.Replace("/", "~") + ".asset";
                }
                //创建 asset
                AssetDatabase.CreateAsset(goRenderersLightmapData, assetResourcesPath);
                //设置打包 assetBundleName
                AssetImporter importer = AssetImporter.GetAtPath(assetResourcesPath);
                importer.assetBundleName = goRenderersLightmapData.perfabName;///<<<<<< Prefab Path
            }*/
            }
            #endregion

            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/", 0, BuildTarget.Android);

            AssetDatabase.Refresh();
        }

        [MenuItem("LightTools/LightMap2AssetBundles/Export LightMap Data Mapping Prefab")]
        public static void ExportLightMapDataMappingPrefab()
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("error", "场景不能在运行状态 ", "ok");
                return;
            }

            ExportLightMapTexData();

            ExportLightMapMappingDataByPrefab();

            BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/", BuildAssetBundleOptions.None, BuildTarget.Android);

            AssetDatabase.Refresh();
        }

        //AssetBundle包体名
        static string[] lmAssetBundleNameArr;
        //AssetBundle 里的 assetName 用来实例化数据
        static string[] assetNameLMColorArr;
        static string[] assetNameLMDirArr;

        static void ExportLightMapTexData()
        {
            #region 获取场景名
            //获取当前打开场景(path)
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");
            #endregion

            #region 贴图打包
            //当前的lightmap设置中的 LightmapData 数组
            LightmapData[] curSceneLightMaps = LightmapSettings.lightmaps;
            int length = curSceneLightMaps.Length;
            if (length <= 0)
            {
                EditorUtility.DisplayDialog("error", "场景" + currSceneName + ":没有LightmapData", "ok");
                return;
            }
            //Lightmap 贴图 Asset 存放目录
            string resourcesPath = Application.dataPath + string.Format("/Resources/Lightmap_{0}", currSceneName);
            if (!Directory.Exists(resourcesPath))
            {
                System.IO.Directory.CreateDirectory(resourcesPath);
            }
            //AssetBundle包体名
            lmAssetBundleNameArr = new string[curSceneLightMaps.Length];
            //AssetBundle 里的 assetName 用来实例化数据
            assetNameLMColorArr = new string[curSceneLightMaps.Length];
            assetNameLMDirArr = new string[curSceneLightMaps.Length];

            for (int i = 0; i < curSceneLightMaps.Length; i++)
            {
                //格式化名称
                string currLightMapColorName = string.Format("Assets/Resources/Lightmap_{1}/LightmapColor_{0}_{2}_{1}", i, currSceneName, lmType);
                string currLightMapDirName = string.Format("Assets/Resources/Lightmap_{1}/LightmapDir_{0}_{2}_{1}", i, currSceneName, lmType);
                string lmAssetBundleName = string.Format("LightmapAssetBundle_{0}_{2}_{1}.lm", i, currSceneName, lmType);

                lmAssetBundleNameArr[i] = lmAssetBundleName;

                //获取 lm Texture2D
                Texture2D currLightMapColor = curSceneLightMaps[i].lightmapColor;
                Texture2D currLightMapDir = curSceneLightMaps[i].lightmapDir;
                //保存 lm Texture2D 为 Assets
                if (currLightMapColor != null)
                {
                    string currLightMapColorPath = AssetDatabase.GetAssetPath(currLightMapColor);
                    string colorSuffix = currLightMapColorPath.Substring(currLightMapColorPath.LastIndexOf("."), currLightMapColorPath.Length - currLightMapColorPath.LastIndexOf("."));

                    string currLightMapColorResourcesPath = currLightMapColorName + colorSuffix;
                    //拷贝光照贴图到 Resources 下
                    AssetDatabase.CopyAsset(currLightMapColorPath, currLightMapColorResourcesPath);
                    //设置打包 assetBundleName
                    AssetImporter importer = AssetImporter.GetAtPath(currLightMapColorResourcesPath);
                    importer.assetBundleName = lmAssetBundleName;

                    assetNameLMColorArr[i] = currLightMapColorResourcesPath.Replace("Assets/", "");
                }
                if (currLightMapDir != null)
                {
                    string currLightMapDirPath = AssetDatabase.GetAssetPath(currLightMapDir);
                    string dirSuffix = currLightMapDirPath.Substring(currLightMapDirPath.LastIndexOf("."), currLightMapDirPath.Length - currLightMapDirPath.LastIndexOf("."));

                    string currLightMapDirResourcesPath = currLightMapDirName + dirSuffix;

                    AssetDatabase.CopyAsset(currLightMapDirPath, currLightMapDirResourcesPath);

                    AssetImporter importer = AssetImporter.GetAtPath(currLightMapDirResourcesPath);
                    importer.assetBundleName = lmAssetBundleName;

                    assetNameLMDirArr[i] = currLightMapDirResourcesPath.Replace("Assets/", "");
                }
            }
            #endregion
        }

        static void ExportLightMapMappingDataByPrefab()
        {
            #region 获取场景名
            //获取当前打开场景(path)
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");
            #endregion

            #region 数据获取 
            //RenderersLightmapDataAsset 存放目录
            string resourcesPath = string.Format("Assets/Resources/PrefabLightmapData_{0}", currSceneName);
            if (!Directory.Exists(Application.dataPath.Replace("/Assets", "/") + resourcesPath))
            {
                System.IO.Directory.CreateDirectory(resourcesPath);
            }

            Dictionary<string, GameObjectLightmapData> prefabPath2GoLmData = new Dictionary<string, GameObjectLightmapData>();

            //获取当前场景中所有 MeshRenderer
            MeshRenderer[] meshRenderers = FindObjectsOfType<MeshRenderer>();
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                MeshRenderer meshRenderer = meshRenderers[i];
                //游戏对象场景路径
                string goPath = CalcGoChildName(meshRenderer.name, meshRenderer.transform);
                RenderersLightmapData goRenderersLightmapData = new RenderersLightmapData();
                //RenderersLightmapData goRenderersLightmapData = ScriptableObject.CreateInstance<RenderersLightmapData>();
                //场景路径信息
                goRenderersLightmapData.m_name = goPath;

                string rldAssetBundleName = string.Format("RenderersLightmapDataAssetBundle_{0}_{1}_{2}.rld", currSceneName, lmType, goPath.Replace("/", "_"));
                //transform 信息
                goRenderersLightmapData.m_position = meshRenderer.transform.position;
                goRenderersLightmapData.m_rotation = meshRenderer.transform.eulerAngles;
                goRenderersLightmapData.m_scale = meshRenderer.transform.lossyScale;
                //光照信息
                goRenderersLightmapData.m_lightmapIndex = meshRenderer.lightmapIndex;
                goRenderersLightmapData.m_lightmapScaleOffset = meshRenderer.lightmapScaleOffset;
                //AssetBundle 信息
                if (meshRenderer.lightmapIndex >= 0)
                {
                    goRenderersLightmapData.lmAssetBundleName = lmAssetBundleNameArr[meshRenderer.lightmapIndex];
                    goRenderersLightmapData.lightMapName = assetNameLMColorArr[meshRenderer.lightmapIndex];
                    goRenderersLightmapData.dirLightMapName = assetNameLMDirArr[meshRenderer.lightmapIndex];
                }
                //是否是预制体
                if (PrefabUtility.GetPrefabType(meshRenderer.gameObject) == PrefabType.PrefabInstance)
                {
                    UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(meshRenderer.gameObject);
                    string prefabAssetPath = AssetDatabase.GetAssetPath(parentObject);
                    //直接设置数据会更新到 Assets 文件
                    goRenderersLightmapData.perfabName = prefabAssetPath;

                    if (prefabPath2GoLmData.ContainsKey(prefabAssetPath))
                    {
                        prefabPath2GoLmData[prefabAssetPath].renderersLightmapDataList.Add(goRenderersLightmapData);
                    }
                    else
                    {
                        GameObjectLightmapData gameObjectLightmapData = CreateInstance<GameObjectLightmapData>();
                        gameObjectLightmapData.lightType = lmType;
                        gameObjectLightmapData.prefabName = prefabAssetPath;
                        gameObjectLightmapData.renderersLightmapDataList = new List<RenderersLightmapData>();
                        gameObjectLightmapData.renderersLightmapDataList.Add(goRenderersLightmapData);
                        prefabPath2GoLmData.Add(prefabAssetPath, gameObjectLightmapData);
                    }
                }
                else
                {
                    Debug.LogWarning(meshRenderer.gameObject + " 不是 Prefab！ ");
                    continue;
                }
                /*
                //场景 路径 默认
                string assetResourcesPath = resourcesPath + "/" + goPath.Replace("/", "~") + ".asset";
                //创建 asset
                AssetDatabase.CreateAsset(goRenderersLightmapData, assetResourcesPath);
                //设置打包 assetBundleName
                AssetImporter importer = AssetImporter.GetAtPath(assetResourcesPath);
                */
                /*
                 * 路径中能存在 []
                 * 如果一个  perfab 有多个 MeshRenderer 将打入一个 assetBundle 中
                 * perfab 1 - 1 assetBundle
                 * perfab 1 - n MeshRenderer(RenderersLightmapData) n - 1 assetBundle
                */
                //importer.assetBundleName = goRenderersLightmapData.perfabName;//string.Format("[{0}][{1}]", goRenderersLightmapData.perfabName, goPath);///<<<<<< Prefab Path
            }
            #endregion

            #region 1.Asset数据创建 2.assetBundle打包名设置
            foreach (string prefabAssetPath in prefabPath2GoLmData.Keys)
            {
                //场景 路径 默认
                string assetResourcesPath = resourcesPath + "/" + prefabAssetPath.Replace("/", "~") + ".asset";
                GameObjectLightmapData gameObjectLightmapData = prefabPath2GoLmData[prefabAssetPath];
                //创建 asset
                AssetDatabase.CreateAsset(gameObjectLightmapData, assetResourcesPath);
                //设置打包 assetBundleName
                AssetImporter importer = AssetImporter.GetAtPath(assetResourcesPath);

                //string assetBundleName = gameObjectLightmapData.prefabName.Substring(gameObjectLightmapData.prefabName.LastIndexOf("/") + 1);
                //currSceneName = currSceneName.Replace(".prefab", "");

                importer.assetBundleName = gameObjectLightmapData.prefabName + "lmd";
            }
            #endregion
        }

        static void CalcAllGameObject(Transform trans)
        {
            if (trans.childCount > 0)
            {
                for (int i = 0; i < trans.childCount; i++)
                {
                    m_transList.Add(trans.GetChild(i));
                    CalcAllGameObject(trans.GetChild(i));
                }
            }
        }

        static void AddChidGameObjectData()
        {
            for (int i = 0; i < m_transList.Count; i++)
            {
                Renderer render = m_transList[i].GetComponent<Renderer>();
                if (render != null)
                {
                    string goPath = CalcGoChildName(m_transList[i].name, m_transList[i]);
                    string goName = goPath.Substring(goPath.IndexOf("/") + 1);
                    //string goName = selectionGameObjects[i].name;
                    Vector3 position = m_transList[i].position;
                    Vector3 rotation = m_transList[i].rotation.eulerAngles;
                    Vector3 scale = m_transList[i].lossyScale;
                    int lightmapIndex = render.lightmapIndex;
                    Vector4 lightmapScaleOffset = render.lightmapScaleOffset;
                    int realtimeLightmapIndex = render.realtimeLightmapIndex;
                    Vector4 realtimeLightmapScaleOffset = render.realtimeLightmapScaleOffset;

                    RenderersLightmapData goRenderersLightmapData = new RenderersLightmapData();
                    goRenderersLightmapData.m_name = goPath;
                    goRenderersLightmapData.m_position = position;
                    goRenderersLightmapData.m_rotation = rotation;
                    goRenderersLightmapData.m_scale = scale;
                    goRenderersLightmapData.m_lightmapIndex = lightmapIndex;
                    goRenderersLightmapData.m_lightmapScaleOffset = lightmapScaleOffset;
                    goRenderersLightmapData.m_realtimeLightmapIndex = realtimeLightmapIndex;
                    goRenderersLightmapData.m_realtimeLightmapScaleOffset = realtimeLightmapScaleOffset;
                    goRenderersLightmapData.ToString();

                    //goRenderersLightmapData.lightMapName = sceneLightMapData.m_lightMapFarName[lightmapIndex];
                    goRenderersLightmapData.lightMapName = sceneLightMapData.m_assetBundleName[lightmapIndex];

                    if (PrefabUtility.GetPrefabType(render.gameObject) == PrefabType.PrefabInstance)
                    {
                        UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(render.gameObject);
                        string assetPath = AssetDatabase.GetAssetPath(parentObject);
                        //直接设置数据会更新到 Assets 文件
                        goRenderersLightmapData.perfabName = assetPath;

                    }



                    sceneLightMapData.m_gameObjectList.Add(goRenderersLightmapData);
                }
            }
        }

        static string CalcGoChildName(string name, Transform trans)
        {
            if (trans.parent == null)
            {
                return name;
            }
            else
            {
                trans = trans.parent;
                return CalcGoChildName(trans.name + "/" + name, trans);
            }
        }

    }
}
