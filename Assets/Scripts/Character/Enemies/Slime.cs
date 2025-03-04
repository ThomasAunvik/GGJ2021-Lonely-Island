using LonelyIsland.Characters;
using LonelyIsland.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LonelyIsland.Characters
{
    public class Slime : Enemy
    {
        [Header("Slime")]
        [SerializeField] float lineOfSight = 10.0f;
        [SerializeField] float personalSpace = 2.0f;
        [SerializeField] float attackSpeed = 3.0f;
        [SerializeField] float attackReach = 5.0f;
        float distance;

        bool isInBattle = false;
        bool canAttack = true;

        [SerializeField] User player;

        [SerializeField] Rigidbody rb;

        [SerializeField] Animator animator;

        [SerializeField] AudioSource audioSource;

        [SerializeField] AudioClip[] atkClips;
        [SerializeField] AudioClip[] moveClips;
        [SerializeField] AudioClip[] deathClips;

        [SerializeField] ParticleSystem particleSys;


        // Update is called once per frame
        void FixedUpdate()
        {
            //check if player is in range
            distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
            //Debug.Log("Distance " + distance + " LoS " + lineOfSight);
            isInBattle = (distance < lineOfSight) ? true : false;
            animator.SetBool("isInBattle", isInBattle);

            Debug.Log("Distance " + distance + " personalSpace " + personalSpace + " attackReach " + attackReach);
            if ((isInBattle) && (distance > personalSpace))
            {
                //raycast to check if there is a clear path to the player

                //Move toward player
                Move(player.transform.position, rb);
                //Update animation
                animator.SetBool("isRunning", true);
            }
            else animator.SetBool("isRunning", false);

            if (canAttack && (attackReach > personalSpace + distance))
            {
                Attack();
            }

        }

        void Move(Vector3 targetPosition, Rigidbody rb)
        {
            Vector3 targetRotation = new Vector3(targetPosition.x, gameObject.transform.position.y, targetPosition.z);
            //Turn Toward Player
            gameObject.transform.LookAt(targetRotation);

            //Move Toward Player
            float x = gameObject.transform.position.x - targetPosition.x;
            float y = gameObject.transform.position.y - targetPosition.y;
            float z = gameObject.transform.position.z - targetPosition.z;

            rb.velocity = new Vector3(-x * MovementSpeed * Time.deltaTime, rb.velocity.y, -z * MovementSpeed * Time.deltaTime);
        }

        protected override void Died()
        {
            int random = Random.Range(0, 4);
            audioSource.clip = deathClips[random];
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.Play();
            animator.Play("Die");
            particleSys.Play();
            GameManager.Instance.Save.Coins += coinLoot;
            Destroy(gameObject, 2f);
        }

        void Attack()
        {
            Debug.Log("ATTACKING");
            //animator.SetBool("isAttacking", true);
            animator.SetTrigger("attack");
            canAttack = false;
            //animator.SetBool("isAttacking", false);
            StartCoroutine("AttackDelay");
        }

        public IEnumerator AttackDelay()
        {
            yield return new WaitForSeconds(attackSpeed);
            animator.ResetTrigger("attack");
            canAttack = true;
        }

        void StepAudioPlay()
        {
            int random = Random.Range(0, 3);
            audioSource.clip = moveClips[random];
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.Play();
        }

        void AtkAudioPlay()
        {
            player.TakeDamage(Random.Range(DamageMin, DamageMax));

            int random = Random.Range(0, 3);
            audioSource.clip = atkClips[random];
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.Play();
        }
    }
}