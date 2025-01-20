using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Random = UnityEngine.Random;
using Toggle = UnityEngine.UI.Toggle;

public class DeckController : MonoBehaviour
{
    private const int maxHandSize = 5;
    [SerializeField]
    private CardPool _cardPool;
    [SerializeField]
    private List<CardData> _cardDatabase;
    [SerializeField]
    private CardsData _cardsData;
    [SerializeField]
    private Card _playerPlayedCard;
    [SerializeField]
    private Card _enemyOnePlayedCard;
    [SerializeField]
    private Card _enemyTwoPlayedCard;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Enemy _enemyOne;
    [SerializeField]
    private Enemy _enemyTwo;
    [SerializeField]
    private Button _moveButton;
    [SerializeField]
    private Button _finishButton;
    [SerializeField]
    private Button _getCardButton;

    private readonly List<CardData> _drawPile = new();
    private readonly List<CardData> _discardPile = new();
    private readonly List<CardData> _playerHand = new();

    private readonly List<CardData> _enemyFirstHand = new();
    private readonly List<CardData> _enemySecondHand = new();

    private Card _selectedCard;


    private void Awake()
    {
        InitializeDeck();
        ClearPlayZone();
        _moveButton.onClick.AddListener(OnMoveButtonClickedHandler);
        _finishButton.onClick.AddListener(OnFinishButtonClickedHandler);
        _getCardButton.onClick.AddListener(OnGetCardButtonClickedHandler);
    }

    private void Start()
    {
        SetPlayerHand();
        SetEnemysHand();
    }
    private void OnDestroy()
    {
        _moveButton.onClick.RemoveAllListeners();
        _finishButton.onClick.RemoveAllListeners();
        _getCardButton.onClick.RemoveAllListeners();
    }

    public void DiscardCard(Card card)
    {
        _playerHand.Remove(card.CardData);
        _discardPile.Add(card.CardData);
        _cardPool.DespawnCards(card);
    }

    private void OnGetCardButtonClickedHandler()
    {
        if (_playerHand.Count > 5)
            return;

        var card = _drawPile.First();
        _playerHand.Add(card);
        _drawPile.Remove(card);
        _cardPool.SpawnCard(card);
    }

    private void OnFinishButtonClickedHandler()
    {
        ClearPlayZone();
        if (_player.Health <= 0)
        {
            Debug.LogError("You lose!");
            return;
        }

        ;

        _player.PlayCard(_playerPlayedCard.CardData, ChosseEnemy());
        _enemyOne.PlayCard(_enemyOnePlayedCard.CardData, _player);
        _enemyTwo.PlayCard(_enemyTwoPlayedCard.CardData, _player);
        _player.UpdateBars();
        _enemyOne.UpdateBars();
        _enemyTwo.UpdateBars();

        Debug.Log($"your stats - Health = {_player.Health}, Energy  = {_player.Energy}, Defense = {_player.Defense}");
        Debug.Log(
            $"enemy stats: enemyOne Health = {_enemyOne.Health}, Energy  = {_enemyOne.Energy}, Defense = {_enemyOne.Defense}");

        Debug.Log($"enemyTwo Health = {_enemyTwo.Health}, Energy  = {_enemyTwo.Energy}, Defense = {_enemyTwo.Defense}");
    }

    private void OnMoveButtonClickedHandler()
    {
        var activeCards = _cardPool.GetActiveCards();
        foreach (var card in activeCards)
        {
            if (card.GetComponent<Toggle>().isOn)
                _selectedCard = card;
        }

        if (_selectedCard != null)
        {
            _playerPlayedCard.ApplyCard(_selectedCard.CardData);
            _playerPlayedCard.gameObject.SetActive(true);
            DiscardCard(_selectedCard);
        }

        else
        {
            Debug.LogError("No selected card");
        }

        if (_enemyFirstHand.Count != 0)
        {
            var card = _enemyFirstHand.First();
            _enemyOnePlayedCard.ApplyCard(card);
            _enemyFirstHand.Remove(card);
            _enemyOnePlayedCard.gameObject.SetActive(true);
            _enemyTwoPlayedCard.gameObject.SetActive(true);
        }

        if (_enemySecondHand.Count != 0)
        {
            var card = _enemySecondHand.First();
            _enemyTwoPlayedCard.ApplyCard(_enemySecondHand.First());
            _enemySecondHand.Remove(card);
        }
    }

    private Enemy ChosseEnemy()
    {
        if (_enemyOne.Health > 0)
            return _enemyOne;

        if (_enemyTwo.Health > 0)
            return _enemyTwo;

        Debug.LogError("You win!");
        return null;
    }

    private void ClearPlayZone()
    {
        _playerPlayedCard.gameObject.SetActive(false);
        _enemyOnePlayedCard.gameObject.SetActive(false);
        _enemyTwoPlayedCard.gameObject.SetActive(false);
    }

    private void InitializeDeck()
    {
        foreach (var card in _cardsData._cards)
        {
            _cardDatabase.Add(card);
        }

        for (int i = 0; i < 50; ++i)
        {
            _drawPile.Add(_cardDatabase[Random.Range(0, _cardDatabase.Count - 1)]);
        }
    }

    private void SetPlayerHand()
    {
        GetCards(maxHandSize, _playerHand);
        foreach (var cardData in _playerHand)
            _cardPool.SpawnCard(cardData);
    }

    private void SetEnemysHand()
    {
        GetCards(maxHandSize, _enemyFirstHand);
        GetCards(maxHandSize, _enemySecondHand);
    }

    private void GetCards(int count, List<CardData> hand)
    {
        for (int i = 0; i < count; i++)
        {
            if (_drawPile.Count == 0)
            {
                Reshuffle();
            }

            if (_drawPile.Count > 0)
            {
                CardData cards = _drawPile[0];
                _drawPile.RemoveAt(0);
                hand.Add(cards);
            }
        }
    }

    private void Reshuffle()
    {
        _drawPile.AddRange(_discardPile);
        _discardPile.Clear();
        Shuffle(_drawPile);
    }

    private void Shuffle(List<CardData> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            CardData temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
}