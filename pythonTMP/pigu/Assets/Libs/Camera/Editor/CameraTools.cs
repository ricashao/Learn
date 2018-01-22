using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraTools : Editor
{
    [MenuItem("Libs/CameraTools/Set 45 look 000")]
    public static void Set45LooK000()
    {
        GameObject go =  Selection.activeGameObject;

        if (go == null) {
            EditorUtility.DisplayDialog("error", "必须选中一个相机 ", "ok");
            return;
        }

        Camera camera = go.GetComponent<Camera>();

        if (camera == null)
        {
            EditorUtility.DisplayDialog("error", "必须选中一个相机 ", "ok");
            return;
        }

        go.transform.position = new Vector3(15.98f, 15.98f, -15.98f);
        //go.transform.localEulerAngles = new Vector3(31.248f, -45.94f, 0);
        go.transform.LookAt(Vector3.zero);

    }

    [MenuItem("Libs/CameraTools/Set 45 look obj")]
    public static void Set45lookObj()
    {
        GameObject go = Selection.activeGameObject;
        
        if (go == null)
        {
            EditorUtility.DisplayDialog("error", "必须选中一个GO ", "ok");
            return;
        }
        /*
        Camera camera = go.GetComponent<Camera>();

        if (camera == null)
        {
            EditorUtility.DisplayDialog("error", "必须选中一个相机 ", "ok");
            return;
        }
        */

        Camera.main.transform.position = go.transform.position + new Vector3(7.98f, 7.98f, -7.98f);

        Camera.main.transform.LookAt(go.transform.position);
    }

}
