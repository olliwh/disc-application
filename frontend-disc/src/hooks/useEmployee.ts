import { useQuery } from "@tanstack/react-query";

import employeeService from "../services/employeeService";

const useEmployee = (id: string) => {
  return useQuery({
    queryKey: ["employee", id],
    queryFn: () => employeeService.getById(parseInt(id)),
    enabled: !!id,
    retry: false,
    throwOnError: true,
    staleTime: 0, 
  });
};

export default useEmployee;
