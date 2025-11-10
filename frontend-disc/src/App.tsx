import { useState } from "react";

import { Grid, GridItem, Show } from "@chakra-ui/react";

import DepartmentList from "./components/DepartmentList";
import DiscProfileSelector from "./components/DiscProfileSelector";
import EmployeeGrid from "./components/EmployeeGrid";
import { NavBar } from "./components/NavBar";
import PositionList from "./components/PositionList";
import type { Department } from "./hooks/useDepartments";
import type { DiscProfile } from "./hooks/useDiscProfiles";
import type { Position } from "./hooks/usePositions";

export interface EmployeeQuery {
  department: Department | null;
  position: Position | null;
  discProfile: DiscProfile | null;
  searchText: string | null;
}

function App() {
  const [employeeQuery, setEmployeeQuery] = useState<EmployeeQuery>(
    {} as EmployeeQuery,
  );

  const handleSelect = (
    key: keyof EmployeeQuery,
    value: Department | Position | DiscProfile | null,
  ) => setEmployeeQuery({ ...employeeQuery, [key]: value });

  const handleSearch = (searchText: string | null) => {
    setEmployeeQuery((prev) => ({ ...prev, searchText }));
  };

  return (
    <Grid
      templateAreas={{ base: `"nav" "main"`, lg: `"nav nav" "aside main"` }}
      templateColumns={{ base: "1fr", lg: "200px 1fr" }}
    >
      <GridItem area="nav">
        <NavBar onSearch={handleSearch} />
      </GridItem>
      <Show above="lg">
        <GridItem area="aside">
          <DepartmentList
            selectedDepartment={employeeQuery.department}
            onSelectDepartment={(department) =>
              handleSelect("department", department)
            }
          />
          <PositionList
            selectedPosition={employeeQuery.position}
            onSelectPosition={(position) => handleSelect("position", position)}
          />
        </GridItem>
      </Show>
      <GridItem area="main">
        <DiscProfileSelector
          selectedDiscProfile={employeeQuery.discProfile}
          onSelectDiscProfile={(discProfile) =>
            handleSelect("discProfile", discProfile)
          }
        />
        <EmployeeGrid employeeQuery={employeeQuery} />
      </GridItem>
    </Grid>
  );
}

export default App;
