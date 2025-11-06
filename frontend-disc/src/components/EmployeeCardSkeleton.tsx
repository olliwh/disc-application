import { Card, CardBody, Skeleton, SkeletonText } from "@chakra-ui/react";

const EmployeeCardSkeleton = () => {
  return (
    <Card height="100%">
      <Skeleton height="240px"/>
      <CardBody>
        <SkeletonText />
      </CardBody>
    </Card>
  );
};

export default EmployeeCardSkeleton;
