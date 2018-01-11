using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public class SelectGroup : MonoBehaviour {

	public SelectItem[] initArr;

    public int selectIndex;
    public object selectData;

    public List<ISelectAble> group = new List<ISelectAble> ();

	public void Awake(){

		if(initArr != null){
			
			for(int i = 0 ; i < initArr.Length; i++ ){
				AddItem (initArr[i]);
			}
		}
	}

	public void SelectByIndex (int index){
		
		for(int i = 0 ;i < group.Count; i++ ){

			if (i == index) {
				group[i].OnSelect();
                selectIndex = index;
                selectData = group[i].GetData();
            } else {
				group[i].UnSelect();
			}
		}
	}

	public void AddItem(ISelectAble selectItem ){
		
		group.Add (selectItem);

		selectItem.SetSelectGroup (this);
		selectItem.SetIndex (group.Count - 1);
	}

	public void Add(object selectItem){
		
		if (selectItem is ISelectAble) {
			AddItem (selectItem as ISelectAble);
		}
	}

	void OnDestroy(){

		group.Clear ();
	}
}
