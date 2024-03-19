import { forwardRef } from "react";

const Input = forwardRef(({ id, title, textarea, ...pros }, ref) => {
  const className =
    "w-full p-1 border-b-2 rounded-sm border-stone-300 bg-stone-200 text-stone-600 focus:outline-none focus:border-stone-600";
  return (
    <p className="flex flex-col gap-1 my-4">
      <label
        htmlFor={id}
        className="text-sm font-bold uppercase text-stone-500"
      >
        {title}
      </label>
      {textarea ? (
        <textarea ref={ref} id={id} {...pros} className={className} />
      ) : (
        <input ref={ref} id={id} {...pros} className={className} />
      )}
    </p>
  );
});

export default Input;
