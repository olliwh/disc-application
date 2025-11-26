import useDepartments from "../hooks/useDepartments";
import useEmployeeQueryStore from "../store";
import CustomList from "./reusableComponents/CustomAsideList";


const DepartmentList = () => {
  const selectedDepartment = useEmployeeQueryStore((s)=> s.employeeQuery.department);
  const setDepartment = useEmployeeQueryStore((s)=> s.setDepartment);
  return (
    <CustomList
      title="Departments"
      useDataHook={useDepartments}
      onSelectItem={setDepartment}
      selectedItem={selectedDepartment}
    />
  );
};
export default DepartmentList;