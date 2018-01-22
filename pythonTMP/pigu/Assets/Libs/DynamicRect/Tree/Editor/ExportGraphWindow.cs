using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace DynamicRectThc
{

    public class ExportGraphWindow : EditorWindow
    {
        [MenuItem("Window/ExportGraph")]
        static void Initialize()
        {
            ExportGraphWindow window = (ExportGraphWindow)EditorWindow.GetWindowWithRect(typeof(ExportGraphWindow), new Rect(0, 0, 386, 582), false, "ExportGraphWindow");
            window.Show();
        }
        //LayerMask layer;
        //string layerName = "Tree";
        /*
        int xNum = 8;
        int zNum = 8;
        int tilex = 20;
        int tilez = 20;
        */
        GraphSetting graphSetting;

        void OnGUI()
        {
            graphSetting = FindObjectOfType<GraphSetting>();
            if (graphSetting == null) {
                graphSetting = new GameObject("GraphSetting").AddComponent<GraphSetting>() ;
            }

            GUILayout.BeginVertical();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            //获取当前打开场景(path)
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");
            GUILayout.Label("SceneName");
            GUILayout.Label(currSceneName);

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();

            GUILayout.BeginArea(new Rect(10, 90, 268, 480));

            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("xNum");
            string xNumStr = (GUILayout.TextField(graphSetting.xNum.ToString()));
            int.TryParse(xNumStr, out graphSetting.xNum);
            GUILayout.Label("zNum");
            string zNumStr = (GUILayout.TextField(graphSetting.zNum.ToString()));
            int.TryParse(zNumStr, out graphSetting.zNum);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("tilex  ");
            string tilexStr = GUILayout.TextField(graphSetting.tilex.ToString());
            int.TryParse(tilexStr, out graphSetting.tilex);  //int.Parse(GUILayout.TextField("20"));
            GUILayout.Label("tilez  ");
            string tilezStr = (GUILayout.TextField(graphSetting.tilez.ToString()));
            int.TryParse(tilezStr, out graphSetting.tilez);

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndArea();

            GUILayout.BeginHorizontal();
            //GUILayout.Label("Layer");
            //layerName = GUILayout.TextField(layerName);

            string[] options = new string[32];//{"CanJump", "CanShoot", "CanSwim"};
            for(int i = 0;i< 32;i++){
                options[i] = LayerMask.LayerToName(i);
            }

            graphSetting.layerMask = EditorGUILayout.MaskField(" 采集 layer ", graphSetting.layerMask, options);

            //EditorGUIUtility.
            //EditorGUILayout.MaskField(layer,

            if (GUILayout.Button("1 Build", GUILayout.Width(80)))
            {
                if (!EditorApplication.isPlaying)
                {
                    EditorUtility.DisplayDialog("error", "场景必须在运行状态 ", "ok");
                    return;
                }
                //Tree8.isDebug = false;
                GraphRoot.create(new GameObject("GraphRoot"), graphSetting.xNum, graphSetting.zNum, graphSetting.layerMask , graphSetting.tilex, graphSetting.tilez);
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.BeginArea(new Rect(10, 90 + 100, 568, 280));
            GUILayout.Label("2.保存场景捕获数据SceneRectData到Resources");
            if (GUILayout.Button("2 Save Assets ", GUILayout.Width(218)))
            {
                if (!EditorApplication.isPlaying)
                {
                    EditorUtility.DisplayDialog("error", "场景必须在运行状态 ", "ok");
                    return;
                }
                GraphRoot.save();
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(10, 90 + 100 + 48, 568, 280));
            GUILayout.Label("3.设置SceneRectData中的Prefab路径 ");
            if (GUILayout.Button("3 Set SceneRectData PrefabPath  ", GUILayout.Width(218)))
            {
                if (EditorApplication.isPlaying)
                {
                    EditorUtility.DisplayDialog("error", "场景不能在运行状态 ", "ok");
                    return;
                }
                //GraphRoot.setPrefab();
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(10, 90 + 100 + 48 * 2, 568, 280));
            GUILayout.Label("4.打包 SceneRectData 到 AssetBundles ");
            if (GUILayout.Button("4 AssetBundles SceneRectData  ", GUILayout.Width(218)))
            {
                GraphEditor.AssetBundles();
            }
            GUILayout.EndArea();
            GUILayout.EndHorizontal();
            /*
            GUILayout.BeginHorizontal();
            //获取当前打开场景(path)
            string currSceneName = EditorApplication.currentScene;
            //获取当前打开场景名称
            currSceneName = currSceneName.Substring(currSceneName.LastIndexOf("/") + 1);
            currSceneName = currSceneName.Replace(".unity", "");
            GUILayout.Label("SceneName");
            GUILayout.Label(currSceneName);

            if (GUILayout.Button("Export", GUILayout.Width(80)))
            {
                ExportSceneLightMap.Export();
            }
            GUILayout.EndHorizontal();

            // Lightmap 列表
            LightmapData[] currSceneLightMaps = LightmapSettings.lightmaps;
            int length = currSceneLightMaps.Length;

            foreach (LightmapData lightmapData in currSceneLightMaps)
            {
                GUILayout.BeginHorizontal();

                //GUILayout.Label(lightmapData.lightmapLight, typeof(Texture2D)) as Texture2D);
                EditorGUILayout.ObjectField(lightmapData.lightmapLight, typeof(Texture2D), true, GUILayout.Width(220));

                GUILayout.EndHorizontal();
            }
            //end Lightmap 列表
            //输出 Lightmap 列表
            for (int i = 0; i < length; i++)
            {
                GUILayout.BeginHorizontal();
                //获取 lm Texture2D
                Texture2D currLightMapColor = currSceneLightMaps[i].lightmapColor;
                Texture2D currLightMapDir = currSceneLightMaps[i].lightmapDir;

                //格式化名称
                string currLightMapColorName = string.Format("Assets/Resources/Lightmap_{1}/LightmapColor_{0}_{1}", i, currSceneName);
                string currLightMapDirName = string.Format("Assets/Resources/Lightmap_{1}/LightmapDir_{0}_{1}", i, currSceneName);

                GUILayout.Label("output >>>>>>> index = " + i);

                if (File.Exists(Application.dataPath + currLightMapDirName.Replace("Assets", "") + ".exr"))
                {
                    Texture2D currAssetLightMapDir = (Texture2D)AssetDatabase.LoadAssetAtPath(currLightMapDirName, typeof(Texture2D));
                    EditorGUILayout.ObjectField(currAssetLightMapDir, typeof(Texture2D), true, GUILayout.Width(160));
                }
                if (File.Exists(Application.dataPath + currLightMapColorName.Replace("Assets", "") + ".exr"))
                {
                    Texture2D currAssetLightMapColor = (Texture2D)AssetDatabase.LoadAssetAtPath(currLightMapColorName + ".exr", typeof(Texture2D));
                    EditorGUILayout.ObjectField(currAssetLightMapColor, typeof(Texture2D), true, GUILayout.Width(160));
                }
                GUILayout.EndHorizontal();
            }*/
            //end 输出 Lightmap 列表
            GUILayout.EndVertical();
        }
    }
}