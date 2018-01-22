//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using XLua;
//using System;

//namespace ZhuYuU3d
//{

    
//    #region define

//    public enum UIType
//    {
//        Normal,
//        Fixed,
//        PopUp,
//        None,      //独立的窗口
//    }

//    public enum UIMode
//    {
//        DoNothing,
//        HideOther,     // 闭其他界面
//        NeedBack,      // 点击返回按钮关闭当前,不关闭其他界面(需要调整好层级关系)
//        NoNeedBack,    // 关闭TopBar,关闭其他界面,不加入backSequence队列
//    }

	

//    public enum UICollider
//    {
//        None,      // 显示该界面不包含碰撞背景
//        Normal,    // 碰撞透明背景
//        WithBg,    // 碰撞非透明背景
//    }
//    #endregion

//    public class UIPage
//    {
//        public string name = string.Empty;

//        //this page's id
//        public int id = -1;

//        //this page's type
//        public UIType type = UIType.Normal;

//        //how to show this page.
//        public UIMode mode = UIMode.DoNothing;
			
//        //the background collider mode
//        public UICollider collider = UICollider.None;

//        //path to load ui
//        public string uiPath = string.Empty;

//        //this ui's gameobject
//        public GameObject gameObject;
//        public Transform transform;
//		// Use this for initialization
//		void Start () {
//			//Load ();
//		}

		
//        //record this ui load mode.async or sync.
//        public bool isAsyncUI = false;

//        //this page active flag
//        protected bool isActived = false;

//        #region virtual api

//        ///When Instance UI Ony Once.
//        public virtual void Awake(GameObject go) { }

//        ///Show UI Refresh Eachtime.
//        public virtual void Refresh() { }

//        ///Active this UI
//        public virtual void Active()
//        {
//            this.gameObject.SetActive(true);
//            isActived = true;
//        }

//        /// <summary>
//        /// Only Deactive UI wont clear Data.
//        /// </summary>
//        public virtual void Hide()
//        {
//            this.gameObject.SetActive(false);
//            isActived = false;
//        }

//        private UIPage() { }
//        public UIPage(UIType type, UIMode mod, UICollider col,string spth,string sName)
//        {
//            this.type = type;
//            this.mode = mod;
//            this.collider = col;
            
//            this.uiPath = spth;
//            this.name = sName;
//        }

//        /// <summary>
//        /// Sync Show UI Logic
//        /// </summary>
//        public void Show()
//        {
//            //1:instance UI
//            if (this.gameObject == null && string.IsNullOrEmpty(uiPath) == false)
//            {
               
//                GameObject go = GameObject.Instantiate(Resources.Load(uiPath)) as GameObject;

//                //protected.
//                if (go == null)
//                {
//                    Debug.LogError("[UI] Cant sync load your ui prefab.");
//                    return;
//                }

//                AnchorUIGameObject(go);

//                //after instance should awake init.
//                Awake(go);

//                //mark this ui sync ui
//                isAsyncUI = false;
//            }

//            //:animation or init when active.
//            Active();

//            //:refresh ui component.
//            Refresh();

//            //:popup this node to top if need back.
//            UIManager um=UIManager.GetInstance();
//            if(um!=null)
//               um.PopNode(this);
//        }


//        /// <summary>
//        /// Async Show UI Logic
//        /// </summary>
//        public void Show(System.Action callback)
//        {
//            CoroutineController.Instance.StartCoroutine(AsyncShow(callback));
//        }

//        IEnumerator AsyncShow(System.Action callback)
//        {
//            //1:Instance UI
//            //FIX:support this is manager multi gameObject,instance by your self.
//            if (this.gameObject == null && string.IsNullOrEmpty(uiPath) == false)
//            {
//                GameObject go = null;
//                bool _loading = true;
//                Libs.AM.I.CreateFromCache(uiPath, (string assetName, UnityEngine.Object objInstantiateTp) =>
//                {
//                    go = objInstantiateTp != null ? GameObject.Instantiate(objInstantiateTp) as GameObject : null;

//                    AnchorUIGameObject(go);

//                    Awake(go);
//                    isAsyncUI = true;
//                    _loading = false;

//                    //:animation active.
//                    Active();

//                    //:refresh ui component.
//                    Refresh();

//                    //:popup this node to top if need back.
//                    UIManager um = UIManager.GetInstance();
//                    if (um != null)
//                        um.PopNode(this);

//                    if (callback != null) callback();
//                });

