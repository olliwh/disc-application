import { useEffect, useState } from "react";

import { type AxiosRequestConfig, CanceledError } from "axios";

import apiClient from "../services/api-client";

const useData = <T>(
  endpoint: string,
  requestConfig?: AxiosRequestConfig,
  dependencies?: unknown[],//any gave error becuase it not type-safe
) => {
  const [data, setData] = useState<T[]>([]);
  const [error, setError] = useState("");
  const [isLoading, setLoading] = useState(false);
  useEffect(
    () => {
      const controller = new AbortController();
      setLoading(true);

      apiClient
        .get<T[]>(endpoint, {
          signal: controller.signal,
          ...requestConfig,
        })
        .then((res) => {
          console.log(endpoint)
          console.log(res.data)
          console.log(import.meta.env["VITE_API_URL"])
          
          setData(res.data);
          setLoading(false);
        })
        .catch((err) => {
          if (err instanceof CanceledError) return;
          setError(err.message);
          setLoading(false);
        });
      return () => controller.abort();
    },
    dependencies ? [...dependencies] : [],
  );
  return { data, error, isLoading };
};
export default useData;
