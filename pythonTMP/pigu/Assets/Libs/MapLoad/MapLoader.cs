using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Libs;

public class MapLoader : MonoBehaviour {

    public InputField inputField;
    public string abPath;

    private int nowProcess;//当前加载进度  
    private AsyncOperation async;

    public string[] scenePaths;

	// Use this for initialization
	void Start () {
        if (inputField == null)
        {
            inputField = GetComponentInChildren<InputField>();
        }
        if (abPath == null || abPath.Equals(""))
        {

        }
        else
        {
            Libs.ABM.I.LoadOne(abPath,OnAbCmp);
        }
	}
	
    public void LoadMap(){
        string text = inputField.text;

        if (text == null && text.Equals(""))
            return;

        Libs.ABM.I.LoadOne(text,OnAbCmp);
    }

    void OnAbCmp(string name,AssetBundle assetBundle){
        StartCoroutine(LoadGameSceneAsync(assetBundle,true));
    }

    IEnumerator LoadGameSceneAsync(AssetBundle assetBundle, bool isAdditive = false){
        scenePaths = assetBundle.GetAllScenePaths();
        async = SceneManager.LoadSceneAsync(scenePaths[0], isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        async.allowSceneActivation = false;
        yield return async;
    }

    void Update()
    {
        if (async == null)
        {
            return;
        }

        //Debug.Log("progress => " + async.progress);

        int toProcess;
        // async.progress 你正在读取的场景的进度值  0---0.9      
        // 如果当前的进度小于0.9，说明它还没有加载完成，就说明进度条还需要移动      
        // 如果，场景的数据加载完毕，async.progress 的值就会等于0.9    
        if (async.progress < 0.9f)
        {
            toProcess = (int)async.progress * 100;
        }
        else
        {
            toProcess = 100;
        }
        // 如果滑动条的当前进度，小于，当前加载场景的方法返回的进度     
        if (nowProcess < toProcess)
        {
            nowProcess++;
        }

        //progressSlider.value = nowProcess / 100f;
        //设置progressText进度显示  
        //ProgressSliderText.text = progressSlider.value * 100 + "%";
        //设置为true的时候，如果场景数据加载完毕，就可以自动跳转场景     
        if (nowProcess == 100)
        {
            async.allowSceneActivation = true;
            async = null;

            //GameObject init = GameObject.Find("Chushengdian");
            //EM.I.Send("MapLoadCmp",init);

            EM.I.Send("MapLoadCmp");
        }
        /*
        if (async.progress == 1)
        {
            async = null;
        }
        */
    }
}
