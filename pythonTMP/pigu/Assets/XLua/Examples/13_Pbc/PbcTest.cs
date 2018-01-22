using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XLua;

public class PbcTest : MonoBehaviour {

	internal static LuaEnv luaEnv = new LuaEnv();

	public TextAsset luaScript;
	private LuaTable scriptEnv;

	void Awake()
	{
		scriptEnv = luaEnv.NewTable ();


	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
