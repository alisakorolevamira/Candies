using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Board _board;
    [SerializeField] private Image _icon;

    public int X;
    public int Y;
    public Tile [] Neighbours => new Tile[] { _left, _right, _top, _bottom };
    public Image Icon { get { return _icon; } }
    public Item Item
    {
        get { return _item; }
        set
        {
            if (_item == value) return;
            _item = value;
        }
    }

    private Item _item;

    private Tile _left => X > 0 ? _board.Tiles[X - 1, Y] : null;
    private Tile _top => Y > 0 ? _board.Tiles[X, Y - 1] : null;
    private Tile _right => X < _board.Width - 1 ? _board.Tiles[X + 1, Y] : null;
    private Tile _bottom => Y < _board.Height - 1 ? _board.Tiles[X, Y + 1] : null;

    private void OnEnable()
    {
        _button.onClick.AddListener(ChooseTile);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ChooseTile);
    }

    public void ChangeTile(Item item)
    {
        _item = item;
        _icon.sprite = item.Icon;
    }

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        var result = new List<Tile> { this, };

        if (exclude == null)
        {
            exclude = new List<Tile> { this, };
        }

        else
        {
            exclude.Add(this);
        }

        foreach (var neighbour in Neighbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.Item != _item) continue;
            
            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }

        return result;
    }

    private void ChooseTile()
    {
        _board.SelectTile(this);
    }
}
