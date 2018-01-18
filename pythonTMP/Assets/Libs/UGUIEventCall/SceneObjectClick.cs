using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneObjectClick : MonoBehaviour,IPointerClickHandler ,IPointerDownHandler,IPointerEnterHandler,IPointerUpHandler{


	public void OnPointerClick (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}

	public void OnPointerDown (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}

	public void OnPointerEnter (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}

	public void OnPointerUp (PointerEventData eventData){
		Debug.LogFormat ("OnEevent {0}",eventData.pointerCurrentRaycast);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
