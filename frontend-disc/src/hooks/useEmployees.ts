import { useInfiniteQuery } from "@tanstack/react-query";
import type { EmployeeQuery } from "../App";
import type { Response } from "../services/api-client";
import ApiClient from "../services/api-client";

const apiClient = new ApiClient<Employee>("/employees");

export interface Employee {
  id: number;
  workEmail: string;
  workPhone: string;
  firstName: string;
  lastName: string;
  imagePath: string;
  departmentId: number;
  positionId: number | null;
  discProfileId: number;
  discProfileColor: string;
}

const useEmployees = (employeeQuery: EmployeeQuery) => {
  console.log(employeeQuery.department)
  return useInfiniteQuery<Response<Employee>, Error>({
    queryKey: ["employees", employeeQuery],

    initialPageParam: 1,

    queryFn: ({ pageParam = 1 }) => {
      console.log("🟦 Fetching employees with page:", pageParam);

      return apiClient.getAll({
        params: {
          departmentId: employeeQuery.department?.id,
          positionId: employeeQuery.position?.id,
          discProfileId: employeeQuery.discProfile?.id,
          search: employeeQuery.searchText,
          page_size: employeeQuery.pageSize,
          pageIndex: pageParam,
        },
      });
    },

    getNextPageParam: (lastPage) => {
      return lastPage.hasNextPage
        ? lastPage.pageIndex + 1
        : undefined;
    },
  });
};

export default useEmployees;
