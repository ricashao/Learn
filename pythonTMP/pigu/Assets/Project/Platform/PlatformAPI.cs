using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAPI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		if (Application.platform == RuntimePlatform.Android) {
			
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			
		} else if (Application.platform == RuntimePlatform.WebGLPlayer) {
			
		} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			
		} else if (Application.platform == RuntimePlatform.OSXPlayer){
			
		} else if (Application.platform == RuntimePlatform.WindowsEditor) {

		} else if (Application.platform == RuntimePlatform.OSXEditor){

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
