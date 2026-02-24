// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace QRCodeGeneratorApp.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ResetPasswordModel> _logger;

        public ResetPasswordModel(UserManager<IdentityUser> userManager, ILogger<ResetPasswordModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            public string Code { get; set; }

        }

        public IActionResult OnGet(string code = null, string email = null)
        {
            if (code == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid password reset link. Please request a new password reset.");
                return Page();
            }

            try
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                    Email = email ?? string.Empty
                };
                return Page();
            }
            catch (Exception ex)
            {
                // Handle malformed token
                _logger.LogWarning(ex, "Malformed password reset token received");
                ModelState.AddModelError(string.Empty, "Invalid password reset link. The link may be corrupted. Please request a new password reset.");
                Input = new InputModel { Email = email ?? string.Empty };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password successfully reset for user: {UserId}", user.Id);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            // Handle password reset errors (invalid/expired token, password policy violations, etc.)
            _logger.LogWarning("Password reset failed for user {UserId}: {Errors}", user.Id, string.Join(", ", result.Errors.Select(e => e.Code)));
            foreach (var error in result.Errors)
            {
                // Provide user-friendly messages for common token errors
                if (error.Code == "InvalidToken")
                {
                    ModelState.AddModelError(string.Empty, "The password reset link is invalid or has expired. Please request a new password reset.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
