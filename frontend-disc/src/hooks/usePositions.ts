import useData from "./useData";


export interface Position {
  id: number;
  name: string;
}

const usePositions = () => useData<Position>("/Positions");
export default usePositions;
