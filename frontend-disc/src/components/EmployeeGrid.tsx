import { SimpleGrid, Text } from "@chakra-ui/react";

import useEmployees from "../hooks/useEmployees";
import EmployeeCard from "./EmployeeCard";
import EmployeeCardSkeleton from "./EmployeeCardSkeleton";
import type { EmployeeQuery } from "../App";
import EmployeeCardContainer from "./reusableComponents/EmployeeCardContainer";

interface Props {
  employeeQuery: EmployeeQuery;
}

const EmployeeGrid = ({ employeeQuery }: Props) => {
  const {
    data: employees,
    error,
    isLoading,
  } = useEmployees(employeeQuery);

  const skeletons = [...Array(10).keys()];

  return (
    <div>
      {error && <Text color="tomato">{error}</Text>}
      <SimpleGrid
        columns={{ sm: 1, md: 2, lg: 3, xl: 5 }}
        spacing={10} //space between cards
        padding="10" //space to sides
      >
        {isLoading &&
          skeletons.map((skeleton) => (
          <EmployeeCardContainer>
            <EmployeeCardSkeleton key={skeleton} />
          </EmployeeCardContainer>
          
          ))}

        {employees?.map((employee) => (
          <EmployeeCardContainer>
            <EmployeeCard employee={employee} key={employee.id} />
          </EmployeeCardContainer>
        ))}
      </SimpleGrid>
    </div>
  );
};
export default EmployeeGrid;
