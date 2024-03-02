import { formatter } from "../util/investment";

export default function ResultItem({ initialInvestment, item }) {
  const totalInterest =
    item.valueEndOfYear - item.annualInvestment * item.year - initialInvestment;
  const totalAmoutInvested = item.valueEndOfYear - totalInterest;

  return (
    <tr>
      <td>{item.year}</td>
      <td>{formatter.format(item.valueEndOfYear)}</td>
      <td>{formatter.format(item.interest)}</td>
      <td>{formatter.format(totalInterest)}</td>
      <td>{formatter.format(totalAmoutInvested)}</td>
    </tr>
  );
}
