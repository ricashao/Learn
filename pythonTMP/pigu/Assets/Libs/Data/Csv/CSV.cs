using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSVFile  {

	public string [][]Array;
    public Hashtable Id2RowIndexMap = new Hashtable();

    public string GetDataByRowAndCol(int nRow, int nCol)
	{
		if (Array.Length <= 0 || nRow >= Array.Length)
			return "";
		if (nCol >= Array[0].Length)
			return "";
		
		return Array[nRow][nCol];
	}
    /// <summary>
    ///  根据 配置Id 和 字段名 查找 字段内容
    /// </summary>
    /// <param name="nId">根据行号</param>
    /// <param name="strName">字段名</param>
    /// <returns></returns>
    public string GetDataByIdAndName(int nId, string strName)
	{
        if (Id2RowIndexMap.ContainsKey(nId)){
            return GetDataByRowIndexAndName((int)Id2RowIndexMap[nId], strName);
        }
    
        if (Array.Length <= 0)
			return "";
		
		int nRow = Array.Length;
		int nCol = Array[0].Length;		
		for (int i = 1; i < nRow; ++i) {
			//string strId = string.Format("\n{0}", nId);
			string strId = string.Format("{0}", nId);
			if (Array[i][0] == strId) {

                Id2RowIndexMap.Add(nId, i);

                for (int j = 0; j < nCol; ++j) {
					if (Array[0][j] == strName) {
						return Array[i][j];
					}
				}
			}
		}
		return "";
	}
    /// <summary>
    /// 根据行号 和 字段名 查找 字段内容
    /// Read ("config/role").GetDataByIdAndName (1, "name")
    /// </summary>
    /// <param name="rowIndex"> 根据行号 </param>
    /// <param name="strName"> 字段名 </param>
    /// <returns></returns>
    public string GetDataByRowIndexAndName(int rowIndex, string strName)
    {
        if (Array.Length <= 0)
            return "";

        int nCol = Array[0].Length;
       
        for (int j = 0; j < nCol; ++j)
        {
            if (Array[0][j] == strName)
            {
                return Array[rowIndex][j];
            }
        }
       
        return "";
    }
    /// <summary>
    /// 根据 配置Id 和 字段名 查找 字段内容
    /// Read ("config/role").GetDataByStringIdAndName("1", "name")
    /// </summary>
    /// <param name="nId"> 配置Id </param>
    /// <param name="strName"> 字段名 </param>
    /// <returns></returns>
    public string GetDataByStringIdAndName(string nId, string strName)
	{
		if (Array.Length <= 0)
			return "";
		
		int nRow = Array.Length;
		int nCol = Array[0].Length;		
		for (int i = 1; i < nRow; ++i) {
			//string strId = string.Format("\n{0}", nId);
			string strId = string.Format(nId);
			if (Array[i][0] == strId) {
				for (int j = 0; j < nCol; ++j) {
					if (Array[0][j] == strName) {
						return Array[i][j];
					}
				}
			}
		}
		
		return "";
	}

	public int GetDataByIdAndNameToInt(int nId, string strName){
        string value = GetDataByIdAndName(nId, strName);
        if(value.Length <= 0)
        {
            value = "0";
        }
        return int.Parse (value);
	}
	
	public float GetDataByIdAndNameToFloat(int nId, string strName){
        string value = GetDataByIdAndName(nId, strName);
        if(value.Length <= 0)
        {
            value = "0";
        }
        return float.Parse (value);
	}

    public long GetDataByIdAndNameToLong(int nId, string strName)
    {
        string value = GetDataByIdAndName(nId, strName);
        if (value.Length <= 0)
        {
            value = "0";
        }
        return long.Parse(value);
    }
}
public class CVS  {

	private static CVS _instance;

	public static CVS Instance
	{
		get{
			if (_instance == null) {
				_instance = new CVS();
			}
			return _instance;
		}
	}

	private Hashtable files = new Hashtable();

