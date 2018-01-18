using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XLua;

namespace ZhuYuU3d{
	
	[LuaCallCSharp]
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

		public static byte[] ReadByte(string fileName){
			
			return ReadRes.ReadByte (fileName);
		}

		public static byte[] ReadByteForLua(string fileName){

			#if UNITY_EDITOR
			return System.IO.File.ReadAllBytes(PathTools.Combine(Application.dataPath+"/Resources",fileName) );
			#else
			return ReadRes.ReadByte (fileName);
			#endif
		}

	}
}