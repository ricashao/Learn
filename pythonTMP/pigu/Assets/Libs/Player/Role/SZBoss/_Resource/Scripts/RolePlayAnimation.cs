using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePlayAnimation : MonoBehaviour {
    //Animation animation;
    // Use this for initialization
    void Start () {
        // = GetComponent<Animation>();

	}

    private void OnGUI()
    {
        Animation anim = GetComponent<Animation>();
        int i = 0;
        foreach (AnimationState state in anim)
        {  // state.speed = 0.5F;
            if (GUI.Button(new Rect(0, 20 * i, 80, 20), state.name)) {
                anim.CrossFade(state.name);
            }
            i++;
        }


    }

}
