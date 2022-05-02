using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public Character character;
    public Animator animator;
    public Reporter reporter;
    public AttackDatas attackDatas;
    public string curActionKey;
    public bool isRunning => this._curRunningTime >= this._runningTime;
    private float _runningTime;
    private float _curRunningTime;
    // Start is called before the first frame update
    void Start()
    {
        character = character ? character : this.GetComponent<Character>();
        animator = animator ? animator : this.GetComponent<Animator>();
    }
    public virtual bool Attack(Character attacked, string attackKey)
    {

        var canAttack = this.character.inputState.HasFlag(InputState.Attack);
        if (canAttack && !isRunning)
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
            Debug.Log("Attack: " + attackKey);
            animator.SetTrigger("attack");

            return true;
        }
        return false;
    }
    public virtual bool Block()
    {
        var actionKey = "block";
        var canBlock = this.character.inputState.HasFlag(InputState.Block);
        if (canBlock && !isRunning)
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

    // Update is called once per frame
    void Update()
    {
        _curRunningTime += Time.deltaTime;
    }
}
