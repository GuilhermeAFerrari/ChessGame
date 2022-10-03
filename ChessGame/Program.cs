using ChessGame;
using Board;
using Game;

try
{
	ChessMatch game = new ChessMatch();

	while (!game.GameOver)
	{
		try
		{
            Console.Clear();

            Screen.PrintMatch(game);

            Console.WriteLine();

            Console.Write("Source: ");
            string dataSource = Console.ReadLine();
            Position source = Screen.ReadChessPosition(dataSource).ToBoardPosition();
            game.ValidateSourcePosition(source);

            bool[,] possiblePositions = game.Board.GetPiece(source).PossibleMoves();

            Console.Clear();
            Screen.PrintBoard(game.Board, possiblePositions);

            Console.WriteLine();

            Console.Write("Target: ");
            string dataTarget = Console.ReadLine();
            Position target = Screen.ReadChessPosition(dataTarget).ToBoardPosition();
            game.ValidateTargetPostion(source, target);

            game.MakesMove(source, target);
        }
		catch (BoardException ex)
		{
            Console.WriteLine(ex.Message);
            Console.ReadLine();
		}
    }

    Console.Clear();
    Screen.PrintMatch(game);
}
catch (BoardException ex)
{
	Console.WriteLine(ex.Message);
}