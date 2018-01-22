using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZhuYuU3d.Game
{

    public class ProgramEntry : MonoBehaviour
    {
        ApplicationFacade mAF = null;
	    // Use this for initialization
		public bool EnabledUpdate = true;
	    void Start ()
        {
			
			if(!EnabledUpdate){
				AsyncOperation asyncOperation = SceneManager.LoadSceneAsync (1, LoadSceneMode.Single);
				return;
			}

            mAF = new ApplicationFacade();
			mAF.StartUp(mstrAssetConfig);
	    }
	
		[SerializeField]
		string mstrAssetConfig="";
	}

}
