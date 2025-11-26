import { Box, Button, Card, CardBody, HStack, Heading, Image } from "@chakra-ui/react";

import type { Employee } from "../hooks/useEmployees";
import useDeleteGame from "../hooks/useDeleteGame";

interface Props {
  employee: Employee;
}

const EmployeeCard = ({ employee }: Props) => {
  const { mutate, isPending } = useDeleteGame();
  const color =  employee.discProfileColor;

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
        <HStack spacing={3}  height="100%">
          <Button
            colorScheme="red"
            size="sm"
            isLoading={isPending}
            onClick={() => mutate(employee.id)}
            >
            Delete
            </Button>
          <Heading size="md" flex="1" isTruncated>
            {employee.firstName} {employee.lastName}
          </Heading>
          <Box
            borderRadius="50%"
            backgroundColor={color}
            width="22px"     
            height="22px" 
            flexShrink={0}
          />
        </HStack>
      </CardBody>
    </Card>
  );
};
export default EmployeeCard;
