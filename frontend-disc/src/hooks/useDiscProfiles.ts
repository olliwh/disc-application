import apiClient from "../services/api-client";
import { useQuery } from "@tanstack/react-query";
import type { Response } from "../services/api-client";

import discprofiles from "../data/discprofiles";

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
      apiClient.get<Response<DiscProfile>>("/disc-profiles").then((res) => res.data),
    staleTime: 5 * 60 * 1000, // 5 minutes
    initialData: discprofiles
});
  
export default useDiscProfiles;
