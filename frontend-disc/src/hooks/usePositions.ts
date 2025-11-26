import { useQuery } from "@tanstack/react-query";
import type { Response } from "../services/api-client";
import ApiClient from "../services/api-client";


import positions from "../data/positions";

const apiClient = new ApiClient<Position>("/positions");
export interface Position {
  id: number;
  name: string;
}

const usePositions = () => 
  useQuery<Response<Position>, Error>({
    queryKey: ["positions"],
    queryFn: () =>
      apiClient.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
    initialData: positions
});
export default usePositions;
