import { useNavigate } from "react-router-dom";

import { useMutation, useQueryClient } from "@tanstack/react-query";

import employeeService from "../services/employeeService";

interface UpdateEmployeeData {
  privateEmail: string;
  privatePhone: string;
}

const useUpdateEmployee = () => {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  return useMutation({
    mutationFn: ({ id, data }: { id: number; data: UpdateEmployeeData }) =>
      employeeService.update(id, data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["employees"] });
      navigate("/");
    },
  });
};

export default useUpdateEmployee;
