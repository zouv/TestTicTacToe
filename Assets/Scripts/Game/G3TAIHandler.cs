/*
 * AI ������������AI���ߣ�
 * 
 * ���ԣ�
 * 1��������ǰ������ÿ�����и��ӵ�Ȩ�أ�
 *      ע�������Ҫ�����N����Ȩ�أ���ͨ������������
 * 2��ͨ����������������ӵ�Ȩ�أ�
 * 3��ȡȨ�ظߣ����߸��ʸߣ��ĸ���Ϊ�������ӵ㣻
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
    private int[,] _weights; // ! ʹ�����Ա��棬�Ա����ظ�����

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
            Thread.Sleep(500); // ��װ AI ��˼�� /Ц
            Evaluation();
        });
    }

    /*
     * ��������
     * 1�������������ø���
     * 2�����ÿ���������ͨ���
     *      a. 3�ո�Ȩ��+1
     *      b. 2�ո�+1ͬ�壬Ȩ��+3
     *      c. 1�ո�+2ͬ�壬Ȩ��+7
     *      d. ���������Ȩ��+0
     *      ע��2b Ȩ��С�� 1c, 2a Ȩ��С�� 1b
     * 3�����������пո��ȡȨ����ߵ�����
     *      ���ڶ�����Ȩ��ʱ��ȡ��һ��
     */
    private void Evaluation()
    {
        var pos = EvaluationPick();
        setResult(pos);
    }

    private PiecePos EvaluationPick()
    {
        // ������
        for (int x = 0; x < _weights.GetLength(0); x++)
            for (int y = 0; y < _weights.GetLength(0); y++)
                _weights[x,y] = -1;

        // �������и���
        var pos = new PiecePos();
        pos.Reset();
        var maxWeight = -1;
        var board = _data.Board;
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(0); y++)
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

    /*
     * ����Ȩ��
     *  ����
     *     ���� x
     *    ��
     *    y
     */
    private int EvaluationCalcWeight(int x, int y)
    {
        // ��
        var weight = 0;
        weight += CalcWeight(x, y, 1, 0);

        // ��
        weight += CalcWeight(x, y, 0, 1);

        // �K
        weight += CalcWeight(x, y, 1, 1);

        // �J
        weight += CalcWeight(x, y, 1, -1);

        //Debug.Log($"EvaluationPick___ {x}, {y}: {weight}");
        return weight;
    }

    private int CalcWeight(int oriX, int oriY, int dirX, int dirY)
    {
        var board = _data.Board;
        var lineLen = board.GetLength(0);
        int comp = 0, empty = 0;
        int x = oriX, y = oriY;
        int count = 0;
        while(count < lineLen)
        {
            count++;
            if (x >= 0 && x < lineLen && y >= 0 && y < lineLen)
            {
                if (board[x, y] == PlaceState.AI) comp++;
                else if (board[x, y] == PlaceState.Empty) empty++;
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
        else if (empty == 2 && comp == 1) return 3;
        else if (empty == 1 && comp == 2) return 7;
        else return 0;
    }
}
