using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Text;
using DynamicRectThc;

public class ExportLightMapWindow : EditorWindow
{
    [MenuItem("Window/ExportLightMap")]
    static void Initialize()
    {
        ExportLightMapWindow window = (ExportLightMapWindow)EditorWindow.GetWindowWithRect(typeof(ExportLightMapWindow), new Rect(0, 0, 386, 382), false, "ExportLightMapWindow");
        window.Show();
    }

    void OnDestroy()
    {

    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Space(20);

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

        GUILayout.Space(20);    
        // Lightmap 列表
        LightmapData[] currSceneLightMaps = LightmapSettings.lightmaps;
        int length = currSceneLightMaps.Length;

        foreach (LightmapData lightmapData in currSceneLightMaps)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("原始LM");
            //GUILayout.Label(lightmapData.lightmapLight, typeof(Texture2D)) as Texture2D);
            EditorGUILayout.ObjectField(lightmapData.lightmapColor, typeof(Texture2D), true, GUILayout.Width(220));

            GUILayout.EndHorizontal();
        }
        //end Lightmap 列表
        GUILayout.Space(20);
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

            if (File.Exists(Application.dataPath + currLightMapDirName.Replace("Assets","") + ".exr")) {
                Texture2D currAssetLightMapDir = (Texture2D) AssetDatabase.LoadAssetAtPath(currLightMapDirName, typeof(Texture2D));
                EditorGUILayout.ObjectField(currAssetLightMapDir, typeof(Texture2D), true, GUILayout.Width(160));
            }
            if (File.Exists(Application.dataPath + currLightMapColorName.Replace("Assets", "") + ".exr"))
            {
                Texture2D currAssetLightMapColor = (Texture2D)AssetDatabase.LoadAssetAtPath(currLightMapColorName + ".exr", typeof(Texture2D));
                EditorGUILayout.ObjectField(currAssetLightMapColor, typeof(Texture2D), true, GUILayout.Width(160));
            }
            GUILayout.EndHorizontal();
        }
        //end 输出 Lightmap 列表
        GUILayout.EndVertical();
    }
}