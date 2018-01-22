using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadRes : MonoBehaviour {

	public static byte[] ReadByte(string fileName){
		
		byte[] data = null;

		if (PathTools.ExistsPersistentPath (fileName)) {
			//bytes = System.IO.File.ReadAllText (PathTools.GetPersistentPath (fileName)).Trim ();
			data = System.IO.File.ReadAllBytes (PathTools.GetPersistentPath (fileName));

			Debug.LogWarningFormat ("ReadRes load >> {0} ",PathTools.GetPersistentPath (fileName));
		}  else {
			if (Application.platform == RuntimePlatform.Android) {

				WWW www = new WWW(PathTools.GetAppContentPath (fileName));
				//yield return www;
				while (true){
					if (www.isDone || !string.IsNullOrEmpty(www.error)){
						System.Threading.Thread.Sleep(50); 
						if (!string.IsNullOrEmpty(www.error)){
							Debug.LogError(www.error);
						}else{
							data = www.bytes;
						}
						break;
					}
				}
			}  else {
				data = System.IO.File.ReadAllBytes (PathTools.GetAppContentPath (fileName));
			}

			Debug.LogFormat ("ReadRes load >> {0} ",PathTools.GetPersistentPath (fileName));
		}
	
		if (data == null) {

			Debug.LogErrorFormat ("error Read streamingAssetsFile {0} ", fileName);
			return null;
		}

		return data;
	}

	public static string ReadStr(string fileName){

		string data = null;

		if (PathTools.ExistsPersistentPath (fileName)) {
			//bytes = System.IO.File.ReadAllText (PathTools.GetPersistentPath (fileName)).Trim ();
			data = System.IO.File.ReadAllText (PathTools.GetPersistentPath (fileName));

			Debug.LogWarningFormat ("ReadRes load >> {0} ",PathTools.GetPersistentPath (fileName));
		}  else {
			if (Application.platform == RuntimePlatform.Android) {

				WWW www = new WWW(PathTools.GetAppContentPath (fileName));
				//yield return www;
				while (true){
					if (www.isDone || !string.IsNullOrEmpty(www.error)){
						System.Threading.Thread.Sleep(50); 
						if (!string.IsNullOrEmpty(www.error)){
							Debug.LogError(www.error);
						}else{
							data = www.text;
						}
						break;
					}
				}
			}  else {
				data = System.IO.File.ReadAllText (PathTools.GetAppContentPath (fileName));
			}

			Debug.LogFormat ("ReadRes load >> {0} ",PathTools.GetPersistentPath (fileName));
		}

		if (data == null) {

			Debug.LogErrorFormat ("error Read streamingAssetsFile {0} ", fileName);
			return null;
		}

		return data;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