//                float _t0 = Time.realtimeSinceStartup;
//                while (_loading)
//                {
//                    if (Time.realtimeSinceStartup - _t0 >= 10.0f)
//                    {
//                        Debug.LogError("[UI] WTF async load your ui prefab timeout!");
//                        yield break;
//                    }
//                    yield return null;
//                }
//            }
//            else
//            {
//                //:animation active.
//                Active();

//                //:refresh ui component.
//                Refresh();

//                //:popup this node to top if need back.
//                UIManager um = UIManager.GetInstance();
//                if (um != null)
//                    um.PopNode(this);


//                if (callback != null) callback();
//            }
//        }

//        internal bool CheckIfNeedBack()
//        {
//            if (type == UIType.Fixed || type == UIType.PopUp || type == UIType.None) return false;
//            else if (mode == UIMode.NoNeedBack || mode == UIMode.DoNothing) return false;
//            return true;
//        }

//        protected void AnchorUIGameObject(GameObject ui)
//        {
//            if (UIManager.GetInstance() == null || ui == null) return;

//            this.gameObject = ui;
//            this.transform = ui.transform;

//            //check if this is ugui or (ngui)?
//            Vector3 anchorPos = Vector3.zero;
//            Vector2 sizeDel = Vector2.zero;
//            Vector3 scale = Vector3.one;
//            if (ui.GetComponent<RectTransform>() != null)
//            {
//                anchorPos = ui.GetComponent<RectTransform>().anchoredPosition;
//                sizeDel = ui.GetComponent<RectTransform>().sizeDelta;
//                scale = ui.GetComponent<RectTransform>().localScale;
//            }
//            else
//            {
//                anchorPos = ui.transform.localPosition;
//                scale = ui.transform.localScale;
//            }

//            Debug.Log("anchorPos:" + anchorPos + "|sizeDel:" + sizeDel);

//            if (type == UIType.Fixed)
//            {
//                ui.transform.SetParent(UIManager.GetInstance()._FixedRoot);
//            }
//            else if (type == UIType.Normal)
//            {
//                ui.transform.SetParent(UIManager.GetInstance()._NormalRoot);
//            }
//            else if (type == UIType.PopUp)
//            {
//                ui.transform.SetParent(UIManager.GetInstance()._PopupRoot);
//            }


//            if (ui.GetComponent<RectTransform>() != null)
//            {
//                ui.GetComponent<RectTransform>().anchoredPosition = anchorPos;
//                ui.GetComponent<RectTransform>().sizeDelta = sizeDel;
//                ui.GetComponent<RectTransform>().localScale = scale;
//            }
//            else
//            {
//                ui.transform.localPosition = anchorPos;
//                ui.transform.localScale = scale;
//            }
//        }

//        public override string ToString()
//        {
//            return ">Name:" + name + ",ID:" + id + ",Type:" + type.ToString() + ",ShowMode:" + mode.ToString() + ",Collider:" + collider.ToString();
//        }

//        public bool isActive()
//        {
//            //fix,if this page is not only one gameObject
//            //so,should check isActived too.
//            bool ret = gameObject != null && gameObject.activeSelf;
//            return ret || isActived;
//        }

//        #endregion

//    }

//    public delegate void OnGameCmp(string assetName);

//    public class UIManager : MonoBehaviour
//    {

//        static UIManager instance;

//        static public UIManager GetInstance()
//        {

//            if (instance)
//            {
//                return instance;
//            }
//            InitRoot();
//            //GameObject gameObject = new GameObject(typeof(UIManager).Name);
//            //GameObject.DontDestroyOnLoad(gameObject);

//            //instance = gameObject.AddComponent<UIManager>();
//            return instance;
//        }

//        Dictionary<string, OnGameCmp> luaCallBackDic = new Dictionary<string, OnGameCmp>();

//        OnGameCmp onGameCmp;

//        LuaEnv env;

//        void Awake()
//        {
//            if (instance == null)
//                instance = this;
//            env = LuaManager.GetInstance().env;
//        }
//        // Use this for initialization
//        void Start()
//        {
//            //Load ();
//        }
//        /// <summary>
//        /// Load the specified panelName and funName.
//        /// </summary>
//        /// <param name="panelName">Panel name.</param>
//        /// <param name="funName">Fun name. 默认在 GameState.curLuaScene 找，如果没有在 Global 找 </param>
//        public void Load(string panelName, string funName)
//        {

