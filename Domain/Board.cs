namespace MyApplication.Domain
{
    public class Board
    {
        public int ColumnCount => 3;
        public int RowCount => 3;

        public CellState[,] Cells { get; set; }

        public Board()
		{
            Cells = new CellState[RowCount, ColumnCount];
		}
        
        public CellState CurrentTurn = CellState.X;

        public void SwitchTurn()
        {
            CurrentTurn = CurrentTurn == CellState.X ? CellState.O : CellState.X;
        }

        public void CellClickWithTurn(int row, int column)
        {
            var gameResult = GetGameResult(out _);
            if(gameResult == GameResult.Unknown)
            {
                if (Cells[row, column] == CellState.Blank)
                {
                    Cells[row, column] = CurrentTurn;

                    SwitchTurn();
                }
            }
            
        }

        public GameResult GetGameResult(out CellPosition[] winCells)
        {
            if (CheckWin(Gamer.X, out winCells))
            {
                return GameResult.WonX;
            }
            else if (CheckWin(Gamer.O, out winCells))
            {
                return GameResult.WonO;
            }
            else
            {
                return GameResult.Unknown;
            }
        }

        public bool CheckWin(Gamer gamer, out CellPosition[] winCells)
        {
            CellState expectedCell;

            if (gamer == Gamer.X)
                expectedCell = CellState.X;
            else
                expectedCell = CellState.O;

            for(int i = 0; i < RowCount; i++)
            {
                var count = 0;
                for(int j = 0; j < ColumnCount; j++)
                {
                    if(Cells[i, j] == expectedCell)
                    {
                        count++;
                    }
                }
                if(count == 3)
                {
                    winCells = new CellPosition[]
                    {
                        new CellPosition(row: i, column: 0),
                        new CellPosition(row: i, column: 1),
                        new CellPosition(row: i, column: 2),
                    };
                    return true;
                }
            }

            for (int j = 0; j < ColumnCount; j++)
            {
                var count = 0;
                for (int i = 0; i < RowCount; i++)
                {
                    if (Cells[i, j] == expectedCell)
                    {
                        count++;
                    }
                }
                if (count == 3)
                {
                    winCells = new CellPosition[]
                    {
                        new CellPosition(row: 0, column: j),
                        new CellPosition(row: 1, column: j),
                        new CellPosition(row: 2, column: j)
                    };
                    return true;
                }
            }

            winCells = new CellPosition[0];
            return false;
        }
    }

    public record CellPosition(int row, int column);

    public enum CellState
    {
        Blank,
        X,
        O
    }

    public enum GameResult
    {
        WonX, 
        WonO,
        Unknown,
        Draw
    }

    public enum Gamer
    {
        X, O
    }
}
