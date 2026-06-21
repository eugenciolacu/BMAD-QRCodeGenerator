import { test } from '../fixtures/auth.fixture';
import { Browser, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';
import { QRCodeCreatePage } from '../pages/QRCodeCreatePage';
import { QRCodeDetailsPage } from '../pages/QRCodeDetailsPage';
import { QRCodeListPage } from '../pages/QRCodeListPage';

const testUser = { email: 'test@example.com', password: 'Test..2026' };

async function createQRCode(browser: Browser, notes: string) {
  const context = await browser.newContext();
  const page = await context.newPage();

  const loginPage = new LoginPage(page);
  await loginPage.goto();
  await loginPage.login(testUser.email, testUser.password);
  await expect(page.getByText(/Logged user:/i)).toBeVisible();

  const createPage = new QRCodeCreatePage(page);
  await createPage.goto();
  await createPage.fillForm('Details Test QR', 'M', '5', notes);
  await createPage.submitForm();

  const url = new URL(page.url()).pathname;
  await context.close();
  return url;
}

test.describe('QR code deletion from details page', () => {
  test.describe.configure({ mode: 'serial' });
  let detailsUrl: string;

  test.beforeAll(async ({ browser }) => {
    detailsUrl = await createQRCode(browser, 'to be deleted from details page');
  });

  test('authenticated user can delete a QR code from details page', async ({ page }) => {
    const detailsPage = new QRCodeDetailsPage(page);
    await page.goto(detailsUrl);
    await detailsPage.deleteQRCode();
    await page.getByLabel('Confirm Deletion').getByRole('button', { name: 'Delete' }).click();
    await expect(page).toHaveURL('/QRCodes');
  });
});

test.describe('QR code deletion from My QR Codes page', () => {
  test.describe.configure({ mode: 'serial' });

  test.beforeAll(async ({ browser }) => {
    await createQRCode(browser, 'to be deleted from My QR Codes page');
  });

  test('authenticated user can delete a QR code from My QR Codes page', async ({ page }) => {
    const listPage = new QRCodeListPage(page);
    await listPage.goto();
    await listPage.applyFilter('', '', '', '', '', 'to be deleted from My QR Codes page');
    await page.getByRole('cell', { name: 'to be deleted from My QR Codes page' }).first().click();
    await listPage.deleteQRCode();
    await page.getByLabel('Confirm Deletion').getByRole('button', { name: 'Delete' }).click();
    await page.waitForTimeout(500); // Wait for the deletion to be processed
    await expect(
      page.getByRole('cell', { name: 'to be deleted from My QR Codes page' }),
    ).toHaveCount(0);
  });
});
