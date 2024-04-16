import { useState } from "react";
import NewProject from "./components/NewProject";
import NoProjectSelected from "./components/NoProjectSelected";
import ProjectSidebar from "./components/ProjectSidebar";
import noProjectsPng from "./assets/no-projects.png";

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

  const handleCancelNewProject = () => {
    setProjectState((prevState) => {
      return {
        ...prevState,
        selectedProjectId: undefined,
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
          onAddProject={handleAddProject}
          onCancelProject={handleCancelNewProject}
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
      <img src={noProjectsPng} alt="temp" width={100} height={100} />
    </main>
  );
}
