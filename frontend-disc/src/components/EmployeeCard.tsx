import { Box, Card, CardBody, HStack, Heading, Image } from "@chakra-ui/react";

import type { Employee } from "../hooks/useEmployees";

interface Props {
  employee: Employee;
}

const EmployeeCard = ({ employee }: Props) => {
  const color = "#" + employee.discProfileColor;
  console.log(employee);
  console.log(color);

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
        <HStack spacing={7}  height="100%">
          <Heading size="md">
            {employee.firstName} {employee.lastName}
          </Heading>
          <Box
            borderRadius="50%"
            backgroundColor={color}
            width="22px"      // Add this line
            height="22px" 
            flexShrink={0} // Prevents the circle from shrinking
          />
        </HStack>
      </CardBody>
    </Card>
  );
};
export default EmployeeCard;
