import { useMutation, useQueryClient } from "@tanstack/react-query";
import ApiClient from "../services/api-client";

const apiClient = new ApiClient("/employees");

const useDeleteEmployee = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (employeeId: number) => apiClient.delete(employeeId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["employees"] });
    },
  });
};

export default useDeleteEmployee;