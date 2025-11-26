import { useQuery } from "@tanstack/react-query";
import type { Response } from "../services/api-client";

import discprofiles from "../data/discprofiles";
import ApiClient from "../services/api-client";
const apiClient = new ApiClient<DiscProfile>("/discProfiles");

export interface DiscProfile {
  id: number;
  name: string;
  color: string;
  description: string;
}
const useDiscProfiles = () => 
  useQuery<Response<DiscProfile>, Error>({
    queryKey: ["discProfiles"],
    queryFn: () =>
      apiClient.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
    initialData: discprofiles
});
  
export default useDiscProfiles;
