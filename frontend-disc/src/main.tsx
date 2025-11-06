import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import App from "./App.tsx";

import { ChakraProvider, ColorModeScript } from "@chakra-ui/react";
import theme from "./theme.ts";


createRoot(document.getElementById("root")!).render(
  <StrictMode>
    {/* makes the theme available to all chakra components */}
    <ChakraProvider theme={theme}>
      <ColorModeScript initialColorMode={theme["config"].initialColorMode} />
      <App />
    </ChakraProvider>
  </StrictMode>
);