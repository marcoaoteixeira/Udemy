export default function Log({ turns }) {
  return (
    <ol id="log">
      {turns.map((turn) => (
        <li key={`${turn.square.rowIndex}:${turn.square.columnIndex}`}>
          {turn.player} selected {turn.square.rowIndex},
          {turn.square.columnIndex}
        </li>
      ))}
    </ol>
  );
}
