using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Row[] _rows;
    [SerializeField] private ItemDatabase _database;
    [SerializeField] private ScoreCounter _scoreCounter;
    [SerializeField] private AudioClip _collectedSound;
    [SerializeField] private AudioSource _audioSource;

    public Tile[,] Tiles { get; private set; }

    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);

    private Tile _firstSelectedTile;
    private Tile _secondSelectedTile;
    private int _numberOfTiles = 5;

    private void Start()
    {
        Tiles = new Tile[_numberOfTiles, _rows.Length];

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var tile = _rows[i].Tiles[j];
                tile.X = j;
                tile.Y = i;

                var newItem = _database.Items[Random.Range(0, _database.Items.Length - 1)];

                tile.ChangeTile(newItem);

                Tiles[j, i] = tile;
            }
        }
    }

    public void RestartBoard()
    {
        Start();
    }

    public void SelectTile(Tile tile)
    {
        if (_firstSelectedTile == null)
        {
            _firstSelectedTile = tile;
        }

        else if (_secondSelectedTile == null)
        {
            _secondSelectedTile = tile;
        }

        if (_firstSelectedTile != null && _secondSelectedTile != null)
        {
            if (_firstSelectedTile.Neighbours.Contains(_secondSelectedTile))
            {
                SwapTiles(_firstSelectedTile, _secondSelectedTile);
                _firstSelectedTile = null;
                _secondSelectedTile = null;

                if (CanPop())
                {
                    Pop();
                }
            }

            else
            {
                _firstSelectedTile = null;
                _secondSelectedTile = null;
            }
        }
    }

    private void SwapTiles(Tile firstTile, Tile secondTile)
    {
        var temporaryItem = firstTile.Item;

        firstTile.ChangeTile(secondTile.Item);
        secondTile.ChangeTile(temporaryItem);
    }

    private bool CanPop()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (Tiles[j, i].GetConnectedTiles().Count() >= 3)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private async void Pop()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var tile = Tiles[j, i];
                var connectedTiles = tile.GetConnectedTiles();

                if (connectedTiles.Count < 3) continue;

                var deflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    _scoreCounter.AddScore(connectedTile.Item.Value);
                    deflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.zero, 1));
                }

                _audioSource.PlayOneShot(_collectedSound);

                await deflateSequence.Play()
                    .AsyncWaitForCompletion();

                var inflateSequence = DOTween.Sequence();

                foreach (var connectedTile in connectedTiles)
                {
                    var newItem = _database.Items[Random.Range(0, _database.Items.Length - 1)];
                    connectedTile.ChangeTile(newItem);

                    inflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.one, 1));
                }

                await inflateSequence.Play()
                    .AsyncWaitForCompletion();

                i = 0;
                j = 0;
            }
        }
    }
}
