import { useState } from "react";

import {
  Alert,
  AlertIcon,
  Button,
  FormControl,
  FormLabel,
  Input,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalHeader,
  ModalOverlay,
  useToast,
} from "@chakra-ui/react";
import { AxiosError } from "axios";

import useAuthStore from "../authStore";
import loginService from "../services/loginService";

interface LoginModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const LoginModal = ({ isOpen, onClose }: LoginModalProps) => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const toast = useToast();
  const { setToken, setUser } = useAuthStore();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);
    setIsLoading(true);
    console.log("📝 Login form submitted for:", username);
    try {
      const response = await loginService.login(username, password);
      console.log("✅ Login API response:", response);
      console.log(
        "📝 Setting token:",
        response.token ? "token received" : "no token",
      );

      setToken(response.token);
      console.log("✅ Token set in store");

      // Extract user data from token
      const employeeId = loginService.getEmployeeIdFromToken();
      console.log("📝 Employee ID from token:", employeeId);

      setUser({
        id: employeeId || 0,
        username: username,
        role: "User", // Parse from token if available
      });
      console.log("✅ User data set in store");

      toast({ status: "success", title: "Login successful" });
      onClose();
    } catch (err) {
      console.error("❌ Login error:", err);
      const axiosError = err as AxiosError<{ message?: string }>;
      setError(axiosError.response?.data?.message || "Login failed");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Admin Login</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          {error && (
            <Alert status="error" mb={4}>
              <AlertIcon />
              {error}
            </Alert>
          )}
          <form onSubmit={handleSubmit}>
            <FormControl mb={4} isRequired>
              <FormLabel>Username</FormLabel>
              <Input
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                autoFocus
              />
            </FormControl>
            <FormControl mb={4} isRequired>
              <FormLabel>Password</FormLabel>
              <Input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </FormControl>
            <Button
              type="submit"
              colorScheme="teal"
              width="full"
              mb={2}
              isLoading={isLoading}
              id="loginBtnModal"
            >
              Login
            </Button>
          </form>
        </ModalBody>
      </ModalContent>
    </Modal>
  );
};

export default LoginModal;
