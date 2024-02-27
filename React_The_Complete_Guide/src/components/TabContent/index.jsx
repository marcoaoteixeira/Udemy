export default function ({ content }) {
  return content ? (
    <div id="tab-content">
      <h3>{content.title}</h3>
      <p>{content.description}</p>
      <pre>
        <code>{content.code}</code>
      </pre>
    </div>
  ) : (
    <p>Please, select a topic.</p>
  );
}
