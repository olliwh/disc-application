import { useState } from "react";
import { GrEdit } from "react-icons/gr";
import { useParams } from "react-router-dom";

import {
  Box,
  Button,
  Divider,
  GridItem,
  HStack,
  Heading,
  Image,
  Input,
  SimpleGrid,
  Spinner,
  Text,
  VStack,
} from "@chakra-ui/react";

import useEmployee from "../hooks/useEmployee";

const EmployeeProfile = () => {
  const { id } = useParams<{ id: string }>();
  const { data: employee, isLoading, error } = useEmployee(id || "");
  const [isEditMode, setIsEditMode] = useState(false);

  if (isLoading) return <Spinner />;
  if (error) return <Text>Error loading employee profile</Text>;
  if (!employee) return <Text>Employee not found</Text>;

  return (
    <Box p={8} maxW="1000px" mx="auto">
      <HStack spacing={8} mb={8} align="start">
        <Image
          src={employee.imagePath}
          alt={`${employee.firstName} ${employee.lastName}`}
          borderRadius="md"
          boxSize="200px"
          objectFit="cover"
          objectPosition="center top"
        />
        <VStack align="start" spacing={2}>
          <Heading size="2xl">{employee.fullName}</Heading>
          <Text fontSize="lg" color="gray.600">
            {employee.positionName}
          </Text>
          <Text fontSize="lg" color="gray.600">
            {employee.departmentName}
          </Text>
        </VStack>
      </HStack>

      <Divider my={8} />

      <SimpleGrid spacing={6} columns={{ base: 1, md: 2 }}>
        <GridItem
          colSpan={{ base: 1, md: 2 }}
          display="flex"
          justifyContent="flex-end"
          mb={4}
        >
          <Button
            leftIcon={<GrEdit />}
            colorScheme="teal"
            variant="outline"
            onClick={() => setIsEditMode(!isEditMode)}
          >
            {isEditMode ? "Cancel" : "Edit"}
          </Button>
        </GridItem>

                    <GridItem>
              <Text>
                <Text as="span" fontWeight="bold">
                  Work Email:
                </Text>{" "}
                {employee.workEmail}
              </Text>
            </GridItem>
            <GridItem>
              <Text>
                <Text as="span" fontWeight="bold">
                  Work Phone:
                </Text>{" "}
                {employee.workPhone}
              </Text>
            </GridItem>
            <GridItem>
              <Text>
                <Text as="span" fontWeight="bold">
                  Disc Profile:
                </Text>{" "}
                {employee.discProfileName}
              </Text>
            </GridItem>
            <GridItem>
              <Text>
                <Text as="span" fontWeight="bold">
                  Username:
                </Text>{" "}
                {employee.username}
              </Text>
            </GridItem>

        {isEditMode ? (
          <>
            <GridItem>
              <Text fontWeight="bold" mb={2}>
                Private Email:
              </Text>
              <Input defaultValue={employee.privateEmail} />
            </GridItem>
            <GridItem>
              <Text fontWeight="bold" mb={2}>
                Private Phone:
              </Text>
              <Input defaultValue={employee.privatePhone} />
            </GridItem>
            <GridItem colSpan={{ base: 1, md: 2 }}>
              <HStack spacing={4}>
                <Button colorScheme="green">Save</Button>
                <Button variant="outline" onClick={() => setIsEditMode(false)}>
                  Cancel
                </Button>
              </HStack>
            </GridItem>
          </>
        ) : (
          <>
            <GridItem>
              <Text>
                <Text as="span" fontWeight="bold">
                  Private Email:
                </Text>{" "}
                {employee.privateEmail}
              </Text>
            </GridItem>
            <GridItem>
              <Text>
                <Text as="span" fontWeight="bold">
                  Private Phone:
                </Text>{" "}
                {employee.privatePhone}
              </Text>
            </GridItem>
          </>
        )}
      </SimpleGrid>
    </Box>
  );
};

export default EmployeeProfile;
