using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneClick : MonoBehaviour, IPointerClickHandler  {

	// Use this for initialization
	public void OnPointerClick (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}
}
