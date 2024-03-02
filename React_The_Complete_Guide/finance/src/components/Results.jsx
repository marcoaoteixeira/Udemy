import { Fragment } from "react";
import { calculateInvestmentResults } from "../util/investment";
import ResultItem from "./ResultItem";

export default function Results({ userInput }) {
  const data = calculateInvestmentResults(userInput);

  const initialInvestment =
    data[0].valueEndOfYear - data[0].interest - data[0].annualInvestment;

  return (
    <table id="result">
      <thead>
        <tr>
          <th>Year</th>
          <th>Investment Value</th>
          <th>Interest (Year)</th>
          <th>Total Interest</th>
          <th>Invested Capital</th>
        </tr>
      </thead>
      <tbody>
        {data.map((item, idx) => (
          <ResultItem
            key={idx}
            initialInvestment={initialInvestment}
            item={item}
          />
        ))}
      </tbody>
    </table>
  );
}
