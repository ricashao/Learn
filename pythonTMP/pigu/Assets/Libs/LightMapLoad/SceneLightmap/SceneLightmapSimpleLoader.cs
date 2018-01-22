using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Scene lightmap simple loader. 
/// 1. 烘焙场景
/// 2. 执行 RendererLMInfo/Add RendererLMInfo
/// 3. 设置 Texture2D
/// 4. 关闭或删除光照
/// </summary>
public class SceneLightmapSimpleLoader : MonoBehaviour {

    public Texture2D[] texture2DlightmapColor;
    public Texture2D[] texture2DlightmapDir;
    public Texture2D[] texture2DlightmapLight;

    //public List<Texture2D> texture2DlightmapColorList;
    //public List<Texture2D> texture2DlightmapDirList;
    //public List<Texture2D> texture2DlightmapLightList ; 
    // Use this for initialization
    virtual public void Start () {

        SetLightMap();
    }

    protected Texture2D[] AddArr(Texture2D[] arr, Texture2D tex) {
        int addNum = 1;
        Texture2D[] arrNew = new Texture2D[arr.Length + addNum];
        arr.CopyTo(arrNew, 0);
        arrNew[arr.Length] = tex;
        return arrNew;
    }

    protected void SetLightMap() {

        //LightmapData[] tempMapDatasOld = new LightmapData[LightmapSettings.lightmaps.Length];
        //LightmapSettings.lightmaps.CopyTo(tempMapDatasOld,0);

        // 光照贴图增量拷贝
        /*    int addNum = 1;
            int newStartIndex = LightmapSettings.lightmaps.Length;
            LightmapData[] tempMapDatas = new LightmapData[LightmapSettings.lightmaps.Length + addNum];
            LightmapSettings.lightmaps.CopyTo(tempMapDatas, 0);
       */
        if (texture2DlightmapLight != null)
        {
            int Count = texture2DlightmapLight.Length;
            int newStartIndex = LightmapSettings.lightmaps.Length;
            LightmapData[] tempMapDatas = new LightmapData[LightmapSettings.lightmaps.Length + Count];
            LightmapSettings.lightmaps.CopyTo(tempMapDatas, 0);

            for (int i = 0; i < Count; i++)
            {
                LightmapData data = new LightmapData();
                data.lightmapColor = texture2DlightmapLight[i];
                tempMapDatas[newStartIndex + i] = data;
            }

            LightmapSettings.lightmaps = tempMapDatas;
        }

        if (texture2DlightmapColor != null &&
            texture2DlightmapDir != null &&
            texture2DlightmapDir.Length > 0 &&
            texture2DlightmapColor.Length == texture2DlightmapDir.Length)
        {
            int Count = texture2DlightmapColor.Length;
            int newStartIndex = LightmapSettings.lightmaps.Length;
            LightmapData[] tempMapDatas = new LightmapData[LightmapSettings.lightmaps.Length + Count];
            LightmapSettings.lightmaps.CopyTo(tempMapDatas, 0);

            for (int i = 0; i < Count; i++)
            {
                LightmapData data = new LightmapData();
                data.lightmapColor = texture2DlightmapColor[i];
                data.lightmapDir = texture2DlightmapDir[i];
                tempMapDatas[newStartIndex + i] = data;
            }

            LightmapSettings.lightmaps = tempMapDatas;
        }

        //设置原来烘焙时的光照模式，这个不设置正确，默认模式就会只显示光照贴图
        LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;

    }
}
