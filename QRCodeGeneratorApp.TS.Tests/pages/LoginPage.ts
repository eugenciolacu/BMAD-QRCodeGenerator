import { type Page, type Locator } from '@playwright/test';

export class LoginPage {
  readonly page: Page;
  readonly emailInputLocator: Locator;
  readonly passwordInputLocator: Locator;
  readonly submitButtonLocator: Locator;

  constructor(page: Page) {
    this.page = page;
    this.emailInputLocator = page.getByLabel('Email');
    this.passwordInputLocator = page.getByLabel('Password');
    this.submitButtonLocator = page.getByRole('button', { name: 'Log in' });
  }

  async goto() {
    await this.page.goto('/Identity/Account/Login');
  }

  async login(email: string, password: string) {
    await this.emailInputLocator.fill(email);
    await this.passwordInputLocator.fill(password);
    await this.submitButtonLocator.click();
  }
}
