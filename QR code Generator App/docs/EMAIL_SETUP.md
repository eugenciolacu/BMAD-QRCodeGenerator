# Email Sender Configuration Guide

## Gmail SMTP Setup - Step by Step

### Problem: "Authentication Required" Error
If you're getting **"5.7.0 Authentication Required"** errors, follow these steps carefully:

---

### Step 1: Verify Gmail Account 2-Factor Authentication

1. Go to: https://myaccount.google.com/security
2. Under "Signing in to Google" → Click on "2-Step Verification"
3. **MUST BE ENABLED** - If not enabled, enable it now
4. Wait 5 minutes after enabling before proceeding

---

### Step 2: Generate App Password

1. Go to: https://myaccount.google.com/apppasswords
   - **OR** Search Google Account for "App Passwords"
   
2. If you see "App passwords are not available for accounts with 2-Step Verification turned off":
   - 2FA isn't enabled yet - go back to Step 1
   
3. Click **"Select app"** dropdown → Choose **"Mail"**

4. Click **"Select device"** dropdown → Choose **"Other (Custom name)"**
   - Enter: `QRCodeGenerator` or any name you want

5. Click **"GENERATE"**

6. Google will show a **16-character password** in a yellow box like:
   ```
   abcd efgh ijkl mnop
   ```

7. **IMPORTANT**: Copy this password **without spaces**: `abcdefghijklmnop`

---

### Step 3: Configure appsettings.json

Open `QRCodeGenerator/appsettings.json` and update the Email section:

```json
"Email": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SmtpUser": "your-gmail@gmail.com",
  "SmtpPass": "abcdefghijklmnop",
  "From": "your-gmail@gmail.com"
}
```

**Critical Rules:**
- ❌ NO spaces in the password
- ❌ NO quotes around the password (JSON handles it)
- ❌ NO "app pass:" prefix
- ✅ Exactly 16 lowercase letters (no spaces)
- ✅ Same email for SmtpUser and From

---

### Step 4: Test the Configuration

1. **Restart your application** (old config might be cached):
   ```powershell
   # Stop the app (Ctrl+C)
   dotnet run
   ```

2. Register a new user with a real email you can access

3. Check the logs at `Logs/log-YYYYMMDD.txt` for:
   ```
   EmailSender configured with SMTP server: smtp.gmail.com:587
   SMTP Client configured - User: your-email@gmail.com, SSL: True, Port: 587
   Email successfully sent to test@example.com with subject 'Confirm your email'
   ```

---

## Troubleshooting

### Error: "Authentication Required"
**Cause**: App password is incorrect or not configured properly

**Solutions**:
1. Verify 2FA is enabled on the Gmail account
2. Generate a **NEW** app password (old ones might be invalid)
3. Copy the password **without spaces**
4. Ensure no trailing spaces in appsettings.json
5. Restart the application after config changes

### Error: "Username and Password not accepted"
**Cause**: Trying to use regular Gmail password instead of app password

**Solution**: 
- Generate an app password (Step 2 above)
- Regular Gmail passwords will NOT work with SMTP

### Email Sends But Not Received
**Cause**: Email might be in spam or blocked

**Solutions**:
1. Check recipient's spam folder
2. Wait a few minutes (Gmail can delay)
3. Check Gmail's "Sent" folder to verify it was sent

### "App Passwords not available"
**Cause**: 2FA not enabled or Gmail workspace account restrictions

**Solutions**:
1. Enable 2-Factor Authentication first
2. If using Google Workspace, admin might need to enable app passwords

---

## Security Best Practices

### ⚠️ NEVER Commit Credentials to Source Control

**For Development:**
Use .NET User Secrets:
```powershell
cd QRCodeGenerator
dotnet user-secrets init
dotnet user-secrets set "Email:SmtpUser" "your-email@gmail.com"
dotnet user-secrets set "Email:SmtpPass" "your-app-password"
dotnet user-secrets set "Email:From" "your-email@gmail.com"
```

**For Production:**
- Use Azure Key Vault
- Use AWS Secrets Manager
- Use environment variables
- Use secure configuration providers

### Add to .gitignore

Ensure `appsettings.json` or `appsettings.Development.json` with real credentials is NOT committed:

```gitignore
# User-specific secrets
appsettings.Development.json
appsettings.Production.json
**/appsettings.*.json
```

---

## Testing Checklist

- [ ] 2FA enabled on Gmail account
- [ ] App password generated (16 characters)
- [ ] Password copied without spaces into appsettings.json
- [ ] Application restarted after config changes
- [ ] Registration sends confirmation email
- [ ] Email arrives in inbox (check spam)
- [ ] Confirmation link works
- [ ] Logs show "Email successfully sent"

---

## Alternative: Using SendGrid (Recommended for Production)

For production, consider using **SendGrid** instead of Gmail:

1. Sign up: https://sendgrid.com/pricing/ (Free tier: 100 emails/day)
2. Get API key
3. Update EmailSender to use SendGrid API
4. More reliable and better deliverability than Gmail SMTP

---

## Support

If still having issues:
1. Check `Logs/log-YYYYMMDD.txt` for detailed error messages
2. Verify Email section exists in appsettings.json
3. Ensure no typos in configuration keys
4. Try sending from Gmail web interface to verify account works
