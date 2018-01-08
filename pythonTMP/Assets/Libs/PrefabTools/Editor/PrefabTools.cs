using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabTools  {
    /// <summary>
    /// Sets the prefab ab path by self path.
    /// 设置打包路径 右键目录中
    /// </summary>
    [MenuItem("Assets/PrefabTools/SetPrefabAbPath by SelfPath")]
    public static void SetPrefabAbPathBySelfPath(){
        Object selectionObject =  Selection.activeContext;
        selectionObject =  Selection.activeObject;
        int activeInstanceID =  Selection.activeInstanceID;
        string[] assetGUIDs =  Selection.assetGUIDs;

        string assetPath = AssetDatabase.GetAssetPath(activeInstanceID);
        string suffix= assetPath.Substring(assetPath.LastIndexOf(".") + 1);
        //设置打包路径
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
        //设置Bundle文件名
        assetImporter.assetBundleName = assetPath.Substring(0,assetPath.LastIndexOf("."));
        //设置Bundle文件的扩展名  
        assetImporter.assetBundleVariant = suffix + "_ab";//"prefab";
        assetImporter.userData = suffix;
        //Selection.selectionChanged += onSelectionChanged;
    }
    /// <summary>
    /// Sets the prefab ab path by self path I.
    /// 设置整个目录的打包路径 右键目录中
    /// </summary>
    [MenuItem("Assets/PrefabTools/SetPrefabAbPath by SelfPath in Path")]
    public static void SetPrefabAbPathBySelfPathIN(){
        //UnityEngine.Object[] arr=Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.TopLevel);  
        //Debug.LogError(Application.dataPath.Substring(0,Application.dataPath.LastIndexOf('/'))+"/"+ AssetDatabase.GetAssetPath(arr[0]));  
        //SelectionMode.DeepAssets
        UnityEngine.Object[] arr=Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);  
        //Debug.LogError(Application.dataPath.Substring(0,Application.dataPath.LastIndexOf('/'))+"/"+ AssetDatabase.GetAssetPath(arr[0])); 

        Object[] activeGOs = Selection.GetFiltered( typeof(GameObject),SelectionMode.Editable | SelectionMode.TopLevel);

        for(int i = 0;i < arr.Length; i++ ){
            
            string assetPath = AssetDatabase.GetAssetPath(arr[i]);
            if (assetPath.LastIndexOf(".") > 0 && assetPath.LastIndexOf(".") > assetPath.LastIndexOf("/"))
            {
                //有后缀的文件
            }else{
                Debug.LogWarningFormat("无后缀的文件 {0} 默认为目录，跳过！",assetPath);
                continue;
            }
            //判断路径是否存在
            //AssetDatabase.IsValidFolder
            //设置打包路径
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            //设置Bundle文件名
            assetImporter.assetBundleName = assetPath.Substring(0,assetPath.LastIndexOf("."));
            //后缀
            string suffix= assetPath.Substring(assetPath.LastIndexOf(".") + 1);
            //设置Bundle文件的扩展名  
            assetImporter.assetBundleVariant = suffix + "_ab";//"prefab";
            assetImporter.userData = suffix;
        }

    }

    static void onSelectionChanged(){
        Debug.LogWarning("Selection.selectionChanged");
    }
}
