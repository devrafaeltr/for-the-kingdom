using System;
using UnityEngine;

namespace FTKingdom
{
    [Serializable]
    public class HPModifierData
    {
        public int Value { get; private set; }
        public DamageType Type { get; private set; }
        public DamageModifier Modifier { get; private set; }
        public CharacterType AttackerType { get; private set; }
        public Transform AttackerTransform { get; private set; }
        public Transform TargetTransform { get; private set; }

        public HPModifierData(int value, DamageType type, DamageModifier damageModifier, CharacterType attackerType,
        Transform attacker, Transform target)
        {
            Value = value;
            Type = type;
            Modifier = damageModifier;
            AttackerType = attackerType;
            AttackerTransform = attacker;
            TargetTransform = target;
        }

        public HPModifierData(int value, DamageType type, CharacterType attackerType,
        Transform target)
        {
            Value = value;
            Type = type;
            AttackerType = attackerType;
            TargetTransform = target;
        }


        public void SetTarget(Transform target)
        {
            TargetTransform = target;
        }
    }
}