using System;
using Assets.Scripts;
using UnityEngine;

namespace _4_4_Scripts
{
    public class RotatePointByCenter : MonoBehaviour
    {
        private void Start()
        {
            Vector2 point = new Vector2(2, 0);
            Debug.Log($"rotating {point}");
            Debug.Log($"90  {point.RotatePoint(90)}");
            Debug.Log($"180  {point.RotatePoint(180)}");
            Debug.Log($"270  {point.RotatePoint(270)}");
        }
    }

    public static class PointRotator
    {
        public static Vector2 RotatePoint(this Vector2 pt, float angle, bool clockWise = false)
        {
            if (clockWise) angle *= -1;
            float fi = Mathf.Atan2(pt.y, pt.x) * Mathf.Rad2Deg;
            float fi2 = fi + angle;

            float magnitude = pt.magnitude;
            pt.y = magnitude * Mathf.Sin(fi2 * Mathf.Deg2Rad);
            pt.x = magnitude * Mathf.Cos(fi2 * Mathf.Deg2Rad);

            return pt;
        }

        public static Point RotatePoint(this Point point, float angle, bool clockWise = false)
        {
            if (clockWise) angle *= -1;
            Vector2 pt = new Vector2(point.X, point.Y);
            float fi = Mathf.Atan2(pt.y, pt.x) * Mathf.Rad2Deg;
            float fi2 = fi + angle;

            float magnitude = pt.magnitude;
            pt.y = magnitude * Mathf.Sin(fi2 * Mathf.Deg2Rad);
            pt.x = magnitude * Mathf.Cos(fi2 * Mathf.Deg2Rad);

            return new Point(Mathf.RoundToInt(pt.x), Mathf.RoundToInt(pt.y));
        }
    }
}