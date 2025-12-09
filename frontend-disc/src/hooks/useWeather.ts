import { useQuery } from "@tanstack/react-query";

import weatherService, { type Weather } from "../services/weatherService";

const useWeather = () =>
  useQuery<Weather, Error>({
    queryKey: ["weather"],
    queryFn: () => weatherService.getWeather(),
    staleTime: 5 * 60 * 1000,
  });
export default useWeather;
