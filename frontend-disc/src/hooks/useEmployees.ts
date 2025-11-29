import { useInfiniteQuery } from "@tanstack/react-query";

import type { Response } from "../services/api-client";
import type { Employee } from "../services/employeeService";
import employeeService from "../services/employeeService";
import useEmployeeQueryStore from "../store";

const useEmployees = () => {
  const employeeQuery = useEmployeeQueryStore((s) => s.employeeQuery);
  return useInfiniteQuery<Response<Employee>, Error>({
    queryKey: ["employees", employeeQuery],
    initialPageParam: 1,
    queryFn: ({ pageParam = 1 }) => {
      return employeeService.getAll({
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
      return lastPage.hasNextPage ? lastPage.pageIndex + 1 : undefined;
    },
  });
};

export default useEmployees;
