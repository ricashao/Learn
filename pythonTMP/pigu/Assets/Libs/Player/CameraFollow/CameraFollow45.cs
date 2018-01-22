using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow45 : MonoBehaviour {

    public string playerTag = "Player";
    public Transform target;
    public Vector3 toCam = Vector3.zero;
    public float distance = 7.98f;
    // Use this for initialization
    void Start () {

    }

    public void initPlayerByTag()
    {
        if (target) return;
        GameObject gameObject = GameObject.FindGameObjectWithTag(playerTag);
        if (gameObject)
        {
            target = gameObject.transform;
            toCam = target.position + new Vector3(distance, distance, -distance);
        }
    }
    // Update is called once per frame
    void Update () {
        initPlayerByTag();
        if (target != null)
        {
            toCam.x = distance;
            toCam.y = distance;
            toCam.z = -distance;

            Camera.main.transform.position = target.position + toCam;
        }
    }
}
