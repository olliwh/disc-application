import { CiUser } from "react-icons/ci";
import { useNavigate } from "react-router-dom";

import { Button, HStack, Image, useDisclosure } from "@chakra-ui/react";

import logo from "../assets/DISC_wheel.png";
import useAuthStore from "../authStore";
import loginService from "../services/loginService";
import { ColorModeSwitch } from "./ColorModeSwitch";
import LoginModal from "./LoginModal";
import SearchInput from "./SearchInput ";

export const NavBar = () => {
  const { token, logout } = useAuthStore();
  const isAuthenticated = !!token;
  const navigate = useNavigate();
  const {
    isOpen: isLoginOpen,
    onOpen: onLoginOpen,
    onClose: onLoginClose,
  } = useDisclosure();

  const handleLogout = () => {
    navigate("/");
    logout();
  };

  const handleUserIconClick = () => {
    const employeeId = loginService.getEmployeeIdFromToken();
    if (employeeId) {
      navigate(`/employees/${employeeId}`);
    }
  };
  const handleToHomePage = () => {
    navigate("/");
  }

  return (
    <HStack justifyContent="space-between" paddingRight={25}>
      <Image padding={2} src={logo} boxSize="60px" onClick={handleToHomePage}/>
      <SearchInput />
      {!isAuthenticated ? (
        <>
          <Button id="loginBtnNavbar" colorScheme="teal" variant="outline" onClick={onLoginOpen}>
            Login
          </Button>
          <LoginModal isOpen={isLoginOpen} onClose={onLoginClose} />
        </>
      ) : (
        <HStack spacing={4}>
          <CiUser
            size={24}
            onClick={handleUserIconClick}
            style={{ cursor: "pointer" }}
            id="toProfileBtn"
          />
          <Button colorScheme="teal" variant="outline" onClick={handleLogout}>
            Logout
          </Button>
        </HStack>
      )}
      <ColorModeSwitch />
    </HStack>
  );
};
