using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;
using System; 

namespace Libs{

	public delegate System.Object CrateNewOne(System.Object[] param);

	public delegate void OnDestroy<T>(T t) where T : class;

	interface ICacheAble{
		
		bool IsFree ();
		void Free ();
		void Busy ();
		System.Object GetInstance ();
		int GetInstanceHashCode();
		string Type();
		void OnDestroy ();
	}

	class CacheItem<T> : ICacheAble where T : class {

		string _type;
		bool _isFree;
		T _instance;
		OnDestroy<T> _onDestroy;

		public void init(T t,string type = null,OnDestroy<T> onDestroy = null){
			_isFree = true;
			_type = type;
			_instance = t;
			_onDestroy = onDestroy;
		}

		public static CacheItem<T> Crate(T t,string type = null,OnDestroy<T> onDestroy = null){
			CacheItem<T> cacheItem = new CacheItem<T> ();
			cacheItem.init(t,type,onDestroy);
			return cacheItem; 
		}

		public string Type()
		{
			return _type;
		}

		public void Free (){

			_isFree = true;
		}

		public void Busy (){
			_isFree = false;
		}

		public bool IsFree (){
			return _isFree;
		}

		public int GetInstanceHashCode(){
			return _instance.GetHashCode ();
		}

		public System.Object GetInstance (){
			return _instance;
		}

		public void OnDestroy (){
			if (_onDestroy != null) {
				_onDestroy (_instance);
			}
		}

		public T GetI() {
			return _instance;
		}
	}

	public class PoolManagar : MonoBehaviour {

