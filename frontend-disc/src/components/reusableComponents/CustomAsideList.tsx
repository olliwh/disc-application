import { useState } from "react";

import {
  Box,
  Button,
  HStack,
  Heading,
  Link,
  List,
  ListItem,
  Spinner,
} from "@chakra-ui/react";
import type { UseQueryResult } from "@tanstack/react-query";

import type { Response } from "../../services/api-client";

interface CustomListProps<T> {
  title: string;
  useDataHook: () => UseQueryResult<Response<T>, Error>;
  selectedItem?: T | null;
  onSelectItem: (item?: T) => void;
}
const CustomList = <T extends { id: number; name: string }>({
  title,
  useDataHook,
  selectedItem,
  onSelectItem,
}: CustomListProps<T>) => {
  const [isExpanded, setIsExpanded] = useState(false);
  const { data, error, isLoading } = useDataHook();
  const items = data?.items;
  const displayedItems = isExpanded ? items : (items ?? []).slice(0, 5); //mayby wrong
  if (error) return null;
  if (isLoading) return <Spinner />;
  return (
    <Box padding={4}>
      <Heading
        fontSize="2xl"
        onClick={() => onSelectItem(undefined)}
        cursor="pointer"
      >
        {title}
      </Heading>
      <List>
        {displayedItems?.map((item) => (
          <ListItem key={item.id} paddingY="5px">
            <HStack>
              <Link
                fontSize="lg"
                onClick={() => onSelectItem(item)}
                colorScheme={selectedItem?.id === item.id ? "yellow" : "white"}
              >
                {item.name}
              </Link>
            </HStack>
          </ListItem>
        ))}
        <Button onClick={() => setIsExpanded(!isExpanded)}>
          {isExpanded ? "Show less" : "Show more"}
        </Button>
      </List>
    </Box>
  );
};
export default CustomList;
