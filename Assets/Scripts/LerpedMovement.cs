using DG.Tweening;
using UnityEngine;

// ReSharper disable InconsistentNaming
namespace Utils.HelperScripts.Utils
{
    public static class CircularLerpedMovement
    {
        public static Tween DOMoveCircularUI(this Transform target, Vector3 to, float duration, bool fromTop)
        {
            Vector3 from = target.position;

            Vector3 center = (from + to) * 0.5f;

            float sign = fromTop ? 1 : -1;

            center -= new Vector3(0, Screen.height / 5f * sign, 0);

            Vector3 fromCircular = from - center;
            Vector3 toCircular = to - center;
            
            Debug.Log("Moving Circular");

            return DOVirtual.Float(0f, 1f, duration,
                time =>
                {
                    target.position = Vector3.Slerp(fromCircular, toCircular, time) + center;
                    
                });
        }
    }
}