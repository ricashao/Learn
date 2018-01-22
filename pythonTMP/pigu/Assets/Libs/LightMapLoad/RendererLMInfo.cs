using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RendererLMInfo : MonoBehaviour {
    [SerializeField]
    public int lightmapIndex;
    [SerializeField]
    public Vector4 lightmapScaleOffset;
    // Use this for initialization
    void Start () {
        Renderer renderer = GetComponent<Renderer>();

        renderer.lightmapIndex = lightmapIndex;
        renderer.lightmapScaleOffset = lightmapScaleOffset;
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("RendererLMInfo/Add RendererLMInfo")]
    static void AddRendererLMInfo() {

        Renderer[] renderers = FindObjectsOfType<Renderer>();
        for (int i =0;i< renderers.Length;i++) {
            Renderer renderer = renderers[i];

            RendererLMInfo rendererLMInfo =renderer.gameObject.GetComponent<RendererLMInfo>();
            if (rendererLMInfo != null) {
                GameObject.DestroyImmediate(rendererLMInfo);
            }

            if (renderer.lightmapIndex >= 0 && renderer.lightmapIndex != 655535) {
                rendererLMInfo = renderer.gameObject.AddComponent<RendererLMInfo>();
                rendererLMInfo.lightmapIndex = renderer.lightmapIndex;
                rendererLMInfo.lightmapScaleOffset = renderer.lightmapScaleOffset;
            }
        }

    }
    [UnityEditor.MenuItem("RendererLMInfo/Rem RendererLMInfo")]
    static void RemRendererLMInfo()
    {

        Renderer[] renderers = FindObjectsOfType<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];

            RendererLMInfo rendererLMInfo = renderer.gameObject.GetComponent<RendererLMInfo>();
            if (rendererLMInfo != null)
            {
                GameObject.DestroyImmediate(rendererLMInfo);
            }
        }

    }
#endif
}
