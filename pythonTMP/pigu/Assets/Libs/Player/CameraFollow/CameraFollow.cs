using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Camera follow.自动寻找 playerTag ，完成跟随目标绑定
/// </summary>
public class CameraFollow : MonoBehaviour {
    
    public string playerTag = "Player";
    public Transform target;
    public Vector3 toCam = Vector3.zero;

    // Use this for initialization
    void Start () {
		Debug.Log ("CameraFollow");
	}

	public void reSet(){
		toCam = Vector3.zero;
	}

    public void initPlayerByTag()
    {
		if (toCam.Equals(Vector3.zero))
		{
			if (target == null) {
				GameObject gameObject = GameObject.FindGameObjectWithTag (playerTag);
				if (gameObject) {
					target = gameObject.transform;
				}
			}
			if (target)
				toCam = Camera.main.transform.position - target.position;
		}
    }
    // Update is called once per frame
    void Update () {
        initPlayerByTag();
        if(target != null)
        Camera.main.transform.position = target.position + toCam;
    }
}
