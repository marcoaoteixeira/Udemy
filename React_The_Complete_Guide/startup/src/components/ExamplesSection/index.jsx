import { useState } from "react";
import TabButton from "../TabButton";
import TabContent from "../TabContent";
import { EXAMPLES } from "../../data-with-examples";
import Section from "../Section";

export default function () {
  const [selectedTopic, setSelectedTopic] = useState();

  const tabButtonClickHandler = (identifier) => {
    setSelectedTopic(identifier);
  };

  return (
    <Section id="examples" title="Examples">
      <menu>
        {Object.entries(EXAMPLES).map(([key, value]) => (
          <TabButton
            key={key}
            isSelected={selectedTopic === key}
            onClick={() => tabButtonClickHandler(key)}
          >
            {value.title}
          </TabButton>
        ))}
      </menu>
      <TabContent content={EXAMPLES[selectedTopic]} />
    </Section>
  );
}
