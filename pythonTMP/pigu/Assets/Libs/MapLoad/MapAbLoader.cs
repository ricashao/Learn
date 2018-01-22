using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapAbLoader : MonoBehaviour {
    AssetBundle assetBundle;
    //AssetBundleRequest assetBundleRequest;
    private int nowProcess;//当前加载进度  
    private AsyncOperation async;

    private string[] scenePaths;

    public string mapName = "zhucheng02.map";
    
    void Start()
    {
        //WWW download = new WWW(AppContentPath() + "test_load_scene_bymainload.map");
        //WWW download = new WWW(AppContentPath() + "heizao_02.map");
        //WWW download = new WWW(AppContentPath() + "testloadshader.ab");

        //StartCoroutine(LoadGameSceneAsync("heizao_02.map"));  
        StartCoroutine(LoadGameSceneAsync(mapName,true));
    }

    //加载 Scene 
    IEnumerator LoadGameSceneAsync(string assetBundleName, bool isAdditive = false)
    {

        WWW download = new WWW(AppContentPath() + assetBundleName);

        yield return download;

        //AssetBundle assetBundle = download.assetBundle;
        assetBundle = download.assetBundle;
        //AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync("");
        scenePaths = assetBundle.GetAllScenePaths();

        async = SceneManager.LoadSceneAsync(scenePaths[0], isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        async.allowSceneActivation = false;
        yield return async;
    }

    public static string AppContentPath()
    {
        string path = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
            case RuntimePlatform.IPhonePlayer:
                path = Application.dataPath + "/Raw/";
                break;
            default:
                path = Application.dataPath + "/StreamingAssets/";
                break;
        }
        return path;
    }



    // Update is called once per frame
    void Update()
    {
        if (async == null)
        {
            return;
        }

        Debug.Log("progress => " + async.progress);

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
        }

        if (async.progress == 1)
        {
            async = null;
            //设置触发起 测试
            //PlayerControllerRay.SetIsTrigger();
            //设置Road事件响应
            //RoadEventHandler.AddSceneAllLayerRoad();

            //EventManager.getInstance().Send("MapLoadCmp");
        }
    }
}
