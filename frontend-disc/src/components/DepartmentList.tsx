import type { Department } from "../hooks/useDepartments";
import useDepartments from "../hooks/useDepartments";
import CustomList from "./reusableComponents/CustomAsideList";

interface Props {
  onSelectDepartment: (department: Department | null) => void;
  selectedDepartment: Department | null;
}
const DepartmentList = ({ onSelectDepartment, selectedDepartment }: Props) => {
  return (
    <CustomList
      title="Departments"
      useDataHook={useDepartments}
      onSelectItem={onSelectDepartment}
      selectedItem={selectedDepartment}
    />
  );
};
export default DepartmentList;