    public List<string> _csv_name_list = new List<string>();
    public delegate void OnEvent();
    public IEnumerator LoadCSVList(OnEvent _event)
    {
        //string filePath = Util.DataPath + "config/config_list.txt";
        string filePath = PathTools.GetAssetPath("config/config_list.txt");
        WWW www = new WWW(filePath);
        yield return www;

        string text = www.text;
        text = text.Replace("\r\n", "");
        string[] list = text.Split(',');

        for(int i = 0; i < list.Length; ++i)
        {
            string _csv_name = list[i].Replace("\\", "");
            _csv_name_list.Add(_csv_name);
        }

        _event();

        yield return null;
    }

    public IEnumerator LoadAllCsv(OnEvent _event)
    {
        for(int i = 0; i < _csv_name_list.Count; ++i)
        {
            //string filePath = Util.DataPath + "config/" + _csv_name_list[i];
            string filePath = PathTools.GetAssetPath("config/" + _csv_name_list[i]);
            WWW www = new WWW(filePath);

            yield return www;

            string text = www.text;

            ReadFromString(text, _csv_name_list[i]);
        }

        _event();

        yield return null;
    }

    public void ReadFromString(string _text,string _fileName)
    {
        CSVFile file = new CSVFile();
        string[] lineArray = _text.Trim().Replace(",", ";").Replace("\"", "").Split("\n"[0]);
        //创建二维数组
        file.Array = new string[lineArray.Length][];

        //把csv中的数据储存在二位数组中
        for (int i = 0; i < lineArray.Length; i++)
        {
            //Array[i] = lineArray[i].Split (',');
            //lineArray[i] = lineArray[i].Trim();
            file.Array[i] = lineArray[i].Trim().Split(';');
        }

        string fileName = _fileName.Replace(".csv", "");

        files[fileName] = file;
    }

    public CSVFile Read (string fileName ="config/role")
	{
		CSVFile file = (CSVFile)files[fileName];
		if (file != null)
			return file;

		file = new CSVFile ();

		//string filePath = Util.DataPath + "config/" + fileName + ".csv";
        string filePath = PathTools.GetAssetPath("config/" + fileName + ".csv");
		//if(!File.Exists(filePath)){
		//	filePath = Util.AppContentPath() + "config/" +  fileName + ".csv";
		//}

		string text = File.ReadAllText(filePath);


		/*
		FileStream  fs1 = new FileStream( filePath,FileMode.Open); 
		StreamReader sw = new StreamReader(fs1);
		string text = sw.ReadToEnd();

		sw.Close();
		sw.Dispose();
		*/

		//读取csv二进制文件
		/*
		TextAsset binAsset = Resources.Load (fileName, typeof(TextAsset)) as TextAsset;		
		string text = binAsset.text;
		*/
		//读取每一行的内容
		//string [] lineArray = binAsset.text.Split ("\r"[0]);
		string [] lineArray = text.Trim().Replace(",",";").Replace("\"","").Split ("\n"[0]);
		//创建二维数组
		file.Array = new string [lineArray.Length][];
		
		//把csv中的数据储存在二位数组中
		for(int i =0;i < lineArray.Length; i++)
		{
			//Array[i] = lineArray[i].Split (',');
			//lineArray[i] = lineArray[i].Trim();
			file.Array[i] = lineArray[i].Trim().Split (';');
		}

		files [fileName] = file;
		return file;
	}

	public CSVFile getFile(string fileName ="config/role"){
        //return Read (fileName);

        CSVFile file = (CSVFile)files[fileName];
        if (file != null)
            return file;

        return null;
    }

