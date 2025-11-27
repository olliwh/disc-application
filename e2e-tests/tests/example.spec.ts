import { expect, test } from "@playwright/test";

test("basic test", async ({ page }) => {
  await page.goto("/", { waitUntil: "networkidle" });
  await page.waitForLoadState("domcontentloaded");

  // Debug: Check page title and URL
  console.log("Page URL:", page.url());
  console.log("Page title:", await page.title());

  // Debug: Take screenshot before assertions
  await page.screenshot({ path: "debug-screenshot.png" });

  // Debug: Get all buttons on page
  const buttons = await page.locator("button").allTextContents();
  console.log("Available buttons:", buttons);

  // Debug: Get all elements with role button
  const roleButtons = await page.locator('[role="button"]').allTextContents();
  console.log("Role=button elements:", roleButtons);

  // Debug: Dump page content
  const pageContent = await page.content();
  console.log("Page HTML length:", pageContent.length);
  console.log(
    "Page contains DiscProfiles:",
    pageContent.includes("DiscProfiles")
  );

  await expect(page.getByRole("button", { name: "Login" })).toBeVisible({
    timeout: 10000,
  });
  await expect(page.getByRole("button", { name: "DiscProfiles" })).toBeVisible({
    timeout: 10000,
  });
  await expect(page.getByRole("img", { name: "Admin Admin" })).toBeVisible({
    timeout: 10000,
  });
});
