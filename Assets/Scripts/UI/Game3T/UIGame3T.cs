using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIGame3T : UIBase
{
    private Text _txtTitle;
    private Text _txtScore;
    private Text _txtWin;
    private Transform _tsPanelMask;
    private Transform _tsWinPanel;

    private Comp3TPiece[,] _board;
    private G3TAIRunner _ai;

    private int _winCount;
    private int _lostCount;
    private int _drawCount;
    private bool _playerFirst;

    void Awake()
    {
        // 部件
        _txtTitle = this.transform.Find("BG/TxtTitle").GetComponent<Text>();
        _txtScore = this.transform.Find("BG/TxtScore").GetComponent<Text>();
        _tsPanelMask = this.transform.Find("Panels/Mask");
        _tsWinPanel = this.transform.Find("Panels/WinPanel");
        _txtWin = this.transform.Find("Panels/WinPanel/Text").GetComponent<Text>();

        // 棋盘
        _board = new Comp3TPiece[G3TDefine.BOARD_SIZE, G3TDefine.BOARD_SIZE];
        int i = 0, j = 0;
        var tsBoard = this.transform.Find("Board");
        foreach (Transform ts in tsBoard)
        {
            if (j >= _board.GetLength(1))
            {
                j = 0;
                i++;
            }
            if (i >= _board.GetLength(0)) break;
            var comp = ts.GetComponent<Comp3TPiece>();
            comp.Init(new PiecePos { x = i, y = j });
            _board[i, j] = comp;
            j++;
        }
    }

    void OnEnable()
    {
        _ai = new G3TAIRunner(G3TDefine.BOARD_SIZE);
        _ai.UIOnAIPlace = OnAIPlace;
        _ai.UIOnWin = OnWin;

        var args = uiArgs as UIAGame3T;
        _winCount = 0;
        _lostCount = 0;
        _drawCount = 0;
        _playerFirst = args.playerFirst;
        InitGame(args.flag, _playerFirst);
    }

    void Update()
    {
        if (_ai != null) _ai.Update();
    }

    // 初始化
    private void InitGame(Game3TFlag flag, bool playerFirst)
    {
        InitPanel(flag);
        _ai.Begin();
        if (!playerFirst) _ai.SetAITurn();
    }
    
    private void InitPanel(Game3TFlag flag)
    {
        _txtTitle.text = flag == Game3TFlag.Classics ? "经典模式" : "死斗模式";
        if (flag == Game3TFlag.Classics)
        {
            _txtScore.text = $"胜利:{_winCount} 平局:{_drawCount} 失败:{_lostCount}";
        }
        else
        {
            var tips = _playerFirst ? "玩家先手" : "AI先手";
            _txtScore.text = $"局数:{_drawCount}\n{tips}";
        }
    }

    private void CleanBoard()
    {
        for (int y = 0; y < _board.GetLength(0); y++)
            for (int x = 0; x < _board.GetLength(1); x++)
                _board[x, y].ResetPlace();
    }

    public void ExitGame()
    {
        _ai.Destory();
        _ai = null;
        UIManager.Instance.ChangeUI(UIManager.UINames.Main);
    }

    public void OnAIPlace(PiecePos pos)
    {
        _board[pos.x, pos.y].AIPlace();
    }

    public void OnWin(Game3TResult flag)
    {
        StartCoroutine(DealyCorouShowWinPane(flag));
    }

    IEnumerator DealyCorouShowWinPane(Game3TResult flag)
    {
        yield return new WaitForSeconds(0.5f);

        // 累加次数
        if (flag == Game3TResult.Win) _winCount++;
        else if (flag == Game3TResult.Lost) _lostCount++;
        else _drawCount++;

        // 游戏结束
        var args = uiArgs as UIAGame3T;
        if (args.flag == Game3TFlag.FightToDeath)
        {
            if (flag != Game3TResult.Draw)
            {
                _drawCount = 0;
                _playerFirst = args.playerFirst;
            }
            else
            {
                _playerFirst = !_playerFirst;
            }
        }

        _txtWin.text = flag == Game3TResult.Draw ? "IT'S DRAW" : flag == Game3TResult.Win ? "YOU WIN!" : "YOU LOST!";
        ShowWinPane(true);
    }

    public void ShowWinPane(bool value)
    {
        _tsWinPanel.gameObject.SetActive(value);
        _tsPanelMask.gameObject.SetActive(value);

        // 重置棋盘
        if (value == false)
        {
            var args = uiArgs as UIAGame3T;
            bool firstMove = args.flag == Game3TFlag.Classics ? args.playerFirst : _playerFirst;
            InitGame(args.flag, firstMove);
            CleanBoard();
        }
    }

    public bool IsAllowedToPlace()
    {
        return _ai.IsAITurn || _ai.IsEnd;
    }

    public void SetPlayerPlace(PiecePos pos)
    {
        _ai.DoPlayerPlace(pos);
    }

    public void AITurn()
    {
        _ai.SetAITurn();
    }
}
