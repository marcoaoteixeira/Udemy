export default function ({ isSelected, children, ...props }) {
  return (
    <li>
      <button className={isSelected ? "active" : null} {...props}>
        {children}
      </button>
    </li>
  );
}
