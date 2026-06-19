import { test, expect } from '@playwright/test';

test('existing user can log in', async ({ page }) => {
  // Logic: Go to the login page (which will be http://webapp:8080 in Docker)
  await page.goto('/Identity/Account/Login');

  // Fill in the login form with the seeded test account
  await page.getByLabel('Email').fill('test@example.com');
  await page.getByLabel('Password').fill('Test..2026');
  await page.getByRole('button', { name: 'Log in' }).click();

  const loggedUserText = page.getByText(/Logged user:/i);
  await expect(loggedUserText).toBeVisible();

  const loggedUserEmail = page.getByText(/test@example\.com/i);
  await expect(loggedUserEmail).toBeVisible();
});
