using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerDirectionController : DirectionController {
    
    public static bool isOnLine = false;
    public string playerTag = "Player";
    public Vector3 move = new Vector3();
    public CharacterController ccr;
    public float speed = 1f;
    public Vector2 initPot;// new Vector2(115f, 115f);
    public Transform target;
    public float angleCameraPlayer = 45f;
        
    IRoleMoveAnimationPlayer m_AniPlayer;
   
    override public void OnDrag(BaseEventData eventData){
        
        base.OnDrag(eventData);

        //计算角度
        angle = Mathf.Atan((basePoint.anchoredPosition.y - thumb.anchoredPosition.y) / (basePoint.anchoredPosition.x - thumb.anchoredPosition.x)) * Mathf.Rad2Deg;
        //计算距离
        //angle = Mathf.Atan((-thumb.anchoredPosition.y) / (-thumb.anchoredPosition.x)) * Mathf.Rad2Deg;

        if (thumb.anchoredPosition.x < basePoint.anchoredPosition.x  && thumb.anchoredPosition.y > basePoint.anchoredPosition.y  && angle < 0)
            angle =  180f + angle;
        if (thumb.anchoredPosition.x < basePoint.anchoredPosition.x  && thumb.anchoredPosition.y < basePoint.anchoredPosition.y  && angle > 0)
            angle = -90f - (90f - angle);

        if (target)
        {
            //相机和人物正方向的夹角 45f
            target.eulerAngles = new Vector3(0, -angle + angleCameraPlayer, 0);
        }

        PlayRun();
    }

    override public void OnEndDrag(BaseEventData eventData){
        base.OnEndDrag(eventData);

        PlayIdle();
    }

    void PlayRun()
    {
        if (m_AniPlayer != null)
        {
            m_AniPlayer.PlayRun(speed, speed);
        }

        if (m_OnMoveIng != null && enabledMove)  
        {
            m_OnMoveIng();
        }
    }

    void PlayIdle()
    {
        if (m_AniPlayer != null)
        {
            m_AniPlayer.PlayIdle();
        }

        if (m_OnMoveEnd != null && enabledMove)
        {
            m_OnMoveEnd();
        } 
    }

    public void initPlayerByTag() {

        GameObject gameObject = GameObject.FindGameObjectWithTag(playerTag);
        if (gameObject) {
            target = gameObject.transform;
            ccr = gameObject.GetComponent<CharacterController>();

            MonoBehaviour[] monoBehaviour = gameObject.GetComponentsInChildren<MonoBehaviour>();
            for ( int i = 0;i< monoBehaviour.Length;i++) {
                if (monoBehaviour[i] is IRoleMoveAnimationPlayer) {
                    m_AniPlayer = monoBehaviour[i] as IRoleMoveAnimationPlayer;
                }
            }
            if(m_AniPlayer == null)
                m_AniPlayer = gameObject.AddComponent<DefaultMoveAnimationPlayer>();
        }
    }

    void FixedUpdate(){
        if (ccr)
        {
            move.y -= 9.8f * speed * Time.deltaTime;
            ccr.Move(move);
        }
    }

    void Update()
    {
        if (target == null) {
            initPlayerByTag(); 
        }
       
        if (ccr && distance > 0)
        {
            if (isOnLine)
            {
                //speed = (float)DataManager.getInstance().m_userData.GetAttrByKey(AttributeEnum.MoveSpeed) / 1000;
            }
            // move.x = Mathf.Cos((targetEulerAnglesY + angle) * Mathf.Deg2Rad) * speed * Time.deltaTime;
            // move.z = Mathf.Sin((targetEulerAnglesY + angle) * Mathf.Deg2Rad) * speed * Time.deltaTime;

            move = target.transform.forward * speed * Time.deltaTime;

        }
        else {
            move.x = move.z = 0;
        }

    }
}
