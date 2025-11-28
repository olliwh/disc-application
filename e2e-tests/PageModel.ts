import { expect, type Locator, type Page } from "@playwright/test";

export class PageModel {
  readonly page: Page;
  readonly positionheading: Locator;
  readonly departmentheading: Locator;
  readonly loginBtnNavBar: Locator;
  readonly usernameInput: Locator;
  readonly pswInput: Locator;
  readonly loginBtnModal: Locator;
  readonly toProfileBtn: Locator;
  readonly editProfileBtn: Locator;
  readonly emailInput: Locator;
  readonly phoneInput: Locator;
  readonly saveBtn: Locator;
  readonly deleteBtn: Locator;
  readonly profileHeading: Locator;
  readonly logoutBtn: Locator;

  constructor(page: Page) {
    this.page = page;
    this.positionheading = page.getByRole("heading", { name: "Positions" });
    this.departmentheading = page.getByRole("heading", { name: "Departments" });
    this.loginBtnNavBar = page.locator("#loginBtnNavbar");
    this.usernameInput = page.getByRole("textbox", { name: "Username" });
    this.pswInput = page.getByRole("textbox", { name: "Password" });
    this.loginBtnModal = page.locator("#loginBtnModal");
    this.toProfileBtn = page.locator("#toProfileBtn");
    this.editProfileBtn = page.locator("#editProfileBtn");
    this.emailInput = page.locator("#email-input");
    this.phoneInput = page.locator("#phone-input");
    this.saveBtn = page.getByRole("button", { name: "Save" });
    this.deleteBtn = page
      .getByRole("heading", { name: "Admin Admin" })
      .locator("..")
      .getByRole("button", { name: "Delete" });
    this.profileHeading = page.getByRole("heading", { name: "Alice Jensen" });
    this.logoutBtn = page.getByRole("button", { name: "Logout" });
  }

  async goto() {
    await this.page.goto("/");
  }

  async login(username: string, password: string) {
    console.log(`🔐 Attempting login for: ${username}`);

    await this.loginBtnNavBar.click();
    await this.usernameInput.fill(username);
    await this.pswInput.fill(password);

    // Wait for any network activity before clicking
    await this.page.waitForLoadState("networkidle");
    await this.page.waitForTimeout(500);

    await this.loginBtnModal.click();
    console.log("✓ Login button clicked");

    // Wait for modal to close (indicates successful login)
    try {
      await this.page
        .locator('div[role="dialog"]')
        .waitFor({ state: "hidden", timeout: 10000 });
      console.log("✓ Login modal closed");
    } catch (error) {
      console.error("❌ Login modal did not close - login likely failed");
      // Take screenshot to debug
      await this.page.screenshot({ path: "login-failed.png" });
      throw error;
    }

    // Wait for page to stabilize
    await this.page.waitForLoadState("networkidle");
    console.log("✓ Page network idle");

    // Wait for logout button to appear
    await this.waitForUserLoggedIn();
  }

  private async waitForUserLoggedIn() {
    console.log("⏳ Waiting for logout button...");
    try {
      await this.logoutBtn.waitFor({ state: "visible", timeout: 15000 });
      console.log("✅ Logout button visible - Login successful!");
    } catch (error) {
      console.error("❌ Logout button not visible after login");

      // Check if there's an error message on the page
      const errorAlert = await this.page.getByRole("alert").textContent();
      console.error("Alert text:", errorAlert);

      // Check page content
      const pageContent = await this.page.content();
      if (pageContent?.includes("Ooops")) {
        console.error("❌ Page shows error state - likely API issue");
      }

      // Take screenshot to debug
      await this.page.screenshot({ path: "logout-btn-missing.png" });
      throw error;
    }
  }

  async goToProfile() {
    await this.waitForUserLoggedIn();
    await this.toProfileBtn.waitFor({ state: "visible", timeout: 15000 });
    await this.toProfileBtn.click();
    await expect(
      this.page.getByRole("heading", { name: "Alice Jensen" })
    ).toBeVisible();
  }

  async editEmail(newEmail: string) {
    await this.editProfileBtn.click();
    await this.emailInput.fill(newEmail);
    await this.saveBtn.click();
  }

  async deleteEmployee(employeeName: string) {
    const employeeCard = this.getEmployeeByName(employeeName);
    const deleteButton = employeeCard
      .locator("..")
      .locator("..")
      .getByRole("button", { name: "Delete" });
    await deleteButton.click();
  }

  getEmployeeByName(employeeName: string): Locator {
    return this.page.getByRole("img", { name: employeeName });
  }

  async logout() {
    await this.logoutBtn.click();
  }
}
