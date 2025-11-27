import { create } from "zustand";

import loginService from "./services/loginService";

export interface User {
  id: number;
  username: string;
  role: string;
}

interface AuthStore {
  user: User;
  token: string;
  setUser: (user: User) => void;
  resetUser: () => void;
  setToken: (token: string) => void;
  resetToken: () => void;
  logout: () => void;
}

const getInitialToken = () => loginService.getToken() || "";
const getInitialUser = () => {
  const userStr = localStorage.getItem("user");
  return userStr ? JSON.parse(userStr) : ({} as User);
};

const useAuthStore = create<AuthStore>((set) => ({
  user: getInitialUser(),
  token: getInitialToken(),
  setUser: (user) => {
    localStorage.setItem("user", JSON.stringify(user));
    set(() => ({ user }));
  },
  resetUser: () => {
    localStorage.removeItem("user");
    set(() => ({ user: {} as User }));
  },
  setToken: (token) => {
    loginService.saveToken(token);
    set(() => ({ token }));
  },
  resetToken: () => {
    loginService.removeToken();
    set(() => ({ token: "" }));
  },
  logout: () => {
    loginService.removeToken();
    localStorage.removeItem("user");
    set(() => ({ token: "", user: {} as User }));
  },
}));

export default useAuthStore;
