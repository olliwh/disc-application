import useData from "./useData";


export interface Department {
  id: number;
  name: string;
}

const useDepartments = () => useData<Department>("/departments");
export default useDepartments;
