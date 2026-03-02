using System;
using UnityEngine;

namespace Surviblewilderness
{
    public class EnemyBasicAttack : MonoBehaviour, IAttacker
    {
        private static readonly int Attack1 = Animator.StringToHash("Attack1");
        private static readonly int Attack5 = Animator.StringToHash("Attack5");
        private const float COLD_DOWN = 2;

        [SerializeField] private int attackDamage = 10;
        [SerializeField] private Animator anim;

        //[SerializeField] private AudioClip attackSound;

        private float currentColdDownCounter;

        private IDamageable target;
        private IDamageable player;

        public event Action OnAttack;



        private void OnTriggerEnter (Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<IDamageable>();
            } else if (other.CompareTag("Prey"))
            {
                target = other.GetComponent<IDamageable>();
            }
        }

        private void OnTriggerExit (Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = null;
            } else if(other.CompareTag("Prey") && other.GetComponent<IDamageable>() == target)
            {
                target = null;
            }
        }

        private void Update ()
        {
            Attack();
        }

        private void UpdateTarget ()
        {
            if (target == null && player == null)
                return;
            if (currentColdDownCounter>0)
                currentColdDownCounter -=Time.deltaTime;
            else
            {
                currentColdDownCounter = COLD_DOWN;
                if(player != null)
                    player.TakeDamage(attackDamage);
                else if (target != null)
                    target.TakeDamage(attackDamage);
                //AudioSource.PlayClipAtPoint(attackSound, transform.position);
            }
        }

        public void Attack()
        {

            if (target == null && player == null)
                return;
            if (currentColdDownCounter > 0)
                currentColdDownCounter -= Time.deltaTime;
            else
            {
                currentColdDownCounter = COLD_DOWN;
                if (player != null)
                {
                    anim.SetTrigger(Attack1);
                    player.TakeDamage(attackDamage);
                } else if (target != null)
                {
                    anim.SetTrigger(Attack5);
                    target.TakeDamage(attackDamage);
                }
                OnAttack?.Invoke(); 
            }
        }
    }
}
