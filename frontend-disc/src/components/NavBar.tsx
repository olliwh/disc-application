import { HStack, Image } from "@chakra-ui/react"
import logo from "../assets/DISC_wheel.png"
import { ColorModeSwitch } from "./ColorModeSwitch"
import SearchInput from "./SearchInput "

export const NavBar = () => {
  return (
  <HStack justifyContent="space-between" paddingRight={25}>
    <Image padding={2} src={logo} boxSize="60px"/>
    <SearchInput />

    <ColorModeSwitch/>
  </HStack>
  )
}


