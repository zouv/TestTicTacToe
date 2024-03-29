/*
 * AI 处理器（负责AI决策）
 * 
 * 策略：
 * 1、在落子前，评估每个空闲格子的权重；
 *      注：如果需要计入后N步的权重，需通过博弈树推算
 * 2、通过评估函数计算格子的权重；
 * 3、取权重高（连线概率高）的格子为最终落子点；
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class G3TAIHandler
{
    public delegate void DeleResultCB(PiecePos a1);
    public DeleResultCB setResult;

    private G3TAIData _data;
    private int[,] _weights; // ! 使用属性保存，以避免重复构建

    public void Init(G3TAIData data)
    {
        _data = data;
        _weights = new int[data.Board.GetLength(0), data.Board.GetLength(1)];
    }

    public void Destory()
    {
        _weights = null;
    }

    public void Run()
    {
        Task task = Task.Run(() =>
        {
            Thread.Sleep(500); // 假装 AI 在思考 /笑
            Evaluation();
        });
    }

    /*
     * 评估函数
     * 1、遍历所有闲置格子
     * 2、检查每个方向的连通情况
     *      a. 3空格，权重+1
     *      b. 2空格+1同棋，权重+3
     *      c. 1空格+2异棋，全中+7
     *      d. 1空格+2同棋，权重+15
     *      e. 其他情况，权重+0
     *      注：2b 权重小于 1c, 2a 权重小于 1b
     * 3、遍历玩所有空格后，取权重最高的落子
     *      存在多个最高权重时，取第一个
     */
    private void Evaluation()
    {
        var pos = EvaluationPick();
        setResult(pos);
    }

    private PiecePos EvaluationPick()
    {
        // 先重置
        for (int y = 0; y < _weights.GetLength(0); y++)
            for (int x = 0; x < _weights.GetLength(1); x++)
                _weights[x,y] = -1;

        // 遍历空闲格子
        var pos = new PiecePos();
        pos.Reset();
        var maxWeight = -1;
        var board = _data.Board;
        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(1); x++)
            {
                if (board[x, y] == PlaceState.Empty)
                {
                    var weight = EvaluationCalcWeight(x, y);
                    _weights[x, y] = weight;
                    if (weight > maxWeight)
                    {
                        maxWeight = weight;
                        pos.x = x;
                        pos.y = y;
                    }
                }
                    
            }
        }

        return pos;
    }

    public int CheckWinner(int camp)
    {
        var count = 0;
        var board = _data.Board;
        for (int y = 0; y < board.GetLength(0); y++)
        {
            for (int x = 0; x < board.GetLength(1); x++)
            {
                if (board[x, y] != PlaceState.Empty)
                {
                    count++;
                    var weight = EvaluationCalcWeight(x, y, camp);
                    if (weight >= 10000) return 1;
                }
            }
        }
        if (count == board.GetLength(0) * board.GetLength(1)) return 0;
        return -1;
    }

    /*
     * 计算权重
     *  轴向：
     *     —→ x
     *    ↓
     *    y
     */
    private int EvaluationCalcWeight(int x, int y, int camp = PlaceState.AI)
    {
        // →
        var weight = 0;
        weight += CalcWeight(x, y, 1, 0, camp);

        // ↓
        weight += CalcWeight(x, y, 0, 1, camp);

        // ↘
        weight += CalcWeight(x, y, 1, 1, camp);

        // ↗
        weight += CalcWeight(x, y, 1, -1, camp);

        return weight;
    }

    private int CalcWeight(int oriX, int oriY, int dirX, int dirY, int camp)
    {
        var board = _data.Board;
        var lineLen = board.GetLength(0);
        int same = 0, diff = 0, empty = 0;
        int sameCamp = PlaceState.AI == camp ? camp : PlaceState.PLAYER;
        int diffCamp = PlaceState.AI != camp ? camp : PlaceState.PLAYER;
        int x = oriX, y = oriY;
        int count = 0;
        while(count < lineLen)
        {
            count++;
            if (x >= 0 && x < lineLen && y >= 0 && y < lineLen)
            {
                if (board[x, y] == sameCamp) same++;
                else if (board[x, y] == diffCamp) diff++;
                else empty++;
            }

            x += dirX;
            y += dirY;
            if (x >= lineLen && y >= lineLen)
            {
                x = 0;
                y = 0;
            }
            else if (x >= lineLen && y < 0)
            {
                x = 0;
                y = lineLen - 1;
            }
            else if (x >= lineLen && dirY == 0)
            {
                x = 0;
            }
            else if (y >= lineLen && dirX == 0)
            {
                y = 0;
            }
        }

        if (empty == 3) return 1;
        else if (empty == 2 && diff == 1) return 2;
        else if (empty == 2 && same == 1) return 3;
        else if (empty == 1 && diff == 2) return 7;
        else if (empty == 1 && same == 2) return 15;
        else if (same == 3) return 10000;
        else return 0;
    }
}
