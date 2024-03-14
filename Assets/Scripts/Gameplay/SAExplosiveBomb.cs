using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class SAExplosiveBomb : MonoBehaviour
    {
        [SerializeField] private GameObject _explodeParticle;
        [SerializeField] private List<SAPlayerController> _botsToKill;
        [SerializeField] private bool _isOverrideBotsToKill;
        
    
        void Start()
        {
            if (!_isOverrideBotsToKill)
            {
                _botsToKill = new();
                foreach (var player in FindObjectsOfType<SAPlayerController>()) if (player.CompareTag("Emeny")) _botsToKill.Add(player);
            }
            _explodeParticle.SetActive(false);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                Destroy(collision.gameObject);
                KillAffectedBots();
            }
            if (collision.gameObject.CompareTag("Hit")) KillAffectedBots();
        }

        private void KillAffectedBots()
        {
            _explodeParticle.transform.parent = null;
            _explodeParticle.SetActive(true);
            foreach (var bot in _botsToKill) bot.OnBotDeath();
            Destroy(gameObject);
        }
    }
}
