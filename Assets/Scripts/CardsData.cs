using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cards", menuName = "Card System/Card")]
public class CardsData : ScriptableObject
{
    public List<CardData> _cards;
}