import { test } from './../fixtures/auth.fixture';
import { expect } from '@playwright/test';
import { QRCodeCreatePage } from '../pages/QRCodeCreatePage';

test('authenticated user can access QR code create page', async ({ page }) => {
  const qrCodeCreatePage = new QRCodeCreatePage(page);
  await qrCodeCreatePage.goto();
  await expect(page.getByRole('heading', { name: 'Create QR Code' })).toBeVisible();
});

test('authenticated user can fill out QR code create form', async ({ page }) => {
  const qrCodeCreatePage = new QRCodeCreatePage(page);
  await qrCodeCreatePage.goto();
  await qrCodeCreatePage.fillForm('Hello, World!', 'M', '5', '"Hello, World!" QR Code');
  await qrCodeCreatePage.submitForm();
  await expect(page.getByRole('heading', { name: 'QR Code Details' })).toBeVisible();
});

test('authenticated user sees validation error for empty input string', async ({ page }) => {
  const qrCodeCreatePage = new QRCodeCreatePage(page);
  await qrCodeCreatePage.goto();
  await qrCodeCreatePage.fillForm('', 'M', '5', '"Hello, World!" QR Code');
  await qrCodeCreatePage.submitForm();
  await expect(page.locator('#decodedTextInput-error')).toBeVisible();
});
