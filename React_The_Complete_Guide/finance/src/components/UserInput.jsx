import { useState } from "react";

export default function UserInput({ userInput, onChange }) {
  return (
    <section id="user-input">
      <div className="input-group">
        <p>
          <label htmlFor="initial-investment">Initial Investment</label>
          <input
            id="initial-investment"
            type="number"
            required
            value={userInput.initialInvestment}
            onChange={(evt) => onChange("initialInvestment", evt.target.value)}
          />
        </p>
        <p>
          <label htmlFor="annual-investment">Annual Investment</label>
          <input
            id="annual-investment"
            type="number"
            required
            value={userInput.annualInvestment}
            onChange={(evt) => onChange("annualInvestment", evt.target.value)}
          />
        </p>
      </div>
      <div className="input-group">
        <p>
          <label htmlFor="expected-return">Expected Return</label>
          <input
            id="expected-return"
            type="number"
            required
            value={userInput.expectedReturn}
            onChange={(evt) => onChange("expectedReturn", evt.target.value)}
          />
        </p>
        <p>
          <label htmlFor="duration">Duration</label>
          <input
            id="duration"
            type="number"
            required
            value={userInput.duration}
            onChange={(evt) => onChange("duration", evt.target.value)}
          />
        </p>
      </div>
    </section>
  );
}
