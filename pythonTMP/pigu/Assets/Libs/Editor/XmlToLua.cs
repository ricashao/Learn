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
    private string path = "Resources/lua/config";
    private string luaName;
    private string type = "";

    private string[] keyword =
    {
        "type",
    };

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
        EditorGUILayout.LabelField("导出类型");
        GUILayout.Space(20);
        type = EditorGUI.TextField(new Rect(0, 60, 400, 20), type);
        GUILayout.Space(20);
        xmlText = EditorGUILayout.ObjectField("XML配置", xmlText, typeof(TextAsset), true) as TextAsset;
        if (GUILayout.Button("转换"))
        {
            if (path == string.Empty)
            {
                EditorUtility.DisplayDialog("Error", "导出路径为空", "ok");
                return;
            }
            if (type == string.Empty)
            {
                EditorUtility.DisplayDialog("Error", "导出类型为空", "ok");
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
        XmlNodeList childnodes = _doc.SelectNodes("XML/MODEL/" + type);
        if (childnodes == null || childnodes.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "xml文件不存在类型:" + type, "ok");
            return;
        }
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
                string exportName = GetExportName(attribute.Name);
                if (types[attribute.Name] == ValueType.number)
                {
                    if (attribute.InnerText.Trim('\'') != string.Empty)
                        luaTxt += exportName + "=" + attribute.InnerText + ",";
                    else
                        luaTxt += exportName + "=" + 0 + ",";
                }
                else if (types[attribute.Name] == ValueType.str)
                {
                    attribute.InnerText = attribute.InnerText.Trim('\'');
                    luaTxt += exportName + "=\"" + attribute.InnerText + "\",";
                }
                else if (types[attribute.Name] == ValueType.any)
                {
                    attribute.InnerText = attribute.InnerText.Trim('\'');
                    luaTxt += exportName + "=\"" + attribute.InnerText + "\",";
                }
            }
            luaTxt = luaTxt.Remove(luaTxt.Length - 1);
            luaTxt += "},\n";
        }
        luaTxt = luaTxt.Remove(luaTxt.Length - 2);
        luaTxt += "\n}";
        SaveLuaFile(luaTxt);
    }

    private string GetExportName(string name)
    {
        if (((IList)keyword).Contains(name))
            return name + 'k';
        else
            return name;
    }

    private void SaveLuaFile(string luaTxt)
    {
        string saveName = path + luaName + ".lua";
        if (FileTools.IsFileExists(saveName))
        {
            string tmp = FileTools.Read(saveName);
            string[] splits = Regex.Split(tmp, "--write by hand");
            luaTxt += "\n--write by hand";
            luaTxt += splits[1];
        }
        else
        {
            luaTxt += "\n--write by hand";
        }
        FileTools.Write(saveName, luaTxt);
        EditorUtility.DisplayDialog("Success", "导出成功，path：" + saveName, "ok");
    }

    private void CheckType(string name, string value)
    {
        bool isNum = IsNumeric(value);
        //服务端的xml表 空''去掉 isAny用来判断到底是不是字符串类型
        bool isAny = value.Trim('\'') == string.Empty;
        ValueType cur;
        types.TryGetValue(name, out cur);
        if (cur == ValueType.none)
        {
            if (isAny)
            {
                types.Add(name, ValueType.any);
            }
            else
            {
                types.Add(name, isNum ? ValueType.number : ValueType.str);
            }
        }
        else if ((cur == ValueType.number) && (!isAny && !isNum))
        {
            types[name] = ValueType.str;
        }
        else if ((cur == ValueType.number) && (isAny && !isNum))
        {
            types[name] = ValueType.number;
        }
        else if ((cur == ValueType.any) && isNum)
        {
            types[name] = ValueType.number;
        }
        else if ((cur == ValueType.any) && !isAny && !isNum)
        {
            types[name] = ValueType.str;
        }
        else if ((cur == ValueType.any) && isAny)
        {
            types[name] = ValueType.any;
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
        any = 3,
    }
}
