using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
    public string playerTag = "Player";
    public GameObject player;
    Animation ani;
    public string skillName = "Attack1";
    AutoAttack m_autoAttack;

    // Use this for initialization
    void Start () {

    }

    void TryInit() {
        if (player == null)
            player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null){
            ani = player.GetComponent<Animation>();
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
            m_autoAttack = player.GetComponent<AutoAttack>();
        }
    }

    public void OnClick() {
        if (ani){
            ani.wrapMode = WrapMode.Once;
            ani.CrossFade(skillName);
        }

        /*
        if(m_autoAttack.m_State != AutoAttackState.STATE_FIND_ENEMY)
        {
            if (!m_autoAttack.FindEnemy())
            {
                //周围没有可攻击的目标 只播放动作
                if (!GameMain.getInstance().m_SelfPlayer.IsCanControl)
                {
                    uint _skillId = 0;
                    bool isUse = GameMain.getInstance().m_SelfPlayer.normalAttackMgr.Attack(player.gameObject.transform.position + player.transform.forward, false,out _skillId);
                    if(isUse && _skillId != 0)
                    {
                        GameMain.getInstance().m_SelfPlayer.PlaySkill(_skillId, player.gameObject.transform.position + player.transform.forward);
                    }
                }
            }
        }*/

    }

    float _cdTime = 0;
    bool _isRunCD = false;
    float _startTime = 0;
    public void BeginCoolDown(float _time)
    {
        _cdTime = _time;
        _isRunCD = true;
        _startTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (player == null) {
            TryInit();
        }

        if (_isRunCD)
        {
            if(Time.time - _startTime >= _cdTime)
            {
                //GameMain.getInstance().m_SkillMgr.m_SkillList[GameMain.getInstance().m_SkillMgr.GetNormalSkillIndex()].ClearCD();
                _isRunCD = false;
            }
        }
    }
}
