using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour {

    int attackIndex = 0;
    float castTime = 0;
    float CanHitTime = 1.2f;
    bool beginHit = false;
    float beginHitTime = 0f;
    float endHitTIme = 0f;

    bool CanHit = false;
	// Use this for initialization
	void Start () {
        attackIndex = 0;
    }
	/*
	// Update is called once per frame
	void Update () {

        if (beginHit)
        {
            if(Time.time >= beginHitTime)
            {
                if(Time.time <= endHitTIme)
                {
                    CanHit = true;
                }
                else
                {
                    CanHit = false;
                    beginHit = false;
                    attackIndex = 0;
                }
            }
        }
	}


    public bool Attack(Vector3 _target,bool _IsSendCmd,out uint _skillId)
    {
        _skillId = 0;
        if (GameMain.getInstance().m_SelfPlayer.IsCanControl)  
        {
            return false;
        }

        if(attackIndex == 0)
        {
            //第一次攻击
            Skill _skill = GameMain.getInstance().m_SkillMgr.m_NormalAttack[attackIndex];
            if (_IsSendCmd)
            {
                _skill.m_attackRange.releasePot = transform.position;
                _skill.m_attackRange.directionAngle = transform.eulerAngles.y;
                _skill.m_attackRange.rangeParameter = _skill.m_angle;
                GameMain.getInstance().m_SkillMgr.UseSkill(_skill.skillListIndex, _target);
            }
            else
            {
                _skill.UseSkill(false);
            }
            _skillId = _skill.m_SkillId;
            castTime = Time.time;
            attackIndex++;
            beginHit = true;

            beginHitTime = Time.time + _skill.m_end_time;
            endHitTIme = beginHitTime + CanHitTime;
        }
        else
        {
            Skill _skill = GameMain.getInstance().m_SkillMgr.m_NormalAttack[attackIndex];
            if (CanHit)
            {
                if (_IsSendCmd)
                {
                    _skill.m_attackRange.releasePot = transform.position;
                    _skill.m_attackRange.directionAngle = transform.eulerAngles.y;
                    _skill.m_attackRange.rangeParameter = _skill.m_angle;
                    GameMain.getInstance().m_SkillMgr.UseSkill(_skill.skillListIndex, _target);
                }
                else
                {
                    _skill.UseSkill(false);
                }
                _skillId = _skill.m_SkillId;
                castTime = Time.time;
                beginHitTime = Time.time + _skill.m_end_time;
                endHitTIme = beginHitTime + CanHitTime;
                attackIndex++;
            }
        }

        if(attackIndex >= GameMain.getInstance().m_SkillMgr.m_NormalAttack.Length)
        {
            attackIndex = 0;
            beginHit = false;
            CanHit = false;
        }

        return true;
    }*/
}
