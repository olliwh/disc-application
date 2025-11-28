import React from "react";
import InfiniteScroll from "react-infinite-scroll-component";

import { SimpleGrid, Spinner, Text } from "@chakra-ui/react";

import useEmployees from "../hooks/useEmployees";
import EmployeeCard from "./EmployeeCard";
import EmployeeCardSkeleton from "./EmployeeCardSkeleton";
import EmployeeCardContainer from "./reusableComponents/EmployeeCardContainer";

const EmployeeGrid = () => {
  const { data, error, isLoading, fetchNextPage, hasNextPage } = useEmployees();

  const skeletons = [...Array(12).keys()];
  const fetchedEmployeesCount =
    data?.pages.reduce((total, page) => total + page.items.length, 0) || 0;
  return (
    <InfiniteScroll
      dataLength={fetchedEmployeesCount}
      next={fetchNextPage}
      hasMore={hasNextPage ?? false}
      loader={<Spinner />}
      scrollThreshold={1}
    >
      {error && <Text color="tomato">{error.message}</Text>}
      <SimpleGrid
        columns={{ sm: 1, md: 2, lg: 3, xl: 4 }}
        spacing={10} //space between cards
        padding="10" //space to sides
      >
        {isLoading &&
          skeletons.map((skeleton) => (
            <EmployeeCardContainer key={skeleton}>
              <EmployeeCardSkeleton />
            </EmployeeCardContainer>
          ))}

        {data?.pages.map((page, index) => (
          <React.Fragment key={index}>
            {page.items.map((employee) => (
              <EmployeeCardContainer key={employee.id}>
                <EmployeeCard employee={employee} />
              </EmployeeCardContainer>
            ))}
          </React.Fragment>
        ))}
      </SimpleGrid>
    </InfiniteScroll>
  );
};
export default EmployeeGrid;
