using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
namespace SOP
{
    /// <summary>
    /// wsad        前后左右
    /// 鼠标左键    选择目标/攻击
    /// 鼠标右键    格挡
    /// shift键     切换攻击方式/垫步/闪避
    /// space键     跳跃
    /// 
    /// </summary>

    public class HeroController : MonoBehaviour
    {
        public CharacterBehaviour heroBehaviour;
        public Character enemy;
        public string attackKey1;
        public string attackKey2;
        SOPControls controls;
        // Update is called once per frame
        void Awake()
        {
            controls = new SOPControls();
            controls.Player.Block.performed += Block;
            controls.Player.Attack.performed += Attack;
        }
        void Block(InputAction.CallbackContext context)
        {
            heroBehaviour.Block();
        }
        void Attack(InputAction.CallbackContext context)
        {
            if (context.interaction is SlowTapInteraction)
            {
                heroBehaviour.Attack(enemy, attackKey2);
            }
            else
            {
                heroBehaviour.Attack(enemy, attackKey1);
            }
        }
        public void OnEnable()
        {
            controls.Enable();
        }

        public void OnDisable()
        {
            controls.Disable();
        }
    }
}