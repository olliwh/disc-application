import type { Position } from "../hooks/usePositions";
import usePositions from "../hooks/usePositions";
import CustomList from "./reusableComponents/CustomAsideList";

interface Props {
  onSelectPosition: (position: Position | null) => void;
  selectedPosition: Position | null;
}
const PositionList = ({ onSelectPosition, selectedPosition }: Props) => {
  return (
    <CustomList
      title="Positions"
      useDataHook={usePositions}
      onSelectItem={onSelectPosition}
      selectedItem={selectedPosition}
    />
  );
};
export default PositionList;
