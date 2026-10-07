import { QueryClientProvider } from "@tanstack/react-query";
import { RouterProvider } from "@tanstack/react-router";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { setOnUnauthenticated } from "./api/api-client";
import { authApi } from "./api/auth/api";
import { meOptions } from "./api/auth/queries";
import { queryClient } from "./configs/query-client";
import "./index.css";
import { router } from "./configs/router";

async function bootstrap() {
  await authApi.antiforgery();

  setOnUnauthenticated(async () => {
    queryClient.setQueryData(meOptions().queryKey, null);
    if (router.state.location.pathname !== "/login") {
      await router.navigate({ to: "/login", replace: true });
    }
  });

  const rootElement = document.getElementById("root");

  if (!rootElement) {
    throw new Error("Root element not found");
  }

  createRoot(rootElement).render(
    <StrictMode>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </StrictMode>,
  );
}

await bootstrap();
