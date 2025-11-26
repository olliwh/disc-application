// import { useEffect, useState } from "react";

// import { type AxiosRequestConfig, CanceledError } from "axios";

// import apiClient from "../services/api-client";
//  interface Response<T> {
//   totalCount: number;
//   next: string | null;
//   items: T[];
// }
// const useData = <T>(
//   endpoint: string,
//   requestConfig?: AxiosRequestConfig,
//   dependencies?: unknown[],//any gave error becuase it not type-safe
// ) => {
//   const [data, setData] = useState<T[]>([]);
//   const [error, setError] = useState("");
//   const [isLoading, setLoading] = useState(false);
//   useEffect(
//     () => {
//       const controller = new AbortController();
//       setLoading(true);

//       apiClient
//         .get<Response<T>>(endpoint, {
//           signal: controller.signal,
//           ...requestConfig,
//         })
//         .then((res) => {
          
//           setData(res.data.items);
//           setLoading(false);
//         })
//         .catch((err) => {
//           if (err instanceof CanceledError) return;
//           setError(err.message);
//           setLoading(false);
//         });
//       return () => controller.abort();
//     },
//     dependencies ? [...dependencies] : [],
//   );
//   return { data, error, isLoading };
// };
// export default useData;
