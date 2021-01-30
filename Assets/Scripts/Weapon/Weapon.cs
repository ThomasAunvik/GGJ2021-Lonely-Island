using LonelyIsland.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LonelyIsland.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private Character weilder;

        private void Start()
        {
            weilder = GetComponentInParent<Character>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Weapon hit: " + other.gameObject.name);
            Character character = other.GetComponentInParent<Character>();
            if(character.GetInstanceID() != weilder.GetInstanceID())
            {
                weilder.Attack(character);
            }
        }
    }
}
