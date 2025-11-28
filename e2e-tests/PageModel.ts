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
  readonly mikkelAndersonCard: Locator;

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
      .getByRole("heading", { name: "Mikkel Andersen" })
      .locator("..") // go to parent
      .getByRole("button", { name: "Delete" });
    this.profileHeading = page.getByRole("heading", { name: "Alice Jensen" });
    this.mikkelAndersonCard = page.getByRole("img", {
      name: "Mikkel Andersen",
    });
  }

  async goto() {
    await this.page.goto("/");
  }
  async login(username: string, password: string) {
    await this.loginBtnNavBar.click();
    await this.usernameInput.fill(username);
    await this.pswInput.fill(password);
    await this.loginBtnModal.click();
  }
  async editEmail(newEmail: string) {
    await this.editProfileBtn.click();
    await this.emailInput.fill(newEmail);
    await this.saveBtn.click();
    await expect(this.mikkelAndersonCard).toBeVisible();
    await this.goToProfile();
    await expect(
      this.page.getByText(`Private Email: ${newEmail}`)
    ).toBeVisible();
  }

  async goToProfile() {
    await this.toProfileBtn.click();
    await expect(
      this.page.getByRole("heading", { name: "Alice Jensen" })
    ).toBeVisible();
  }

  async deleteEmployee(employeeName: string) {
    const employeeCard = this.page.getByRole("img", { name: employeeName });

    // Navigate up to find the card container, then find the delete button within it
    const deleteButton = employeeCard
      .locator("..")
      .locator("..")
      .getByRole("button", { name: "Delete" });

    await deleteButton.click();
  }
}
