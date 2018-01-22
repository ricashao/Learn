using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGoOnOff : MonoBehaviour {
    public Transform[] golist;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnGUI () {
        int i = 0;
        foreach (Transform go in golist) {
            if (GUI.Button(new Rect(780, 160* i, 200, 160), go.name + "_" + go.gameObject. activeSelf))
            {
                go.gameObject.SetActive(!go.gameObject.activeSelf);
            }
            i++;
        }
    }
}
