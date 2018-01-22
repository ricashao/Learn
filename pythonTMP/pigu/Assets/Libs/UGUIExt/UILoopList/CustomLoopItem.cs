using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;
using ZhuYuU3d;

public class CustomLoopItem : UILoopItem {

    public override void Data(object data)
    {
        base.Data(data);

        Text t = GetComponentInChildren<Text>();
        if (t != null)
        {
            t.text = data.ToString();
        }
    }

}
