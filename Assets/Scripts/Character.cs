using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace SOP
{
    public enum InputState
    {

        Idle = 0001,
        /// <summary>
        /// 监听翻滚/闪避
        /// </summary>
        Dodge = 0010,
        /// <summary>
        /// 监听攻击
        /// </summary>
        Attack = 0100,
        /// <summary>
        /// 监听跳跃
        /// </summary>
        Jump = 1000,
        /// <summary>
        /// 监听格挡
        /// </summary>
        Block = 00010000,
        /// <summary>
        /// 监听移动
        /// </summary>
        Move = 00100000,
    }


    public enum BattleEventType
    {
        /// <summary>
        /// 成功攻击
        /// </summary>
        Hurt,
        /// <summary>
        /// 受击
        /// </summary>
        Hurted,
        /// <summary>
        /// 成功格挡
        /// </summary>
        Block,

        /// <summary>
        /// 被格挡
        /// </summary>
        Blocked,
        /// <summary>
        /// 躯干耗尽
        /// </summary>
        SpEmpty,
        /// <summary>
        /// 血量耗尽
        /// </summary>
        HpEmpty


    }

    [Serializable]
    public class ActionReport
    {
        public string content;
        [Min(0)]
        /// <summary>
        /// 在此期间不能做任何其他InputAction
        /// </summary>
        public float duration;
        public InputState stateType;
        public string keyName;
        public ActionReport() { }
        public ActionReport(string content, float duration, InputState stateType, string keyName)
        {
            this.content = content;
            this.duration = duration;
            this.stateType = stateType;
            this.keyName = keyName;
        }
    }

    public class Character : MonoBehaviour
    {
        #region 角色属性
        public float maxHp;
        public float maxSp;

        public float hp;
        public float sp;
        public Vector2 position;
        #endregion

        #region  特殊判断的属性
        /// <summary>
        /// 格挡
        /// </summary>
        public float block = 0;

        /// <summary>
        /// 翻滚/闪避
        /// </summary>
        public float dodge = 0;
        /// <summary>
        /// 跳跃
        /// </summary>
        public float jump = 0;
        /// <summary>
        /// 看破/垫步
        /// </summary>
        public float crack = 0;
        /// <summary>
        /// 面部朝向
        /// </summary>
        public Vector2 forward = Vector2.zero;
        /// <summary>
        /// 位置临时突变
        /// </summary>
        public Vector2 deltaPos;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float speed = 0;
        /// <summary>
        /// 攻击半径
        /// </summary>

        public float radius = 1;

        /// <summary>
        /// 是否监听输入
        /// </summary>
        [EnumFlags]
        public InputState inputState;

        /// <summary>
        /// 是否被攻击后会触发轻硬直（被打断）
        /// </summary>
        public bool isBreakable;
        /// <summary>
        /// 是否被攻击时会被扣血、扣躯干、扣韧性
        /// </summary>
        public bool isHurtable;
        /// <summary>
        /// 躯干值基础变化速度
        /// </summary>
        public float altSp;
        /// <summary>
        /// 是否正在进行伤害判断
        /// </summary>
        public bool isAttacking;
        /// <summary>
        /// 是否正处于攻击动作
        /// </summary>
        public bool isInAttackAction;
        #endregion

        #region 行动内容
        public List<ActionReport> action_attacks;
        public ActionReport action_idle = new ActionReport("调…息…片…刻", 0, InputState.Idle, "idle");
        public ActionReport action_block = new ActionReport("持剑…格挡！", 1.5f, InputState.Block, "block");
        public ActionReport action_jump = new ActionReport("下…蹲……跳起！", 2f, InputState.Jump, "jump");
        public ActionReport action_dodge = new ActionReport("抬脚…闪避！", 1.5f, InputState.Dodge, "dodge");
        public ActionReport action_blocked = new ActionReport("咳……被格挡！", 0.5f, InputState.Idle, "blocked");
        public ActionReport action_attacked = new ActionReport("踉跄……被击中！", 1.75f, InputState.Idle, "attacked");
        public ActionReport action_blkSuc = new ActionReport("格挡成功！", 0f, InputState.Idle, "blockSucceed");
        #endregion
        /// <summary>
        /// 通过actionKey寻找对应的InputAction。可能返回null
        /// </summary>
        /// <param name="actionKey"></param>
        /// <returns></returns>
        public ActionReport GetInputAction(string actionKey)
        {
            if (actionKey == "idle") return action_idle;
            if (actionKey == "block") return action_block;
            if (actionKey == "jump") return action_jump;
            if (actionKey == "dodge") return action_dodge;
            if (actionKey == "attacked") return action_attacked;
            if (actionKey == "blocked") return action_blocked;
            if (actionKey == "blockSucceed") return action_blkSuc;
            return action_attacks.Find((ActionReport a) => a.keyName == actionKey);
        }
        /// <summary>
        /// 进行攻击判定
        /// </summary>
        public AttackResult TestAttack(Character attacked, AttackData attackData)
        {
            if (attackData == null || attacked == null)
            {
                //没有攻击目标（没有攻击内容，或者目标不存在）
                return AttackResult.NoTarget;
            }
            var distance = GetDistance(attacked, attackData.coeRadius);
            if (distance > 0)
            {
                //距离过远，没有命中
                return AttackResult.Missed;
            }
            else if (attackData.blockIgnore < attacked.block)
            {
                //格挡成功
                return AttackResult.Blocked;
            }
            else if (attackData.dodgeIgnore < attacked.dodge)
            {
                //闪避成功
                return AttackResult.Blocked;
            }
            else if (attackData.jumpIgnore < attacked.jump)
            {
                //跳跃成功
                return AttackResult.Blocked;
            }
            else if (attackData.crackIgnore < attacked.crack)
            {
                //识破/垫步成功
                return AttackResult.Cracked;
            }
            return AttackResult.Hitted;
        }


        public void InvokeAttack()
        {
            this.isAttacking = true;
        }
        public void EndAttack()
        {
            this.isAttacking = false;
        }
        public float GetDistance(Character other, float attackCoeRadius = 1)
        {
            return Vector2.Distance(this.position, other.position) - attackCoeRadius * this.radius - other.radius;
        }

        void Start()
        {
            this.hp = maxHp;
            this.sp = maxSp;
        }
    }
}