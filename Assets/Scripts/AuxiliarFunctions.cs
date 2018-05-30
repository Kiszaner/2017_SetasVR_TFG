using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR
{
    public static class AuxiliarFunctions
    {

        public static string FirstUpper(string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public static T[] RandomPickWithoutRepetition<T>(T[] arrayOfItems, int numRequired)
        {
            T[] result = new T[numRequired];

            int numToChoose = numRequired;

            for (int numLeft = arrayOfItems.Length; numLeft > 0; numLeft--)
            {

                float prob = (float)numToChoose / (float)numLeft;

                if (Random.value <= prob)
                {
                    numToChoose--;
                    result[numToChoose] = arrayOfItems[numLeft - 1];

                    if (numToChoose == 0)
                    {
                        break;
                    }
                }
            }
            return result;
        }

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

        public static void DrawCircleGizmo(Transform t, float firstRadius, float secondRadius = 0f)
        {
            Gizmos.color = Color.white;
            float theta = 0;
            float x = firstRadius * Mathf.Cos(theta);
            float y = firstRadius * Mathf.Sin(theta);
            Vector3 pos = t.position + new Vector3(x, 0, y);
            Vector3 newPos = pos;
            Vector3 lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x = firstRadius * Mathf.Cos(theta);
                y = firstRadius * Mathf.Sin(theta);
                newPos = t.position + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);
            if (secondRadius != 0f) DrawCircleGizmo(t, secondRadius);
        }

        public static Vector3 PickRandomPosAroundPoint(Vector3 point, float maxRadius, float minRadius = 0.1f)
        {
            Vector2 flatPos = GetRandomPointBetweenTwoCircles(minRadius, maxRadius);
            Vector3 pos = new Vector3(point.x + flatPos.x, point.y, point.z + flatPos.y);
            return pos;
        }

        public static Vector3 PickRandomPosAroundPoint(Vector3 point, float maxRadius, Terrain terr, float minRadius = 0.1f)
        {
            Vector3 pos = PickRandomPosAroundPoint(point, maxRadius, minRadius);
            if (terr == null) return pos;
            Debug.Log("Pos: " + pos);
            RaycastHit hit;
            if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")) ||
                Physics.Raycast(pos, Vector3.up, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            {
                Debug.Log("Hit");
                Debug.Log("TerrPos: " + hit);
                return hit.point;
            }
            return hit.point;
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
