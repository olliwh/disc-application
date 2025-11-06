import { Button, Menu, MenuButton, MenuItem, MenuList } from "@chakra-ui/react";

import type { DiscProfile } from "../hooks/useDiscProfiles";
import useDiscProfiles from "../hooks/useDiscProfiles";

interface Props {
  onSelectDiscProfile: (discProfile: DiscProfile | null) => void;
  selectedDiscProfile: DiscProfile | null;
}

const DiscProfileSelector = ({
  onSelectDiscProfile,
  selectedDiscProfile,
}: Props) => {
  const { data: discProfiles, error } = useDiscProfiles();
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
          onClick={() => onSelectDiscProfile(null)}
        >
          Clear
        </MenuItem>
        {discProfiles?.map((discProfile) => (
          <MenuItem
            key={discProfile.id}
            onClick={() => onSelectDiscProfile(discProfile)}
          >
            {discProfile.name}
          </MenuItem>
        ))}
      </MenuList>
    </Menu>
  );
};
export default DiscProfileSelector;
