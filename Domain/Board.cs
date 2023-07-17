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
            if (Cells[row, column] == CellState.Blank)
            {
                Cells[row, column] = CurrentTurn;

                SwitchTurn();
            }
        }

        public GameResult GetGameResult()
        {
            if (CheckWin(Gamer.X))
            {
                return GameResult.WonX;
            }
            else if (CheckWin(Gamer.O))
            {
                return GameResult.WonO;
            }
            else
            {
                return GameResult.Draw;
            }
        }

        public bool CheckWin(Gamer gamer)
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
                    return true;
                }
            }

            return false;
        }
    }

    public enum CellState
    {
        Blank,
        X,
        O
    }

    public enum GameResult
    {
        WonX, WonO, Draw
    }

    public enum Gamer
    {
        X, O
    }
}
