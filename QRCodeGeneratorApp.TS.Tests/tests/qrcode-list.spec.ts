import { test } from './../fixtures/auth.fixture';
import { expect } from '@playwright/test';
import { QRCodeListPage } from '../pages/QRCodeListPage';

test('authenticated user can access QR code list page', async ({ page }) => {
  const qrCodeListPage = new QRCodeListPage(page);
  await qrCodeListPage.goto();
  await expect(page.getByRole('heading', { name: 'My QR Codes' })).toBeVisible();
});
