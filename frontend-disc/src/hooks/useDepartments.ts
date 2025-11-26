import ApiClient from "../services/api-client";
import { useQuery } from "@tanstack/react-query";
import type { Response } from "../services/api-client";

import departments from "../data/departments";

const apiClient = new ApiClient<Department>("/departments");


export interface Department {
  id: number;
  name: string;
}

const useDepartments = () =>
  useQuery<Response<Department>, Error>({
    queryKey: ["departments"],
    queryFn: () =>
      apiClient.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
    initialData: departments
});
export default useDepartments;
