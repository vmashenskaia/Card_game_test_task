using System;
using UnityEngine;

[Serializable]
public class CardData
{
    public string cardName;
    public Stat[] stats = new Stat[4];
    public Sprite icon;
}