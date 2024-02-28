import reactLogoImg from "../../assets/react-core-concepts.png";
import "./index.css";

const reactDescriptions = ["Fundamental", "Crucial", "Core"];
const getRandomInt = (max) => Math.floor(Math.random() * (max + 1));

export default function () {
  const description = reactDescriptions[getRandomInt(2)];

  return (
    <header>
      <img src={reactLogoImg} alt="Stylized atom" />
      <h1>React Essentials</h1>
      <p>
        {description} React concepts you will need for almost any app you are
        going to build!
      </p>
    </header>
  );
}
