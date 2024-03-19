import { useRef, useState } from "react";

export default function Player() {
  const playerNameRef = useRef();

  const [playerName, setPlayerName] = useState(null);

  const handleSubmitClick = () => {
    setPlayerName((current) => (current = playerNameRef.current.value));
  };

  return (
    <section id="player">
      <h2>Welcome {playerName ?? "unknown entity"}</h2>
      <p>
        <input type="text" ref={playerNameRef} />
        <button onClick={handleSubmitClick}>Set Name</button>
      </p>
    </section>
  );
}
