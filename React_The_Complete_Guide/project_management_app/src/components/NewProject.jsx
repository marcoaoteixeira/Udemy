import { useRef } from "react";
import Input from "./Input";

export default function NewProject({ onAddProject }) {
  const titleRef = useRef();
  const descriptionRef = useRef();
  const dueDateRef = useRef();

  const handleSave = () => {
    const projectData = {
      title: titleRef.current.value,
      description: descriptionRef.current.value,
      dueDate: dueDateRef.current.value,
    };
    onAddProject(projectData);
  };

  return (
    <div className="w-[35rem] mt-16">
      <menu className="flex items-center justify-end gap-4 my-4">
        <li>
          <button className="text-stone-800 hover:text-stone-950">
            Cancel
          </button>
        </li>
        <li>
          <button
            className="px-6 py-2 rounded-md bg-stone-800 text-stone-50 hover:bg-stone-950"
            onClick={handleSave}
          >
            Save
          </button>
        </li>
      </menu>
      <div>
        <Input ref={titleRef} id="title-input" title="Title" />
        <Input
          ref={descriptionRef}
          id="description-input"
          title="Description"
          textarea
        />
        <Input ref={dueDateRef} id="due-date" title="Due Date" type="date" />
      </div>
    </div>
  );
}
