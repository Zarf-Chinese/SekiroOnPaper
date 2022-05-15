using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SOP
{

    public class CharacterBehaviour : MonoBehaviour
    {
        public Character character;
        public Animator animator;
        public Reporter reporter;
        public AttackDatas attackDatas;
        public string curActionKey;
        public bool isRunning => this._curRunningTime < this._runningTime;
        private float _runningTime;
        private float _curRunningTime;
        public AttackData curAttackData;
        public CharacterBehaviour curTargetBehaviour;
        public Character target => curTargetBehaviour ? curTargetBehaviour.character : null;
        // Start is called before the first frame update
        void Start()
        {
            character = character ? character : this.GetComponent<Character>();
            animator = animator ? animator : this.GetComponent<Animator>();
        }
        #region 行动函数
        public virtual bool Attack(CharacterBehaviour attacked, string attackKey)
        {

            var canAttack = this.character.inputState.HasFlag(InputState.Attack);
            if (canAttack)
            {
                var attackData = attackDatas.TryFind(attackKey);
                if (attackData == null)
                {
                    Debug.LogError("未找到 attackData" + attackKey);
                    return false;
                }
                var action = character.GetInputAction(attackKey);
                if (action == null)
                {
                    Debug.LogError("未找到 action: " + attackKey);
                    return false;
                }
                //todo: 执行attack
                Debug.Log(this.name + " Attack: " + attackKey);
                animator.SetTrigger("attack");
                curTargetBehaviour = attacked;
                curAttackData = attackData;
                reporter.DoAction(action.content);
                _runningTime = action.duration;
                _curRunningTime = 0;
                curActionKey = attackKey;

                return true;
            }
            return false;
        }
        public virtual bool Block()
        {
            var actionKey = "block";
            var canBlock = this.character.inputState.HasFlag(InputState.Block);
            if (canBlock)
            {
                var block_action = character.GetInputAction(actionKey);
                if (block_action == null)
                {
                    Debug.LogError("未找到 action: " + actionKey);
                    return false;
                }
                Debug.Log(block_action.content);
                animator.SetTrigger(actionKey);
                reporter.DoAction(block_action.content);
                _runningTime = block_action.duration;
                _curRunningTime = 0;
                curActionKey = actionKey;
                return true;
            }
            return false;
        }
        public virtual bool Jump() { return false; }
        public virtual bool Crack() { return false; }
        public virtual bool Dodge() { return false; }
        #endregion
        #region 反应函数
        public virtual void Attacked()
        {
            if (character.isBreakable)
            {
                character.EndAttack();
                var actionKey = "attacked";
                var attacked_action = character.GetInputAction(actionKey);
                if (attacked_action == null)
                {
                    Debug.LogError("未找到 action: " + actionKey);
                    return;
                }
                Debug.Log(attacked_action.content);
                animator.SetTrigger(actionKey);
                reporter.DoAction(attacked_action.content);
                _runningTime = attacked_action.duration;
                _curRunningTime = 0;
                curActionKey = actionKey;
            }
            return;
        }
        public virtual void Blocked()
        {
            if (character.isBreakable)
            {
                character.EndAttack();
                var actionKey = "blocked";
                var blocked_action = character.GetInputAction(actionKey);
                if (blocked_action == null)
                {
                    Debug.LogError("未找到 action: " + actionKey);
                    return;
                }
                Debug.Log(blocked_action.content);
                animator.SetTrigger(actionKey);
                reporter.DoAction(blocked_action.content);
                _runningTime = blocked_action.duration;
                _curRunningTime = 0;
                curActionKey = actionKey;
            }
            return;
        }
        public virtual void BlockSucceed()
        {
            if (character.isBreakable)
            {

                var actionKey = "blockSucceed";
                var blkSuc_action = character.GetInputAction(actionKey);
                if (blkSuc_action == null)
                {
                    Debug.LogError("未找到 action: " + actionKey);
                    return;
                }
                Debug.Log(blkSuc_action.content);
                animator.SetTrigger(actionKey);
                reporter.DoAction(blkSuc_action.content);
                _runningTime = blkSuc_action.duration;
                _curRunningTime = 0;
                curActionKey = actionKey;
            }
            return;
        }
        #endregion
        /// <summary>
        /// 执行attack的数值调整，不做动作等其他逻辑
        /// </summary>
        /// <param name="result"></param>
        public void DoAttack(AttackResult result)
        {
            if (result == AttackResult.Missed || result == AttackResult.NoTarget)
            {
                return;
            }
            //执行attack 逻辑 todo
            Debug.Log("test attack result: " + result.ToString());
            if (result == AttackResult.Hitted)
            {
                if (target.isHurtable)
                {
                    //todo:执行伤害

                }
                curTargetBehaviour.Attacked();
            }
            else if (result == AttackResult.Blocked)
            {
                //todo:执行反冲
                Blocked();
                this.curTargetBehaviour.BlockSucceed();
            }
            else if (result == AttackResult.Cracked)
            {

            }
        }

        // Update is called once per frame
        void Update()
        {
            if (character.isAttacking)
            {
                DoAttack(character.TestAttack(curTargetBehaviour.character, curAttackData));
            }
        }
        void LateUpdate()
        {
            _curRunningTime += Time.deltaTime;
        }
    }
}