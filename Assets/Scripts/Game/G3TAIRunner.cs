/*
 * AI 运行器（负责与外部交互）
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class G3TAIRunner : AIRunner
{
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
    public PiecePos _aiPlace;
    public PiecePos AiPlace
    {
        set { _aiPlace = value; }
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

    public delegate void DeleUIOnPlace(PiecePos pos);
    public DeleUIOnPlace UIOnAIPlace;

    public override void Destory()
    {
        _handler.Destory();
        _handler = null;

        _data.Destory();
        _data = null;
    }

    public override void Update()
    {
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

    public override void SetAITurn()
    {
        _aiState = EAIState.TURN;
    }

    public void SetAIPlace(PiecePos v)
    {
        _aiPlace = v;
    }

    public void DoAIPlace(PiecePos v)
    {
        Debug.Log($"DoAIPlace___ {v}");
        _data.OnAIPlace(v);
        UIOnAIPlace(v);
    }

    public void DoPlayerPlace(PiecePos v)
    {
        _data.OnPlayerPlace(v);
    }
}
