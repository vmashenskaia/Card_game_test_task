using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BasePlayer : MonoBehaviour
    {
        [SerializeField]
        private Image _healthBar;
        [SerializeField]
        private Image _energyBar;

        private  int _maxHealth = 100;
        private  int _maxEnergy = 100;
        private int _health = 100;
        private int _energy = 100;
        private int _defense = 0;
        
        public int Health
        {
            get => _health;
            private set => _health = value;
        }
        public int Energy
        {
            get => _energy;
            private set => _energy = value;
        }
        public int Defense
        {
            get => _defense;
            private set => _health = value;
        }

        private void Awake()
        {
            UpdateBars();
        }

        public void UpdateBars()
        {
            float normalizedHealth = Mathf.Clamp01((float) _health / _maxHealth);
            _healthBar.fillAmount = normalizedHealth;
            
            float normalizedEnergy = Mathf.Clamp01((float) _energy / _maxEnergy);
            _energyBar.fillAmount = normalizedEnergy;
        }

        public void PlayCard(CardData cardsData, Player player)
        {
            if (player == null)
                return;
            var energyDataCount = GetStatCount(cardsData, StatType.Energy);
            if (_energy < energyDataCount) return;

            _energy -= energyDataCount;

            var attackDataCount = GetStatCount(cardsData, StatType.Attack);
            if (attackDataCount > 0)
                player.GetDamage(attackDataCount);

            var defenseDataCount = GetStatCount(cardsData, StatType.Defense);
            if (defenseDataCount > 0)
                GainDefense(defenseDataCount);

            var healDataCount = GetStatCount(cardsData, StatType.Heal);
            if (healDataCount > 0)
                Heal(healDataCount);
        }

        public void GetDamage(int damage)
        {
            var finalDamage = damage - _defense;
            if (finalDamage >= 0)
                _health -= finalDamage;
        }

        private int GetStatCount(CardData cardsData, StatType statType)
        {
            return cardsData.stats.First(x => x.type == statType).count;
        }

        private void GainDefense(int defense)
        {
            _defense += defense;
        }

        private void Heal(int amount)
        {
            _health = Mathf.Min(_health + amount, _maxHealth);
        }
    }
}