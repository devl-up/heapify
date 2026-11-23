import { useQuery } from "@tanstack/react-query";
import { useRouteContext } from "@tanstack/react-router";
import { ButtonLink } from "@/components/ui/button-link";

function ProjectBreadcrumb() {
  const { getProjectOptions } = useRouteContext({
    from: "/_authenticated/projects/$projectId",
  });

  const projectQuery = useQuery(getProjectOptions);

  if (!projectQuery.data) {
    return null;
  }

  return (
    <>
      <span>/</span>
      <ButtonLink
        to="/projects/$projectId"
        params={{ projectId: projectQuery.data.id }}
        intent="ghost"
      >
        {projectQuery.data?.name}
      </ButtonLink>
    </>
  );
}

export { ProjectBreadcrumb };
