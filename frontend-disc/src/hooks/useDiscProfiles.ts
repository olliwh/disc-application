import useData from "./useData";

export interface DiscProfile {
  id: number;
  name: string;
  color: string;
  description: string;
}
const useDiscProfiles = () => useData<DiscProfile>("/DiscProfiles");
export default useDiscProfiles;
