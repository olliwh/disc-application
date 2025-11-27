import ApiClient from "./api-client";

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresAt: string;
}

const apiClient = new ApiClient<LoginResponse>("/Auth");

const loginService = {
  /**
   * Log in a user with username and password.
   * @param username - The user's username.
   * @param password - The user's password.
   * @returns A promise resolving to the login response containing the JWT token.
   */
  login: async (username: string, password: string): Promise<LoginResponse> => {
    return apiClient.post<LoginResponse>("/login", { username, password });
  },

  /**
   * Save a token to localStorage.
   * @param token - The JWT token string.
   */
  saveToken: (token: string) => {
    localStorage.setItem("token", token);
  },

  /**
   * Get the current token from localStorage.
   * @returns The token string, or null if not present.
   */
  getToken: () => localStorage.getItem("token"),

  /**
   * Remove the token from localStorage.
   */
  removeToken: () => {
    localStorage.removeItem("token");
  },

  /**
   * Get the employee ID from the JWT token payload.
   * @returns The employee ID, or null if the token is invalid.
   */
  getEmployeeIdFromToken: (): number | null => {
    const token = loginService.getToken();
    if (!token) return null;
    try {
      const [, payloadBase64] = token.split(".");
      const payload = JSON.parse(atob(payloadBase64));
      return payload.employeeId ? parseInt(payload.employeeId, 10) : null;
    } catch {
      return null;
    }
  },
};

export default loginService;
