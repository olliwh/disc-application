import { extendTheme, type ThemeConfig } from "@chakra-ui/react";
//create config object that tells chakra how to handle colormode, and give it to theme
const config: ThemeConfig ={
    useSystemColorMode: true,
}
//extendTheme() is a Chakra UI helper that merges your custom settings with Chakra’s default theme.

const theme = extendTheme({ config })

export default theme