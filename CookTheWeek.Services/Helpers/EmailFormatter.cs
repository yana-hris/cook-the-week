﻿namespace CookTheWeek.Services.Data.Helpers
{
    using System.Web;

    internal class EmailFormatter
    {
        internal string GetPasswordResetHtmlContent(string userName, string callbackUrl, string tokenExpirationTime)
        {
            string safeCallbackUrl = HttpUtility.HtmlEncode(callbackUrl);

            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Password Reset Request</title>
        <style>
@import url('https://fonts.googleapis.com/css2?family=League+Spartan:wght@400;700&display=swap');
            body {{
                font-family: 'League Spartan', Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
                line-height: 1.6;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background: #ffffff;
                padding: 20px;
                border: 1px solid #ddd;
                border-radius: 5px;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            }}
            .email-header {{
                text-align: center;
                margin-bottom: 20px;
            }}
            .email-body {{
                font-size: 16px;
                margin-bottom: 20px;
            }}
            a {{
                color: #2e6930 !important;
                text-decoration: none;
            }}
            a:hover {{
                color: #2e6930 !important;
                text-decoration: underline;
            }}
            .button {{
                display: inline-block;
                padding: 10px 20px;
                font-size: 16px;
                font-weight: bold;
                color: #fff !important;
                border: 1px solid #2e6930;
                background-color: #2e6930;
                text-decoration: none;
                border-radius: 5px;
                text-align: center;
            }}
            .button:hover {{
                background-color: #fff;
                color: #2e6930 !important;
                border: 1px solid #2e6930;
                text-decoration: none !important;
            }}
            .footer {{
                font-size: 12px;
                color: #2e6930;
                text-align: center;
                margin-top: 30px;
            }}
        </style>
    </head>
    <body>
        <div class=""email-container"">
            <div class=""email-header"">
                <img src=""https://i.imgur.com/M9KlsA5.png"" alt=""CookTheWeek Logo"" width=""auto"" height=""80px"" style=""display: block; margin: 0 auto; max-height: 6rem;"">
            </div>
            <div class=""email-body"">
                <p>Dear <strong>{userName}</strong>,</p>
                <p>We received a request to reset the password for your account. If you did not request this change, <strong>please ignore this email</strong>. Your account remains secure, and no changes have been made.</p>
                <p>If you did request a password reset, please click the button below to create a new password:</p>
                <p style=""text-align: center;"">
                    <a href=""{safeCallbackUrl}"" class=""button"">Reset Password</a>
                </p>
                <p>The link will remain active until <strong>{tokenExpirationTime}</strong>.</p>
                <p>If you have any concerns, please contact our support team at <a href=""mailto:support@cooktheweek.com"">support@cootheweek.com</a>.</p>
            </div>
            <div class=""footer"">
                &copy; 2024 CookTheWeek. All rights reserved.
            </div>
        </div>
    </body>
    </html>";
        }

        internal string GetEmailConfirmationHtml(string userName, string callbackUrl)
        {
            return $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Welcome to CookTheWeek!</title>
        <style>
            @import url('https://fonts.googleapis.com/css2?family=League+Spartan:wght@400;700&display=swap');
            body {{
                font-family: 'League Spartan', Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
                line-height: 1.6;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background: #ffffff;
                padding: 20px;
                border: 1px solid #ddd;
                border-radius: 5px;
                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            }}
            .email-header {{
                text-align: center;
                margin-bottom: 20px;
            }}
            .email-body {{
                font-size: 16px;
                margin-bottom: 20px;
            }}
            a {{
                color: #2e6930 !important;
                text-decoration: none;
            }}
            a:hover {{
                color: #2e6930 !important;
                text-decoration: underline;
            }}
            .button {{
                display: inline-block;
                padding: 10px 20px;
                font-size: 16px;
                font-weight: bold;
                color: #fff !important;
                border: 1px solid #2e6930;
                background-color: #2e6930;
                text-decoration: none;
                border-radius: 5px;
                text-align: center;
            }}
            .button:hover {{
                background-color: #fff;
                color: #2e6930 !important;
                border: 1px solid #2e6930;
                text-decoration: none !important;
            }}            
            .welcome-text {{
                text-align: center;
                font-size: 22px;
                font-weight: bold;
                margin: 20px 0;
            }}
            
            .how-it-works {{
                text-align: left;
                margin-top: 30px;
            }}
            .step {{
                margin: 20px 0;
            }}
            .step-number {{
                font-size: 28px;
                font-weight: bold;
                color: #28a745;
                margin-right: 10px;
                display: inline-block;
            }}
            .step-text {{
                display: inline-block;
                vertical-align: top;
            }}
            .footer {{
                font-size: 12px;
                color: #2e6930;
                text-align: center;
                margin-top: 30px;
            }}
        </style>
    </head>
    <body>
        <div class=""email-container"">
            <!-- Header -->
            <div class=""email-header"">
                <img src=""https://i.imgur.com/M9KlsA5.png"" alt=""CookTheWeek Logo"" width=""auto"" height=""80px"" style=""display: block; margin: 0 auto; max-height: 6rem;"">
            </div>

            <!-- Welcome Message -->
            <div class=""welcome-text"">
                Welcome to CookTheWeek, <span style=""color: #28a745;"">{userName}</span>!
            </div>

            <!-- Confirmation Button -->
            <div class=""button"">
                <p>Please confirm your email address by clicking the button below:</p>
                <a href=""{callbackUrl}"">Confirm Email</a>
            </div>

            <!-- How It Works Section -->
            <div class=""how-it-works"">
                <h3 style=""text-align: center;"">Your Weekly Meals in 4 Simple Steps</h3>
                <div class=""step"">
                    <span class=""step-number"">01</span>
                    <div class=""step-text"">
                        <strong>Pick Recipes</strong> <br>
                        Browse through our extensive collection of delicious recipes from various cuisines.
                    </div>
                </div>
                <div class=""step"">
                    <span class=""step-number"">02</span>
                    <div class=""step-text"">
                        <strong>Plan Your Meals</strong> <br>
                        Add recipes to your meal plan and organize your weekly schedule effortlessly.
                    </div>
                </div>
                <div class=""step"">
                    <span class=""step-number"">03</span>
                    <div class=""step-text"">
                        <strong>Go Shopping</strong> <br>
                        Generate a personalized shopping list based on your selected meals.
                    </div>
                </div>
                <div class=""step"">
                    <span class=""step-number"">04</span>
                    <div class=""step-text"">
                        <strong>Enjoy Cooking</strong> <br>
                        Follow step-by-step instructions to cook delicious meals at home.
                    </div>
                </div>
            </div>

            <!-- Footer -->
            <div class=""footer"">
                &copy; 2024 CookTheWeek. All rights reserved. <br>
                If you have any questions, contact us at <a href=""mailto:support@cooktheweek.com"">support@cooktheweek.com</a>.
            </div>
        </div>
    </body>
    </html>";
        }

    }
}
