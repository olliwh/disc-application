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
    await this.loginBtnNavBar.click();
    await this.usernameInput.fill(username);
    await this.pswInput.fill(password);
    await this.loginBtnModal.click();

    // Wait for login to complete
  }

  async goToProfile() {
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
    await this.page.waitForLoadState("networkidle");
    await expect(employeeCard).toBeHidden();
  }

  getEmployeeByName(employeeName: string): Locator {
    return this.page.getByRole("img", { name: employeeName });
  }

  async logout() {
    await this.logoutBtn.click();
  }
}