//            LuaTable gameState = env.Global.Get<LuaTable>("GameState");
//            LuaTable curLuaScene = gameState.Get<LuaTable>("curLuaScene");

//            onGameCmp = curLuaScene.Get<OnGameCmp>(funName);

//            if (onGameCmp == null)
//            {
//                Debug.LogWarningFormat("can not find lua function {0} in GameState.curLuaScene ", funName);
//                onGameCmp = env.Global.Get<OnGameCmp>(funName);
//            }
//            if (onGameCmp == null)
//            {
//                Debug.LogErrorFormat("can not find lua function {0} ", funName);
//                return;
//            }
//            luaCallBackDic.Add(panelName, onGameCmp);

//            Libs.AM.I.CreateFromCache(panelName, OnCmp);
//        }

//        void OnCmp(string assetName, UnityEngine.Object objInstantiateTp)
//        {

//            GameObject objInstantiate = Instantiate((GameObject)objInstantiateTp);
//            objInstantiate.name = objInstantiate.name.Replace("(Clone)", "");
//            objInstantiate.transform.SetParent(_FixedRoot, false);

//            OnGameCmp curOnGameCmp;

//            luaCallBackDic.TryGetValue(assetName, out curOnGameCmp);
//            if (curOnGameCmp != null)
//                curOnGameCmp(assetName);

//            luaCallBackDic.Remove(assetName);
//        }

//        // Update is called once per frame
//        void Update()
//        {

//        }


//        /// <summary>
//        /// Init The UI Root
//        /// 
//        /// UIRoot
//        /// -Canvas
//        /// --FixedRoot
//        /// --NormalRoot
//        /// --PopupRoot
//        /// </summary>
        
//        Transform root;
//        public Transform _RootCache
//        {
//            get
//            {
//                return root;
//            }
//        }

//        Transform fixedRoot;
//        public Transform _FixedRoot
//        {
//            get
//            {
//                return fixedRoot;
//            }
//        }

//        Transform normalRoot;
//        public Transform _NormalRoot
//        {
//            get
//            {
//                return normalRoot;
//            }
//        }

//        Transform popupRoot;

//        public Transform _PopupRoot
//        {

//            get
//            {
//                return popupRoot;
//            }
//        }

//        Camera UICamera;

//        //all pages with the union type
//        private Dictionary<string, UIPage> m_allPages;
//        public Dictionary<string, UIPage> allPages
//        { get { return m_allPages; } }

//        //control 1>2>3>4>5 each page close will back show the previus page.
//        private List<UIPage> m_currentPageNodes;
//        public List<UIPage> currentPageNodes
//        { get { return m_currentPageNodes; } }


//        #region manage-page api

//        private bool CheckIfNeedBack(UIPage page)
//        {
//            return page != null && page.CheckIfNeedBack();
//        }

//        /// <summary>
//        /// make the target node to the top.
//        /// </summary>
//        public void PopNode(UIPage page)
//        {
//            if (m_currentPageNodes == null)
//            {
//                m_currentPageNodes = new List<UIPage>();
//            }

//            if (page == null)
//            {
//                Debug.LogError("[UI] page popup is null.");
//                return;
//            }

//            //sub pages should not need back.
//            if (CheckIfNeedBack(page) == false)
//            {
//                return;
//            }

//            bool _isFound = false;
//            for (int i = 0; i < m_currentPageNodes.Count; i++)
//            {
//                if (m_currentPageNodes[i].Equals(page))
//                {
//                    m_currentPageNodes.RemoveAt(i);
//                    m_currentPageNodes.Add(page);
//                    _isFound = true;
//                    break;
//                }
//            }

//            //if dont found in old nodes
//            //should add in nodelist.
//            if (!_isFound)
//            {
//                m_currentPageNodes.Add(page);
//            }

//            //after pop should hide the old node if need.
//            HideOldNodes();
//        }

//        private void HideOldNodes()
//        {
//            if (m_currentPageNodes.Count < 0) return;
//            UIPage topPage = m_currentPageNodes[m_currentPageNodes.Count - 1];
//            if (topPage.mode == UIMode.HideOther)
//            {
//                //form bottm to top.
//                for (int i = m_currentPageNodes.Count - 2; i >= 0; i--)
//                {
//                    if (m_currentPageNodes[i].isActive())
//                        m_currentPageNodes[i].Hide();
//                }
//            }
//        }

//        //public void ClearNodes()
//        //{
//        //    m_currentPageNodes.Clear();
//        //}



