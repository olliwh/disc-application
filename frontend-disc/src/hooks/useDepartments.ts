import apiClient from "../services/api-client";
import { useQuery } from "@tanstack/react-query";
import type { Response } from "../services/api-client";

import departments from "../data/departments";


export interface Department {
  id: number;
  name: string;
}

const useDepartments = () =>
  useQuery<Response<Department>, Error>({
    queryKey: ["departments"],
    queryFn: () =>
      apiClient.get<Response<Department>>("/departments").then((res) => res.data),
    staleTime: 5 * 60 * 1000, // 5 minutes
    initialData: departments
});
export default useDepartments;
