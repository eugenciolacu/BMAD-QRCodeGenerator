import { test, expect } from '@playwright/test';
import { test as authTest } from '../fixtures/auth.fixture';
import { HomePage } from '../pages/HomePage';

test.describe('anonymous user', () => {
  let homePage: HomePage;

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page);
    await homePage.goto();
  });

  test('homepage has correct title', async ({ page }) => {
    await expect(page).toHaveTitle(/Dashboard - QR Code Generator/i);
  });

  test('register link is present', async () => {
    await expect(homePage.registerLink).toBeVisible();
  });

  test('login link is present', async () => {
    await expect(homePage.loginLink).toBeVisible();
  });
});

authTest.describe('authenticated user', () => {
  let homePage: HomePage;

  authTest.beforeEach(async ({ page }) => {
    homePage = new HomePage(page);
    await homePage.goto();
  });

  authTest('greeting with email is visible', async () => {
    await expect(homePage.userGreeting).toBeVisible();
  });

  authTest('create QR code link is present', async () => {
    await expect(homePage.createQRCodeLink).toBeVisible();
  });

  authTest('view my QR codes link is present', async () => {
    await expect(homePage.viewMyQRCodesLink).toBeVisible();
  });
});
