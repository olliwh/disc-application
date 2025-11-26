import { Button, Menu, MenuButton, MenuItem, MenuList } from "@chakra-ui/react";
import useDiscProfiles from "../hooks/useDiscProfiles";
import useEmployeeQueryStore from "../store";

const DiscProfileSelector = () => {
  const selectedDiscProfile = useEmployeeQueryStore(
    (s) => s.employeeQuery.discProfile,
  );
  const setDiscProfile = useEmployeeQueryStore((s) => s.setDiscProfile);
  const { data, error } = useDiscProfiles();
  if (error) return null;

  return (
    <Menu>
      <MenuButton as={Button}>
        {selectedDiscProfile ? selectedDiscProfile.name : "DiscProfiles"}
      </MenuButton>
      <MenuList>
        <MenuItem
          hidden={!selectedDiscProfile}
          color="red"
          onClick={() => setDiscProfile(undefined)}
        >
          Clear
        </MenuItem>
        {data?.items.map((discProfile) => (
          <MenuItem
            key={discProfile.id}
            onClick={() => setDiscProfile(discProfile)}
          >
            {discProfile.name}
          </MenuItem>
        ))}
      </MenuList>
    </Menu>
  );
};
export default DiscProfileSelector;
