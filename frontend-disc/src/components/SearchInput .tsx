import { useRef } from "react";
import { BsSearch } from "react-icons/bs";

import { Input, InputGroup, InputLeftElement } from "@chakra-ui/react";



const SearchInput = () => {
  const ref = useRef<HTMLInputElement>(null);

  return (
    <form

    >
      <InputGroup>
        <InputLeftElement children={<BsSearch />} />
        <Input
          ref={ref}
          borderRadius={20}
          placeholder="Search for games..."
          variant="filled"
        />
      </InputGroup>
    </form>
  );
};

export default SearchInput;