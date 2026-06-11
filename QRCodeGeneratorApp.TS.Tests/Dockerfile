# Use the official Microsoft Playwright image (includes Node.js and Browsers)
FROM mcr.microsoft.com/playwright:v1.60.0-jammy

WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy the rest of your test code
COPY . .

# Command to run tests
# We use --project=chromium as an example
# CMD ["npx", "playwright", "test"]
# Keep container alive, run tests manually via exec for better control and debugging
CMD ["sleep", "infinity"]