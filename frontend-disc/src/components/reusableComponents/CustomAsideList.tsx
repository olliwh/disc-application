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

interface CustomListProps<T> {
  title: string;
  useDataHook: () => { data: T[]; error: string; isLoading: boolean };
  selectedItem: T | null;
  onSelectItem: (item: T) => void;
}
const CustomList = <T extends { id: number; name: string }>({
  title,
  useDataHook,
  selectedItem,
  onSelectItem,
}: CustomListProps<T>) => {
  const [isExpanded, setIsExpanded] = useState(false);
  const { data: items, error, isLoading } = useDataHook();
  const displayedItems = isExpanded ? items : (items ?? []).slice(0, 5);
  if (error) return <p>Error: {error}</p>;
  if (isLoading) return <Spinner />;
  return (
    <Box padding={4}>
      <Heading fontSize="2xl">{title}</Heading>
      <List>
        {displayedItems.map((item) => (
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
