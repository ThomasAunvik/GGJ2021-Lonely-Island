using LonelyIsland.Characters;
using LonelyIsland.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToWorld : MonoBehaviour
{
    [SerializeField] private int travelToWorldIndex;
    [SerializeField] private float waitTime;
    private float timer;
    private bool startTeleport;

    private void Update()
    {
        if (!GameManager.Instance) return;
        if (!startTeleport) return;

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            startTeleport = false;
            GameManager.Instance.TeleportToWorld(travelToWorldIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<User>())
        {
            timer = waitTime;
            startTeleport = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<User>())
        {
            timer = waitTime;
            startTeleport = false;
        }
    }
}
