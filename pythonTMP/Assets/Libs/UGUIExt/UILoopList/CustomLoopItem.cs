using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLoopItem : UILoopItem {

	// Use this for initialization
	void Start ()
    {
		
	}

    public override void Data(object data)
    {
        base.Data(data);
        Image im = GetComponent<Image>();
        if(im != null)
        {
            im.sprite = Resources.Load("1", typeof(Sprite)) as Sprite;
        }
        SetData(data,this.gameObject);
        
    }
    


}
