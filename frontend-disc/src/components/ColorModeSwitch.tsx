import { HStack, Switch, useColorMode, Text } from "@chakra-ui/react";

export const ColorModeSwitch = () => {
  const { colorMode, toggleColorMode } = useColorMode();

  return (
    <HStack>
      <Switch
        colorScheme="blue"
        isChecked={colorMode === "dark"}
        onChange={toggleColorMode}
      ></Switch>
      <Text>Dark Mode</Text>
    </HStack>
  );
};