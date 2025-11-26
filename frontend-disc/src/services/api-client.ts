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

  getById = (id: number) =>
    axiosInstance
      .get<Response<T>>(`${this.endpoint}/${id}`)
      .then((res) => res.data);

  delete = (id: number) =>
    axiosInstance.delete(`${this.endpoint}/${id}`).then((res) => res.data);


  create = (data: Partial<T>) =>
    axiosInstance
      .post<Response<T>>(this.endpoint, data)
      .then((res) => res.data);


  update = (id: number, data: Partial<T>) =>
    axiosInstance
      .put<Response<T>>(`${this.endpoint}/${id}`, data)
      .then((res) => res.data);
}

export default ApiClient;
