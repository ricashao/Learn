using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;  
	GUIStyle style = new GUIStyle();
	Rect rect ;
	float msec ;
	float fps;

    public Light[] lights;

	void Start(){
		int w = Screen.width, h = Screen.height;
		rect = new Rect(0, 0, w, h * 4 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 4 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
	
	}

	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}
	
	void OnGUI()
	{
		msec = deltaTime * 1000.0f;
		fps = 1.0f / deltaTime;

		GUI.Label(rect, string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps), style);

        for(int i=0; lights != null && i<lights.Length;i++ ){
            if(GUI.Button(new Rect(0,48 * (1+i),120,48),lights[i].name +"_"+ lights[i].enabled)){
                lights[i].enabled = !lights[i].enabled;
            }
            /*
            if(GUI.Button(new Rect(120,48 * ++i,120,48),light.name +"_"+ lights[i])){
                lights[i].enabled = !lights[i].enabled;
            }*/

        }
	}
}