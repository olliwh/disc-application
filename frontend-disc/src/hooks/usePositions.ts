import apiClient from "../services/api-client";
import { useQuery } from "@tanstack/react-query";
import type { Response } from "./useData";
import positions from "../data/positions";


export interface Position {
  id: number;
  name: string;
}

const usePositions = () => 
  useQuery<Response<Position>, Error>({
    queryKey: ["positions"],
    queryFn: () =>
      apiClient.get<Response<Position>>("/positions").then((res) => res.data),
    staleTime: 5 * 60 * 1000, // 5 minutes
    initialData: positions
});
export default usePositions;
