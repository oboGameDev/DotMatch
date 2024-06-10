using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts._4_4_Scripts
{
    [CreateAssetMenu]
    public class GoalInfo : ScriptableObject
    {
        public GoalItem[] Goals;
    }

    [Serializable]
    public class GoalItem
    {
        public int Types;
        public int[] Count;
        }
}