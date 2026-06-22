import { test } from './../fixtures/auth.fixture';
import { expect } from '@playwright/test';
import { QRCodeListPage } from '../pages/QRCodeListPage';
import { QRCodeCreatePage } from '../pages/QRCodeCreatePage';
import { RegisterPage } from '../pages/RegisterPage';

test('authenticated user can access QR code list page', async ({ page }) => {
  const qrCodeListPage = new QRCodeListPage(page);
  await qrCodeListPage.goto();
  await expect(page.getByRole('heading', { name: 'My QR Codes' })).toBeVisible();
});

test('authenticated user cannot see QR codes of other users', async ({ page, browserName }) => {
  test.skip(
    browserName !== 'chromium',
    'Runs on Chromium only to avoid duplicate email conflicts across browsers',
  );

  const createPage = new QRCodeCreatePage(page);
  await createPage.goto();
  await createPage.fillForm('1st QR code', 'M', '5', 'first QR code created by test@example.com');
  await createPage.submitForm();

  await createPage.goto();
  await createPage.fillForm('2nd QR code', 'M', '5', 'second QR code created by test@example.com');
  await createPage.submitForm();

  // Logout by clearing session cookies
  await page.context().clearCookies();

  const registerPage = new RegisterPage(page);
  await registerPage.goto();
  await registerPage.register('anotherTestUser@example.com', 'Test..2026');
  await expect(page.getByText(/anotherTestUser@example.com/i)).toBeVisible();

  const qrCodeListPage = new QRCodeListPage(page);
  await qrCodeListPage.goto();
  await qrCodeListPage.applyFilter('', '', '', '', '', 'first QR code created by test@example.com');
  await expect(
    page.getByRole('cell', { name: 'first QR code created by test@example.com' }),
  ).toHaveCount(0);

  await qrCodeListPage.applyFilter(
    '',
    '',
    '',
    '',
    '',
    'second QR code created by test@example.com',
  );
  await expect(
    page.getByRole('cell', { name: 'second QR code created by test@example.com' }),
  ).toHaveCount(0);
});
