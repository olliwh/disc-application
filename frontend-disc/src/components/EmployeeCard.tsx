import {
  Box,
  Button,
  Card,
  CardBody,
  HStack,
  Heading,
  Image,
} from "@chakra-ui/react";

import useAuthStore from "../authStore";
import useDeleteGame from "../hooks/useDeleteEmployee";
import type { Employee } from "../services/employeeService";
import loginService from "../services/loginService";

interface Props {
  employee: Employee;
}

const EmployeeCard = ({ employee }: Props) => {
  const { mutate, isPending } = useDeleteGame();
  const { token } = useAuthStore();

  const isAdmin = token && loginService.getRoleFromToken() === "Admin";

  return (
    <Card height="100%">
      <Image
        src={employee.imagePath}
        alt={`${employee.firstName} ${employee.lastName}`}
        width="100%"
        height={"240px"}
        objectFit="cover"
        objectPosition="center top"
      />
      <CardBody>
        <HStack spacing={3} height="100%">
          {isAdmin && (
            <Button
              colorScheme="red"
              size="sm"
              isLoading={isPending}
              onClick={() => mutate(employee.id)}
            >
              Delete
            </Button>
          )}
          <Heading size="md" flex="1" isTruncated>
            {employee.firstName} {employee.lastName}
          </Heading>
          {employee.discProfileColor && (
            <Box
              borderRadius="50%"
              backgroundColor={employee.discProfileColor}
              width="22px"
              height="22px"
              flexShrink={0}
            />
          )}
        </HStack>
      </CardBody>
    </Card>
  );
};
export default EmployeeCard;
