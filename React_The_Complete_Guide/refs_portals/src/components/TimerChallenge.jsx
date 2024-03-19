import { Fragment, useRef, useState } from "react";
import ResultModal from "./ResultModal";

const INTERVAL = 10;

export default function TimerChallenge({ title, targetTime }) {
  const timer = useRef();
  const dialog = useRef();

  const targetTimeInSeconds = targetTime * 1000;

  const [timeRemaining, setTimeRemaining] = useState(targetTimeInSeconds);
  const isTimerActive =
    timeRemaining > 0 && timeRemaining < targetTimeInSeconds;

  if (timeRemaining < 0) {
    clearInterval(timer.current);
    dialog.current.open();
  }

  const handleReset = () => {
    setTimeRemaining(targetTimeInSeconds);
  };

  const handleStart = () => {
    timer.current = setInterval(() => {
      setTimeRemaining((previousValue) => previousValue - INTERVAL);
    }, INTERVAL);
  };

  const handleStop = () => {
    dialog.current.open();
    clearInterval(timer.current);
  };

  return (
    <Fragment>
      <ResultModal
        ref={dialog}
        targetTime={targetTime}
        remainingTime={timeRemaining}
        onReset={handleReset}
      />
      <section className="challenge">
        <h2>{title}</h2>
        <p className="challenge-time">
          {targetTime} second{targetTime > 1 ? "s" : undefined}
        </p>
        <p>
          <button onClick={isTimerActive ? handleStop : handleStart}>
            {isTimerActive ? "Stop" : "Start"} Challenge
          </button>
        </p>
        <p className={isTimerActive ? "active" : undefined}>
          {isTimerActive ? "Timer is running" : "Timer inactive"}
        </p>
      </section>
    </Fragment>
  );
}
