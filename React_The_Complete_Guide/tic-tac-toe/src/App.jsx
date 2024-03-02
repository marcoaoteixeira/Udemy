import { useState } from "react";
import GameBoard from "./components/GameBoard";
import Player from "./components/Player";
import Log from "./components/Log";
import { WINNING_COMBINATIONS } from "./winning-combinations";
import GameOver from "./components/GameOver";

// Constants
const PLAYERS = {
  X: "Player 1",
  O: "Player 2",
};

const INITIAL_GAME_BOARD = [
  [null, null, null],
  [null, null, null],
  [null, null, null],
];

// Functions
const getActivePlayer = (turns) =>
  turns.length > 0 && turns[0].player === "X" ? "O" : "X";

const runGameBoard = (turns) => {
  let gameBoard = [...INITIAL_GAME_BOARD.map((inner) => [...inner])];

  for (const turn of turns) {
    const { square, player } = turn;
    const { rowIndex, columnIndex } = square;

    gameBoard[rowIndex][columnIndex] = player;
  }

  return gameBoard;
};

const getWinner = (gameBoard, players) => {
  let winner;
  for (const combination of WINNING_COMBINATIONS) {
    const firstSquareSymbol =
      gameBoard[combination[0].row][combination[0].column];
    console.log("firstSquareSymbol: ", firstSquareSymbol);
    const secondSquareSymbol =
      gameBoard[combination[1].row][combination[1].column];
    console.log("secondSquareSymbol: ", firstSquareSymbol);
    const thirdSquareSymbol =
      gameBoard[combination[2].row][combination[2].column];
    console.log("thirdSquareSymbol: ", firstSquareSymbol);

    if (
      firstSquareSymbol &&
      firstSquareSymbol === secondSquareSymbol &&
      firstSquareSymbol === thirdSquareSymbol
    ) {
      winner = players[firstSquareSymbol];
    }
  }

  return winner;
};

export default function App() {
  const [players, setPlayers] = useState(PLAYERS);
  const [gameTurns, setGameTurns] = useState([]);

  const activePlayer = getActivePlayer(gameTurns);
  const gameBoard = runGameBoard(gameTurns);
  const winner = getWinner(gameBoard, players);
  const hasDraw = gameTurns.length === 9 && !winner;

  // Handlers
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
    setGameTurns([]);
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
            name={PLAYERS.X}
            symbol="X"
            isActive={activePlayer === "X"}
            onChangeName={handlePlayerNameChange}
          />
          <Player
            name={PLAYERS.O}
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
