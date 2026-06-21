import { type Page, type Locator } from '@playwright/test';

export class QRCodeListPage {
  readonly page: Page;
  readonly filterContentInputLocator: Locator;
  readonly filterErrorCorrectionCapacityInputLocator: Locator;
  readonly filterVersionInputLocator: Locator;
  readonly filterDateFromInputLocator: Locator;
  readonly filterDateToInputLocator: Locator;
  readonly filterNotesInputLocator: Locator;
  readonly applyFilterButtonLocator: Locator;
  readonly clearFilterLinkLocator: Locator;
  readonly deleteButtonLocator: Locator;

  constructor(page: Page) {
    this.page = page;
    this.filterContentInputLocator = page.locator('input[name="filterContent"]');
    this.filterErrorCorrectionCapacityInputLocator = page.locator('input[name="filterEcc"]');
    this.filterVersionInputLocator = page.locator('input[name="filterVersion"]');
    this.filterDateFromInputLocator = page.locator('input[name="filterDateFrom"]');
    this.filterDateToInputLocator = page.locator('input[name="filterDateTo"]');
    this.filterNotesInputLocator = page.locator('input[name="filterNotes"]');
    this.applyFilterButtonLocator = page.getByRole('button', { name: 'Apply' });
    this.clearFilterLinkLocator = page.getByRole('link', { name: 'Clear' });
    this.deleteButtonLocator = page.getByRole('button', { name: 'Delete' });
  }

  async goto() {
    await this.page.goto('/QRCodes');
  }

  async deleteQRCode() {
    await this.deleteButtonLocator.click();
  }

  async applyFilter(
    content: string = '',
    ecc: string = '',
    version: string = '',
    dateFrom: string = '',
    dateTo: string = '',
    notes: string = '',
  ) {
    await this.filterContentInputLocator.fill(content);
    await this.filterErrorCorrectionCapacityInputLocator.fill(ecc);
    await this.filterVersionInputLocator.fill(version);
    await this.filterDateFromInputLocator.fill(dateFrom);
    await this.filterDateToInputLocator.fill(dateTo);
    await this.filterNotesInputLocator.fill(notes);

    await this.applyFilterButtonLocator.click();
  }
}
