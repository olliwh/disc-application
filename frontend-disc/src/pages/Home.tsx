import { Grid, GridItem, Show } from "@chakra-ui/react";
import DepartmentList from "../components/DepartmentList";
import PositionList from "../components/PositionList";
import DiscProfileSelector from "../components/DiscProfileSelector";
import EmployeeGrid from "../components/EmployeeGrid";


function App() {
  return (
    <Grid
      templateAreas={{ base: `"nav" "main"`, lg: `"nav nav" "aside main"` }}
      templateColumns={{ base: "1fr", lg: "200px 1fr" }}
    >
      <Show above="lg">
        <GridItem area="aside">
          <DepartmentList />
          <PositionList />
        </GridItem>
      </Show>
      <GridItem area="main">
        <DiscProfileSelector />
        <EmployeeGrid  />
      </GridItem>
    </Grid>
  );
}

export default App;
