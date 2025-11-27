import { createBrowserRouter } from "react-router-dom";
import Layout from "./pages/Layout";
import ErrorPage from "./pages/ErrorPage";
import Home from "./pages/Home";
import EmployeeProfile from "./pages/EmployeeProfile";



const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    errorElement: <ErrorPage />,
    children: [
      { path: "/", element: <Home /> },
      { path: "/employees/:id", element: <EmployeeProfile /> },
    ],
  },
]);

export default router;