import { test, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';
import { RegisterPage } from '../pages/RegisterPage';

const testUser = { email: 'test@example.com', password: 'Test..2026' };

const registerTestUsers = [
  { email: 'user1@example.com', password: 'Test..2026' },
  { email: 'user2@example.com', password: 'Test..2026' },
];

test('existing user can log in successfully', async ({ page }) => {
  const loginPage = new LoginPage(page);
  loginPage.goto();
  await loginPage.login(testUser.email, testUser.password);
  await expect(page.getByText(/Logged user:/i)).toBeVisible();
  await expect(page.getByText(testUser.email)).toBeVisible();
});

test.describe('registration', () => {
  test.skip(
    ({ browserName }) => browserName !== 'chromium',
    'Runs on Chromium only to avoid duplicate email conflicts across browsers',
  );

  for (const { email, password } of registerTestUsers) {
    test(`register user with email ${email} successfully`, async ({ page }) => {
      const registerPage = new RegisterPage(page);
      registerPage.goto();
      await registerPage.register(email, password);
      await expect(page.getByText(/Logged user:/i)).toBeVisible();
      await expect(page.getByText(email)).toBeVisible();
    });
  }
});
