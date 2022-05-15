using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOP;
namespace BehaviorDesigner.Runtime.Tasks.SOP
{
    [TaskCategory("SOP")]
    [TaskDescription("目标敌人-正在攻击自己，并且距离满足进攻条件。")]
    public class EnemyGetAttackCondition : Conditional
    {
        public GameObject target;
        CharacterBehaviour targetBehaviour;
        CharacterBehaviour selfBehvaiour;
        bool isTriggered = false;
        public override void OnStart()
        {
            base.OnStart();
            if (!this.target)
            {
                this.target = (GameObject)GlobalVariables.Instance.GetVariable("hero").GetValue();
            }
            targetBehaviour = this.target.GetComponent<CharacterBehaviour>();
            selfBehvaiour = this.GetComponent<CharacterBehaviour>();
            isTriggered = true;
        }
        public override TaskStatus OnUpdate()
        {
            if (isTriggered && targetBehaviour.target == selfBehvaiour.character)
            {
                if (targetBehaviour.character.isInAttackAction)
                {
                    //测试是否可以打到自己
                    var result = targetBehaviour.character.TestAttack(selfBehvaiour.character, targetBehaviour.curAttackData);
                    if (result == AttackResult.Hitted)
                    {
                        isTriggered = false;
                        return TaskStatus.Success;
                    }
                }
            }
            return TaskStatus.Failure;

        }
    }
}