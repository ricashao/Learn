using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能的施放类型
public enum SkillCastType
{
    SKILL_NORMAL,       //普通技能 不需要目标
    SKILL_RECT,         //矩形范围
    SKILL_CIRCLE,       //圆形
    SKILL_SECTOR,       //扇形范围
    SKILL_DOT,           //点对点攻击
    SKILL_SELF_CIRCLE
}

//技能施放目标
public enum SkillBaseType
{
    BASE_DAMAGE = 1,        //伤害技能
    BASE_HELP,          //治疗技能
    BASE_NORMAL         //普攻技能
}

//技能的当前状态
public enum SkillState
{
    STATE_READY,            //准备就绪
    STATE_USE,              //使用中
    STATE_COOLDOWN          //cd中
}

public class Skill {

    public SkillCastType m_CastType;
    public SkillBaseType m_BaseType;
    public SkillState m_SkillState;

    public uint m_SkillLevel;            //技能的等级
    public uint m_SkillId;               //技能id
    public float m_MaxRadius;                //技能范围
    public float m_angle;                 //技能施放范围

    public float m_ready_time;          //技能前摇时间
    public float m_check_time;          //检测目标时间
    public float m_end_time;            //结束僵直事件
    public int m_effectID;              //特效id

    public int m_MaxAttackNum;          //最大攻击人数上限
    public Vector3 m_useSkillDir;       //使用技能的朝向
    public uint m_SkillStartTime;       //开始使用技能的时间
    public string skillName = "Attack1";

    public float m_skillCD;             //技能的cd

    float _cdTime = 0;
    bool _isCheckDamage = true;        //是否检测过攻击目标
    bool _isCheckStiffen = false;      //是否检测僵直
    public SkillAttackRange m_attackRange;

    public int normalAttackIndex = 0;
    public int skillListIndex = 0;

 
}
