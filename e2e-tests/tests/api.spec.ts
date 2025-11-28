import { expect, test } from "@playwright/test";

const BASE_URL = "http://localhost:5000/api";

// Helper function to login
async function login(request: any, username: string) {
  const response = await request.post(`${BASE_URL}/Auth/login`, {
    data: {
      username: username,
      password: "Pass@word1",
    },
  });

  expect(response.status()).toBe(200);
  const data = await response.json();
  expect(data.token).toBeTruthy();
  return data.token;
}

// Helper function to test get all endpoint
async function testGetAllEndpoint(
  request: any,
  endpoint: string,
  expectedCount: number
) {
  const response = await request.get(`${BASE_URL}/${endpoint}`);

  expect(response.status()).toBe(200);
  const data = await response.json();
  expect(data.items).toBeDefined();
  expect(Array.isArray(data.items)).toBe(true);
  expect(data.totalCount).toBe(expectedCount);
}
async function testCreateEndpoint(
  request: any,
  endpoint: string,
  name: string,
  description: string
) {
  const createResponse = await request.post(`${BASE_URL}/${endpoint}`, {
    data: {
      name: name,
      description: description,
    },
  });
  expect(createResponse.status()).toBe(201);
  const data = await createResponse.json();
  expect(data.id).toBeTruthy();
  expect(data.name).toBe(name);
  expect(data.description).toBe(description);

  return data.id; // Return the ID
}
async function testUpdateEndpoint(
  request: any,
  endpoint: string,
  name: string,
  description: string,
  id: number
) {
  const createResponse = await request.put(`${BASE_URL}/${endpoint}/${id}`, {
    data: {
      name: name,
      description: description,
    },
  });
  expect(createResponse.status()).toBe(200);
  const data = await createResponse.json();
  expect(data.id).toBeTruthy();
  expect(data.name).toBe(name);
  expect(data.description).toBe(description);
}
async function testDeleteEndpoint(request: any, endpoint: string, id: number) {
  const deleteResponse = await request.delete(`${BASE_URL}/${endpoint}/${id}`);
  expect(deleteResponse.status()).toBe(200);
}

