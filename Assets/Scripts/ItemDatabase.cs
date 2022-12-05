using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private Item[] _items;
    public Item[] Items { get { return _items; } }
}
