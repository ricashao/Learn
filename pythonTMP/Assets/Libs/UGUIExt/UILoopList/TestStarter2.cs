using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class TestStarter2 : MonoBehaviour {

    private LuaEnv luaEnv;
    private void Awake()
    {
        luaEnv = new LuaEnv();
    }
    // Use this for initialization
    void Start () {
        luaEnv.DoString(@"
            local looplist = CS.UnityEngine.GameObject.Find('Canvas/Scroll View/Viewport/Content'):GetComponent('UILoopList');
            looplist.SetData = function(data,go)
                local label = go:GetComponentInChildren(typeof(CS.UnityEngine.UI.Text))
                local image = go:GetComponent('Image')
                image.sprite = CS.UnityEngine.Resources.Load(data[2],typeof(CS.UnityEngine.Sprite))
                label.text = data[1]
            end
            looplist:Data({{1,'1'},{2,'2'},{3,'3'},{4,'4'},{5,'5'},{6,'6'},{7,'7'},{8,'8'}})
            print(string.char(97))
        ");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
