import { create } from "zustand";

import type { Department } from "./services/departmentService";
import type { DiscProfile } from "./services/discProfileService";
import type { Position } from "./services/positionService";

export interface EmployeeQuery {
  department?: Department;
  position?: Position;
  discProfile?: DiscProfile;
  searchText?: string;
  pageSize?: number;
}
interface EmployeeQueryStore {
  employeeQuery: EmployeeQuery;
  setDepartment: (department?: Department) => void;
  setDiscProfile: (discProfile?: DiscProfile) => void;
  setPosition: (position?: Position) => void;
  setSortOrder: (sortOrder?: string) => void;
  setSearchText: (searchText?: string) => void;
}
const useEmployeeQueryStore = create<EmployeeQueryStore>((set) => ({
  employeeQuery: {},
  setDepartment: (department) =>
    set((state) => ({
      employeeQuery: { ...state.employeeQuery, department },
    })),
  setDiscProfile: (discProfile) =>
    set((state) => ({
      employeeQuery: { ...state.employeeQuery, discProfile },
    })),
  setPosition: (position) =>
    set((state) => ({
      employeeQuery: { ...state.employeeQuery, position },
    })),
  setSortOrder: (sortOrder) =>
    set((state) => ({
      employeeQuery: { ...state.employeeQuery, sortOrder },
    })),
  setSearchText: (searchText) =>
    set(() => ({
      employeeQuery: { searchText }, // Reset other filters when search text is set
    })),
}));

export default useEmployeeQueryStore;
