using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBUSetasVR
{
    public class LookAtPlayer : MonoBehaviour
    {
        GameObject player;
        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (player == null) return;
            transform.LookAt(2 * transform.position - player.transform.position);
        }
    }
}