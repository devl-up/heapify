import { Route, Router } from "@solidjs/router";
import "./index.css";
import { lazy } from "solid-js";
import { render } from "solid-js/web";

const Home = lazy(() => import("./routes/home").then((module) => ({ default: module.Home })));

const root = document.getElementById("root");

if (!root) {
  throw new Error("Root element not found");
}

render(
  () => (
    <Router>
      <Route path="/" component={Home} />
    </Router>
  ),
  root,
);
