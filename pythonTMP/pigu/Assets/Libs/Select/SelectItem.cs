using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public class SelectItem : MonoBehaviour, ISelectAble {
	protected SelectGroup selectGroup;

	public int index;

    public object data;

	public void SetSelectGroup (SelectGroup selectGroup){
		this.selectGroup = selectGroup;
	}

	public void SetIndex (int index){
		this.index = index;
	}

	public int GetIndex (){
		return index;
	}

	public object GetData (){
		return data;
	}

	public void Select (){
		selectGroup.SelectByIndex (index);
	}

	virtual public void OnSelect (){

	}

	virtual public void UnSelect (){

	}
}
