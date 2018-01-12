using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UILoopItem : MonoBehaviour {

    //[System.NonSerialized]
    public int itemIndex;
    //[System.NonSerialized]
    public GameObject itemObject;
    protected object data;
	public void UpdateItem(int index,GameObject item)
	{
        itemIndex = index;
        itemObject = item;
	}
    public virtual void Data(object data)
	{
        //Debug.Log("Data:" + data.ToString());
		Debug.LogWarningFormat ("index:{0} Data:{1}",itemIndex,data.ToString());
        this.data = data;
	}
    public virtual object GetData()
    {
        return data;
    }
    public virtual void SetSelected(bool selected)
    {

    }
}
