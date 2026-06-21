import { type Page, type Locator } from '@playwright/test';

export class QRCodeDetailsPage {
  readonly page: Page;
  readonly headingLocator: Locator;
  readonly qrCodeImageLocator: Locator;
  readonly contentValueLocator: Locator;
  readonly errorCorrectionValueLocator: Locator;
  readonly qrVersionValueLocator: Locator;
  readonly notesValueLocator: Locator;
  readonly deleteButtonLocator: Locator;

  constructor(page: Page) {
    this.page = page;
    this.headingLocator = page.getByRole('heading', { name: 'QR Code Details' });
    this.qrCodeImageLocator = page.getByRole('img', { name: 'QR Code Preview' });
    this.contentValueLocator = page.locator('dt:has-text("Content") + dd');
    this.errorCorrectionValueLocator = page.locator('dt:has-text("Error Correction") + dd');
    this.qrVersionValueLocator = page.locator('dt:has-text("QR Version") + dd');
    this.notesValueLocator = page.locator('dt:has-text("Notes") + dd');
    this.deleteButtonLocator = page.getByRole('button', { name: 'Delete' });
  }

  async deleteQRCode() {
    await this.deleteButtonLocator.click();
  }
}
