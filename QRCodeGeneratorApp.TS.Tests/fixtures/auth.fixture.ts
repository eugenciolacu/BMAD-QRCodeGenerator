import { test as base, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';

export const test = base.extend({
  page: async ({ page }, use) => {
    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.login('test@example.com', 'Test..2026');
    await expect(page.getByText(/Logged user:/i)).toBeVisible();
    await use(page);
  },
});
