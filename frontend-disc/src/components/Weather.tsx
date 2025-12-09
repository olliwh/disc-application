import { HStack, Text } from "@chakra-ui/react";

import useWeather from "../hooks/useWeather";
import { getWeatherIcon } from "../services/weatherService";

const Weather = () => {
  const { data: weather, isLoading, error } = useWeather();

  if (isLoading) return null;
  if (error) return null;
  if (!weather) return null;

  return (
    <div>
      <HStack padding={4}>
        <Text fontSize="3xl">{weather.temperature}°C</Text>
        <Text fontSize="5xl">{getWeatherIcon(weather.weatherCode)}</Text>
      </HStack>
    </div>
  );
};
export default Weather;
