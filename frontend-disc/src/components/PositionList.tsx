import usePositions from "../hooks/usePositions";
import useEmployeeQueryStore from "../store";
import CustomList from "./reusableComponents/CustomAsideList";

const PositionList = () => {
  const selectedPosition = useEmployeeQueryStore(
    (s) => s.employeeQuery.position,
  );
  const setPosition = useEmployeeQueryStore((s) => s.setPosition);
  return (
    <CustomList
      title="Positions"
      useDataHook={usePositions}
      onSelectItem={setPosition}
      selectedItem={selectedPosition}
    />
  );
};
export default PositionList;
