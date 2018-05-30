using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR
{
    public class DrawCircleGizmo : MonoBehaviour
    {
        public float firstRadius = 1.3f;
        public float secondRadius = 1.7f;

        private void OnDrawGizmosSelected()
        {
            Transform t = GetComponent<Transform>();
            AuxiliarFunctions.DrawCircleGizmo(t, firstRadius, secondRadius);

        }
    }
}