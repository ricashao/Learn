/// <summary>
/// LuaFramework 
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libs
{
	public delegate void TimerUpdate(TimerInfo timerInfo);

	//namespace LuaFramework {
	public class TimerInfo {
		public float interval;
		public float surplus;

		public long tick;
		public bool stop;
		public bool delete;
		public Object target;
		public string className;

		public TimerUpdate timerUpdate;

		public TimerInfo(){
			
		}

		public TimerInfo(string className, Object target) {
			this.className = className;
			this.target = target;
			delete = false;
		}

		virtual public void Update(float curInterval){

			surplus =surplus - curInterval;

			if (surplus < 0) {
				timerUpdate (this);
				surplus = interval; 
			}
		}

	}
	//TimerInterval

	public class TimerManager :MonoBehaviour {

		private static TimerManager instance;
		public static TimerManager getInstance()
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject("TimerManager");
				DontDestroyOnLoad(gameObject);
				instance = gameObject.AddComponent<TimerManager>();
			}
			return instance;
		}
		public static TimerManager initForGameObject(GameObject dontDestroyOnLoadGameObject)
		{
			if (instance == null)
			{
				instance = dontDestroyOnLoadGameObject.AddComponent<TimerManager>();
			}
			return instance;
		}

	private float interval = 0;
		protected List<TimerInfo> objects = new List<TimerInfo>();

	public float Interval {
		get { return interval; }
		set { interval = value; }
	}

		public float DFInterval = 1.0f;

	// Use this for initialization
	void Start() {
			StartTimer(DFInterval);
	}

	/// <summary>
	/// Æô¶¯¼ÆÊ±Æ÷
	/// </summary>
	/// <param name="interval"></param>
	public void StartTimer(float value) {
		interval = value;
		InvokeRepeating("Run", 0, interval);
	}

	/// <summary>
	/// Í£Ö¹¼ÆÊ±Æ÷
	/// </summary>
	public void StopTimer() {
		CancelInvoke("Run");
	}
			
	/// <summary>
	/// Ìí¼Ó¼ÆÊ±Æ÷ÊÂ¼þ
	/// </summary>
	/// <param name="name"></param>
	/// <param name="o"></param>
	public void AddTimerEvent(TimerInfo info) {
		if (!objects.Contains(info)) {
			objects.Add(info);
		}
	}

	/// <summary>
	/// É¾³ý¼ÆÊ±Æ÷ÊÂ¼þ
	/// </summary>
	/// <param name="name"></param>
	public void RemoveTimerEvent(TimerInfo info) {
		if (objects.Contains(info) && info != null) {
			info.delete = true;
		}
	}

	/// <summary>
	/// Í£Ö¹¼ÆÊ±Æ÷ÊÂ¼þ
	/// </summary>
	/// <param name="info"></param>
	public void StopTimerEvent(TimerInfo info) {
		if (objects.Contains(info) && info != null) {
			info.stop = true;
		}
	}

	/// <summary>
	/// ¼ÌÐø¼ÆÊ±Æ÷ÊÂ¼þ
	/// </summary>
	/// <param name="info"></param>
	public void ResumeTimerEvent(TimerInfo info) {
		if (objects.Contains(info) && info != null) {
			info.delete = false;
		}
	}

	/// <summary>
	/// ¼ÆÊ±Æ÷ÔËÐÐ
	/// </summary>
	void Run() {
		if (objects.Count == 0) return;
		for (int i = 0; i < objects.Count; i++) {
			TimerInfo o = objects[i];
			if (o.delete || o.stop) { continue; }
			//ITimerBehaviour timer = o.target as ITimerBehaviour;
			//timer.TimerUpdate();
				o.Update(interval);

			o.tick++;
		}
		/////////////////////////Çå³ý±ê¼ÇÎªÉ¾³ýµÄÊÂ¼þ///////////////////////////
		for (int i = objects.Count - 1; i >= 0; i--) {
			if (objects[i].delete) { objects.Remove(objects[i]); }
		}
	}

		protected virtual void OnDestroy(){
			objects.Clear ();
		}
}

}