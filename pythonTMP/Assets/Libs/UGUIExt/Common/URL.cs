using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URL
{
    private static URL m_Instance;
    public static URL Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new URL();
            }
            return m_Instance;
        }
    }

    
    public string StreamingJsonURL = "";     //Json在StreamingAssets中路径
    public string CachJsonURL = "";           //Json在temporaryCachePath中路径
    public string StreamingIconURL = "";     //Icon在StreamingAssets中路径
    public string StreamingModelURL = "";     //模型在StreamingAssets中路径
    public string CachIconURL = "";           //Icon在temporaryCachePath中路径
    public string StreamingXmlURL = "";     //Xml在StreamingAssets中路径
    public string CachXmlURL = "";     //Xml在temporaryCachePath中路径
    public string MdoleXmlURL = "";     //Xml在StreamingAssets中路径
    public string CachModleURL = "";     //模型在在temporaryCachePath中路径
    public string CachModleAnimationURL = "";     //模型动画在在temporaryCachePath中路径
    public string CachModleXmlURL = "";     //模型xml在在temporaryCachePath中路径
    public string StreamingModelAnimationURL = "";     //模型动画在在StreamingAssets中路径
    public string ServerDataURL = "";
    public static string VersionXml = "XmlVersion.Xml";     //配置表名字

    public string CachPath = "";     //临时文件夹根目录
    public string TempPath = "";     //临时文件夹根目录

    public string PathPruffix = "";     //临时文件夹根目录
    private URL()
    {
        StreamingIconURL = Application.streamingAssetsPath + "/Icon/";
        CachIconURL = Application.temporaryCachePath + "/Icon/";
        StreamingJsonURL = Application.streamingAssetsPath + "/Json/";
        CachJsonURL = Application.temporaryCachePath + "/Json/";
        StreamingXmlURL = Application.streamingAssetsPath + "/Xml/";
        CachXmlURL = Application.temporaryCachePath + "/Xml/";
        CachModleURL = Application.temporaryCachePath + "/Model/";
        CachModleAnimationURL = Application.temporaryCachePath + "/ModelAnimation/";
        CachModleXmlURL = Application.temporaryCachePath + "/XMLModle/";
        CachPath = Application.temporaryCachePath + "/";
        TempPath = Application.temporaryCachePath + "/Temp/";
        StreamingModelURL = Application.streamingAssetsPath + "/Model/";
        StreamingModelAnimationURL = Application.streamingAssetsPath + "/ModelAnimation/";
        MdoleXmlURL = Application.streamingAssetsPath + "/ModelXml/";
#if UNITY_EDITOR
        PathPruffix = "file:///";

#elif UNITY_IPHONE			
           PathPruffix = "file:///";      
#elif UNITY_ANDROID
        PathPruffix = "jar:file://";  
        StreamingIconURL = Application.dataPath+"!/assets" + "/Icon/";
        CachIconURL = Application.temporaryCachePath + "/Icon/";
        StreamingJsonURL = Application.dataPath+"!/assets" + "/Json/";
         
        CachJsonURL = Application.temporaryCachePath + "/Json/";
        StreamingXmlURL = Application.dataPath+"!/assets" + "/Xml/";
        CachXmlURL = Application.temporaryCachePath + "/Xml/";
        CachModleURL = Application.temporaryCachePath + "/Model/";
        CachModleAnimationURL = Application.temporaryCachePath + "/ModelAnimation/";
        CachModleXmlURL = Application.temporaryCachePath + "/XMLModle/";
        ServerDataURL = ServerURL + "Android/";
        CachPath = Application.temporaryCachePath + "/";
        TempPath = Application.temporaryCachePath + "/Temp/";
        StreamingModelURL = Application.dataPath+"!/assets" + "/Model/";  
        StreamingXmlURL = Application.dataPath+"!/assets" + "/Xml/";  
        StreamingModelAnimationURL = Application.dataPath+"!/assets" + "/ModelAnimation/";  
        MdoleXmlURL = Application.dataPath+"!/assets" + "/ModelXml/";  
#endif
    }
}
