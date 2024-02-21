using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class UIGame3T : UIBase
{
    private Comp3TPiece[,] _board;
    private G3TAIRunner _ai;

    void Awake()
    {
        _board = new Comp3TPiece[G3TDefine.BOARD_SIZE, G3TDefine.BOARD_SIZE];
        int i = 0, j = 0;
        var tsBoard = this.transform.Find("BG/Board");
        foreach (Transform ts in tsBoard)
        {
            if (j >= _board.GetLength(0))
            {
                j = 0;
                i++;
            }
            if (i >= _board.GetLength(1)) break;
            var comp = ts.GetComponent<Comp3TPiece>();
            comp.Init(new PiecePos { x = i, y = j });
            _board[i, j] = comp;
            j++;
        }
    }

    void OnEnable()
    {
        var args = uiArgs as UIAGame3T;
        GameplayMgr.Instance.StartGame(G3TDefine.BOARD_SIZE, args.PlayerFirstMove, args.Flag);
        _ai = GameplayMgr.Instance.AI as G3TAIRunner;
        _ai.UIOnAIPlace = OnAIPlace;
    }

    void Update()
    {
        GameplayMgr.Instance.Update();
    }

    public void ExitGame()
    {
        GameplayMgr.Instance.StopGame();
    }

    public void OnAIPlace(PiecePos pos)
    {
        _board[pos.x, pos.y].AIPlace();
    }

    public void SetPlayerPlace(PiecePos pos)
    {
        _ai.DoPlayerPlace(pos);
    }

    public bool IsAllowedToPlace()
    {
        return _ai.IsAITurn;
    }

    public void AITurn()
    {
        _ai.SetAITurn();
    }
}
