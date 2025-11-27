import { Button, HStack, Image, useDisclosure } from "@chakra-ui/react";

import logo from "../assets/DISC_wheel.png";
import useAuthStore from "../authStore";
import { ColorModeSwitch } from "./ColorModeSwitch";
import LoginModal from "./LoginModal";
import SearchInput from "./SearchInput ";

export const NavBar = () => {
  const { token, logout } = useAuthStore();
  const isAuthenticated = !!token;
  const {
    isOpen: isLoginOpen,
    onOpen: onLoginOpen,
    onClose: onLoginClose,
  } = useDisclosure();

  const handleLogout = () => {
    logout();
  };

  return (
    <HStack justifyContent="space-between" paddingRight={25}>
      <Image padding={2} src={logo} boxSize="60px" />
      <SearchInput />
      {!isAuthenticated ? (
        <>
          <Button colorScheme="teal" variant="outline" onClick={onLoginOpen}>
            Login
          </Button>
          <LoginModal isOpen={isLoginOpen} onClose={onLoginClose} />
        </>
      ) : (
        <Button colorScheme="teal" variant="outline" onClick={handleLogout}>
          Logout
        </Button>
      )}
      <ColorModeSwitch />
    </HStack>
  );
};
