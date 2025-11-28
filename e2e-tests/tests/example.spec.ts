import { expect, test } from "@playwright/test";
import { PageModel } from "../PageModel";

const EMPLOYEE_NAMES: Record<string, string> = {
  chromium: "Sofie Nielsen",
  webkit: "Emma Christensen",
  firefox: "Freja Mortensen",
};

test.describe("Disc application e2e tests", () => {
  test.beforeEach(async ({ page }) => {
    const discPage = new PageModel(page);
    await discPage.goto();
  });

  test("Login and edit as alice", async ({ page }, testInfo) => {
    const discPage = new PageModel(page);
    const browserName = testInfo.project?.name || "chromium";
    const employeeName = EMPLOYEE_NAMES[browserName];
    const newEmail = "aliiiiice@@ok.dk";

    // Login as alice
    await discPage.login("alice", "Pass@word1");
    await expect(discPage.logoutBtn).toBeVisible({ timeout: 15000 });

    // Edit profile
    await expect(discPage.deleteBtn).toBeHidden();
    await discPage.goToProfile();
    await discPage.editEmail(newEmail);
    await expect(discPage.getEmployeeByName(employeeName)).toBeVisible();

    // Verify changes persisted
    await discPage.goToProfile();
    await expect(
      discPage.page.getByText(`Private Email: ${newEmail}`)
    ).toBeVisible();

    // Login as admin and delete employee
    await discPage.logout();
    await discPage.login("admin", "Pass@word1");
    await expect(discPage.deleteBtn).toBeVisible();

    const employeeCard = discPage.getEmployeeByName(employeeName);
    await expect(employeeCard).toBeVisible();

    await discPage.deleteEmployee(employeeName);
    await page.waitForLoadState("networkidle");
    await page.waitForTimeout(2000);

    await expect(employeeCard).toBeHidden();
    await discPage.logout();
  });
});
