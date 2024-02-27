import { CORE_CONCEPTS } from "../../data-with-examples";
import CoreConcepts from "../CoreConcept";
import Section from "../Section";

export default function () {
  return (
    <Section id="core-concepts" title="Core Concepts">
      <ul>
        {CORE_CONCEPTS.map((item, idx) => (
          <CoreConcepts key={idx} {...item} />
        ))}
      </ul>
    </Section>
  );
}
