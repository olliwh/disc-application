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

test.describe("API Tests", () => {
  let token: string;
  let adminToken: string;

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
      const response = await request.get(`${BASE_URL}/positions`);

      expect(response.status()).toBe(200);
      const data = await response.json();
      expect(data.items).toBeDefined();
      expect(Array.isArray(data.items)).toBe(true);
      expect(data.totalCount).toBeGreaterThan(0);
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
  });

  test.describe("Departments Endpoint", () => {
    test("should get all departments", async ({ request }) => {
      const response = await request.get(`${BASE_URL}/departments`);

      expect(response.status()).toBe(200);
      const data = await response.json();
      expect(data.items).toBeDefined();
      expect(Array.isArray(data.items)).toBe(true);
      expect(data.totalCount).toBeGreaterThan(0);
    });
  });

  test.describe("Employees Endpoint", () => {
    test("should get all employees", async ({ request }) => {
      const response = await request.get(`${BASE_URL}/employees`);

      expect(response.status()).toBe(200);
      const data = await response.json();
      expect(data.items).toBeDefined();
      expect(Array.isArray(data.items)).toBe(true);
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
          departmentId: 1,
        },
      });

      expect(response.status()).toBe(200);
      const data = await response.json();
      expect(data.items).toBeDefined();
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

    test("should delete employee (admin only)", async ({ request }) => {
      const response = await request.delete(`${BASE_URL}/employees/999`, {
        headers: {
          Authorization: `Bearer ${adminToken}`,
        },
      });

      expect([404, 401]).toContain(response.status());
    });

    test("should create employee (admin only)", async ({ request }) => {
      const authToken = await login(request, "admin");

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

  test.describe("DiscProfiles Endpoint", () => {
    test("should get all disc profiles", async ({ request }) => {
      const response = await request.get(`${BASE_URL}/discprofiles`);

      expect(response.status()).toBe(200);
      const data = await response.json();
      expect(data.items).toBeDefined();
      expect(Array.isArray(data.items)).toBe(true);
    });
  });
});
