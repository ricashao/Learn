using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Libs{

	public delegate void OnRoleLoadCmpCallBack(GameObject roleGo);

    public class RoleLoader : MonoBehaviour {
        
        public InputField inputField; 
		public bool mapLoadCmp;
        public string abPath;

        AssetBundle assetBundle ;

		public GameObject instantiateGameObject;
        CharacterController characterController;
        /// <summary>
        /// 地图初始位置标识对象
        /// </summary>
        public string initPointGoName;
        public Vector3 init = Vector3.zero;

		public OnRoleLoadCmpCallBack onRoleLoadCmpCallBack;

    	void Start () {

            if (inputField == null)
            {
                inputField = GetComponentInChildren<InputField>();
            }
                
            if (abPath == null || abPath.Equals(""))
            {
               
            }
            else
            {
                Load();
            }
			if(mapLoadCmp == false)
           		EM.I.Add("MapLoadCmp",OnMapLoadCmp);
    	}
    	
        bool OnMapLoadCmp(string name,object data){
            
            if (name.Equals("MapLoadCmp"))
            {
                GameObject initPointGo = GameObject.Find(initPointGoName);
                //GameObject initPointGo = data as GameObject;
                if (initPointGo)
                {
                    instantiateGameObject.transform.position = initPointGo.transform.position;
                }
                if (characterController)
                {
                    characterController.enabled = true;
                }
            }
            return false;
        }

        public void Load(){

            if(abPath == null || abPath.Equals(""))
            {
                abPath = inputField.text;        
            }
            ABM.I.LoadOne(abPath,OnABCmp);

        }

        void OnABCmp(string name,AssetBundle ab){
        
            assetBundle = ab;

            string [] ns = ab.GetAllAssetNames ();
            GameObject go = ab.LoadAsset<GameObject>(ns[0]);
            instantiateGameObject = Instantiate (go);

            characterController = instantiateGameObject.GetComponentInChildren<CharacterController>();
            if (characterController)
            {
				characterController.enabled = mapLoadCmp;
            }

            instantiateGameObject.transform.position = init;

			if (onRoleLoadCmpCallBack != null) {
				onRoleLoadCmpCallBack (instantiateGameObject);
			}
        }

    	// Update is called once per frame
    	void Update () {
    		
    	}
    }

}
