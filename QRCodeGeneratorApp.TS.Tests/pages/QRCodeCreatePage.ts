import { type Page, type Locator } from '@playwright/test';

export class QRCodeCreatePage {
  readonly page: Page;
  readonly inputStringLocator: Locator;
  readonly errorCorrectionLevelLocator: Locator;
  readonly qrVersionLocator: Locator;
  readonly notesLocator: Locator;
  readonly generateAndSaveButtonLocator: Locator;

  constructor(page: Page) {
    this.page = page;
    this.inputStringLocator = page.getByRole('textbox', { name: 'Up to 100 printable ASCII' });
    this.errorCorrectionLevelLocator = page.getByLabel('Error Correction Level');
    this.qrVersionLocator = page.getByLabel('QR Version');
    this.notesLocator = page.getByRole('textbox', { name: 'Optional annotation...' });
    this.generateAndSaveButtonLocator = page.getByRole('button', { name: 'Generate & Save' });
  }

  async goto() {
    await this.page.goto('/QRCodes/Create');
  }

  async fillForm(
    inputString: string,
    errorCorrectionLevel: string,
    qrVersion: string,
    notes: string,
  ) {
    await this.inputStringLocator.fill(inputString);
    await this.errorCorrectionLevelLocator.selectOption(errorCorrectionLevel);
    await this.qrVersionLocator.selectOption(qrVersion);
    await this.notesLocator.fill(notes);
  }

  async submitForm() {
    await this.generateAndSaveButtonLocator.click();
  }
}