test.describe("API Tests", () => {
  let token: string;
  let adminToken: string;
  let ogNumDepartments = 6;
  let ogNumPositions = 6;
  let ogNumEmployees = 23;
  let createdPositionId: number;
  let createdDepartmentId: number;

  test.describe("Authentication", () => {
    test("should login with valid credentials (alice)", async ({ request }) => {
      token = await login(request, "alice");
    });

    test("should login with valid credentials (admin)", async ({ request }) => {
      adminToken = await login(request, "admin");
    });

    test("should fail login with invalid password", async ({ request }) => {
      const response = await request.post(`${BASE_URL}/Auth/login`, {
        data: {
          username: "alice",
          password: "wrongpassword",
        },
      });

      expect(response.status()).toBe(401);
    });

    test("should fail login with invalid username", async ({ request }) => {
      const response = await request.post(`${BASE_URL}/Auth/login`, {
        data: {
          username: "nonexistent",
          password: "Pass@word1",
        },
      });

      expect(response.status()).toBe(401);
    });
  });

  test.describe("Positions Endpoint", () => {
    test("should get all positions", async ({ request }) => {
      await testGetAllEndpoint(request, "positions", ogNumPositions);
    });

    test("should get positions with pagination", async ({ request }) => {
      const response = await request.get(`${BASE_URL}/positions`, {
        params: {
          pageIndex: 1,
          pageSize: 10,
        },
      });

      expect(response.status()).toBe(200);
      const data = await response.json();
      expect(data.pageIndex).toBe(1);
      expect(data.pageSize).toBe(10);
    });

    test("should create, update, and delete position", async ({ request }) => {
      // Create
      createdPositionId = await testCreateEndpoint(
        request,
        "positions",
        "Project Manager",
        "Manages project timelines and team coordination"
      );
      await testGetAllEndpoint(request, "positions", ogNumPositions + 1);

      // Update
      await testUpdateEndpoint(
        request,
        "positions",
        "Project Manager edited",
        "Manages project timelines and team coordination edited",
        createdPositionId
      );

      // Delete
      await testDeleteEndpoint(request, "positions", createdPositionId);
      await testGetAllEndpoint(request, "positions", ogNumPositions);
    });
  });

  test.describe("Departments Endpoint", () => {
    test("should get all departments", async ({ request }) => {
      await testGetAllEndpoint(request, "departments", ogNumDepartments);
    });

    test("should create, update, and delete department", async ({
      request,
    }) => {
      // Create
      createdDepartmentId = await testCreateEndpoint(
        request,
        "departments",
        "Management",
        "Management and executive leadership department"
      );
      await testGetAllEndpoint(request, "departments", ogNumDepartments + 1);

      // Update
      await testUpdateEndpoint(
        request,
        "departments",
        "Management edited",
        "Management and executive leadership department edited",
        createdDepartmentId
      );

      // Delete
      await testDeleteEndpoint(request, "departments", createdDepartmentId);
      await testGetAllEndpoint(request, "departments", ogNumDepartments);
    });
  });

  test.describe("DiscProfiles Endpoint", () => {
    test("should get all disc profiles", async ({ request }) => {
      await testGetAllEndpoint(request, "discprofiles", 4);
    });
  });

  test.describe("Employees Endpoint", () => {
    test("should get all employees", async ({ request }) => {
      await testGetAllEndpoint(request, "employees", ogNumEmployees);
    });

    test("should get employee by id", async ({ request }) => {
      const authToken = await login(request, "admin");
      const employeeId = 1;
      const url = `${BASE_URL}/employees/${employeeId}`;

      const response = await request.get(url, {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
      });

      expect(response.status()).toBe(200);
      const data = await response.json();

      expect(data.id).toBe(employeeId);
      expect(data.fullName).toBeTruthy();
      expect(data.workEmail).toBeTruthy();
      expect(data.positionName).toBeTruthy();
      expect(data.departmentName).toBeTruthy();
      expect(data.privateEmail).toBeTruthy();
      expect(data.privatePhone).toBeTruthy();
    });

    test("should filter employees by department", async ({ request }) => {
      const response = await request.get(`${BASE_URL}/employees`, {
        params: {
          departmentId: 4,
        },
      });

      expect(response.status()).toBe(200);
      const data = await response.json();
      expect(data.items).toBeDefined();
      expect(Array.isArray(data.items)).toBe(true);
      expect(data.items.length).toBeGreaterThan(0);

      // Verify all items belong to department 4
      data.items.forEach((employee: any) => {
        expect(employee.id).toBeTruthy();
        expect(employee.workEmail).toBeTruthy();
        expect(employee.firstName).toBeTruthy();
        expect(employee.lastName).toBeTruthy();
        expect(employee.departmentId).toBe(4);
      });
    });

    test("should update employee private data (authenticated)", async ({
      request,
    }) => {
      const authToken = await login(request, "admin");

      const listResponse = await request.get(`${BASE_URL}/employees`);
      const listData = await listResponse.json();
      const employeeId = listData.items[0]?.id;

      if (!employeeId) {
        test.skip();
      }

      const updateResponse = await request.put(
        `${BASE_URL}/employees/${employeeId}`,
        {
          headers: {
            Authorization: `Bearer ${authToken}`,
          },
          data: {
            privateEmail: "newemail@example.com",
            privatePhone: "12345678",
          },
        }
      );

      expect(updateResponse.status()).toBe(200);
    });

    test("should fail to update without authentication", async ({
      request,
    }) => {
      const response = await request.put(`${BASE_URL}/employees/1`, {
        data: {
          privateEmail: "test@example.com",
          privatePhone: "12345678",
        },
      });

      expect(response.status()).toBe(401);
    });

    test("should create and delete employee (admin only)", async ({
      request,
    }) => {
      const authToken = await login(request, "admin");

      // Create employee
      const createResponse = await request.post(`${BASE_URL}/employees`, {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
        data: {
          privateEmail: "jane@mail.dom",
          privatePhone: "122334455",
          firstName: "jane",
          lastName: "smith",
          workEmail: `jane.smith.${Date.now()}@techcorp.com`,
          departmentId: 3,
          positionId: 1,
          discProfileId: 2,
          cpr: "1122334455",
          username: `jane.smith.${Date.now()}`,
          password: "Pass@word1",
        },
      });

      expect(createResponse.status()).toBe(201);
      const data = await createResponse.json();
      expect(data.id).toBeTruthy();
      expect(data.firstName).toBe("jane");
      expect(data.lastName).toBe("smith");
      await testGetAllEndpoint(request, "employees", ogNumEmployees + 1);

      // Delete the created employee
      const createdEmployeeId = data.id;
      const deleteResponse = await request.delete(
        `${BASE_URL}/employees/${createdEmployeeId}`,
        {
          headers: {
            Authorization: `Bearer ${authToken}`,
          },
        }
      );

      expect(deleteResponse.status()).toBe(200);
      await testGetAllEndpoint(request, "employees", ogNumEmployees);

    });

    test("should fail to create employee without admin role", async ({
      request,
    }) => {
      const authToken = await login(request, "alice");

      const createResponse = await request.post(`${BASE_URL}/employees`, {
        headers: {
          Authorization: `Bearer ${authToken}`,
        },
        data: {
          privateEmail: "jane@mail.dom",
          privatePhone: "122334455",
          firstName: "jane",
          lastName: "smith",
          workEmail: "jane.smith@techcorp.com",
          departmentId: 3,
          positionId: 1,
          discProfileId: 2,
          cpr: "1122334455",
          username: "jane.smith",
          password: "Pass@word1",
        },
      });

      expect(createResponse.status()).toBe(403);
    });

    test("should fail to create employee without authentication", async ({
      request,
    }) => {
      const createResponse = await request.post(`${BASE_URL}/employees`, {
        data: {
          privateEmail: "jane@mail.dom",
          privatePhone: "122334455",
          firstName: "jane",
          lastName: "smith",
          workEmail: "jane.smith@techcorp.com",
          departmentId: 3,
          positionId: 1,
          discProfileId: 2,
          cpr: "1122334455",
          username: "jane.smith",
          password: "Pass@word1",
        },
      });

      expect(createResponse.status()).toBe(401);
    });
  });
});
