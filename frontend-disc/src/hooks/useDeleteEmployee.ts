import { useMutation, useQueryClient } from "@tanstack/react-query";
import employeeService from "../services/employeeService";


const useDeleteEmployee = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (employeeId: number) => employeeService.delete(employeeId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["employees"] });
    },
  });
};

export default useDeleteEmployee;