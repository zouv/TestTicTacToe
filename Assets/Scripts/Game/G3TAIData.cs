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

    public void OnAIPlace(PiecePos pos)
    {
        _board[pos.x, pos.y] = PlaceState.AI;
    }

    public void OnPlayerPlace(PiecePos pos)
    {
        _board[pos.x, pos.y] = PlaceState.PLAYER;
    }
}
