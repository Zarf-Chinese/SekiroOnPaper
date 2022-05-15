using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOP;
namespace BehaviorDesigner.Runtime.Tasks.SOP
{
    [TaskCategory("SOP")]
    [TaskDescription("对目标敌人-立即执行某个动作，若未能执行或被打断，则返回false。")]
    public class EnemyAction : Action
    {
        public GameObject target;
        CharacterBehaviour targetBehaviour;
        CharacterBehaviour selfBehvaiour;
        bool isTriggered = false;
        public string actionKey;
        // Start is called before the first frame update
        public override void OnStart()
        {
            base.OnStart();
            if (!this.target)
            {
                this.target = (GameObject)GlobalVariables.Instance.GetVariable("hero").GetValue();
            }
            targetBehaviour = this.target.GetComponent<CharacterBehaviour>();
            selfBehvaiour = this.GetComponent<CharacterBehaviour>();
            bool result = false;
            switch (actionKey)
            {
                case "block":
                    result = this.selfBehvaiour.Block();
                    break;
                default:
                    result = this.selfBehvaiour.Attack(targetBehaviour, actionKey);
                    break;
            }
            if (!result)
            {
                Debug.Log("行动失败：" + selfBehvaiour.name + " :" + actionKey);
                isTriggered = false;
                return;
            }
            isTriggered = true;
        }

        // Update is called once per frame
        public override TaskStatus OnUpdate()
        {
            base.OnUpdate();
            if (this.selfBehvaiour.curActionKey != actionKey || !isTriggered)
            {
                return TaskStatus.Failure;
            }
            if (this.selfBehvaiour.isRunning)
            {
                return TaskStatus.Running;
            }
            isTriggered = false;
            return TaskStatus.Success;
        }
    }
}