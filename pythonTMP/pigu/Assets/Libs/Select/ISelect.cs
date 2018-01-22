using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISelectAble
{

    void SetSelectGroup(SelectGroup selectGroup);

    void SetIndex(int index);

    int GetIndex();

    object GetData();

    void Select();

    void OnSelect();

    void UnSelect();
}