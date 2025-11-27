import ApiClient from "../services/api-client";

export interface Employee {
  id: number;
  workEmail: string;
  workPhone: string | null;
  firstName: string;
  lastName: string;
  imagePath: string;
  departmentId: number;
  positionId: number | null;
  discProfileId: number | null;
  discProfileColor: string | null;
  fullName: string;
  discProfileName: string | null;
  positionName: string | null;
  departmentName: string;
  privateEmail: string;
  privatePhone: string;
  username: string;
}

export default new ApiClient<Employee>("/employees");
