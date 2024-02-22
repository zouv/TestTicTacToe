using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3TAIData
{
    private int[,] _board;
    public int[,] Board
    {
        get { return _board; }
    }

    public void Init(uint size)
    {
        _board = new int[size, size];
    }

    public void Destory()
    {
        _board = null;
    }

    public void Reset()
    {
        for (int y = 0; y < _board.GetLength(0); y++)
            for (int x = 0; x < _board.GetLength(1); x++)
                _board[x, y] = PlaceState.Empty;
    }

    public void OnAIPlace(PiecePos pos)
    {
        _board[pos.x, pos.y] = PlaceState.AI;
    }

    public void OnPlayerPlace(PiecePos pos)
    {
        _board[pos.x, pos.y] = PlaceState.PLAYER;
    }
}
