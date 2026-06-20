import { type Page, type Locator } from '@playwright/test';

export class HomePage {
  readonly page: Page;
  readonly registerLink: Locator;
  readonly loginLink: Locator;
  readonly userGreeting: Locator;
  readonly createQRCodeLink: Locator;
  readonly viewMyQRCodesLink: Locator;

  constructor(page: Page) {
    this.page = page;
    this.registerLink = page.getByRole('link', { name: 'Register' });
    this.loginLink = page.getByRole('link', { name: 'Login' });
    this.userGreeting = page.getByText(/Logged user:/i);
    this.createQRCodeLink = page.getByRole('link', { name: '+ Create QR Code' });
    this.viewMyQRCodesLink = page.getByRole('link', { name: 'View My QR Codes' });
  }

  async goto() {
    await this.page.goto('/');
  }
}
