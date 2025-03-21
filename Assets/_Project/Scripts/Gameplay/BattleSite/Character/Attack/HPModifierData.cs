using System;
using UnityEngine;

namespace FTKingdom
{
    [Serializable]
    public class HPModifierData
    {
        public int Value { get; private set; }
        public DamageTrigger Type { get; private set; }
        public CharacterType AttackerType { get; private set; }
        public Transform AttackerTransform { get; private set; }
        public Transform TargetTransform { get; private set; }

        public HPModifierData(int value, DamageTrigger type, CharacterType attackerType, Transform attacker,
        Transform target)
        {
            Value = value;
            Type = type;
            AttackerType = attackerType;
            AttackerTransform = attacker;
            TargetTransform = target;
        }

        public HPModifierData(int value, DamageTrigger type, CharacterType attackerType, Transform target)
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

        public void SetValue(int value)
        {
            Value = value;
        }
    }
}