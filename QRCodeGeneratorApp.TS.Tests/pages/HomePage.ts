import { type Page, type Locator } from '@playwright/test';

export class HomePage {
  readonly page: Page;
  readonly registerLinkLocator: Locator;
  readonly loginLinkLocator: Locator;
  readonly userGreetingLocator: Locator;
  readonly createQRCodeLinkLocator: Locator;
  readonly viewMyQRCodesLinkLocator: Locator;
  readonly menuHomeLinkLocator: Locator;
  readonly menuCreateQRCodeLinkLocator: Locator;
  readonly menuMyQRCodesLinkLocator: Locator;

  constructor(page: Page) {
    this.page = page;
    this.registerLinkLocator = page.getByRole('link', { name: 'Register' });
    this.loginLinkLocator = page.getByRole('link', { name: 'Login' });
    this.userGreetingLocator = page.getByText(/Logged user:/i);
    this.createQRCodeLinkLocator = page.getByRole('link', { name: '+ Create QR Code' });
    this.viewMyQRCodesLinkLocator = page.getByRole('link', { name: 'View My QR Codes' });
    this.menuHomeLinkLocator = page.getByRole('navigation').getByRole('link', { name: 'Home' });
    this.menuCreateQRCodeLinkLocator = page
      .getByRole('navigation')
      .getByRole('link', { name: 'Create QR Code' });
    this.menuMyQRCodesLinkLocator = page
      .getByRole('navigation')
      .getByRole('link', { name: 'My QR Codes' });
  }

  async goto() {
    await this.page.goto('/');
  }
}
