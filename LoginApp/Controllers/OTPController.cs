using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LoginApp.Controllers
{
    public class OTPController : Controller
    {
        private string? storedOTP;

        public IActionResult Index()
        {
            return View();
        }

        // Generate OTP
        public string OTPGenerate()
        {
            int OTPLength = 4;
            string[] AllowedCharacter = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string OTP = string.Empty;
            Random rand = new Random();
            for (int i = 0; i < OTPLength; i++)
            {
                int randomIndex = rand.Next(0, AllowedCharacter.Length);
                string randomChar = AllowedCharacter[randomIndex];
                OTP += randomChar;
            }
            return OTP;
        }

        // Send OTP in Email
        public async Task<IActionResult> SendEmail()
        {
            string otp = OTPGenerate();
            storedOTP = otp;
            var fromAddress = new MailAddress("Sender Email Address", "SenderName");
            var toAddress = new MailAddress("Receiver Email Address", "Receiver Name");
            const string subject = "OTP Verification";
            string body = $"Your OTP: {otp}";
            using (var message = new MailMessage(fromAddress, toAddress))
            {
                message.Subject = subject;
                message.Body = body;
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("Sender Email Address", "Sender SMTP credentials");
                    smtpClient.EnableSsl = true;
                    try
                    {
                        await smtpClient.SendMailAsync(message);
                        ViewBag.SuccessMessage = "OTP sent successfully!";
                    }
                    catch (SmtpException ex)
                    {
                        ViewBag.ErrorMessage = $"Failed to send OTP: {ex.Message}";
                    }
                }
            }
            return View("Index");
        }



        [HttpPost]

        public IActionResult VerifyOTP(string enteredOTP)
        {
            HttpContext.Session.SetString("OTP", enteredOTP);
            enteredOTP=HttpContext.Session.GetString(enteredOTP);
            if (enteredOTP == storedOTP)
            {
                // OTP is correct, perform the desired action (e.g., redirect to success page)
                return RedirectToAction("Index", "Customers");
            }
            else
            {
                // OTP is incorrect, display error message
                ViewBag.ErrorMessage = "Wrong OTP entered.";
                return View("Index");
            }
        }
    }
}
