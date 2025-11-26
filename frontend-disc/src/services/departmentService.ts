import ApiClient from "./api-client";

export interface Department {
  id: number;
  name: string;
}

export default new ApiClient<Department>("departments")