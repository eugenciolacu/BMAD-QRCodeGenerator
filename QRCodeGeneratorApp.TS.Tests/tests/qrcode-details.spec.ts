import { test } from '../fixtures/auth.fixture';
import { expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';
import { QRCodeCreatePage } from '../pages/QRCodeCreatePage';
import { QRCodeDetailsPage } from '../pages/QRCodeDetailsPage';

const testUser = { email: 'test@example.com', password: 'Test..2026' };

test.describe('QR code details', () => {
  test.describe.configure({ mode: 'serial' });
  let detailsUrl: string;

  test.beforeAll(async ({ browser }) => {
    const context = await browser.newContext();
    const page = await context.newPage();

    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.login(testUser.email, testUser.password);

    const createPage = new QRCodeCreatePage(page);
    await createPage.goto();
    await createPage.fillForm('Details Test QR', 'M', '5', 'beforeAll note');
    await createPage.submitForm();

    detailsUrl = new URL(page.url()).pathname;
    await context.close();
  });

  test('authenticated user can access QR code details page', async ({ page }) => {
    const detailsPage = new QRCodeDetailsPage(page);
    await page.goto(detailsUrl);
    await expect(detailsPage.headingLocator).toBeVisible();
  });

  test('details page shows created QR code details', async ({ page }) => {
    const detailsPage = new QRCodeDetailsPage(page);
    await page.goto(detailsUrl);
    await expect(detailsPage.qrCodeImageLocator).toBeVisible();
    await expect(detailsPage.contentValueLocator).toHaveText('Details Test QR');
    await expect(detailsPage.errorCorrectionValueLocator).toHaveText('M');
    await expect(detailsPage.qrVersionValueLocator).toHaveText('5');
    await expect(detailsPage.notesValueLocator).toHaveText('beforeAll note');
  });
});
