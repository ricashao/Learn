using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStarter : MonoBehaviour {

    List<string> lstcontent = new List<string>();

	public UILoopList ull;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 100; i++)
        {
            lstcontent.Add(i.ToString());
        }
		ull.Data(lstcontent.ToArray());
	}
	
}
