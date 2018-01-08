using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZhuYuU3d{
	public class LuaCallCsFun {

		public static void JumpScene(int index){
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync (index, LoadSceneMode.Single);
		}
		 
		public static void JumpSceneName(string sceneName){

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Single);
			//AsyncOperation asyncOperation = SceneManager.LoadSceneAsync (index, LoadSceneMode.Single);
		}

		public static void JumpToRun(){

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync ("Run", LoadSceneMode.Single);
		}

		public static void JumpToLoading(){

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync ("Loading", LoadSceneMode.Single);
		}

		public static void JumpToLauncher(){

			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync ("Launcher", LoadSceneMode.Single);
		}
	}
}