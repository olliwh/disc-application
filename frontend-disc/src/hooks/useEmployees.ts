import { em } from "framer-motion/client";
import type { EmployeeQuery } from "../App";
import useData from "./useData";

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
  useData<Employee>(
    "/employees",
    {
      params: {
        departmentId: employeeQuery.department?.id,
        positionId: employeeQuery.position?.id,
        discProfileId: employeeQuery.discProfile?.id,
        search: employeeQuery.searchText,
      },
    },
    [employeeQuery],
  );
export default useEmployees;
