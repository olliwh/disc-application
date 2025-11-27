import { useQuery } from "@tanstack/react-query";
import employeeService from "../services/employeeService";

const useEmployee = (id: string) => {
  return useQuery({
    queryKey: ["employees", id],
    queryFn: () => employeeService.getById(parseInt(id)),
    enabled: !!id,
  });
};

export default useEmployee;