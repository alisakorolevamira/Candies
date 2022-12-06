using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Board _board;
    [SerializeField] private Image _icon;

    private int _x;
    private int _y;

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

    private Tile _left => _x > 0 ? _board.Tiles[_x - 1, _y] : null;
    private Tile _top => _y > 0 ? _board.Tiles[_x, _y - 1] : null;
    private Tile _right => _x < _board.Width - 1 ? _board.Tiles[_x + 1, _y] : null;
    private Tile _bottom => _y < _board.Height - 1 ? _board.Tiles[_x, _y + 1] : null;

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

    public void SetPlace(int x, int y)
    {
        _x = x;
        _y = y;
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
