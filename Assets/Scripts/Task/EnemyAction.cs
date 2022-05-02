using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorDesigner.Runtime.Tasks.SOP
{
    [TaskCategory("SOP")]
    [TaskDescription("敌人-立即执行某个动作，若未能执行或被打断，则返回false。")]
    public class EnemyAction : Action
    {
        public CharacterBehaviour enemy;
        public bool isTriggered = false;
        public string actionKey;
        // Start is called before the first frame update
        public override void OnStart()
        {
            base.OnStart();
            var result = this.enemy.Block();
            if (!result)
            {
                Debug.Log("行动失败：" + enemy.name + " :" + actionKey);
                isTriggered = false;
                return;
            }
            isTriggered = true;
        }

        // Update is called once per frame
        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();
            if (this.enemy.curActionKey == actionKey || !isTriggered)
            {
                return TaskStatus.Failure;
            }
            if (this.enemy.isRunning)
            {
                return TaskStatus.Running;
            }
            isTriggered = false;
            return TaskStatus.Success;
        }
    }
}

