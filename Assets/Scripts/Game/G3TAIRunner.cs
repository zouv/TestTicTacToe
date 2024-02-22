/*
 * AI 运行器（负责与外部交互）
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class G3TAIRunner : AIRunner
{
    public delegate void DeleUIOnPlace(PiecePos pos);
    public DeleUIOnPlace UIOnAIPlace;
    public delegate void DeleUIOnWin(Game3TResult flag);
    public DeleUIOnWin UIOnWin;

    // ai 状态
    enum EAIState
    {
        IDLE,
        TURN,
        RUNNING,
    }
    private EAIState _aiState;
    public bool IsAITurn
    {
        get { return _aiState != EAIState.IDLE; }
    }

    // ai 落子位置
    private PiecePos _aiPlace;
    public PiecePos AiPlace
    {
        set { _aiPlace = value; }
    }

    private bool _isEnd = true;
    public bool IsEnd
    {
        get { return _isEnd; }
    }
    
    private G3TAIHandler _handler;
    private G3TAIData _data;

    public G3TAIRunner(uint size)
    {
        _data = new G3TAIData();
        _data.Init(size);

        _handler = new G3TAIHandler();
        _handler.Init(_data);
        _handler.setResult = SetAIPlace;

        _aiState = EAIState.IDLE;
        _aiPlace.Reset();
    }

    public override void Destory()
    {
        _handler.Destory();
        _handler = null;

        _data.Destory();
        _data = null;
    }

    public override void Update()
    {
        if (_isEnd) return;

        // 进入 AI 回合
        if (_aiState == EAIState.TURN)
        {
            _aiState = EAIState.RUNNING;
            _handler.Run();
        }

        // 处理 AI 落子
        if (_aiPlace.IsPending())
        {
            DoAIPlace(_aiPlace);
            _aiPlace.Reset();
            _aiState = EAIState.IDLE;
        }
    }

    public void Begin()
    {
        _data.Reset();

        _aiState = EAIState.IDLE;
        _aiPlace.Reset();

        _isEnd = false;
    }

    public void SetAITurn()
    {
        _aiState = EAIState.TURN;
    }

    public void SetAIPlace(PiecePos v)
    {
        _aiPlace = v;
    }

    public void DoAIPlace(PiecePos v)
    {
        _data.OnAIPlace(v);
        UIOnAIPlace(v);
        var result = _handler.CheckWinner(PlaceState.AI);
        if (result >= 0)
        {
            _isEnd = true;
            var winFlag = result == 0 ? Game3TResult.Draw : Game3TResult.Lost;
            UIOnWin(winFlag);
        }
    }

    public void DoPlayerPlace(PiecePos v)
    {
        _data.OnPlayerPlace(v);
        var result = _handler.CheckWinner(PlaceState.PLAYER);
        if (result >= 0)
        {
            _isEnd = true;
            var winFlag = result == 0 ? Game3TResult.Draw : Game3TResult.Win;
            UIOnWin(winFlag);
        }
    }
}
