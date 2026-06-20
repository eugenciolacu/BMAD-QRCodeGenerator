import { test, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';

test('existing user can log in successfully', async ({ page }) => {
  const loginPage = new LoginPage(page);
  loginPage.goto();
  await loginPage.login('test@example.com', 'Test..2026');
  await expect(page.getByText(/Logged user:/i)).toBeVisible();
  await expect(page.getByText('test@example.com')).toBeVisible();
});
