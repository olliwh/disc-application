import { test, expect } from '@playwright/test';

test('basic test', async ({ page }) => {
  await page.goto('http://localhost:3000/');
  await expect(page.getByRole('button', { name: 'Login' })).toBeVisible();
  await expect(page.getByRole('button', { name: 'DiscProfiles' })).toBeVisible();
  await expect(page.getByRole('img', { name: 'Admin Admin' })).toBeVisible();
});


