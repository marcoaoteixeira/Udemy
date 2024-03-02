import { useState } from "react";
import GameBoard from "./components/GameBoard";
import Player from "./components/Player";
import Log from "./components/Log";
import { WINNING_COMBINATIONS } from "./winning-combinations";
import GameOver from "./components/GameOver";

const getActivePlayer = (gameTurns) =>
  gameTurns.length > 0 && gameTurns[0].player === "X" ? "0" : "X";

const initialPlayers = {
  X: "Player 1",
  O: "Player 2",
};

const initialGameBoard = [
  [null, null, null],
  [null, null, null],
  [null, null, null],
];

function App() {
  const [gameTurns, setGameTurns] = useState([]);
  const [players, setPlayers] = useState(initialPlayers);
  const activePlayer = getActivePlayer(gameTurns);

  let gameBoard = [...initialGameBoard.map((inner) => [...inner])];

  for (const turn of gameTurns) {
    const { square, player } = turn;
    const { rowIndex, columnIndex } = square;

    gameBoard[rowIndex][columnIndex] = player;
  }

  let winner;
  for (const combination of WINNING_COMBINATIONS) {
    const firstSquareSymbol =
      gameBoard[combination[0].row][combination[0].column];
    const secondSquareSymbol =
      gameBoard[combination[1].row][combination[1].column];
    const thirdSquareSymbol =
      gameBoard[combination[2].row][combination[2].column];

    if (
      firstSquareSymbol &&
      firstSquareSymbol === secondSquareSymbol &&
      firstSquareSymbol === thirdSquareSymbol
    ) {
      winner = players[firstSquareSymbol];
    }
  }

  const hasDraw = gameTurns.length === 9 && !winner;

  const handleSelectSquare = (rowIndex, columnIndex) => {
    setGameTurns((previousTurns) => {
      const currentPlayer = getActivePlayer(previousTurns);

      const updatedTurns = [
        {
          square: { rowIndex, columnIndex },
          player: currentPlayer,
        },
        ...previousTurns,
      ];

      return updatedTurns;
    });
  };

  const handleRematch = () => {
    setGameTurns(() => []);
  };

  const handlePlayerNameChange = (symbol, newName) => {
    setPlayers((previousPlayer) => {
      return {
        ...previousPlayer,
        [symbol]: newName,
      };
    });
  };

  return (
    <main>
      <div id="game-container">
        <ol id="players" className="highlight-player">
          <Player
            name={initialPlayers.X}
            symbol="X"
            isActive={activePlayer === "X"}
            onChangeName={handlePlayerNameChange}
          />
          <Player
            name={initialPlayers.O}
            symbol="O"
            isActive={activePlayer === "0"}
            onChangeName={handlePlayerNameChange}
          />
        </ol>
        {(winner || hasDraw) && (
          <GameOver winner={winner} onRematch={handleRematch} />
        )}
        <GameBoard onSelectSquare={handleSelectSquare} board={gameBoard} />
      </div>
      <Log turns={gameTurns} />
    </main>
  );
}

export default App;
