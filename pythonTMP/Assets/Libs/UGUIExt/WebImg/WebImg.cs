using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Libs;

public class WebImg : MonoBehaviour {

	public Image image;

	public string url;
	public string pbName;

	void Awake(){
		if(image == null)
		image = GetComponent<Image> ();
	}
    [SerializeField]
    private bool isSetNative = false;

	// Use this for initialization
	void Start () {
		//http://192.168.8.159/res/0.png
		//Libs.AssetManager.getInstance().InitAssetName2abPathDic("StreamingAssets_u3d_res_project");

		if(!string.IsNullOrEmpty(pbName)){
			LoadFromAb (pbName);
		}else
		if (!string.IsNullOrEmpty (this.url)) {
			Load();
		}
	}

	void Load (){

		if (url.StartsWith ("http")) {
			StartCoroutine (DownloadImage(url,image));
		} else {
			StartCoroutine (LoadLocalImage(url,image));
		}
	}

	void LoadFromAb(string pbName){
		
		Libs.AM.I.CreateFromCache(pbName, delegate (string eventName, Object objInstantiateTp){
			Texture2D texture2D = Instantiate(objInstantiateTp as Texture2D);

			Sprite sprite = Sprite.Create(texture2D,new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
			image.sprite = sprite;
            if (this.isSetNative) image.SetNativeSize();

        });
	}

	IEnumerator DownloadImage(string url, Image image)  
	{  
		Debug.Log("downloading new image:" + url.GetHashCode());//url转换HD5作为名字  
		WWW www = new WWW(url);  
		yield return www;  

		Texture2D tex2d = www.texture;  
		//将图片保存至缓存路径  
		byte[] pngData = tex2d.EncodeToPNG();  
		File.WriteAllBytes( PathTools.Combine( Application.persistentDataPath , url.GetHashCode().ToString()) , pngData);  

		Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));  
		image.sprite = m_sprite;
        if (this.isSetNative) image.SetNativeSize();
    }  

	IEnumerator LoadLocalImage(string path, Image image)  
	{  
		string streamingAssetsFileName = path;
		WWW www;
		if (PathTools.ExistsPersistentPath (streamingAssetsFileName)) {
			www = new WWW("file://"+PathTools.GetPersistentPath (streamingAssetsFileName)); 
			//fileText = System.IO.File.ReadAllText (PathTools.GetPersistentPath (streamingAssetsFileName)).Trim ();
		}  else {
			if (Application.platform == RuntimePlatform.Android) {
				www = new WWW (PathTools.GetAppContentPath (streamingAssetsFileName));
			} else {
				www = new WWW ("file://" + PathTools.GetAppContentPath (streamingAssetsFileName));
			}
		}
		//string filePath = PathTools.GetAssetPath(path);

		Debug.Log("getting local image:" + www.url);  
		///WWW www = new WWW(filePath);  
		yield return www;  

		Texture2D texture = www.texture;  
		Sprite m_sprite = Sprite.Create(texture,new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));  
		image.sprite = m_sprite;
        if (this.isSetNative) image.SetNativeSize();
    }  
			
	public void PbName (string name) {
		if (name != null && !name.Equals (this.pbName)) {
			this.pbName = name;

			LoadFromAb(pbName);
		} else {
			Debug.LogWarningFormat (" Ignore {0} ",url);
		}
	}

	// Update is called once per frame
	public void Url (string url) {

		if (url != null && !url.Equals (this.url)) {
			this.url = url;

			Load();
		} else {
			Debug.LogWarningFormat (" Ignore {0} ",url);
		}
	}
}
