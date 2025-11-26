import ApiClient from "../services/api-client";

export default  new ApiClient<Position>("/positions");
export interface Position {
  id: number;
  name: string;
}