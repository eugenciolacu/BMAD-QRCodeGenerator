import { type Page, type Locator } from '@playwright/test';

export class RegisterPage {
  readonly page: Page;
  readonly emailInputLocator: Locator;
  readonly passwordInputLocator: Locator;
  readonly confirmPasswordInputLocator: Locator;
  readonly registerButtonLocator: Locator;

  constructor(page: Page) {
    this.page = page;
    this.emailInputLocator = page.getByRole('textbox', { name: 'Email', exact: true });
    this.passwordInputLocator = page.getByRole('textbox', { name: 'Password', exact: true });
    this.confirmPasswordInputLocator = page.getByRole('textbox', { name: 'Confirm Password' });
    this.registerButtonLocator = page.getByRole('button', { name: 'Register' });
  }

  async goto() {
    await this.page.goto('/Identity/Account/Register');
  }

  async register(email: string, password: string) {
    await this.emailInputLocator.fill(email);
    await this.passwordInputLocator.fill(password);
    await this.confirmPasswordInputLocator.fill(password);
    await this.registerButtonLocator.click();
  }
}
