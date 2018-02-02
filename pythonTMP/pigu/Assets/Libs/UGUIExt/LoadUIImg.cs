using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public class LoadUIImg : DDOLSingleton<LoadUIImg>
{

    
	// Use this for initialization
	void Start ()
    {
     }

    public void LoadImgFromResource(string spath,Transform troot)
    {
        Texture2D texturegold = (Texture2D)Resources.Load(spath);

        Sprite sprgold = Sprite.Create(texturegold, new Rect(0, 0, texturegold.width, texturegold.height), Vector2.zero);

        if (sprgold!=null)
        {
            Image imgObj = troot.GetComponent<Image>();
            if(imgObj!=null)
            {
                imgObj.sprite = sprgold;
                imgObj.SetNativeSize();
            }
        }
        //imgObj.sprite = sprgold;

        //imgObj.SetNativeSize();
    }



}
