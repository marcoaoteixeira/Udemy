import { useState } from "react";

const initialGameBoard = [
  [null, null, null],
  [null, null, null],
  [null, null, null],
];

export default function GameBoard({ onSelectSquare, activePlayerSymbol }) {
  const [gameBoard, setGameBoard] = useState(initialGameBoard);

  const handlePlayerTurn = (row, column) => {
    setGameBoard((previousGameBoard) => {
      const updatedGameBoard = [
        ...previousGameBoard.map((inner) => [...inner]),
      ];

      updatedGameBoard[row][column] = activePlayerSymbol;

      return updatedGameBoard;
    });

    onSelectSquare();
  };

  return (
    <ol id="game-board">
      {gameBoard.map((row, rowIdx) => (
        <li key={rowIdx}>
          <ol>
            {row.map((symbol, columnIdx) => (
              <li key={columnIdx}>
                <button onClick={() => handlePlayerTurn(rowIdx, columnIdx)}>
                  {symbol}
                </button>
              </li>
            ))}
          </ol>
        </li>
      ))}
    </ol>
  );
}
