using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderExt : MonoBehaviour
{
    Slider _sliderIns;

    void Start()
    {
        _sliderIns = GetComponentInParent<Slider>();
        if (_sliderIns != null)
        {
            _sliderIns.onValueChanged.AddListener(OnSliderValueChange);
        }
    }

    [SerializeField]
    GameObject mgoHandle,mgoInfo;


    public void OnSliderValueChange(float fper)
    {
        Debug.Log("Current Per:" + fper);

        RectTransform rtself = GetComponent<RectTransform>();
        float fcurpos = _sliderIns.GetComponent<RectTransform>().sizeDelta.x * fper;

        RectTransform rtgoHandle=mgoHandle.GetComponent<RectTransform>();
        if (rtgoHandle != null)
        {
            rtgoHandle.anchoredPosition = new Vector2(fcurpos, rtgoHandle.anchoredPosition.y);
        }

        RectTransform rtinfoHandle = mgoInfo.GetComponent<RectTransform>();
        if (rtinfoHandle != null)
        {
            rtinfoHandle.anchoredPosition = new Vector2(fcurpos, rtinfoHandle.anchoredPosition.y);
        }

        Text txtInfo = mgoInfo.GetComponent<Text>();
        if(txtInfo!=null)
        {
            int nper=(int)Mathf.Round(fper * 100);
            txtInfo.text = nper.ToString() + "%";
        }

    }

    //void Update()
    //{
        
    //}

    //private void OnGUI()
    //{
    //    if (Input.GetKeyUp(KeyCode.S))
    //    {
    //        _sliderIns.value += 0.05f;

    //    }
    //}

}
