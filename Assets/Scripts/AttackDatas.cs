using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AttackData
{
    AttackData() { }
    /// <summary>
    /// 描述
    /// </summary>
    public string name = "普通向前挥刀";
    [Range(0.01f, 1)]
    /// <summary>
    /// 格挡抗性（格挡程度低于该值时，可无视格挡）
    /// </summary>
    public float blockIgnore = 0.01f;
    [Range(0.01f, 1)]
    /// <summary>
    /// 跳跃抗性
    /// </summary>
    public float jumpIgnore = 0.01f;
    [Range(0.01f, 1)]
    /// <summary>
    /// 识破/垫步抗性
    /// </summary>
    public float crackIgnore = 0.01f;
    [Range(0.01f, 1)]
    /// <summary>
    /// 躲避抗性
    /// </summary>
    public float dodgeIgnore = 0.01f;
    [Range(0, 100)]
    /// <summary>
    /// 生效距离系数（基础距离受攻击方的体型、动作和武器长度影响）
    /// </summary>
    public float coeRadius = 1;
    [Range(0, 100)]
    /// <summary>
    /// 生命基础伤害，未进行系数运算
    /// </summary>
    public float hpDamage = 10;
    [Range(0, 100)]
    /// <summary>
    /// 躯干的基础伤害，未进行系数运算
    /// </summary>
    public float spDamage = 10;
    [Range(0, 10)]
    /// <summary>
    /// 被格挡时，攻击方反受的躯干伤害系数
    /// </summary>
    public float coeSpBlocked = 1;
    [Range(0, 10)]
    /// <summary>
    /// 被识破/垫步时，攻击方反受的躯干伤害系数
    /// </summary>
    public float coeSpDodged = 0;
    [Range(0, 100)]
    /// <summary>
    /// 攻击时，攻击方反受的躯干伤害
    /// </summary>
    public float spCost = 5;
}

public enum AttackResult
{
    Missed,
    Blocked,
    Cracked,
    Hitted
}
[CreateAssetMenu(menuName = "Data/AttackDatas")]

public class AttackDatas : ScriptableObject
{
    public List<AttackData> datas;
    public AttackData TryFind(string name)
    {
        return datas.Find((AttackData d) =>
        {
            return d.name == name;
        });
    }
}
