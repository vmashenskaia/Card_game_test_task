using System.Collections.Generic;
using UnityEngine;

public class CardPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private int _startCapacity = 5;
    [SerializeField]
    private Transform _poolContainer;

    private readonly Stack<Card> _inactiveCards = new();
    private readonly List<Card> _activeCards = new();

    private void Awake()
    {
        Card card;
        for (int i = 0; i < _startCapacity; ++i)
        {
            card = Instantiate(_cardPrefab, _poolContainer).GetComponent<Card>();
            card.gameObject.SetActive(false);
            _inactiveCards.Push(card);
        }
    }

    public bool IsInactiveCardsEmpty()
    {
        return _inactiveCards.Count == 0;
    }

    public Card SpawnCard(CardData cardsData)
    {
        var card = _inactiveCards.Pop();

        if (!IsInactiveCardsEmpty())
        {
            card.transform.SetAsLastSibling();
            card.ApplyCard(cardsData);
            card.gameObject.SetActive(true);
        }
        else
        {
            card = Instantiate(_cardPrefab, _poolContainer).GetComponent<Card>();
            card.ApplyCard(cardsData);
        }

        _activeCards.Add(card);

        return card;
    }

    public void DespawnCards(Card card)
    {
        card.gameObject.SetActive(false);
        _inactiveCards.Push(card);
        _activeCards.Remove(card);
    }

    public List<Card> GetActiveCards()
    {
        return _activeCards;
    }
}