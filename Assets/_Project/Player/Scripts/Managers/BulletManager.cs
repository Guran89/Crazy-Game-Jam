using _Project.Data;
using _Project.Enemies.Scripts.Components;
using _Project.Particle_Effects;
using _Project.Player.Data;
using _Project.Player.Scripts.Components;
using UnityEngine;

namespace _Project.Player.Scripts.Managers
{
   public class BulletManager : MonoBehaviour
   {
       [SerializeField] private PlayerData _playerData;
       [SerializeField] private ElementData.Element _enemyElement;
       [SerializeField] private ElementData.Element _playerElement;
       [SerializeField] private EnemyHealthComponent _enemyHealth;
       [SerializeField] private ParticleSystem _bulletHitParticle;

       private float _damageToDeal;
       private const float BULLET_LIFETIME = 5f;

       private void Start()
       {
           InitializeBullet();
       }

       private void InitializeBullet()
       {
           Invoke(nameof(DestroyBullet), BULLET_LIFETIME);
           
           var playerObj = GameObject.Find("Player");
           if (playerObj != null)
           {
               var elementComponent = playerObj.GetComponent<ElementComponent>();
               if (elementComponent != null)
               {
                   _playerElement = elementComponent.CurrentElement;
               }
           }
           
           _damageToDeal = _playerData != null ? _playerData.AttackDamage : 0f;
       }

       private void DestroyBullet()
       {
           Destroy(gameObject);
       }

       private void OnCollisionEnter(Collision other)
       {
           if (!other.gameObject.CompareTag("Enemy")) return;
           
           HandleEnemyCollision(other.gameObject);
       }

       private void HandleEnemyCollision(GameObject enemy)
       {
           ProcessEnemyDamage(enemy);  // Get enemy element first
           SpawnHitParticle();         // Then spawn particle with correct color
           DestroyBullet();
       }

       private void SpawnHitParticle()
       {
           if (_bulletHitParticle == null) return;
           
           var bulletHit = Instantiate(_bulletHitParticle, transform.position, transform.rotation);
           ParticleColorChanger.SetParticleElementColor(bulletHit, _enemyElement);
           bulletHit.Play();
       }

       private void ProcessEnemyDamage(GameObject enemy)
       {
           _enemyHealth = enemy.GetComponent<EnemyHealthComponent>();
           var enemyElementComponent = enemy.GetComponent<EnemyElementComponent>();
           
           if (enemyElementComponent != null)
           {
               _enemyElement = enemyElementComponent.CurrentElement;
           }

           DealDamage(_damageToDeal);
           ValidateReferences();
       }

       private void ValidateReferences()
       {
           if (_enemyHealth == null)
               Debug.LogError($"EnemyHealth not assigned to {gameObject.name}!");
           if (_playerData == null)
               Debug.LogError($"PlayerData not assigned to {gameObject.name}!");
           if (_bulletHitParticle == null)
               Debug.LogError($"BulletHitParticle not assigned to {gameObject.name}!");
       }

       private void DealDamage(float damage)
       {
           if (_enemyHealth == null) return;
           
           float damageMultiplier = GetDamageMultiplier(_playerElement, _enemyElement);
           float finalDamage = _damageToDeal * damageMultiplier;
           
           _enemyHealth.TakeDamage(finalDamage);
           
           LogDamageDetails(finalDamage);
       }

       private void LogDamageDetails(float finalDamage)
       {
           Debug.Log($"Player attacked for {finalDamage} damage! (Base: {_damageToDeal})");
           Debug.Log($"Enemy health: {_enemyHealth.CurrentHealth} / {_enemyHealth.MaxHealth}");
       }

       private static float GetDamageMultiplier(ElementData.Element attackerElement,
           ElementData.Element defenderElement)
       {
           // If attacker has advantage, double the damage
           if (attackerElement == ElementData.Element.Fire && defenderElement == ElementData.Element.Wind) return 2f;
           if (attackerElement == ElementData.Element.Wind && defenderElement == ElementData.Element.Earth) return 2f;
           if (attackerElement == ElementData.Element.Earth && defenderElement == ElementData.Element.Water) return 2f;
           if (attackerElement == ElementData.Element.Water && defenderElement == ElementData.Element.Fire) return 2f;

           // If defender has advantage, reduce damage
           if (defenderElement == ElementData.Element.Fire && attackerElement == ElementData.Element.Wind) return 0.5f;
           if (defenderElement == ElementData.Element.Wind && attackerElement == ElementData.Element.Earth) return 0.5f;
           if (defenderElement == ElementData.Element.Earth && attackerElement == ElementData.Element.Water) return 0.5f;
           if (defenderElement == ElementData.Element.Water && attackerElement == ElementData.Element.Fire) return 0.5f;

           if (defenderElement == attackerElement) return -1.5f;

           return 1f; // Normal damage for all other combinations
       }
   }
}