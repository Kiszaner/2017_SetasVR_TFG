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
            //Debug.Log("Picking new random pos");
            Vector2 flatPos = GetRandomPointBetweenTwoCircles(minRadius, maxRadius);
            Vector3 pos = new Vector3(point.x + flatPos.x, point.y, point.z + flatPos.y);
            //Debug.Log("RandomPos: " + pos);
            return pos;
        }

        public static Vector3 PickRandomPosAroundPoint(Vector3 point, float maxRadius, Terrain terr, float minRadius = 0.1f)
        {
            Vector3 pos = PickRandomPosAroundPoint(point, maxRadius, minRadius);
            if (terr == null) return pos;
            Debug.Log("Pos: " + pos);
            RaycastHit hit;
            if(Physics.Raycast(pos, Vector3.down, out hit, LayerMask.NameToLayer("Floor")))
            {
                Debug.Log("Hit");
                Debug.Log("TerrPos: " + hit);
                return hit.point;
            }
            float posy = terr.SampleHeight(new Vector3(pos.x, 0, pos.z));
            //pos = ConvertWordCor2TerrCor(terr, pos);
            Debug.Log("TerrPos2: " + pos);
            return pos;
        }

        private static Vector3 ConvertWordCor2TerrCor(Terrain ter, Vector3 wordCor)
        {
            Vector3 vecRet = new Vector3();
            //Terrain ter = Terrain.activeTerrain;
            Vector3 terPosition = ter.transform.position;
            vecRet.x = ((wordCor.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
            vecRet.z = ((wordCor.y - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
            return vecRet;
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
