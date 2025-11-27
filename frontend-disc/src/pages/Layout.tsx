
import { Box } from "@chakra-ui/react";

import { NavBar } from "../components/NavBar";
import { Outlet } from "react-router";

const Layout = () => {
  return (
    <>
      <NavBar />
      <Box p={4}>
        <Outlet />
      </Box>
    </>
  );
};

export default Layout;