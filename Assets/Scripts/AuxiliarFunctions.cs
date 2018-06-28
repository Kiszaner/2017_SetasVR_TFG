using UnityEngine;

namespace UBUSetasVR
{
    /// <summary>
    /// Class that defines certain utility or auxiliary functions used in various places of the code.
    /// </summary>
    public static class AuxiliarFunctions
    {
        /// <summary>
        /// Converts the first leter of a string to upper case.
        /// </summary>
        /// <param name="s">String to convert it's firt leter</param>
        /// <returns>The provided string with the first leter in upper case</returns>
        public static string FirstUpper(string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Picks a certain number of items without repetition from an array of items.
        /// </summary>
        /// <typeparam name="T">Type of the items to be picked</typeparam>
        /// <param name="arrayOfItems">Array of items to pick from</param>
        /// <param name="numRequired">Number of elements required</param>
        /// <returns>The elements picked from the array</returns>
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

        /// <summary>
        /// Checks if there is any object from a layer mask inside a circle of certain radius around a point.
        /// </summary>
        /// <param name="hitPoint">Point to from</param>
        /// <param name="radius">Radius of the circle to check</param>
        /// <param name="environmentMask">Layer mask of objects to check</param>
        /// <returns>True if the is any object inside the radius, false otherwise</returns>
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

        /// <summary>
        /// Draws a debug flat ring from a certain point based on a min and a max radius.
        /// </summary>
        /// <param name="t">Transform to draw the circle from</param>
        /// <param name="firstRadius">Inner ring radius</param>
        /// <param name="secondRadius">Outer ring radius</param>
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

        /// <summary>
        /// Picks a random position around a certain point.
        /// </summary>
        /// <param name="point">Point to choose from</param>
        /// <param name="maxRadius">Maximum radius to pick</param>
        /// <param name="minRadius">Minimum radius to pick</param>
        /// <returns>The random point chosen</returns>
        public static Vector3 PickRandomPosAroundPoint(Vector3 point, float maxRadius, float minRadius = 0.1f)
        {
            Vector2 flatPos = GetRandomPointBetweenTwoCircles(minRadius, maxRadius);
            Vector3 pos = new Vector3(point.x + flatPos.x, 30f, point.z + flatPos.y);
            return pos;
        }

        /// <summary>
        /// Picks a random point in a terrain around a certain point.
        /// </summary>
        /// <param name="point">Point to choose from</param>
        /// <param name="maxRadius">Maximum radius to pick</param>
        /// <param name="terr">Terrain to pick the point from</param>
        /// <param name="minRadius">Minimum radius to pick</param>
        /// <returns>The random point chosen from the terrain</returns>
        public static Vector3 PickRandomPosAroundPoint(Vector3 point, float maxRadius, Terrain terr, float minRadius = 0.1f)
        {
            Vector3 pos = PickRandomPosAroundPoint(point, maxRadius, minRadius);
            if (terr == null) return pos;
            RaycastHit hit;
            if (RaycastDownToFloor(pos, out hit))
            {
                return hit.point;
            }
            else
            {
                Debug.LogError("NoMushroomHit. Position below terrain. Position will change to 0,0,0. Further errors expected");
            }
            return hit.point;
        }

        /// <summary>
        /// Raycasts downwards from a point to hit a floor.
        /// </summary>
        /// <param name="startingPoint">Point to raycast from</param>
        /// <param name="hit">Element to save the hit information if any</param>
        /// <returns>True if there is a hit with a floor, false otherwise</returns>
        public static bool RaycastDownToFloor(Vector3 startingPoint, out RaycastHit hit)
        {
            return Physics.Raycast(startingPoint, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Floor"));
        }

        /// <summary>
        /// Returns a random point in the space between two concentric circles.
        /// </summary>
        /// <param name="minRadius">Inner circle radius</param>
        /// <param name="maxRadius">Outer circle radius</param>
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
        /// <returns>The Vector3</returns>
        /// <param name="min">Minimum</param>
        /// <param name="max">Max</param>
        public static Vector3 GetRandomVector3Between(Vector3 min, Vector3 max)
        {
            return min + Random.value * (max - min);
        }
    }
}
