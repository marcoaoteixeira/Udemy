import { useState } from "react";
import NewProject from "./components/NewProject";
import NoProjectSelected from "./components/NoProjectSelected";
import ProjectSidebar from "./components/ProjectSidebar";

export default function App() {
  const [projectState, setProjectState] = useState({
    selectedProjectId: undefined,
    projects: [],
  });

  const handleStartNewProject = () => {
    setProjectState((prevState) => {
      return {
        ...prevState,
        selectedProjectId: null,
      };
    });
  };

  const handleAddProject = (projectData) => {
    setProjectState((prevState) => {
      const newProject = {
        ...projectData,
        id: Math.random(),
      };

      return {
        ...prevState,
        selectedProjectId: undefined,
        projects: [...prevState.projects, newProject],
      };
    });
  };

  let content;
  switch (projectState.selectedProjectId) {
    case null:
      content = (
        <NewProject
          onStartNewProject={handleStartNewProject}
          onAddProject={handleAddProject}
        />
      );
      break;
    case undefined:
      content = <NoProjectSelected onStartNewProject={handleStartNewProject} />;
      break;
  }

  return (
    <main className="h-screen my-8 flex gap-8">
      <ProjectSidebar
        onStartNewProject={handleStartNewProject}
        projects={projectState.projects}
      />
      {content}
    </main>
  );
}