		private static PoolManagar  instance;
		public static PoolManagar  GetInstance()
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject("PoolManagar");
				DontDestroyOnLoad(gameObject);
				instance = gameObject.AddComponent<PoolManagar>();
			}
			return instance;
		}

		public static PoolManagar I{
			get {
				return GetInstance();
			}
		}
		List<ICacheAble > cache = new List<ICacheAble >();

		Dictionary<string,List<ICacheAble > > cacheDic = new Dictionary<string,List<ICacheAble> >(); 

		Dictionary<object,ICacheAble> busyDic = new Dictionary<object, ICacheAble> ();

		public void Clear(){
			FreeAllBusy ();
			cacheDic.Clear ();
			cache.Clear ();
		}
			
		public void Clear(string type){
			//List<CacheItem<T> > arr = new List<CacheItem<T>>[busyDic.Values.Count];
			//busyDic.Values.CopyTo (arr,0);

			foreach (ICacheAble curCacheItem in busyDic.Values) {
				if (type.Equals (curCacheItem.Type())) {
					curCacheItem.OnDestroy ();
					busyDic.Remove (curCacheItem);
				}
			}

			cacheDic.Remove (type);
		}

		public void FreeAllBusy(){
			foreach(ICacheAble curCacheItem in busyDic.Values){
				curCacheItem.Free ();
			}

			busyDic.Clear ();
		}

		public void AddOneFree<T>(T t) where T : class {
			
			CacheItem<T> curCacheItem = CacheItem<T>.Crate(t);
			cache.Add (curCacheItem);

		}
			
		public void AddOneFree<T>(T t,string type) where T : class{

			List<ICacheAble > list;

			cacheDic.TryGetValue (type,out list);

			if (list == null) {
				list = new List<ICacheAble> ();
				cacheDic.Add (type,list);
			}

			CacheItem<T> curCacheItem = CacheItem<T>.Crate(t,type);
			list.Add (curCacheItem);

		}

		public void FreeOne(object t) {

			ICacheAble curCacheItem;
			busyDic.TryGetValue (t,out curCacheItem);

			if (curCacheItem != null) {
				curCacheItem.Free ();
				busyDic.Remove (t);
			}

		}

		public void Free<T>(T t) where T : class {
			
			ICacheAble curCacheItem;
			busyDic.TryGetValue (t,out curCacheItem);

			if (curCacheItem != null) {
				curCacheItem.Free ();
				busyDic.Remove (t);
			}

		}

		public T Get<T>(string group ,CrateNewOne crateNewOne = null,System.Object[] param = null, OnDestroy<T> onDestroy = null) where T : class{

			List<ICacheAble> cacheList;
			cacheDic.TryGetValue (group,out cacheList);

			ICacheAble curCacheItem;

			if (cacheList != null) {

				Debug.LogWarning (String.Format( "cacheList.Count {0}",cacheList.Count));

				for(int i = 0; i< cacheList.Count;i++){
					curCacheItem = cacheList[i];
					if (curCacheItem.IsFree()) {
						T t = (T)curCacheItem.GetInstance();
						curCacheItem.Busy ();
						busyDic.Add (t, curCacheItem);
						return t;
					}
				}

				T instance = (T) crateNewOne (param); 
				curCacheItem = CacheItem<T>.Crate(instance,group,onDestroy);
				curCacheItem.Busy ();
				cacheList.Add(curCacheItem);
				busyDic.Add (instance, curCacheItem);

				return instance;
			}else
			if(crateNewOne != null){

				cacheList = new List<ICacheAble> ();
				// call your factory
				T instance = (T) crateNewOne (param); 
				curCacheItem = CacheItem<T>.Crate(instance,group,onDestroy);
				curCacheItem.Busy ();
				cacheList.Add(curCacheItem);
				cacheDic.Add(group,cacheList);
				busyDic.Add (instance, curCacheItem);

				return instance;
			}
				
			return null;
		}

		public T Get<T>(CrateNewOne crateNewOne = null) where T : class{

			return GetInstanceInList<T>(cache,crateNewOne);
		}

		private T GetInstanceInList<T>(List<ICacheAble > list,CrateNewOne crateNewOne = null,System.Object[] param = null,OnDestroy<T> onDestroy = null) where T : class{

			ICacheAble curCacheItem;

			for(int i = 0; i< list.Count;i++){
				curCacheItem = list[i];
				if (curCacheItem.IsFree()) {
					T t = (T)curCacheItem.GetInstance ();
					busyDic.Add (t, curCacheItem);
					return t ;
				}
			}

			if(crateNewOne != null){
				
				T instance = (T) crateNewOne(param); 
				curCacheItem = CacheItem<T>.Crate(instance,null,onDestroy);
				busyDic.Add (instance, curCacheItem);
				return instance;
			}

			return null;
		}

		public void OnDestroy (){
			Clear ();
		}
		// 获取引用类型的内存地址方法 
		public static string GetMemory(object o)    
		{  
			//引用跟踪
			GCHandle h = GCHandle.Alloc(o, GCHandleType.Pinned); 
			//GCHandle h = GCHandle.Alloc(o, GCHandleType.Normal);  
			IntPtr addr = GCHandle.ToIntPtr(h);  
			h.Free ();
			return "0x" + addr.ToString("X");  
		}    

	}

	public class PM : PoolManagar {}

}
/*

List<CacheItem<T> > cache = new List<CacheItem<T> >();

Dictionary<string,List<CacheItem<T> > > cacheDic = new Dictionary<string,List<CacheItem<T> > >(); 

Dictionary<int,CacheItem<T>> busyDic = new Dictionary<int, CacheItem<T>> ();

public void Clear(){
	FreeAllBusy ();
	cacheDic.Clear ();
	cache.Clear ();
}

public void Clear(string type){
	//List<CacheItem<T> > arr = new List<CacheItem<T>>[busyDic.Values.Count];
	//busyDic.Values.CopyTo (arr,0);

	foreach (CacheItem<T> curCacheItem in busyDic.Values) {
		if (type.Equals (curCacheItem.type)) {
			busyDic.Remove (curCacheItem.GetI().GetHashCode());
		}
	}

	cacheDic.Remove (type);
}

public void FreeAllBusy(){
	foreach(CacheItem<T> curCacheItem in busyDic.Values){
		curCacheItem.Free ();
	}

	busyDic.Clear ();
}

public void AddOneFree(T t){

	CacheItem<T> curCacheItem = CacheItem<T>.Crate(t);
	cache.Add (curCacheItem);

}

public void AddOneFree(T t,string type){

	List<CacheItem<T> > list;

	cacheDic.TryGetValue (type,out list);

	if (list == null) {
		list = new List<CacheItem<T>> ();
		cacheDic.Add (type,list);
	}

	CacheItem<T> curCacheItem = CacheItem<T>.Crate(t,type);
	list.Add (curCacheItem);

}

public void Free(T t){

	CacheItem<T> curCacheItem;
	busyDic.TryGetValue (t.GetHashCode(),out curCacheItem);

	if (curCacheItem != null) {
		curCacheItem.Free ();
		busyDic.Remove (t.GetHashCode ());
	}

}

public T Get(string type ,CrateNewOne crateNewOne = null,System.Object[] param = null){

	List<CacheItem<T>> cacheList;
	cacheDic.TryGetValue (type,out cacheList);

	CacheItem<T> curCacheItem;

	if (cacheList != null) {

		for(int i = 0; i< cacheList.Count;i++){
			curCacheItem = cacheList[i];
			if (curCacheItem.IsFree()) {
				T t = curCacheItem.GetI ();
				curCacheItem.Busy ();
				busyDic.Add (t.GetHashCode (), curCacheItem);
				return t;
			}
		}

	} else if(crateNewOne != null){

		cacheList = new List<CacheItem<T>> ();

		T instance = (T) crateNewOne (param); 
		curCacheItem = CacheItem<T>.Crate(instance,type);
		curCacheItem.Busy ();
		cacheList.Add(curCacheItem);
		cacheDic.Add(type,cacheList);
		busyDic.Add (instance.GetHashCode (), curCacheItem);

		return instance;
	}

	return null;
}

public T Get(CrateNewOne crateNewOne = null){

	return GetInstanceInList(cache,crateNewOne);
}

private T GetInstanceInList(List<CacheItem<T> > list,CrateNewOne crateNewOne = null,System.Object[] param = null){

	CacheItem<T> curCacheItem;

	for(int i = 0; i< list.Count;i++){
		curCacheItem = list[i];
		if (curCacheItem.IsFree()) {
			T t = curCacheItem.GetI ();
			busyDic.Add (t.GetHashCode (), curCacheItem);
			return t ;
		}
	}

	if(crateNewOne != null){

		T instance = (T) crateNewOne(param); 
		curCacheItem = CacheItem<T>.Crate(instance);
		busyDic.Add (instance.GetHashCode (), curCacheItem);
		return instance;
	}

	return null;
}
}
*/
/*
public class PoolManagar: MonoBehaviour {

	private static PoolManagar instance;
	public static PoolManagar getInstance()
	{
		if (instance == null)
		{
			GameObject gameObject = new GameObject("PoolManagar");
			DontDestroyOnLoad(gameObject);
			instance = gameObject.AddComponent<PoolManagar>();
		}
		return instance;
	}
	public static PoolManagar initForGameObject(GameObject dontDestroyOnLoadGameObject)
	{
		if (instance == null)
		{
			instance = dontDestroyOnLoadGameObject.AddComponent<PoolManagar>();
		}
		return instance;
	}

	Dictionary<string,List<ICacheAble> >  poolDic = new Dictionary<string, List<ICacheAble>>();

	// Use this for initialization
	void Start () {
		
	}

	public T GetInstance<T>(CrateNewOne crateNewOne = null)  where T :class{
		
		List<ICacheAble> cacheList;
		poolDic.TryGetValue (typeof(T).ToString(),out cacheList);

		ICacheAble curCacheItem;

		if (cacheList != null) {

			for(int i = 0; i< cacheList.Count;i++){
				curCacheItem = cacheList[i];
				if (curCacheItem.IsFree()) {
					return (T) curCacheItem.GetInstance() ;
				}
			}

		} else if(crateNewOne != null){
		
			cacheList = new List<ICacheAble> ();
			poolDic.Add (typeof(T).ToString(),cacheList);

			T instance = (T) crateNewOne (); 
			curCacheItem = CacheItem<T>.Crate(instance);
			cacheList.Add(curCacheItem);
		
			return instance;
		}

		return null;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
*/
