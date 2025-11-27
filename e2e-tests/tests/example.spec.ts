import { test, expect } from '@playwright/test';

test('basic test', async ({ page }) => {
  await page.goto('/', { waitUntil: 'networkidle' });
  await page.waitForLoadState('domcontentloaded');
  
  await expect(page.getByRole('button', { name: 'Login' })).toBeVisible({ timeout: 10000 });
  await expect(page.getByRole('button', { name: 'DiscProfiles' })).toBeVisible({ timeout: 10000 });
  await expect(page.getByRole('img', { name: 'Admin Admin' })).toBeVisible({ timeout: 10000 });
});


