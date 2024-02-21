
public class G3TDefine
{
    public const int BOARD_SIZE = 3;
}

public struct PiecePos
{
    public int x;
    public int y;

    public void Reset() {
        x = -1;
        y = -1;
    }

    public bool IsPending()
    {
        return x != -1 && y != -1;
    }

    public override string ToString()
    {
        return $"PiecePos(x:{x}, y:{y})";
    }
}

public static class PlaceState
{
    public const int Empty = 0;
    public const int AI = 1;
    public const int PLAYER = 2;
}
