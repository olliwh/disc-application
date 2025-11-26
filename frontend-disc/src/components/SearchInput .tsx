import { useRef } from "react";
import { BsSearch } from "react-icons/bs";

import { Input, InputGroup, InputLeftElement } from "@chakra-ui/react";
import useEmployeeQueryStore from "../store";



const SearchInput = () => {
  const onSearch = useEmployeeQueryStore((s) => s.setSearchText)

  const ref = useRef<HTMLInputElement>(null);

  return (
    <form
        onSubmit={(event) => {
          console.log(ref.current?.value)
        event.preventDefault();
        onSearch(ref.current?.value || "");
      }}
    >
      <InputGroup>
        <InputLeftElement children={<BsSearch />} />
        <Input
          ref={ref}
          borderRadius={20}
          placeholder="Search for Employees"
          variant="filled"
        />
      </InputGroup>
    </form>
  );
};

export default SearchInput;