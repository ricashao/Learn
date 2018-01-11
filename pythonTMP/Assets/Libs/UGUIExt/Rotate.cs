using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    float _fspeed = 10.0f;

     void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), _fspeed * Time.deltaTime);
    }

}
