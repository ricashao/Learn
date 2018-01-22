using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LightBakeTools : Editor {

	// Use this for initialization
    [MenuItem("LightTools/Bake/Set Reneder LightMapOnly",false, 11)]
    public static void SetRenederLightMapOnly () {
        GameObject[] gos = Selection.gameObjects;
        if(gos == null || gos.Length == 0){
            if (EditorUtility.DisplayDialog("LOG", "请选择一个gameObject ", "ok")){
            }
            return;
        }
        foreach(GameObject go in gos){
            Renderer[] rs = go.GetComponentsInChildren<Renderer>();
            foreach(Renderer r in rs )
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
	}
	
    [MenuItem("LightTools/Bake/Set Reneder ShadowCasting Off",false, 11)]
    public static void SetRenederShadowCastingOff () {
        GameObject[] gos = Selection.gameObjects;
        if(gos == null || gos.Length == 0){
            if (EditorUtility.DisplayDialog("LOG", "请选择一个gameObject ", "ok")){
            }
            return;
        }
        foreach (GameObject go in gos)
        {
            Renderer[] rs = go.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }
    }

    [MenuItem("LightTools/Bake/Set Reneder ShadowCasting Off",false, 11)]
    public static void SetRenederShadowCastingOn () {
        GameObject[] gos = Selection.gameObjects;
        if(gos == null || gos.Length == 0){
            if (EditorUtility.DisplayDialog("LOG", "请选择一个gameObject ", "ok")){
            }
            return;
        }
        foreach (GameObject go in gos)
        {
            Renderer[] rs = go.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

            }
        }
    }

    [MenuItem("LightTools/Bake/Set Reneder ShadowCasting TwoSided",false, 11)]
    public static void SetRenederShadowCastingTwoSided () {
        GameObject[] gos = Selection.gameObjects;
        if(gos == null || gos.Length == 0){
            if (EditorUtility.DisplayDialog("LOG", "请选择一个gameObject ", "ok")){
            }
            return;
        }
        foreach (GameObject go in gos)
        {
            Renderer[] rs = go.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
            }
        }
    }
}
