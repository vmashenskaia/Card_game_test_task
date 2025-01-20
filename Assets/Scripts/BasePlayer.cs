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

        public int Health { get; private set; } = 100;

        public int Energy { get; private set; } = 100;

        public int Defense
        {
            get => _defense;
            private set => Health = value;
        }

        private readonly int _maxHealth = 100;
        private readonly int _maxEnergy = 100;
        private int _defense;

        private void Awake()
        {
            UpdateBars();
        }

        public void UpdateBars()
        {
            float normalizedHealth = Mathf.Clamp01((float) Health / _maxHealth);
            _healthBar.fillAmount = normalizedHealth;

            float normalizedEnergy = Mathf.Clamp01((float) Energy / _maxEnergy);
            _energyBar.fillAmount = normalizedEnergy;
        }

        public void PlayCard(CardData cardsData, Player player)
        {
            if (player == null)
                return;

            var energyDataCount = GetStatCount(cardsData, StatType.Energy);
            if (Energy < energyDataCount) return;

            Energy -= energyDataCount;

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
                Health -= finalDamage;
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
            Health = Mathf.Min(Health + amount, _maxHealth);
        }
    }
}