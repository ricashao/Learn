using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AutoAttackState
{
    STATE_NONE,
    STATE_FIND_ENEMY,               //搜索敌人
    STATE_MOVETO,                   //移动中
    STATE_ATTACK                    //攻击
}


//自动攻击
public class AutoAttack : MonoBehaviour {

    struct EnemyData
    {
        public void Clear()
        {
            _obj = null;
            _dis = 999;
        }
        public GameObject _obj;
        public float _dis;
    }

    public float m_MaxFindRadis = 5.0f;             //最大的索敌范围
    public float m_LastRadis = 3.0f;                //保留目标的距离
    public float m_AttackInterval = 1.2f;            //攻击间隔
    public float m_AttackStartTime = 0f;
    public int SearchType = 0;                             //0 距离优先  1优先玩家  2优先怪物

    public AutoAttackState m_State;

    EnemyData m_FaceObj = new EnemyData();                   //正面距离最近的
    EnemyData m_BackObj = new EnemyData();                   //反面距离最近的

    GameObject m_target;
  
    Skill normalSkill;                                      //普攻技能
    /*
    // Use this for initialization
    void Start () {
   
        m_State = AutoAttackState.STATE_NONE;
    }

    public bool FindEnemy()
    {
        if (IsFindLastEnemy())
        {
            return true;
        }
        m_FaceObj.Clear();
        m_BackObj.Clear();
        m_target = null;
        m_State = AutoAttackState.STATE_FIND_ENEMY;
        //开始遍历附近的敌人
        if(SearchType == 0)
        {
            SearchPlayer();
            SearchMonster();
        }
        else if(SearchType == 1)        //优先搜索玩家
        {
            SearchPlayer();
            if (m_FaceObj._obj == null && m_BackObj._obj == null)
            {
                SearchMonster();
            }
        }
        else if(SearchType == 2)        //优先怪物
        {
            SearchMonster();
            if (m_FaceObj._obj == null && m_BackObj._obj == null)
            {
                SearchPlayer();
            }
        }


        if (m_FaceObj._obj == null && m_BackObj._obj == null)
        {
            //附近没有目标
            m_State = AutoAttackState.STATE_NONE;
            return false;
        }
        else if(m_BackObj._obj == null)
        {
            //只有正面目标
            m_target = m_FaceObj._obj;
        }
        else if(m_FaceObj._obj != null)
        {
            //只有背面目标
            m_target = m_BackObj._obj;
        }
        else if (Mathf.Abs(m_FaceObj._dis - m_BackObj._dis) > 2.0f && m_BackObj._dis < m_FaceObj._dis)
        {
            //如果背面目标比正面目标近而且近很多 则优先攻击背面目标
            m_target = m_BackObj._obj;
        }
        else
        {
            //攻击正面目标
            m_target = m_FaceObj._obj;
        }

        //如果在攻击范围内立刻进行攻击
        float fdis = Vector3.Distance(m_target.transform.position, transform.position);
        if(fdis <= normalSkill.m_MaxRadius)
        {
            m_State = AutoAttackState.STATE_ATTACK;
            m_AttackStartTime = 0;
            return true;
        }

        //不在攻击范围内 向目标进行移动
        m_State = AutoAttackState.STATE_MOVETO;
        m_MoveController.OnMoveToTarget(m_target.transform, normalSkill.m_MaxRadius - 0.5f, OnMoveEnd);
        return true;
    }

    public void SearchPlayer()
    {
        foreach (var value in GameMain.getInstance().m_PlayerMgr.m_otherPlayerGroup)
        {
            GameObject _other = value.Value.objInstantiate;

            float _dis = Vector3.Distance(_other.transform.position, transform.position);

            if (_dis > m_MaxFindRadis || value.Value.IsDeath)
            {
                //超出最大索敌范围
                continue;
            }

            //计算该玩家是否在正面
            Vector3 dir = _other.transform.position - transform.position; //位置差，方向  
            Vector3 cross = Vector3.Cross(transform.forward, dir.normalized);//点乘判断左右  // cross.y>0在左  <0在右   
            Vector3 cross1 = Vector3.Cross(transform.right, dir.normalized);//点乘判断前后  // cross.y>0在前  <0在后 

            if (cross1.y > 0)
            {
                //判断是否需要更新前方目标
                if (m_FaceObj._dis > _dis || m_FaceObj._dis == 0)
                {
                    m_FaceObj._obj = _other;
                    m_FaceObj._dis = _dis;
                }
            }
            else
            {
                //更新背面数组
                if (m_BackObj._dis > _dis || m_BackObj._dis == 0)
                {
                    m_BackObj._obj = _other;
                    m_BackObj._dis = _dis;
                }
            }
        }
    }

    public void SearchMonster()
    {
        foreach (var value in GameMain.getInstance().m_NpcMgr.DicMonsterObj)
        {
            GameObject _other = value.Value.objInstantiate;

            float _dis = Vector3.Distance(_other.transform.position, transform.position);

            if (_dis > m_MaxFindRadis || value.Value.IsDeath())
            {
                //超出最大索敌范围
                continue;
            }

            //计算该玩家是否在正面
            Vector3 dir = _other.transform.position - transform.position; //位置差，方向  
            Vector3 cross = Vector3.Cross(transform.forward, dir.normalized);//点乘判断左右  // cross.y>0在左  <0在右   
            Vector3 cross1 = Vector3.Cross(transform.right, dir.normalized);//点乘判断前后  // cross.y>0在前  <0在后 

            if (cross1.y > 0)
            {
                //判断是否需要更新前方目标
                if (m_FaceObj._dis > _dis || m_FaceObj._dis == 0)
                {
                    m_FaceObj._obj = _other;
                    m_FaceObj._dis = _dis;
                }
            }
            else
            {
                //更新背面数组
                if (m_BackObj._dis > _dis || m_BackObj._dis == 0)
                {
                    m_BackObj._obj = _other;
                    m_BackObj._dis = _dis;
                }
            }
        }
    }

    public void OnMoveEnd()
    {
        m_State = AutoAttackState.STATE_ATTACK;
        m_AttackStartTime = 0;// Time.time;
    }

    public bool IsFindLastEnemy()
    {
        if (normalSkill == null)
        {
            normalSkill = GameMain.getInstance().m_SkillMgr.GetSkill(GameMain.getInstance().m_SkillMgr.GetNormalSkillIndex());
        }
        if (m_target == null)
        {
            return false;
        }
        return false;
        float _dis = Vector3.Distance(m_target.transform.position, transform.position);
        if(_dis <= m_LastRadis)
        {
            if (normalSkill != null)
            {
                if (_dis <= normalSkill.m_MaxRadius)
                {
                    if (normalSkill.CanUse())
                    {
                        GameMain.getInstance().m_SkillMgr.UseSkill(GameMain.getInstance().m_SkillMgr.GetNormalSkillIndex(), m_target.transform.position);
                    }
                    m_State = AutoAttackState.STATE_ATTACK;
                    m_AttackStartTime = Time.time;
                    return true;
                }
            }
            m_MoveController.OnMoveToTarget(m_target.transform, normalSkill.m_MaxRadius - 0.5f, OnMoveEnd);
            return true;
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_target == null)
        {
            m_State = AutoAttackState.STATE_NONE;
            return;
        }

        float _dis = Vector3.Distance(m_target.transform.position, transform.position);
        if(m_State == AutoAttackState.STATE_ATTACK)
        {
            if(Time.time - m_AttackStartTime >= m_AttackInterval)
            {
                //如果目标在攻击范围内 则自动进行攻击
                if (normalSkill != null)
                {
                    if (_dis <= normalSkill.m_MaxRadius)
                    {
                        uint _skillId = 0;  
                        GameMain.getInstance().m_SelfPlayer.normalAttackMgr.Attack(m_target.transform.position, true, out _skillId);
                        //GameMain.getInstance().m_SkillMgr.UseSkill(GameMain.getInstance().m_SkillMgr.GetNormalSkillIndex(), m_target.objInstantiate.transform.position);
                    }
                    else
                    {
                        //无法攻击目标 停止自动攻击
                        m_State = AutoAttackState.STATE_NONE;
                    }
                    m_AttackStartTime = Time.time;
                }
                else
                {
                    m_State = AutoAttackState.STATE_NONE;
                }
            }
        }
	}*/
}
