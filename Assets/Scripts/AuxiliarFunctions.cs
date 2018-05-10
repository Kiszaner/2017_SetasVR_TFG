using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR
{
    public static class AuxiliarFunctions
    {
        public static bool CheckObjectsAround(Vector3 hitPoint, float radius, LayerMask environmentMask)
        {
            Debug.Log("CheckAround");
            Collider[] colliders;
            colliders = Physics.OverlapSphere(hitPoint, radius, environmentMask);
            if (colliders.Length > 0)
            {
                return true;
            }
            return false;
        }

        public static Vector3 PickRandomPosAroundPoint(Vector3 point, float maxRadius, float minRadius = 0.1f)
        {
            //Debug.Log("Picking new random pos");
            Vector2 flatPos = GetRandomPointBetweenTwoCircles(minRadius, maxRadius);
            Vector3 pos = new Vector3(point.x + flatPos.x, point.y, point.z + flatPos.y);
            //Debug.Log("RandomPos: " + pos);
            return pos;
        }

        /// <summary>
        /// Returns a random point in the space between two concentric circles.
        /// </summary>
        /// <param name="minRadius"></param>
        /// <param name="maxRadius"></param>
        /// <returns></returns>
        public static Vector3 GetRandomPointBetweenTwoCircles(float minRadius, float maxRadius)
        {
            //Get a point on a unit circle (radius = 1) by normalizing a random point inside unit circle.
            Vector3 randomUnitPoint = Random.insideUnitCircle.normalized;
            //Now get a random point between the corresponding points on both the circles
            return GetRandomVector3Between(randomUnitPoint * minRadius, randomUnitPoint * maxRadius);
        }

        /// <summary>
        /// Returns a random vector3 between min and max. (Inclusive)
        /// </summary>
        /// <returns>The Vector3.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        public static Vector3 GetRandomVector3Between(Vector3 min, Vector3 max)
        {
            return min + Random.value * (max - min);
        }
    }
}
