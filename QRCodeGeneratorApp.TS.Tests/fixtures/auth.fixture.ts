import { test as base } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';

export const test = base.extend({
  page: async ({ page }, use) => {
    const loginPage = new LoginPage(page);
    loginPage.goto();
    await loginPage.login('test@example.com', 'Test..2026');
    await use(page);
  },
});
