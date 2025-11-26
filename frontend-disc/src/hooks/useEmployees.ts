import type { EmployeeQuery } from "../App";
import apiClient from "../services/api-client";
import { useQuery } from "@tanstack/react-query";
import type { Response } from "../services/api-client";


export interface Employee {
  id: number;
  email: string;
  phone: string;
  firstName: string;
  lastName: string;
  experience: number;
  imagePath: string;
  companyId: number;
  discProfileColor: string;
}

const useEmployees = (employeeQuery: EmployeeQuery) =>
  useQuery<Response<Employee>, Error>({

    queryKey: ["employees", employeeQuery],
    queryFn: () =>
      apiClient.get<Response<Employee>>("/employees", {
        params: {
        departmentId: employeeQuery.department?.id,
        positionId: employeeQuery.position?.id,
        discProfileId: employeeQuery.discProfile?.id,
        search: employeeQuery.searchText,
      }
    }).then((res) => res.data),



  });
  // useData<Employee>(
  //   "/employees",
  //   {
  //     params: {
  //       departmentId: employeeQuery.department?.id,
  //       positionId: employeeQuery.position?.id,
  //       discProfileId: employeeQuery.discProfile?.id,
  //       search: employeeQuery.searchText,
  //     },
  //   },
  //   [employeeQuery],
  // );
export default useEmployees;
