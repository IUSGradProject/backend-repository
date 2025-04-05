using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using APIs;
using BLL.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BLL.Services
{
    public class EmailService : BackgroundService, IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _username;
        private readonly string _password;
        private readonly string _fromEmail;
        private readonly string APIKey;
        private readonly IServiceProvider _serviceProvider;

        public EmailService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _smtpServer = configuration["EmailSettings:SMTPServer"];
            _smtpPort = int.Parse(configuration["EmailSettings:SMTPPort"]);
            _username = configuration["EmailSettings:Username"];
            _password = configuration["EmailSettings:Password"];
            _fromEmail = configuration["EmailSettings:FromEmail"];
            APIKey = configuration["EmailSettings:APIkey"];
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessInactiveUsersAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in EmailService: {ex.Message}");
                }

                // Delay before the next check
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessInactiveUsersAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                //Truncate time
                var date = DateTime.UtcNow;
                DateTime cutoffDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute - 2, 0);

                var inactiveUsers = await userService.GetInactiveUsers(cutoffDate);

                foreach (var user in inactiveUsers)
                {
                    string subject = "We Miss You! Your Cart is Waiting 🛍️";
                    string htmlBody = GetHtmlEmailTemplate();

                    await SendEmailAsync(user.Email, subject, htmlBody);
                }
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_username, _password);
                client.EnableSsl = true; 
                

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, "Aurora"),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                await client.SendMailAsync(mailMessage);
            }

        }
        public async Task SendOrderConfirmationEmailAsync(string toEmail, string productName, decimal price, int quantity)
        {
            decimal totalPrice = price * quantity;
            string HtmlBody = GetOrderEmailTemplate(productName, price, quantity, totalPrice);
            string subject = "Your Aurora Order Confirmation ✨";
            await SendEmailAsync(toEmail, subject, HtmlBody);
        }

        private string GetOrderEmailTemplate(string productName, decimal price, int quantity, decimal totalPrice)
        {
            return $@"
<html>
<head>
    <style>
        body {{
            font-family: 'Arial', sans-serif;
            background-color: #f3f4f6;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: auto;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #4CAF50;
            text-align: center;
            font-size: 28px;
            margin-bottom: 20px;
        }}
        .p{{
            font-size:15px;
            }}
        .order-details {{
            margin-top: 20px;
            border-top: 2px solid #f0f0f0;
            padding-top: 20px;
        }}
        .order-details div {{
            margin-bottom: 15px;
        }}
        .order-details strong {{
            color: #333333;
            font-size: 16px;
        }}
        .order-details span {{
            color: #555555;
            font-size: 16px;
        }}
        .total-price {{
            font-size: 18px;
            font-weight: bold;
            color: #ff6f61;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 14px;
            text-align: center;
            color: #888888;
        }}
        .btn {{
            display: inline-block;
            background-color: #4CAF50;
            color: white;
            padding: 12px 25px;
            text-decoration: none;
            border-radius: 5px;
            text-align: center;
            font-size: 16px;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h2>Thank You for Your Order! 🎉</h2>
        <p style='text-align: center;'>We are excited to process your order. Below are the details of your recent purchase:</p>
        <div class='order-details'>
            <div>
                <strong>Product:</strong> <span>{productName}</span>
            </div>
            <div>
                <strong>Price per product:</strong> <span>${price}</span>
            </div>
            <div>
                <strong>Quantity:</strong> <span>{quantity}</span>
            </div>
            <div class='total-price'>
                <strong>Total Price:</strong> <span>${totalPrice}</span>
            </div>
        </div>
        <p style='text-align: center;'>We’ll notify you once it’s shipped. 🚚</p>
        <p class='footer'>Need help? Contact support@aurora.com</p>
        <p class='footer'>
            <a href='https://yourstore.com' class='btn'>Visit Our Store</a>
        </p>
    </div>
</body>
</html>";
        }


        private string GetHtmlEmailTemplate()
        {
            return @"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body { font-family: Arial, sans-serif; color: #333; text-align: center; padding: 20px; }
            .container { max-width: 600px; margin: auto; background: #f8f8f8; padding: 20px; border-radius: 10px; }
            .btn { background-color: #ff6f61; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; margin-top: 20px; }
            .footer { margin-top: 20px; font-size: 12px; color: gray; }
        </style>
    </head>
    <body>
        <div class='container'>
            <h2>Hey, We Miss You! 💖</h2>
            <p>Your cart still has some great items waiting for you!</p>
            <p>Come back and grab them before they sell out. 🎉</p>
            <a href='https://yourstore.com/cart' class='btn'>Return to Your Cart</a>
            <p class='footer'>If you have any questions, contact us at support@aurora.com.</p>
        </div>
    </body>
    </html>";
        }
    }
}
