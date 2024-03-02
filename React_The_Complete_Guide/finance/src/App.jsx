import { Fragment, useState } from "react";
import Header from "./components/Header";
import UserInput from "./components/UserInput";
import Results from "./components/Results";

export default function App() {
  // Stats
  const [userInput, setUserInput] = useState({
    initialInvestment: 10000,
    annualInvestment: 1000,
    expectedReturn: 6,
    duration: 10,
  });

  // Handlers
  const handleChange = (field, newValue) => {
    setUserInput((previousValue) => {
      return {
        ...previousValue,
        [field]: parseFloat(newValue),
      };
    });
  };

  const isDurationValid = userInput.duration >= 1;

  return (
    <Fragment>
      <Header />
      <UserInput userInput={userInput} onChange={handleChange} />
      {!isDurationValid && (
        <p className="center">Duration value should be greater than zero.</p>
      )}
      {isDurationValid && <Results userInput={userInput} />}
    </Fragment>
  );
}
