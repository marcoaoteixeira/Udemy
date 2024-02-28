import { useState } from "react";

export default function Player({ name, symbol, isActive }) {
  const [playerName, setPlayerName] = useState(name);
  const [isEditing, setIsEditing] = useState(false);

  const handleEditClick = () => {
    setIsEditing((isEditing) => !isEditing);
  };

  const handlePlayerNameChange = (evt) => {
    setPlayerName((playerName) => (playerName = evt.target.value));
  };

  let playerNameElement = <span className="player-name">{playerName}</span>;
  let editButtonLabel = "Edit";

  if (isEditing) {
    playerNameElement = (
      <input
        type="text"
        required
        value={playerName}
        onChange={handlePlayerNameChange}
      />
    );
    editButtonLabel = "Save";
  }

  return (
    <li className={isActive ? "active" : undefined}>
      <span className="player">
        {playerNameElement}
        <span className="player-symbol">{symbol}</span>
      </span>
      <button onClick={handleEditClick}>{editButtonLabel}</button>
    </li>
  );
}
