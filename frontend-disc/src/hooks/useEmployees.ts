import { useInfiniteQuery } from "@tanstack/react-query";

import type { Response } from "../services/api-client";
import type { Employee } from "../services/employeeService";
import employeeService from "../services/employeeService";
import useEmployeeQueryStore from "../store";

const useEmployees = () => {
  const employeeQuery = useEmployeeQueryStore((s) => s.employeeQuery);
  return useInfiniteQuery<Response<Employee>, Error>({
    queryKey: [
      "employees",
      employeeQuery.department?.id,
      employeeQuery.position?.id,
      employeeQuery.discProfile?.id,
      employeeQuery.searchText,
    ],
    initialPageParam: 1,
    queryFn: ({ pageParam = 1 }) => {
      return employeeService
        .getAll({
          params: {
            departmentId: employeeQuery.department?.id,
            positionId: employeeQuery.position?.id,
            discProfileId: employeeQuery.discProfile?.id,
            search: employeeQuery.searchText,
            page_size: employeeQuery.pageSize,
            pageIndex: pageParam,
          },
        })
        .catch((error) => {
          console.error("Employee fetch error:", error);
          throw error;
        });
    },
    getNextPageParam: (lastPage) => {
      return lastPage.hasNextPage ? lastPage.pageIndex + 1 : undefined;
    },
    staleTime: 5 * 60 * 1000, // 5 minutes
    gcTime: 10 * 60 * 1000, // 10 minutes (was cachTime)
  });
};

export default useEmployees;
