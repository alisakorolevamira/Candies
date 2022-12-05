using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 11)]

public class Item : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _value;

    public Sprite Icon { get { return _icon; } }
    public int Value { get { return _value; } }
}
