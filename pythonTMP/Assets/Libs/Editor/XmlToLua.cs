using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

public class XmlToLua : EditorWindow
{

    private TextAsset xmlText;
    private string path = "/Resources/lua/config/";
    private string luaName;

    private Dictionary<string, ValueType> types = new Dictionary<string, ValueType>();

    [MenuItem("Window/XmlToLua")]
    private static void Init()
    {
        EditorWindow.GetWindow(typeof(XmlToLua));
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("导出路径");
        GUILayout.Space(20);
        path = EditorGUI.TextField(new Rect(0, 20, 400, 20), path);
        xmlText = EditorGUILayout.ObjectField("XML配置", xmlText, typeof(TextAsset), true) as TextAsset;
        if (GUILayout.Button("转换"))
        {
            if (path == string.Empty)
            {
                EditorUtility.DisplayDialog("Error", "导出路径为空", "ok");
                return;
            }
            if (!path.EndsWith("/"))
            {
                path += "/";
            }
            if (!path.StartsWith("/"))
            {
                path = path.Insert(0, "/");
            }

            if (xmlText == null)
            {
                EditorUtility.DisplayDialog("Error", "xml文件为空", "ok");
                return;
            }
            FileTools.CreateFolder(path);


            CreateLuaDataFile();
        }
    }

    private void CreateLuaDataFile()
    {
        XmlDocument _doc = new XmlDocument();
        _doc.LoadXml(xmlText.text.Trim());
        XmlNodeList childnodes = _doc.SelectSingleNode("XML/MODEL").ChildNodes;
        luaName = childnodes[0].Name.Substring(0, 1).ToUpper() + childnodes[0].Name.Substring(1).ToLower();
        string luaTxt = luaName;
        types.Clear();
        foreach (XmlNode node in childnodes)
        {
            XmlNodeList attributes = node.ChildNodes;
            foreach (XmlNode attribute in attributes)
            {
                this.CheckType(attribute.Name, attribute.InnerText);
            }
        }
        luaTxt += " ={\n";
        foreach (XmlNode node in childnodes)
        {
            luaTxt += "\t{";
            XmlNodeList attributes = node.ChildNodes;
            foreach (XmlNode attribute in attributes)
            {
                if (types[attribute.Name] == ValueType.number)
                {
                    luaTxt += attribute.Name + "=" + attribute.InnerText + ",";
                }
                else
                {
                    attribute.InnerText = attribute.InnerText.Trim('\'');
                    luaTxt += attribute.Name + "=\"" + attribute.InnerText + "\",";
                }
            }
            luaTxt = luaTxt.Remove(luaTxt.Length - 1);
            luaTxt += "},\n";
        }
        luaTxt = luaTxt.Remove(luaTxt.Length - 2);
        luaTxt += "\n}";
        SaveLuaFile(luaTxt);
    }

    private void SaveLuaFile(string luaTxt)
    {
        string saveName = path + luaName + ".lua"; 
        Debug.Log(saveName);
        FileTools.Write(saveName, luaTxt);
    }

    private void CheckType(string name, string value)
    {
        bool isNum = IsNumeric(value);
        ValueType cur;
        types.TryGetValue(name, out cur);
        if (cur == ValueType.none)
        {
            types.Add(name, isNum ? ValueType.number : ValueType.str);
        }
        else if ((cur == ValueType.number) && (!isNum))
        {
            types[name] = ValueType.str;
        }
    }

    public static bool IsNumeric(string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
    }

    enum ValueType
    {
        none = 0,
        number = 1,
        str = 2,
    }
}