//        private void ShowPage(string pageName,Action callback, object pageData, bool isAsync,
//            string strPath="",
//            UIType ut=UIType.Fixed,
//            UIMode um=UIMode.HideOther,
//            UICollider uc=UICollider.None
//            )
//        {
//            if (string.IsNullOrEmpty(pageName) )
//            {
//                Debug.LogError("[UI] show page error with :" + pageName + " maybe null instance.");
//                return;
//            }

//            if (m_allPages == null)
//            {
//                m_allPages = new Dictionary<string, UIPage>();
//            }

//            //if (pageInstance == null)
//            //{
//            //    pageInstance = new UIPage(ut, um, uc, strPath, pageName);
//            //}

//            UIPage page = null;

//            if (m_allPages.ContainsKey(pageName))
//            {
//                page = m_allPages[pageName];
//            }
//            else
//            {
//                page = new UIPage(ut, um, uc, strPath, pageName);
//                m_allPages.Add(pageName, page);
//                //page = pageInstance;
//            }

            

//            //if active before,wont active again.
//            //if (page.isActive() == false)
//            {
//                //before show should set this data if need. maybe.!!



//                if (isAsync)
//                    page.Show(callback);
//                else
//                    page.Show();
//            }
//        }


//        public void ShowPage(string pageName,
//            string strPath = "",
//            UIType ut = UIType.Fixed,
//            UIMode um = UIMode.HideOther,
//            UICollider uc = UICollider.None
//            )
//        {
//            ShowPage(pageName, null, null, false,strPath,ut,um,uc);
//        }


//        /// <summary>
//        /// Async Show Page with Async loader bind in 'TTUIBind.Bind()'
//        /// </summary>
//        public void ShowPage(string pageName,
//            Action callback
//            ,string strPath = "",
//            UIType ut = UIType.Fixed,
//            UIMode um = UIMode.HideOther,
//            UICollider uc = UICollider.None
//            )
//        {
//            ShowPage(pageName, callback, null, true, strPath, ut, um, uc);
//        }


//        /// <summary>
//        /// close current page in the "top" node.
//        /// </summary>
//        public void ClosePage()
//        {
//            Debug.Log("Back&Close PageNodes Count:" + m_currentPageNodes.Count);

//            if (m_currentPageNodes == null || m_currentPageNodes.Count <= 0) return;

//            UIPage closePage = m_currentPageNodes[m_currentPageNodes.Count - 1];
//            m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);
//            closePage.Hide();
//            //show older page.
//            //TODO:Sub pages.belong to root node.
//            if (m_currentPageNodes.Count > 0)
//            {
//                UIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
//                if (page.isAsyncUI)
//                {
//                    ShowPage
//                       (
//                        page.name,
//                        null,
//                        page.uiPath,
//                        page.type,
//                        page.mode,
//                        page.collider
//                        );


//                }
//                else
//                {
//                    ShowPage
//                       (page.name,
//                        page.uiPath,
//                        page.type,
//                        page.mode,
//                        page.collider);

//                    //after show to hide().
                    
//                }
//            }
//        }

//        /// <summary>
//        /// Close target page
//        /// </summary>
//        void ClosePage(UIPage target)
//        {
//            if (target == null) return;
//            if (target.isActive() == false)
//            {
//                if (m_currentPageNodes != null)
//                {
//                    for (int i = 0; i < m_currentPageNodes.Count; i++)
//                    {
//                        if (m_currentPageNodes[i] == target)
//                        {
//                            m_currentPageNodes.RemoveAt(i);
//                            break;
//                        }
//                    }
//                    return;
//                }
//            }

//            if (m_currentPageNodes != null && m_currentPageNodes.Count >= 1 && m_currentPageNodes[m_currentPageNodes.Count - 1] == target)
//            {
//                m_currentPageNodes.RemoveAt(m_currentPageNodes.Count - 1);

//                target.Hide();
//                //show older page.
//                //TODO:Sub pages.belong to root node.
//                if (m_currentPageNodes.Count > 0)
//                {
//                    UIPage page = m_currentPageNodes[m_currentPageNodes.Count - 1];
//                    if (page.isAsyncUI)
//                        ShowPage
//                        (
//                         page.name,
//                         null,
//                         page.uiPath,
//                         page.type,
//                         page.mode,
//                         page.collider
//                         );
//                    else
//                    {
//                        ShowPage
//                      (page.name,
//                       page.uiPath,
//                       page.type,
//                       page.mode,
//                       page.collider);
//                    }

