import ApiClient from "../services/api-client";
export interface Employee {
  id: number;
  workEmail: string;
  workPhone: string;
  firstName: string;
  lastName: string;
  imagePath: string;
  departmentId: number;
  positionId: number | null;
  discProfileId: number;
  discProfileColor: string;
}
export default new ApiClient<Employee>("/employees");