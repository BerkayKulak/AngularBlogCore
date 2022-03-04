using System;
using System.Net.Mail;
using AngularBlogCore.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularBlogCore.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        [HttpPost]

        public IActionResult SendContactEmail(Contact contact)

        {

            System.Threading.Thread.Sleep(3000);

            try

            {

                MailMessage mailMessage = new MailMessage();



                SmtpClient smtpClient = new SmtpClient();



                mailMessage.From = new MailAddress("kulakberkay15@gmail.com");

                mailMessage.To.Add("beko_468@hotmail.com"); // kime gidicek email burada belirtiyoruz



                mailMessage.Subject = contact.Subject;

                mailMessage.Body = contact.Message;

                mailMessage.IsBodyHtml = true;

                smtpClient.Host = "smtp.gmail.com";

                smtpClient.Port = 587;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtpClient.UseDefaultCredentials = false;

                smtpClient.Credentials = new System.Net.NetworkCredential("kulakberkay15@gmail.com", "43795164825Tr*");

                smtpClient.EnableSsl = true;

                smtpClient.Send(mailMessage);

                return Ok();

            }

            catch (Exception ex)

            {

                return BadRequest(ex.Message);

            }

        }

    }
}
