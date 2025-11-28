import { expect, test } from "@playwright/test";
import { PageModel } from "../PageModel";

test.describe("Disc application e2e tests", () => {
  test.beforeEach(async ({ page }) => {
    //runs before each test in this describe block
    const discPage = new PageModel(page);
    await discPage.goto();
  });
  test("Login and edit as alice", async ({ page }) => {
    const discPage = new PageModel(page);
    await discPage.login("alice", "Pass@word1");
    await expect(discPage.deleteBtn).toBeHidden();
    await discPage.goToProfile();
    await discPage.editEmail("aliiiiice@@ok.dk");
  });

  //partial generations:
  test("Login and delete as admin", async ({ page }) => {
    const discPage = new PageModel(page);
    await discPage.login("admin", "Pass@word1");
    await expect(discPage.deleteBtn).toBeVisible();
    await expect(discPage.mikkelAndersonCard).toBeVisible();

    await discPage.deleteEmployee("Mikkel Andersen");
    await page.waitForLoadState("networkidle");
    await page.waitForTimeout(2000); // Wait for cache to update

    await expect(discPage.mikkelAndersonCard).toBeHidden();
  });
});
