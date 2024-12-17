namespace CookTheWeek.Services.Data.Helpers
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
                color: #333333;
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
                margin-top: 20px;
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
                <p>If you have any concerns, please contact our support team at <a href=""mailto:support@example.com"">support@example.com</a>.</p>
            </div>
            <div class=""footer"">
                &copy; 2024 CookTheWeek. All rights reserved.
            </div>
        </div>
    </body>
    </html>";
        }

    }
}
