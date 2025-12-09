import ApiClient from "./api-client";

export interface Weather {
  temperature: number;
  weatherCode: number;
}

export const getWeatherIcon = (weatherCode: number): string => {
  return (
    {
      0: "☀️",
      1: "⛅",
      2: "⛅", 
      3: "☁️", 
      45: "🌫️", 
      48: "🌫️", 
      51: "🌧️", 
      53: "🌧️", 
      55: "🌧️", 
      56: "🌧️", 
      57: "🌧️", 
      61: "🌧️", 
      63: "🌧️", 
      65: "🌧️", 
      66: "🌧️", 
      67: "🌧️", 
      71: "🌨️", 
      73: "🌨️", 
      75: "🌨️", 
      77: "🌨️", 
      80: "🌧️", 
      81: "🌧️", 
      82: "🌧️", 
      85: "🌨️", 
      86: "🌨️", 
      95: "⛈️",
      96: "⛈️", 
      99: "⛈️", 
    }[weatherCode] || "☁️" 
  );
};

export default new ApiClient<Weather>("weather");
