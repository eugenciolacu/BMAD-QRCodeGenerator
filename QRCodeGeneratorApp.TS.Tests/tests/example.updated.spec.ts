import { test, expect } from '@playwright/test';

test('homepage has correct title', async ({ page }) => {
  // Logic: Go to the home page (which will be http://webapp:8080 in Docker)
  await page.goto('/');

  // Update this to match your actual app's title
  await expect(page).toHaveTitle(/QR Code Generator/i);
});

test('can see register link', async ({ page }) => {
  await page.goto('/');
  
  // Checking if the Identity system (Register link) is visible
  const registerLink = page.getByRole('link', { name: /Register/i });
  await expect(registerLink).toBeVisible();
});