//                    return;
//                }
//            }
//            else if (target.CheckIfNeedBack())
//            {
//                for (int i = 0; i < m_currentPageNodes.Count; i++)
//                {
//                    if (m_currentPageNodes[i] == target)
//                    {
//                        m_currentPageNodes.RemoveAt(i);
//                        target.Hide();
//                        break;
//                    }
//                }
//            }

//            target.Hide();
//        }



//        public void ClosePage(string pageName)
//        {
//            if (m_allPages != null && m_allPages.ContainsKey(pageName))
//            {
//                ClosePage(m_allPages[pageName]);
//            }
//            else
//            {
//                Debug.LogError(pageName + " havnt show yet!");
//            }
//        }
//        #endregion


//        static void InitRoot()
//        {
//            GameObject go = new GameObject("UIManager");
//            go.layer = LayerMask.NameToLayer("UI");
//            instance = go.AddComponent<UIManager>();
//            go.AddComponent<RectTransform>();

//            Canvas can = go.AddComponent<Canvas>();
//            can.renderMode = RenderMode.ScreenSpaceCamera;
//            can.pixelPerfect = true;

//            go.AddComponent<UnityEngine.UI.GraphicRaycaster>();
//            instance.root = go.transform;

       
//            CanvasScaler cs = go.AddComponent<CanvasScaler>();
//            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
//            cs.referenceResolution = new Vector2(1920f, 1080f);
//            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

//            GameObject camObj = new GameObject("UICamera");
//            camObj.layer = LayerMask.NameToLayer("UI");
//            camObj.transform.parent = go.transform;
//            camObj.transform.localPosition = new Vector3(0, 0, -100f);
//            Camera cam = camObj.AddComponent<Camera>();
//            cam.clearFlags = CameraClearFlags.Depth;
//            cam.orthographic = true;
//            cam.farClipPlane = 200f;
//            can.worldCamera = cam;
//            cam.cullingMask = 1 << 5;
//            cam.nearClipPlane = -50f;
//            cam.farClipPlane = 50f;

//            instance.UICamera = cam;

//            //add audio listener
//            //camObj.AddComponent<AudioListener>();
//            camObj.AddComponent<GUILayer>();

//            GameObject subRoot;

//            subRoot = CreateSubCanvasForRoot(go.transform); 
//            subRoot.name = "NormalRoot";
//            instance.normalRoot = subRoot.transform;
//            instance.normalRoot.transform.localScale = Vector3.one;

//            subRoot = CreateSubCanvasForRoot(go.transform);//, 250);
//            subRoot.name = "FixedRoot";
//            instance.fixedRoot = subRoot.transform;
//            instance.fixedRoot.transform.localScale = Vector3.one;

//            subRoot = CreateSubCanvasForRoot(go.transform);//, 500);
//            subRoot.name = "PopupRoot";
//            instance.popupRoot = subRoot.transform;
//            instance.popupRoot.transform.localScale = Vector3.one;

//            GameObject esObj = GameObject.Find("EventSystem");
//            if (esObj != null)
//            {
//                GameObject.DestroyImmediate(esObj);
//            }

//            GameObject eventObj = new GameObject("EventSystem");
//            eventObj.layer = LayerMask.NameToLayer("UI");
//            eventObj.transform.SetParent(go.transform);

//            eventObj.AddComponent<UnityEngine.EventSystems.EventSystem>();

//            eventObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();


//        }

//        static GameObject CreateSubCanvasForRoot(Transform root)
//        {
//            GameObject go = new GameObject("canvas");
//            go.transform.parent = root;
//            go.layer = LayerMask.NameToLayer("UI");

//            RectTransform rect = go.AddComponent<RectTransform>();
//            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
//            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
//            rect.anchorMin = Vector2.zero;
//            rect.anchorMax = Vector2.one;

//            //  Canvas can = go.AddComponent<Canvas>();
//            //  can.overrideSorting = true;
//            //  can.sortingOrder = sort;
//            //  go.AddComponent<GraphicRaycaster>();

//            return go;
//        }

//        /// Get Now Scene Show Page
//        /// 
//        /// </summary>
//        /// <returns>Return the Scene Show Normal Page</returns>
//        public UIPage getCurrentShowNormalPage()
//        {
//            if (m_currentPageNodes == null)
//                return null;
//            if(m_currentPageNodes.Count>0)
//                return m_currentPageNodes[m_currentPageNodes.Count - 1];
//            return null;
//        }




//    }

//}