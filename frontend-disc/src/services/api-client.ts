import axios, { type AxiosRequestConfig } from "axios";

export interface Response<T> {
  items: T[];
  totalCount: number;
  pageIndex: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
const axiosInstance = axios.create({
  baseURL: import.meta.env["VITE_API_URL"],
});
class ApiClient<T> {
  endpoint: string;
  constructor(endpoint: string) {
    this.endpoint = endpoint;
  }
  getAll(config?: AxiosRequestConfig) {
    return axiosInstance
      .get<Response<T>>(this.endpoint, config)
      .then((res) => res.data);
  }
}

export default ApiClient;
