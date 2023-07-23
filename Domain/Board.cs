using OpenAI.ChatGpt;
using OpenAI.ChatGpt.Modules.StructuredResponse;

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

        public Gamer CurrentGamer { get; set; } = Gamer.X;

        private void SwitchGamer()
        {
            CurrentGamer = CurrentGamer == Gamer.X ? Gamer.O : Gamer.X;
        }

        public void CellClickWithTurn(int row, int column)
        {
            var gameResult = GetGameResult(out _);
            if(gameResult == GameResult.Unknown)
            {
                if (Cells[row, column] == CellState.Blank)
                {
                    if (CurrentGamer == Gamer.X)
                    {
                        Cells[row, column] = CellState.X;
                        SwitchGamer();
                    }
                    else
                    {
                        Cells[row, column] = CellState.O;
                        SwitchGamer();
                    }
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
            else if (IsDraw())
            {
                return GameResult.Draw;
            }
            else
            {
                return GameResult.Unknown;
            }
        }

        public bool CheckWin(Gamer gamer, out CellPosition[] winCells)
        {   
            int i, j;
            CellState expectedCell;

            if (gamer == Gamer.X)
                expectedCell = CellState.X;
            else
                expectedCell = CellState.O;

            for(i = 0; i < RowCount; i++)
            {
                var count = 0;
                for(j = 0; j < ColumnCount; j++)
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

            for (j = 0; j < ColumnCount; j++)
            {
                var count = 0;
                for (i = 0; i < RowCount; i++)
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
                        new CellPosition(row: 2, column: j),
                    };
                    return true;
                }
            }

            var countDL = 0;
            for (i = 0, j = 0; i < RowCount && j < ColumnCount; i++, j++)
            {
                if (Cells[i, j] == expectedCell)
                {
                    countDL++;
                }
                if(countDL == 3)
                {
                    winCells = new CellPosition[]
                    {
                        new CellPosition(row: 0, column: 0),
                        new CellPosition(row: 1, column: 1),
                        new CellPosition(row: 2, column: 2),
                    };
                    return true;
                }
            }

            var countDR = 0;
            for (i = 0, j = ColumnCount - 1; i < RowCount && j >= 0; i++, j--)
            {
                if(Cells[i, j] == expectedCell)
                {
                    countDR++;
                }
                if(countDR == 3)
                {
                    winCells = new CellPosition[]
                    {
                        new CellPosition(row: 0, column: 2),
                        new CellPosition(row: 1, column: 1),
                        new CellPosition(row: 2, column: 0),
                    };
                    return true;
                }
            }

            winCells = new CellPosition[0];
            return false;
        }

        public bool IsDraw()
        {
            int CellsBlank = 0;

            for(int j = 0; j < ColumnCount; j++)
            {
                for(int i = 0; i < RowCount; i++)
                {
                    CellsBlank = Cells[i, j] == CellState.Blank ? CellsBlank + 1 : CellsBlank;
                }
            }

            return CellsBlank == 0;
        }

        public void MakeTurnByChatGpt()
        {
            var client = new OpenAiClient("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");
            var board = $"{Cells[0, 0]} {Cells[0, 1]} {Cells[0, 2]}" +
                        $"{Cells[1, 0]} {Cells[1, 1]} {Cells[1, 2]}" +
                        $"{Cells[2, 0]} {Cells[2, 1]} {Cells[2, 2]}";
            var prompt = "You are an AI playing a game of Tic Tac Toe as the 'O' player. Your goal is to place three of your marks in a horizontal, vertical, or diagonal row to win the game. The game starts with the 'X' player making the first move. Players alternate turns, placing their mark in an empty square. You are highly skilled and strive to play the optimal move. Each square on the game board can be identified by its row (from top to bottom) and column (from left to right), both ranging from 0 to 2. The current state of the board is represented by a 3x3 grid. 'X' represents the opponent's pieces, 'O' represents your own pieces, and '_' represents an empty space. Here's the current state of the game:\n"
                         + board
                         + "\nReading from left to right, the columns are 0, 1, and 2, and reading from top to bottom, the rows are 0, 1, and 2.It's your turn to play as the 'O' player. Please specify your move as a row and column number. What is your next move?";

            var dialog = Dialog.StartAsSystem(prompt);
            var nextTurn = client.GetStructuredResponse<CellPosition>(dialog).Result;

            CellClickWithTurn(nextTurn.row, nextTurn.column);
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
