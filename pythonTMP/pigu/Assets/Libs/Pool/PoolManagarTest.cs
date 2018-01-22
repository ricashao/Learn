using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libs;

public class Data{
	public int id = 1;
	public string name = "data";

	public Data (int idp,string namep){
		id = idp;
		name = namep;
	}
}

public class PoolManagarTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		Data data = PM.I.Get<Data>("data",delegate( object[] param) {
			
			return new Data(int.Parse(param[0].ToString()),param[1].ToString());

		},new object[] {1,"data0123456"});

		Debug.Log ("id = "+data.id.ToString() +",name = "+ data.name+",GetHashCode = " + data.GetHashCode().ToString() +",MemoryAdd = "+PM.GetMemory(data));

		Data data2 = PM.I.Get<Data> ("data");

		Debug.Log ("data2 = " + (data2 == null).ToString());

		PM.I.Free (data);

		Data data3 = PM.I.Get<Data>("data");

		Debug.Log ("data3 id = "+data3.id.ToString() +",name = "+ data3.name+",GetHashCode = " + data3.GetHashCode().ToString()+",MemoryAdd = "+PM.GetMemory(data3));
	
		Debug.Log(" " + (data == data3).ToString() +" "+ System.IntPtr.ReferenceEquals(data,data3) + "," + System.IntPtr.ReferenceEquals(data,new Data(2,"123")) );
		/*
		unsafe {
			int* dp = &data.id;
			int* d3p = &data3.id;

			Debug.LogFormat ("dp = 0:X,d3p = 1:X" , dp , d3p);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
