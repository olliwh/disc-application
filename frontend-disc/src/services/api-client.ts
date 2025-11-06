import axios from "axios";

const apiClient = axios.create({
  baseURL: import.meta.env["VITE_API_URL"],

  // baseURL: "https://company-disc-api.onrender.com/api",
  // params: {
  //   key: import.meta.env["VITE_COMPANY_DISC_API_KEY"]
  // },
  //   headers: {
  //   'Content-Type': 'application/json'
  // }
});
export default apiClient;