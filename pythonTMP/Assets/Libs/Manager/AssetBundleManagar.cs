using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libs{
/*
 * 	// Use this for initialization
	void Start () {
		Libs.ABM.I.Load ("cube",OnABCmp);
	}

	void OnABCmp(string name,AssetBundle ab){
		string [] ns = ab.GetAllAssetNames ();
		GameObject go = ab.LoadAsset<GameObject>(ns[0]);
		Instantiate (go);
	}

    private void Load()
    {
        //path = "mage_female_02.role";
        AssetBundleManagar.getInstance().Load("mage_female_02.role", OnLoadAssetBundle);
    }

    void OnLoadAssetBundle(string eventName, AssetBundle assetBundle)
    {
        //perfab "mage_female_02"
        AssetManager.getInstance().CreateAsync(assetBundle, "mage_female_02", OnCreate);
    }
   
    public void OnCreate(string eventName, Object go)
    {
        GameObject obj = go as GameObject;
        GameObject objInstantiate = Instantiate(obj);
    }
*/
    //AssetBundle 加载完成回调
    public delegate void OnLoadAssetBundle(string path, AssetBundle assetBundle);
    /// <summary>
    /// AssetBundle加载器抽象！
    /// </summary>
    public abstract class AssetBundleLoader{

        public string name;
        public AssetBundle assetBundle;
        uint _referenceount = 0;

        public uint referenceount {
            get{
                return _referenceount;
            }
        }

        public OnLoadAssetBundle onLoadAssetBundle;

        public uint Retain(){
            return ++_referenceount;
        }

        public uint Release(){
            return --_referenceount;
        }

        virtual public bool IsCmp()
        {
            return false;
        }

        virtual public float Progress()
        {
            return 0;
        }

		public void AddCallBack(OnLoadAssetBundle onLoadAssetBundlep){
			if (onLoadAssetBundlep == null) {
				return;
			}
			if (onLoadAssetBundle == null) {
				onLoadAssetBundle = onLoadAssetBundlep;
			}
			else {
				System.Delegate[] invocationList = onLoadAssetBundle.GetInvocationList();

				for (int i = invocationList.Length - 1; i >= 0; i--)
				{
                    System.Delegate curDelegate = invocationList[i];
                    //同一个ab 同一个对象 同一个方法 只能监听一次
                    if (curDelegate.Target == onLoadAssetBundlep.Target && curDelegate.Method == onLoadAssetBundlep.Method )
					{
						Debug.LogWarningFormat ("Target {0}, Method{1},is already in AssetBundleLoader {2}",onLoadAssetBundlep.Target ,onLoadAssetBundlep.Method , name);
						return;
					}
				}
				onLoadAssetBundle += onLoadAssetBundlep;
			}
		}

        virtual public void CallBack(){
			if (onLoadAssetBundle != null) {
				onLoadAssetBundle (name,assetBundle);
                onLoadAssetBundle = null;
                assetBundle = null;
                _referenceount = 0;
			}
		}
  
        virtual public IEnumerator LoadAsync(string assetBundleName) {
            yield return null;
        }
    }
    /// <summary>
    /// www 加载方式
    /// </summary>
    public class AssetBundleWWWLoader : AssetBundleLoader
    {
        public WWW download;
        bool isDone;
        override public IEnumerator LoadAsync(string assetBundleName)
        {
            //download = new WWW(AppContentPath() + assetBundleName );
			download = new WWW(PathTools.GetAssetPath(assetBundleName));
            yield return download;

            assetBundle = download.assetBundle;

            isDone = true;
        }

        override public bool IsCmp()
        {
            if (download == null) return false;
            //return download.isDone;
            return isDone;
        }

        override public float Progress() {
            if (download == null) return 0;
            return download.progress;
        }

        override public void CallBack(){
            base.CallBack();
            if (download != null)
            {
                download.Dispose();
                download = null;
            }
        }
    }
    /// <summary>
    /// LoadFromFileAsync 加载方式
    /// </summary>
    public class AssetBundleLoadFromFileLoader : AssetBundleLoader
    {
        AssetBundleCreateRequest r;
        override public IEnumerator LoadAsync(string assetBundleName)
        {
            r = AssetBundle.LoadFromFileAsync(PathTools.GetAssetPathForLoadPath(assetBundleName));
            yield return r;

            assetBundle = r.assetBundle;
        }

        override public bool IsCmp()
        {
            if (r == null) return false;
            return r.isDone;
        }

        override public float Progress()
        {
            if (r == null) return 0;
            return r.progress;
        }
    }
    /// <summary>
    /// Asset bundle create from memory. 解密方式
    /// </summary>
    public class AssetBundleCreateFromMemory: AssetBundleLoader{
        
        WWW download;
        bool isDone;

        override public IEnumerator LoadAsync(string assetBundleName){
            
            download = new WWW(PathTools.GetAssetPath(assetBundleName));
            yield return download;

            byte[] encrypedData = download.bytes;
            byte[] decryptedData = DecryptionMethod(encrypedData);//解密函数
            AssetBundleCreateRequest abcr = AssetBundle.LoadFromMemoryAsync(decryptedData);

            yield return abcr;

            assetBundle = abcr.assetBundle;

            isDone = true;
        }

        byte[] DecryptionMethod(byte[] encrypedData ){
            //解密算法
            byte[] decryptedData = encrypedData;
            return decryptedData;
        }

        override public bool IsCmp()
        {
            if (download == null) return false;
            //return download.isDone;
            return isDone;
        }

        override public float Progress() {
            if (download == null) return 0;
            return download.progress;
        }

        override public void CallBack(){
            base.CallBack();
            if (download != null)
            {
                download.Dispose();
                download = null;
            }
        }
    }
    /// <summary>
    /// AssetBundle 管理器 
    /// 封装AssetBundle 加载过程
    /// 提供加载队列
    /// 提供存取接口
    /// </summary>
    public class AssetBundleManagar : MonoBehaviour {

        private static AssetBundleManagar instance;
        public static AssetBundleManagar getInstance()
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("AssetBundleManagar");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<AssetBundleManagar>();
            }
            return instance;
        }
        public static AssetBundleManagar initForGameObject(GameObject dontDestroyOnLoadGameObject)
        {
            if (instance == null)
            {
                instance = dontDestroyOnLoadGameObject.AddComponent<AssetBundleManagar>();
            }
            return instance;
        }

		public static bool debugLog = true;

        /// <summary>
        /// AssetBundle 实例 管理 引用次数
        /// </summary>
        class AssetBundleInstance{
			public string name;
            public AssetBundle assetBundle;
            uint referenceCount ;

            public uint Retain(){
                return ++referenceCount;
            }

            public uint Release(){
                return --referenceCount;
            }

            public void SetReferenceCount(uint count){
                referenceCount = count;
            }

            public uint GetReferenceCount(){
                return referenceCount;
            }

            public void Unload(){
                assetBundle.Unload(true);
                assetBundle = null;
            }
        }

        Dictionary<string, AssetBundleInstance> cache = new Dictionary<string, AssetBundleInstance>();

        Queue<AssetBundleLoader> waiting = new Queue<AssetBundleLoader>();

        Dictionary<string,AssetBundleManifest> assetBundleManifestDic = new Dictionary<string,AssetBundleManifest>();

        AssetBundleManifest[] assetBundleManifestArr;

        AssetBundleLoader curAssetBundleLoader;
           
        AssetBundleManifest manifest;

        bool isStop;    

        void Awake(){
            //加载主 manifest
            LoadAssetBundleManifest();
        }

        public void Clear()
        {
            /*
            waiting.Clear();
            cache.Clear();
            */
        }
       
        public void Stop(){
            isStop = true;
        }

        public void Run(){
            isStop = false;
        }
        // 加载主 Manifest
        public void LoadAssetBundleManifest()
        {
            if (manifest != null)
                return;
            
			AssetBundle bundle;

			#if UNITY_5
				bundle = AssetBundle.LoadFromFile (PathTools.GetAssetPathForLoadPath("StreamingAssets"));
				//bundle = AssetBundle.LoadFromFile (Application.streamingAssetsPath+"/StreamingAssets");
			#else 
				string manifestPath = PathTools.GetAssetPath("StreamingAssets");
				WWW www = new WWW(manifestPath);
				while (!www.isDone) {}
			 	bundle = www.assetBundle;
			#endif

            manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //string[] dependencies = manifest.GetAllDependencies("cube.pb");
            //Debug.LogFormat("assetBundle {0} dependencies {1}","cube.pb",dependencies.Length);
            // 压缩包释放掉
            bundle.Unload(false);
            bundle = null;

        }
        /// <summary>
        /// Loads the asset bundle manifest add.
		/// 增加依赖列表文件 美术工程的 StreamingAssets
        /// </summary>
        /// <param name="streamingAssetsFileName">Streaming assets file name.</param>
        public void LoadAssetBundleManifestAdd(string streamingAssetsFileName)
        {
            if (assetBundleManifestDic.ContainsKey(streamingAssetsFileName))
            {
                Debug.LogFormat("manifest {0} 已缓存...",streamingAssetsFileName);
                return;
            }

			AssetBundle bundle;
			#if UNITY_5
			bundle = AssetBundle.LoadFromFile (PathTools.GetAssetPathForLoadPath(streamingAssetsFileName));
			//bundle = AssetBundle.LoadFromFile (Application.streamingAssetsPath+"/StreamingAssets");
			#else 
			string manifestPath = PathTools.GetAssetPath("StreamingAssets");
			WWW www = new WWW(manifestPath);
			while (!www.isDone) {}
			bundle = www.assetBundle;
			#endif
            AssetBundleManifest manifestAdd = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            assetBundleManifestDic.Add(streamingAssetsFileName,manifestAdd);
            assetBundleManifestArr = new AssetBundleManifest[assetBundleManifestDic.Values.Count];
            assetBundleManifestDic.Values.CopyTo(assetBundleManifestArr,0);

            string[] dependencies = manifestAdd.GetAllDependencies("cube");

            Debug.LogFormat("assetBundle {0} dependencies {1}","cube",dependencies.Length);
            
            // 压缩包释放掉
            bundle.Unload(false);
            bundle = null;
        }
        /// <summary>
        /// Loads the and dependencies.
        /// 加载 AssetBundle 和其依赖的 AssetBundle
        /// </summary>
        /// <param name="abName">Ab name.</param>
        /// <param name="onLoadAssetBundle">On load asset bundle.</param>
        protected void LoadAndDependencies(string abName,OnLoadAssetBundle onLoadAssetBundle){
			
			int dependenciesCount = 0;

			if (manifest != null) {
				string[] dependencies = manifest.GetAllDependencies(abName);
				dependenciesCount = dependencies.Length;
				//for(int i = 0;i < dependenciesCount;i++){
				for(int i = dependenciesCount -1;i >= 0 ;i--){
					LoadOne(dependencies[i],null);
				}
			}

			if (dependenciesCount == 0) {
				for (int i = 0; assetBundleManifestArr != null && i < assetBundleManifestArr.Length; i++) {
					string[] dependencies = assetBundleManifestArr[i].GetAllDependencies(abName);
					//for(int j = 0;j < dependenciesCount;j++){
					for(int j = dependenciesCount -1;j >= 0 ;j--){
						LoadOne(dependencies[j],null);
					}
					if (dependencies.Length > 0) {
						LoadOne(abName,onLoadAssetBundle);
						return;
					}
					/*
					dependenciesCount = LoadDependencies (abName, assetBundleManifestArr [i], onLoadAssetBundle);
					//在附加库中查找依赖
					if (dependenciesCount > 0) {
						break;
					}*/
				}

			}
			if (manifest != null) {
				//dependenciesCount = LoadDependencies (abName, manifest,onLoadAssetBundle);
				LoadOne(abName,onLoadAssetBundle);
			}
            //LoadOne(abName, onLoadAssetBundle);
        }
        /// <summary>
        /// 加载 AssetBundle 和其依赖的 AssetBundle 外部调用接口
        /// </summary>
        /// <param name="abName">Ab name.</param>
        /// <param name="onLoadAssetBundle">On load asset bundle.</param>
        public void Load(string abName,OnLoadAssetBundle onLoadAssetBundle){
            LoadAndDependencies(abName,onLoadAssetBundle);
            //LoadOne(abName,onLoadAssetBundle);
        }
        /// <summary>
        /// Loads the dependencies.
        /// 加载和子依赖 
        /// 引用计数加 1 
        /// </summary>
        /// <param name="abName">Ab name.</param>
		protected int LoadDependencies(string abName,AssetBundleManifest assetBundleManifest,OnLoadAssetBundle onLoadAssetBundle = null){
            
            string[] dependencies = assetBundleManifest.GetAllDependencies(abName);
            //加载子依赖

            for(int i = 0 ;i < dependencies.Length; i++){
				int dependencieLength = LoadDependencies(dependencies[i],assetBundleManifest);
				Debug.LogWarningFormat ("assetBundle {0} ,dependencie {1} ,dependencieLength {2} ",abName,dependencies[i],dependencieLength);
            }
            
            //1. 检测 AssetBundle 缓存
            AssetBundleInstance assetBundleInstance;
            cache.TryGetValue(abName,out assetBundleInstance);
            if (assetBundleInstance != null) {
                //当前 引用计数加 1
                assetBundleInstance.Retain();
                Debug.LogFormat("{0} 已缓存...",abName);
				if(onLoadAssetBundle != null){
					onLoadAssetBundle (assetBundleInstance.name ,assetBundleInstance.assetBundle);
				}
                return dependencies.Length;
            }
            //2. 检查 是否在等待队列里
            AssetBundleLoader[] waitings = waiting.ToArray();
            AssetBundleLoader curWaitingAssetBundleLoader;
            for(int i = 0;i < waitings.Length;i++){
                curWaitingAssetBundleLoader = waitings[i];
                if (curWaitingAssetBundleLoader.name.Equals(abName))
                {
                    //当前 引用计数加 1
                    curWaitingAssetBundleLoader.Retain();
					curWaitingAssetBundleLoader.AddCallBack (onLoadAssetBundle);
                    Debug.LogFormat("{0} 正在加载等待中...",curWaitingAssetBundleLoader.name);
                    return dependencies.Length;
                }
            }
            //3. 检测 同名资源是否正在加载中   
            if (curAssetBundleLoader != null && curAssetBundleLoader.name.Equals(abName))
            {
                //当前 引用计数加 1
                curAssetBundleLoader.Retain();
				curAssetBundleLoader.AddCallBack (onLoadAssetBundle);
			
                Debug.LogFormat("{0} 正在加载中...",curAssetBundleLoader.name);
                return dependencies.Length;
            }
            //4. 创建加载器
            AssetBundleLoader assetBundleLoader = new AssetBundleWWWLoader();
            assetBundleLoader.name = abName;
			assetBundleLoader.onLoadAssetBundle = onLoadAssetBundle;
            assetBundleLoader.Retain();
            //5. 开始加载或加入等待队列
            StartOrWait(assetBundleLoader);

            return dependencies.Length;
        }
        /// <summary>
        /// 加载并缓存
        /// 引用计数加 1 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onLoadAssetBundle"></param>
        public void LoadOne(string abName, OnLoadAssetBundle onLoadAssetBundle)
        {
            //1.检测 AssetBundle 缓存
            AssetBundleInstance assetBundleInstance;
            cache.TryGetValue(abName,out assetBundleInstance);
            if (assetBundleInstance != null) {
                //当前 引用计数加 1
                assetBundleInstance.Retain();
                Debug.LogFormat("{0} 已缓存...",abName);

                if (onLoadAssetBundle != null)
                {
                    onLoadAssetBundle(abName,assetBundleInstance.assetBundle);
                }
                return;
            }
            //2. 检查 是否在等待队列里
            AssetBundleLoader[] waitings = waiting.ToArray();
            AssetBundleLoader curWaitingAssetBundleLoader;
            for(int i = 0;i < waitings.Length;i++){
                curWaitingAssetBundleLoader = waitings[i];
                if (curWaitingAssetBundleLoader.name.Equals(abName))
                {
                    //当前 引用计数加 1
                    curWaitingAssetBundleLoader.Retain();
					curWaitingAssetBundleLoader.AddCallBack (onLoadAssetBundle);
                    Debug.LogFormat("{0} 正在加载等待中...",curWaitingAssetBundleLoader.name);
                    return;
                }
            }
            //3. 同名资源是否正在加载中   
            if (curAssetBundleLoader != null && curAssetBundleLoader.name.Equals(abName))
            {
                //System.Delegate[] invocationList = curAssetBundleLoader.onLoadAssetBundle.GetInvocationList();
                //当前 引用计数加 1
                curAssetBundleLoader.Retain();
				curAssetBundleLoader.AddCallBack(onLoadAssetBundle);
                //System.Delegate[] invocationList1 = curAssetBundleLoader.onLoadAssetBundle.GetInvocationList();
                return;
            }
            //4. 创建加载器
            AssetBundleLoader assetBundleLoader = new AssetBundleWWWLoader();
            assetBundleLoader.name = abName;
            assetBundleLoader.onLoadAssetBundle = onLoadAssetBundle;
            assetBundleLoader.Retain();
			//5. 开始加载或加入等待队列
            StartOrWait(assetBundleLoader);
        }

        /// <summary>
        /// 释放一个 ab 
        /// </summary>
        /// <param name="name"></param>
        public int Release(string abName,AssetBundleManifest assetBundleManifest = null) {
            
            bool isManifest = (assetBundleManifest == null && manifest != null);
            int dependenciesCount = 0; 
            if (isManifest)
            {
                assetBundleManifest = manifest;
            }

            ReleaseOne(abName);

            string[] dependencies = assetBundleManifest.GetAllDependencies(abName);
            dependenciesCount = dependencies.Length;
            //manifest 子依赖
            if (dependencies.Length > 0)
            {
                //释放子依赖
                for (int i = 0; i < dependencies.Length; i++)
                {
                    if(debugLog)
                        Debug.LogWarningFormat("assetBundle {0} dependencies {1}", abName, dependencies[i]);
                    ReleaseOne(dependencies[i]);
                }
            } else if(!isManifest && assetBundleManifestArr != null){
                 //释放子依赖 在外部manifest列表中
                 for (int i = 0 ; i < assetBundleManifestArr.Length ; i++){
                     //在附加库中查找依赖
                     AssetBundleManifest curAssetBundleManifest = assetBundleManifestArr[i];
                     string[] curDependencies = curAssetBundleManifest.GetAllDependencies(abName);
                     dependenciesCount = curDependencies.Length;

                     if(dependenciesCount > 0){
                        for (int j = 0; j < dependenciesCount; j++)
                        {
                            ReleaseOne(abName);
                        }
                        break;
                     }
                }
            }
            /*
            AssetBundleInstance assetBundleInstance = null;
            cache.TryGetValue(abName,out assetBundleInstance);

            if (assetBundleInstance != null)
            {
                Debug.LogErrorFormat("AssetBundleManagar cache 中没有 {0} ", abName);
                return 0;
            }
            //引用计数减 1 
            if (assetBundleInstance.Release() == 0)
            {
                assetBundleInstance.assetBundle.Unload(true);
                cache.Remove(abName);

                string[] dependencies = assetBundleManifest.GetAllDependencies(abName);
                dependenciesCount = dependencies.Length;
                //manifest 子依赖
                if (dependencies.Length > 0)
                {
                    //释放子依赖
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        Debug.LogWarningFormat("assetBundle {0} dependencies {1}", abName, dependencies[i]);
                        Release(dependencies[i], assetBundleManifest);
                    }
                }else if(isManifest && assetBundleManifestArr != null){
                    //释放子依赖 在外部manifest列表中
                    for (int i = 0 ; i < assetBundleManifestArr.Length ; i++){
                        //在附加库中查找依赖
                        dependenciesCount = Release(abName,assetBundleManifestArr[i]);
                        if(dependenciesCount > 0){
                        break;
                        }
                    }
                }
            }
            */
            return dependenciesCount;
        }

        uint ReleaseOne(string abName){

            AssetBundleInstance assetBundleInstance = null;
            cache.TryGetValue(abName,out assetBundleInstance);

            if (assetBundleInstance == null)
            {
                Debug.LogErrorFormat("AssetBundleManagar cache 中没有 {0} ", abName);
                return 0;
            }
            //引用计数减 1 
            if (assetBundleInstance.Release() == 0)
            {
                assetBundleInstance.Unload();
                cache.Remove(abName);
                if (debugLog)
                    Debug.LogErrorFormat("释放 {0}", abName);
                return 0;
            }
            if (debugLog)
            {
                Debug.LogErrorFormat("引用减 1 {0} ReferenceCount {1}",abName,assetBundleInstance.GetReferenceCount());
            }

            return assetBundleInstance.GetReferenceCount();
        }

        /// <summary>
        /// 强制移除 不检查引用
        /// </summary>
        /// <param name="name"></param>
        public void Rem(string name){
            AssetBundleInstance assetBundleInstance = null;
            cache.TryGetValue(name,out assetBundleInstance);
            if (assetBundleInstance != null) {
                assetBundleInstance.assetBundle.Unload(true);
                cache.Remove(name);
            } 
        }
        /// <summary>
        /// Starts the or wait.
        /// </summary>
        void StartOrWait(AssetBundleLoader assetBundleLoader){
            //当前正在加载中
            if (IsBusy()){
                //加入等待队列
                waiting.Enqueue(assetBundleLoader);
            }
            else {
                //开始加载！
                curAssetBundleLoader = assetBundleLoader;
                StartCoroutine(curAssetBundleLoader.LoadAsync(assetBundleLoader.name));
            }
        }
        /// <summary>
        /// 加载器是否处于忙碌状态
        /// </summary>
        /// <returns><c>true</c> if this instance is busy; otherwise, <c>false</c>.</returns>
        bool IsBusy(){
            return curAssetBundleLoader != null;
        } 
        /// <summary>
        /// 是否有加载器正在运行
        /// </summary>
        /// <returns><c>true</c> if this instance is running; otherwise, <c>false</c>.</returns>
        bool IsRunning(){
            return IsBusy();
        }
        /// <summary>
        /// 执行下一个加载任务
        /// </summary>
        void Next() {
            //检测队列
            if (curAssetBundleLoader == null && waiting.Count > 0)
            {
                //弹出排队目标
                curAssetBundleLoader = waiting.Dequeue();
                //检测缓存
                AssetBundleInstance assetBundleInstance;
                cache.TryGetValue(curAssetBundleLoader.name, out assetBundleInstance);
                if (assetBundleInstance != null)
                {
                    curAssetBundleLoader.onLoadAssetBundle(curAssetBundleLoader.name, assetBundleInstance.assetBundle);
                    curAssetBundleLoader = null;
                    return;
                }
                else
                {
                    //启动加载
                    StartCoroutine(curAssetBundleLoader.LoadAsync(curAssetBundleLoader.name));
                }
            }
        }
        /// <summary>
        /// 加入缓存 Caches the new one.
        /// </summary>
        /// <param name="assetBundleLoader">Asset bundle loader.</param>
        void CacheNewOne(AssetBundleLoader assetBundleLoader){
            //检测缓存
            AssetBundleInstance assetBundleInstance;
            cache.TryGetValue(curAssetBundleLoader.name, out assetBundleInstance);
            if (assetBundleInstance == null)
            {
                assetBundleInstance = new AssetBundleInstance();
				assetBundleInstance.name = assetBundleLoader.name;
                assetBundleInstance.assetBundle = curAssetBundleLoader.assetBundle;
                assetBundleInstance.SetReferenceCount(curAssetBundleLoader.referenceount);
                //添加到缓存
                cache.Add(curAssetBundleLoader.name, assetBundleInstance);
            }
            else
            {
                Debug.LogErrorFormat("{0} 重复加载!",assetBundleInstance);
            }
        }
        /// <summary>
        /// 检查当前运行 curAssetBundleLoader 是否加载完成
        /// </summary>
        void Check(){
            if (curAssetBundleLoader.IsCmp())
            {
                if (curAssetBundleLoader.assetBundle == null) {
                    Debug.LogErrorFormat("AssetBundle {0} 加载失败", curAssetBundleLoader.name);
                    //重置流
                    curAssetBundleLoader = null;
                    return;
                }
                
                CacheNewOne(curAssetBundleLoader);

                //回调完成委托
				/*
                if (curAssetBundleLoader.onLoadAssetBundle != null)
                {
                    curAssetBundleLoader.onLoadAssetBundle(curAssetBundleLoader.name,
                                                           curAssetBundleLoader.assetBundle);
                }*/
				curAssetBundleLoader.CallBack();
                /* 调试委托调用
                System.Delegate[] invocationList = curAssetBundleLoader.onLoadAssetBundle.GetInvocationList();
                for (int i = 0; i < invocationList.Length; i++)
                {
                    invocationList[i].DynamicInvoke(
                        curAssetBundleLoader.name,
                        curAssetBundleLoader.assetBundle);
                }*/

				if (debugLog) {
					Debug.LogFormat (" AssetBundleLoader {0}, 加载完成!,队列总数 {1}",curAssetBundleLoader.name,waiting.Count);
				}

                //重置流
                curAssetBundleLoader = null;
            }
            else
            {
                //正在加载中！
                //Debug.Log("curAssetBundleLoader = " + curAssetBundleLoader.progress());
            }
        }
        // Update is called once per frame
        void FixedUpdate() {

            if (IsRunning())
            {
                Check();
            }
            else if(!isStop)
            {
                //加载下一个
                Next();
            }
        }

		void OnDestroy()
		{
			//Dictionary<string, AssetBundleInstance> cache = new Dictionary<string, AssetBundleInstance>();
			cache.Clear ();
			cache = null;
			//Queue<AssetBundleLoader> waiting = new Queue<AssetBundleLoader>();
			waiting.Clear ();
			waiting = null;
			//Dictionary<string,AssetBundleManifest> assetBundleManifestDic = new Dictionary<string,AssetBundleManifest>();
			assetBundleManifestDic.Clear ();
			assetBundleManifestDic = null;
			//AssetBundleManifest[] assetBundleManifestArr;
			assetBundleManifestArr = null;
			//AssetBundleLoader curAssetBundleLoader;
			curAssetBundleLoader = null;
			//AssetBundleManifest manifest;
			manifest = null;

		}

    }//class end

    /// <summary>
    /// 简化调用接口
    /// </summary>
    public class ABM:AssetBundleManagar{

        public static AssetBundleManagar I{
            get{
                return AssetBundleManagar.getInstance();
            }
        }
           
    }
    /// <summary>
    ///   Dictionary<string,string> abDic = new Dictionary<string, string>();
    ///   Libs.ManifestFileTools.ReadAssetsame2AssetBundleInDic("StreamingAssets_loadAb"+"_AssetsName2AssetBundleAll.txt" ,abDic);
    ///   string ab = abDic["Cube"];
    ///   Debug.Log(ab);
    /// </summary>
    public class ManifestFileTools{
        /// <summary>
        /// Reads the assetsame  asset bundle in dic.
        /// </summary>
        /// <param name="streamingAssetsFileName">Streaming assets file name.</param>
        /// <param name="abDic">Ab dic.</param>
        public static void ReadAssetsName2AssetBundleInDic(string streamingAssetsFileName, Dictionary<string,string> abDic ){
            
            string fileText = System.IO.File.ReadAllText(PathTools.GetAssetPathForLoadPath(streamingAssetsFileName)).Trim();
            string[] allLines = fileText.Split('\n');
            string curLine;
            int curIndex;
            for(int i = 0;i < allLines.Length; i++){
                curLine = allLines[i];
                curLine=curLine.Trim();
                curIndex = curLine.IndexOf("=");
                if (curIndex < 0)
                {
                    Debug.LogErrorFormat("ReadAssetsame2AssetBundleInDic Error Line {0}" , curLine);
                    continue;
                }
                else
                {
                    abDic.Add( curLine.Substring(0,curIndex) , curLine.Substring(curIndex + 1 ) );
                }
            }

        }

        public static void CreateAssetsName2AssetBundle(string streamingAssetsFileName){
            
            string filePath = PathTools.GetAssetPathForLoadPath(streamingAssetsFileName);
            AssetBundle bundle = AssetBundle.LoadFromFile(filePath);
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            string [] assetBundles = manifest.GetAllAssetBundles();

            string path = PathTools.GetAssetPathForLoadPath(streamingAssetsFileName+"_AssetsName2AssetBundleAll.txt" );
            System.IO.StreamWriter sw = System.IO.File.CreateText(path);
            foreach(string abName in assetBundles){
                if (abName.StartsWith("StreamingAssets")){
                    continue;
                }
                string[] namesArr = GetManifestAssetsNames(abName);
                foreach(string line in namesArr){
                    /*
                    if (line.Equals("Assets/Map/Z!M.O.B.A Environment Art") || line.StartsWith("Assets/Map/Z!M.O.B.A Environment Art"))
                    {
                        Debug.Log("");
                    }
                    */
                    sw.WriteLine(line);
                    sw.Flush();
                }
            }

            sw.Close();
        }

        public static string[] GetManifestAssetsNames(string assetBundleName){
            
            string path = PathTools.GetAssetPathForLoadPath(assetBundleName + ".manifest");
            string fileText = System.IO.File.ReadAllText(path);
            string allLines = fileText.Substring(fileText.LastIndexOf("Assets:\n- ") + "Assets:\n- ".Length ,
                                                 fileText.LastIndexOf("Dependencies:") + 1 - "Dependencies:".Length - fileText.LastIndexOf("Assets:") +1);
            string[] lineArr = allLines.Trim().Split('\n');
            string[] assetsNameLineArr = new string[lineArr.Length * 2];
            //string[] assetsPathLineArr = new string[lineArr.Length];

            string curLine;
            for(int i = 0; i < lineArr.Length; i++){
                curLine = lineArr[i];
                assetsNameLineArr[i * 2] = curLine.Substring(curLine.LastIndexOf("/") + 1 ,curLine.LastIndexOf(".") - curLine.LastIndexOf("/") -1 ) + "=" + assetBundleName;
                //assetsPathLineArr[i] = curLine.Replace("-").Trim();
                assetsNameLineArr[i * 2 + 1] = curLine.Replace("-","").Trim() + "=" + assetBundleName; 
            }
            /*
            path = PathTools.GetAssetPathForLoadPath(assetBundleName + "_Assets.txt");
            System.IO.StreamWriter sw = System.IO.File.CreateText(path);
            foreach(string line in assetsNameLineArr){
                sw.WriteLine(line);
                sw.Flush();
            }
            sw.Close();
            */
            return assetsNameLineArr;
        }
    }
}