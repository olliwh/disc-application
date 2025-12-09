import { describe, expect, it, vi } from "vitest";

import { getWeatherIcon } from "./weatherService";

// Mock ApiClient before importing weatherService
vi.mock("./api-client", () => ({
  default: class MockApiClient {
    constructor() {}
  },
}));

describe("test getWeatherIcon", () => {
  it.each([
    [0, "☀️"],
    [1, "⛅"],
    [2, "⛅"],
    [3, "☁️"],
    [45, "🌫️"],
    [48, "🌫️"],
    [51, "🌧️"],
    [53, "🌧️"],
    [55, "🌧️"],
    [56, "🌧️"],
    [57, "🌧️"],
    [61, "🌧️"],
    [63, "🌧️"],
    [65, "🌧️"],
    [66, "🌧️"],
    [67, "🌧️"],
    [71, "🌨️"],
    [73, "🌨️"],
    [75, "🌨️"],
    [77, "🌨️"],
    [80, "🌧️"],
    [81, "🌧️"],
    [82, "🌧️"],
    [85, "🌨️"],
    [86, "🌨️"],
    [95, "⛈️"],
    [96, "⛈️"],
    [99, "⛈️"],
    [999, "☁️"],
    [-1, "☁️"],
  ])("should return %s emoji for weather code %i", (code, emoji) => {
    expect(getWeatherIcon(code)).toBe(emoji);
  });
});
