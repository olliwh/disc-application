import axios from "axios";

export interface Response<T> {
  totalCount: number;
  next: string | null;
  items: T[];
}
const apiClient = axios.create({
  baseURL: import.meta.env["VITE_API_URL"],


});
export default apiClient;