import ApiClient from "../services/api-client";

export interface DiscProfile {
  id: number;
  name: string;
  color: string;
  description: string;
}
export default new ApiClient<DiscProfile>("/discProfiles");