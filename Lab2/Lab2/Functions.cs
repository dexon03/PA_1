namespace Lab2;

public static class FunctionsAndConstants
{
    public static int[,] goalState = new int[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }; 
    public static List<State> FindPath(State state)
    {
        var result = new List<State>();
        while (state.Parent != null)
        {
            result.Add(state);
            state = state.Parent;
        }

        return result;
    }

    public static List<State> GenerateChildren(State state)
    {
        (int x, int y) = state.Board.IndexOfBlank();
        var children = new List<State>();

        var rightState = state.MoveBlankToRight(x, y);
        if (rightState != null)
        {
            rightState.LastMove = "right";
            rightState.Parent = state;
            rightState.SearchDepth++;
            rightState.F = rightState.GetF();
            children.Add(rightState);
        }

        var leftState = state.MoveBlankToLeft(x, y);
        if (leftState != null)
        {
            leftState.LastMove = "left";
            leftState.Parent = state;
            leftState.SearchDepth++;
            leftState.F = leftState.GetF();
            children.Add(leftState);
        }

        var downState = state.MoveBlankToDown(x, y);
        if (downState != null)
        {
            downState.LastMove = "down";
            downState.Parent = state;
            downState.SearchDepth++;
            downState.F = downState.GetF();
            children.Add(downState);
        }

        var upState = state.MoveBlankToUp(x, y);
        if (upState != null)
        {
            upState.LastMove = "up";
            upState.Parent = state;
            upState.SearchDepth++;
            upState.F = upState.GetF();
            children.Add(upState);
        }
        return children;
    }
}