using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _cardName;
    [SerializeField]
    private List<StatWidget> _stats;
    [SerializeField]
    private GameObject selectionHighlight;
    [SerializeField]
    private Toggle _toggle;

    public CardData CardData { get; set; }

    private bool _isSeleced;

    private void OnEnable()
    {
        if (_toggle != null)
            _toggle.onValueChanged.AddListener(SelectCardHandler);
    }

    private void OnDisable()
    {
        if (_toggle != null)
            _toggle.onValueChanged.RemoveAllListeners();
    }

    public void ApplyCard(CardData cardsData)
    {
        _cardName.text = cardsData.cardName;
        for (int index = 0; index < _stats.Count; index++)
        {
            var stat = _stats[index];
            stat.ApplyStat(cardsData.stats[index]);
        }

        CardData = cardsData;
    }

    private void SelectCardHandler(bool isSelected)
    {
        _isSeleced = isSelected;
        selectionHighlight.SetActive(isSelected);
    }
}