    /*
	// Use this for initialization
	void Start () {

		Read ("config/role");
		Debug.Log (Read ("config/role").GetDataByIdAndName (1, "name"));
		Read ("config/npc");
		Debug.Log  (Read ("config/npc").GetDataByIdAndName (1, "name"));
		Read ("config/obj");
		Debug.Log  (Read ("config/obj").GetDataByIdAndName (1, "name"));
		Read ("config/building");
		Debug.Log  (Read ("config/building").GetDataByIdAndName (1, "name"));
		Debug.Log  (Read ("config/building").GetDataByIdAndNameToInt (1, "xw"));
		Debug.Log  (Read ("config/building").GetDataByIdAndNameToInt (1, "zw"));
		Read ("config/terrain");
		Debug.Log  (Read ("config/terrain").GetDataByIdAndName (1, "name"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    */

	/*
local a = {["x"] = 12, ["mutou"] = 99, [3] = "hello"}
print(a["x"]);

local a = {x = 12, mutou = 99, [3] = "hello"}
print(a["x"]);

local a = {x = 12, mutou = 99, [3] = "hello"}
print(a.x);

local a = {[1] = 12, [2] = 43, [3] = 45, [4] = 90}

local a = {12, 43, 45, 90}
print(a[1]);

local a = {
 {x = 1, y = 2},
 {x = 3, y = 10}
}
	 */
	static public string CSVToLua (string fileName)
	{
		//读取csv二进制文件
		TextAsset binAsset = Resources.Load (fileName, typeof(TextAsset)) as TextAsset;		
		
		//读取每一行的内容
		//string [] lineArray = binAsset.text.Split ("\r"[0]);
		string [] lineArray = binAsset.text.Trim().Replace(",type,",",_type,").Replace("repeat","_repeat").Replace(",",";").Replace("\"","").Split ("\n"[0]);

		string [] headArr = null;
		if(lineArray.Length > 0){
			headArr = lineArray[0].Trim().Split (';');
		}
		string table = "" + fileName.Replace("/","_").Replace("\\","_") + " = {\n";
		//把csv中的数据储存在二位数组中
		for(int i =1;i < lineArray.Length; i++)
		{
			//Array[i] = lineArray[i].Split (',');
			//lineArray[i] = lineArray[i].Trim();
			string [] column = lineArray[i].Trim().Split (';');

			string lualine = (i>1 ? ",":"")  + "[\""+ column[0] +"\"] = {";

			for(int j = 0;j < column.Length; j++)
			{
				lualine += (j>0 ? ",":"") + headArr[j] + "=" + "\"" + column[j] + "\"";
			}

			lualine += "}\n";

			table += lualine;
		}

		table += "}";

		Debug.Log(table);

		return table;
	}

	static public string CSVToLuaByStr (string str,string fileName){
		//读取每一行的内容
		//string [] lineArray = binAsset.text.Split ("\r"[0]);

		string [] lineArray = str.Trim().Replace(",type,",",_type,").Replace("repeat","_repeat").Replace(",",";").Replace("\"","").Split ("\n"[0]);
		
		string [] headArr = null;
		if(lineArray.Length > 0){
			headArr = lineArray[0].Trim().Split (';');
		}
		string table = "" + fileName.Replace("/","_").Replace("\\","_") + " = {\n";
		//把csv中的数据储存在二位数组中
		for(int i =1;i < lineArray.Length; i++)
		{
			//Array[i] = lineArray[i].Split (',');
			//lineArray[i] = lineArray[i].Trim();
			string [] column = lineArray[i].Trim().Split (';');
			
			string lualine = (i>1 ? ",":"")  + "[\""+ column[0] +"\"] = {";

			if(column.Length != headArr.Length){
				UnityEngine.Debug.LogError("fileName = " +fileName+ " line = " + i + " headArr----------->>>" + column.Length + " column----------->>>" + headArr.Length);
			}

			for(int j = 0;j < column.Length && j< headArr.Length; j++)
			{
				lualine += (j>0 ? ",":"") + headArr[j] + "=" + "\"" + column[j] + "\"";
			}
			
			lualine += "}\n";
			
			table += lualine;
		}
		
		table += "}";
		
		Debug.Log(table);
		
		return table;
	}
}
