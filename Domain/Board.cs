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

        public CellState Cell_0_0 { get; set; }
        public CellState Cell_0_1 { get; set; }
        public CellState Cell_0_2 { get; set; }
        public CellState Cell_1_0 { get; set; }
        public CellState Cell_1_1 { get; set; }
        public CellState Cell_1_2 { get; set; }
        public CellState Cell_2_0 { get; set; }
        public CellState Cell_2_1 { get; set; }
        public CellState Cell_2_2 { get; set; }
        
        public CellState CurrentTurn = CellState.X;

        public void SwitchTurn()
        {
            CurrentTurn = CurrentTurn == CellState.X ? CellState.O : CellState.X;
        }

        public void CellClick(int row, int column)
        {
            if (Cells[row, column] == CellState.Blank)
            {
                Cells[row, column] = CurrentTurn;

                SwitchTurn();
            }
        }
    }

    public enum CellState
    {
        Blank,
        X,
        O
    }
}
