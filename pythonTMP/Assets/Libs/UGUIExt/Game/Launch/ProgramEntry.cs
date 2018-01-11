using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZhuYuU3d.Game
{

    public class ProgramEntry : MonoBehaviour
    {
        ApplicationFacade mAF = null;
	    // Use this for initialization
	    void Start ()
        {
            mAF = new ApplicationFacade();
            mAF.StartUp();
	    }
	
	}

}
