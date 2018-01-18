using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Libs{
    /*
    GameObject cacheGo = Libs.AM.I.CreateFromCache("Cube", delegate (string eventName, Object objInstantiateTp){
        GameObject objInstantiate = Instantiate(objInstantiateTp as GameObject);
        InitGo(objInstantiate);
        });
    //已经从 模板实例创建的 GameObject 实例
    InitGo(cacheGo);
    */
	[CSharpCallLua]
    public delegate void OnCreate(string eventName, Object gameObject);
	/// <summary>
	/// 创建器基类
	/// Asset creator base.
	/// </summary>
    public abstract class AssetCreatorBase
    {
        public string assetBundlePath;
        public string assetName;//prefab name
        public AssetBundle assetBundle;
        public OnCreate onCreate;
        //public OnLoadAssetBundle onLoadAssetBundle;

        virtual public void LoadAsset(){
           
        }

        virtual public void LoadAssetBundle(){
            
        }

        virtual public IEnumerator LoadAssetAsync(AssetBundle assetBundle, string assetName)
        {
            yield return null;
        }

        virtual public void OnLoadAssetBundle(string name,AssetBundle assetBundle){
            
        }

        virtual public bool isCmp()
        {
            return false;
        }

        virtual public float progress()
        {
            return 0;
        }

        virtual public UnityEngine.Object getData()
        {
            return null;
        }

        virtual public void CallBack(){
        
        }

        virtual public void Cache(){

        }

        virtual public void Clear(){
            assetBundlePath = null;
            assetName = null;//prefab name
            assetBundle = null;
            onCreate = null;
        }
    }

    public class AssetCreator : AssetCreatorBase
    {
        AssetBundleRequest assetBundleRequest;

        override public void LoadAsset(){
            if (assetBundle != null)
            {
                AssetManager.getInstance().StartCoroutine(LoadAssetAsync(assetBundle, assetName));
            }
            else if(assetName != null && !assetName.Equals(""))
            {
                assetBundlePath = AssetManager.getInstance().FindAbPathByAssetName(assetName);
                LoadAssetBundle();
            }
        }

        override public void LoadAssetBundle(){
            if (assetBundlePath != null && !assetBundlePath.Equals(""))
                AssetBundleManagar.getInstance().Load(assetBundlePath, OnLoadAssetBundle);
            else
                Debug.LogErrorFormat("LoadAssetBundle() 参数错误！{0}",this.ToString());
        }

        override public IEnumerator LoadAssetAsync(AssetBundle assetBundlep, string assetName)
        {
            assetBundle = assetBundlep;
            assetBundleRequest = assetBundle.LoadAssetAsync(assetName);
            yield return assetBundleRequest;
            //isDone = true;
        }

        override public void OnLoadAssetBundle(string abName,AssetBundle assetBundlep)
        {
            assetBundlePath = abName;
            assetBundle = assetBundlep;

            LoadAsset();
        }

        override public bool isCmp()
        {
            if (assetBundleRequest == null) return false;
            //return assetBundleRequest.isDone;
            return assetBundleRequest.progress == 1;
        }

        override public float progress()
        {
            if (assetBundleRequest == null) return 0;
            return assetBundleRequest.progress;
        }

        override public  UnityEngine.Object getData()
        {
            return assetBundleRequest.asset;
        }
            
        override public void Cache(){
            
        }

        override public void CallBack(){
            if (onCreate != null)
            {
                onCreate(assetName, getData());
            }
            assetBundle = null;
            assetBundleRequest = null;
        }
    }

    public class AssetCreatorSynchronization : AssetCreatorBase
    {
        UnityEngine.Object asset;

        override public void LoadAsset(){
            if (assetBundle != null)
            {
                asset = assetBundle.LoadAsset(assetName);
                //AssetManager.getInstance().StartCoroutine(LoadAssetAsync(assetBundle, assetName));
            }
            else if(assetName != null && !assetName.Equals(""))
            {
                assetBundlePath = AssetManager.getInstance().FindAbPathByAssetName(assetName);

                LoadAssetBundle();
            }
        }

        override public void LoadAssetBundle(){
            if (assetBundlePath != null && !assetBundlePath.Equals(""))
                AssetBundleManagar.getInstance().Load(assetBundlePath, OnLoadAssetBundle);
            else
				Debug.LogErrorFormat("LoadAssetBundle() 参数错误！{0}，{1}",this.ToString(),assetName);
        }
        /*
        void LoadAssetOne(AssetBundle assetBundlep, string assetName)
        {
            assetBundle = assetBundlep;
            //assetBundleRequest = assetBundle.LoadAssetAsync(assetName);
            //yield return assetBundleRequest;
            //isDone = true;
        }
        */
        override public void OnLoadAssetBundle(string abName,AssetBundle assetBundlep)
        {
            assetBundlePath = abName;
            assetBundle = assetBundlep;

            LoadAsset();
        }

        override public bool isCmp()
        {
            if(assetBundle == null)
                return false;
            return true;
        }

        override public float progress()
        {
            return 1;
        }

        override public  UnityEngine.Object getData()
        {
            return asset;
        }

        override public void Cache(){

        }

        override public void CallBack(){
            if (onCreate != null)
            {
                onCreate(assetName, getData());
                //Resources.UnloadAsset(asset);
            }
            assetBundle = null;
            asset = null;
        }
    }

    public class AssetManager : MonoBehaviour {

        private static AssetManager instance;
        public static AssetManager getInstance()
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("AssetManager");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<AssetManager>();
            }
            return instance;
        }
        public static AssetManager initForGameObject(GameObject dontDestroyOnLoadGameObject)
        {
            if (instance == null)
            {
                instance = dontDestroyOnLoadGameObject.AddComponent<AssetManager>();
            }
            return instance;
        }
        
        Dictionary<string,string> assetName2abPathDic = new Dictionary<string, string>();

        Dictionary<string,Object> assetPrefabDic = new Dictionary<string, Object>();
    
        Queue<AssetCreatorBase> assetLoading = new Queue<AssetCreatorBase>();

        Stack<HashSet<string>> loadStack = new Stack<HashSet<string>>(); 

        string[] curReleaseArr;

        Hashtable assetLoadingTable = new Hashtable();

        AssetCreatorBase curAssetCreator;

        public string curAssetCreatorClass = "Libs.AssetCreatorSynchronization";

        System.Reflection.Assembly objAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        /// <summary>
        /// 在打包时 AssetBundleManagar 生成 [manifestFileName]_AssetsName2AssetBundleAll.txt
            /// Inits the asset name2ab path dic. streamingAssetsFileName = "StreamingAssets_loadAb" 
            /// </summary>
            /// <param name="streamingAssetsFileName">Streaming assets file name.</param>
        public void InitAssetName2abPathDic(string manifestFileName)
        {
            Libs.ManifestFileTools.ReadAssetsName2AssetBundleInDic(manifestFileName+"_AssetsName2AssetBundleAll.txt" ,assetName2abPathDic);  
        }

        public string FindAbPathByAssetName(string assetName)
        {
            string abPath;
            assetName2abPathDic.TryGetValue(assetName,out abPath);
            return abPath;
        }

		public void ClearABDic()
		{
			if (assetName2abPathDic != null)
				assetName2abPathDic.Clear ();
			
		}

        /// <summary>
        /// Creates from cache.
        /// </summary>
        /// <returns>The from cache.</returns>
        /// <param name="assetName">Asset name. 不带后缀区分大大小写 或 Asset全路径 </param>
        /// <param name="onCreate">On create.</param>
		public void CreateFromCache(string assetName, OnCreate onCreate){

            Object gotp = null;
            assetPrefabDic.TryGetValue(assetName ,out gotp);
			if (gotp) {
				onCreate (assetName, gotp);
				return ;
			}  
            CreateAsync(assetName,onCreate);
        }

		public void CreateFromCacheByObj(string assetName,System.Object fun ){
			OnCreate onCreate = (OnCreate)fun;
			Object gotp = null;
			assetPrefabDic.TryGetValue(assetName ,out gotp);
			if (gotp) {
				onCreate (assetName, gotp);
				return ;
			}  
			CreateAsync(assetName,onCreate);
		}

        public void Release(string assetName){
            Object gotp = null;
            assetPrefabDic.TryGetValue(assetName ,out gotp);
            if (gotp){
                //1.删除Resources区 模板 在 缓存中的引用
                assetPrefabDic.Remove(assetName);
                //2.卸载Resources区
                if(!( gotp is GameObject))
                Resources.UnloadAsset(gotp);
                //Destroy(gotp);
                Debug.LogWarningFormat("AssetManager Release {0}" , assetName);
                //3.ab镜像区引用计数-1
                ABM.I.Release(FindAbPathByAssetName(assetName));
            }
        }

		public void ReleaseAllGameObject(){
			Object gotp = null;
			foreach(string assetName in assetPrefabDic.Keys){
				//Release (assetName);
				gotp = assetPrefabDic[assetName];
				//2.卸载Resources区
				if(!( gotp is GameObject))
					Resources.UnloadAsset(gotp);
				//Destroy(gotp);
				Debug.LogWarningFormat("AssetManager Release {0}" , assetName);
				//3.ab镜像区引用计数-1
				//ABM.I.Release(FindAbPathByAssetName(assetName));
				//AssetBundleManagar.getInstance().Release(assetName);
			}
			assetPrefabDic.Clear ();
		}

        public void Pop(){
            if (loadStack.Count > 0)
            {
                HashSet<string> curReleaseSet= loadStack.Pop();
                curReleaseArr = new string[curReleaseSet.Count];

                curReleaseSet.CopyTo(curReleaseArr);
            }
        }

        public void Push(){
            HashSet<string> gameObjectArr = new HashSet<string>();
            loadStack.Push(gameObjectArr);
        }

        public bool ReleaseStack(){
            
            if (curReleaseArr != null && curReleaseArr.Length > 0)
            {
                int release = -1;
                string curAssetName;

                for(int i = curReleaseArr.Length - 1; i >= 0 ;i--){

                    curAssetName = curReleaseArr[i];

                    if (curAssetName != null)
                    {
                        Release(curAssetName);

                        curReleaseArr[i] = null;
                        release = i;
                        break;
                    }
                }
                if (release == 0)
                {
                    curReleaseArr = null;

                    return true; 
                }
            }

            return false; 
        }
		/// <summary>
		/// 缓存一个模板对象
		/// Caches the game object.
		/// </summary>
		/// <param name="assetName">Asset name.</param>
		/// <param name="gotp">Gotp.</param>
        void CacheGameObject(string assetName,Object gotp){

            Object cacheGotp = null;
            assetPrefabDic.TryGetValue(assetName ,out cacheGotp);
            if (!cacheGotp)
            {
                if (assetPrefabDic.ContainsKey(assetName))
                    assetPrefabDic.Remove(assetName);
                
                assetPrefabDic.Add(assetName ,gotp );

                if (loadStack.Count > 0)
                    loadStack.Peek().Add(assetName);
            }
            /*
            if (!assetPrefabDic.ContainsKey(assetName))
            {
                assetPrefabDic.Add(assetName ,gotp as GameObject);
                if (loadStack.Count > 0)
                    loadStack.Peek().Add(assetName);
            }
            */
        }
		/// <summary>
		/// 异步创建 
		/// </summary>
		/// <param name="assetName">Asset name.</param>
		/// <param name="onCreate">On create.</param>
        public void CreateAsync(string assetName, OnCreate onCreate)
        {
            //当前创建器为空
            if (curAssetCreator == null)
            {
                curAssetCreator = objAssembly.CreateInstance(curAssetCreatorClass) as AssetCreatorBase;
                curAssetCreator.assetName = assetName;
                curAssetCreator.onCreate = onCreate;
                curAssetCreator.LoadAsset();
                //StartCoroutine(curAssetCreator.LoadAssetAsync(assetBundle, curAssetCreator.assetName));
            } 
            //同名资源追加 回调方法， 当前创建器 assetName 
            else if(curAssetCreator.assetName.Equals(assetName))
            {
                curAssetCreator.onCreate += onCreate; 
            }
            //检查当前创建器等待队列
            else if(assetLoadingTable.ContainsKey(assetName))
            {
                (assetLoadingTable[assetName] as AssetCreatorBase).onCreate += onCreate; 
            }
            else
            {
				//新创建加载器
                //AssetCreatorBase assetCreator = new AssetCreator();
                AssetCreatorBase assetCreator =objAssembly.CreateInstance(curAssetCreatorClass) as AssetCreatorBase;
                assetCreator.assetName = assetName;
                assetCreator.assetBundle = null;
                assetCreator.onCreate = onCreate;
                //加入等待队列
                assetLoading.Enqueue(assetCreator);
                //加入等待队列映射
                assetLoadingTable.Add(assetCreator.assetName,assetCreator);
            }
        }
		/// <summary>
		/// 加载完成后回调 注册委托
		/// </summary>
		/// <param name="assetBundle">Asset bundle.</param>
		/// <param name="assetName">Asset name.</param>
		/// <param name="onCreate">On create.</param>
        void Create(AssetBundle assetBundle, string assetName, OnCreate onCreate)
        {
            Object obj = assetBundle.LoadAsset(assetName) as Object;

            onCreate(assetName, obj);
        }
        /// <summary>
        /// Creates the async. 
        /// 此方法不支持 AssetBundle 引用计数
        /// </summary>
        /// <param name="assetBundle">Asset bundle.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="onCreate">On create.</param>
        public void CreateAsync(AssetBundle assetBundle, string assetName, OnCreate onCreate) {

            Debug.LogFormat("加载 assetName = {0}", assetName);

            if (curAssetCreator == null)
            {
                curAssetCreator = new AssetCreator();
                curAssetCreator.assetName = assetName;
                curAssetCreator.onCreate = onCreate;
                StartCoroutine(curAssetCreator.LoadAssetAsync(assetBundle, curAssetCreator.assetName));
            } else {
                AssetCreator assetCreator = new AssetCreator();
                assetCreator.assetName = assetName;
                assetCreator.assetBundle = assetBundle;
                assetCreator.onCreate = onCreate;
                //加入等待队列
                assetLoading.Enqueue(assetCreator);
                //加入等待队列映射
                assetLoadingTable.Add(assetCreator.assetName,assetCreator);
            }
        }

        bool IsBusy(){
            return curAssetCreator != null;
        } 

        bool IsFree(){
            return curAssetCreator == null;
        }

        void Free(){
            curAssetCreator.Clear();
            curAssetCreator = null;
        }

        void Update()
        {
            //卸载中创建器不工作
            if (ReleaseStack())
            {
                return;
            }

            if (IsFree())
            {
                //检测队列
                if (assetLoading.Count > 0)
                {
                    //弹出排队目标
                    curAssetCreator = assetLoading.Dequeue();
                    //加入等待队列映射
                    assetLoadingTable.Remove(curAssetCreator.assetName);
                    //启动加载
                    //StartCoroutine(curAssetCreator.LoadAssetAsync(curAssetCreator.assetBundle, curAssetCreator.assetName));
                    curAssetCreator.LoadAsset();
                }
            }
            else {
                if (curAssetCreator.isCmp()) {
                    
                    //AssetManager.ResetShader(curAssetCreator.getData());
                    //缓存模板实例
                    CacheGameObject(curAssetCreator.assetName, curAssetCreator.getData());
                    //返回 assts 资源回调
                    curAssetCreator.CallBack();
                    //释放当前加载状态
                    Free();
                } else {
                    //正在加载中！
                    //Debug.Log("curAssetBundleLoader = " + curAssetCreator.progress());
                }
            }
        }//end Update()
            
		void OnDestroy(){

			ReleaseAllGameObject ();

			//Dictionary<string,string> assetName2abPathDic = new Dictionary<string, string>();
			assetName2abPathDic.Clear();
			assetName2abPathDic = null;

			//Dictionary<string,Object> assetPrefabDic = new Dictionary<string, Object>();
			assetPrefabDic.Clear();
			assetPrefabDic = null;

			//Queue<AssetCreatorBase> assetLoading = new Queue<AssetCreatorBase>();
			assetLoading.Clear ();
			assetLoading = null;

			//Stack<HashSet<string>> loadStack = new Stack<HashSet<string>>(); 
			loadStack.Clear();
			loadStack = null;

			//string[] curReleaseArr;
			curReleaseArr = null;

			//Hashtable assetLoadingTable = new Hashtable();
			assetLoadingTable = null;

			instance = null;

			Debug.LogWarning ("AssetManager OnDestroy");
		}
		/// <summary>
		/// Destroy this instance.销毁
		/// </summary>
		public static void Destroy(){

			if (instance != null) {
				DestroyObject (instance);
			}
		}

        static List<Material> listMat = new List<Material>();
        public static void ResetShader(UnityEngine.Object obj)
        {
    #if UNITY_EDITOR

            listMat.Clear();
            if (obj is Material)
            {
                Material m = obj as Material;
                listMat.Add(m);
            }

            else if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                Renderer[] rends = go.GetComponentsInChildren<Renderer>();
                if (null != rends)
                {
                    foreach (Renderer item in rends)
                    {
                        Material[] materialsArr = item.sharedMaterials;
                        foreach (Material m in materialsArr)
                            listMat.Add(m);
                    }
                }
            }
            for (int i = 0; i < listMat.Count; i++)
            {
                Material m = listMat[i];
                if (null == m)
                    continue;
                var shaderName = m.shader.name;
                var newShader = Shader.Find(shaderName);
                if (newShader != null)
                    m.shader = newShader;
            }
            listMat.Clear();
    #endif
        }
    }

    /// <summary>
    /// 简化调用接口
    /// </summary>
    public class AM : AssetManager{

        public static AssetManager I{
            get{
                return AssetManager.getInstance();
            }
        }
    }

}