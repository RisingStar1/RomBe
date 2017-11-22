using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using RomBe.Helpers;
using RomBe.Entities.DAL;
using RomBe.Entities;
using RomBe.Entities.Class;
using RomBe.Entities.Class.Common;
using RomBe.Entities.Class.Request;

namespace RomBe.Logic
{
    public class EmailLogic
    {

        private bool _isEMailSent = false;
        private string _smtpAddress = SystemConfigurationHelper.SmtpServerAddress;
        private int _portNumber = SystemConfigurationHelper.SmtpServerPort;
        private bool _enableSSL = true;
        private string _emailFromAddress = SystemConfigurationHelper.EmailFromAddress;
        private string _password = SystemConfigurationHelper.EmailFromPassword;

        public async Task SendActivationEmailAsync(string emailTo)
        {
            string _activationEmailCode = "A1";
            try
            {
                SystemMessage _emailFormat = await new SystemMessageDAL().GetSystemMessageAsync(_activationEmailCode);
                if (_emailFormat.IsNull())
                {
                    return;
                }



                string _subject = _emailFormat.MessageTitle;
                string _body = await BuildActivationEmailBodyAsync(emailTo, _emailFormat.MessageContent);
                if (_body.IsNull())
                {
                    return;
                }
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailFromAddress);
                    mail.To.Add(emailTo);
                    mail.Subject = _subject;
                    mail.Body = _body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
                        smtp.EnableSsl = _enableSSL;
                        smtp.SendCompleted += SendCompletedCallback;
                        await smtp.SendMailAsync(mail).ConfigureAwait(false);
                    }
                    //mark the email as sent in db
                    if (_isEMailSent)
                    {
                        await new EmailDAL().CreateEmailLog(emailTo);
                    }
                }

            }
            catch (Exception)
            {
                //throw;
            }
        }

        public async Task SendRecoverPasswordEmailAsync(string emailTo)
        {
            string _recoverPasswordEmailCode = "A2";
            try
            {
                SystemMessage _emailFormat = await new SystemMessageDAL().GetSystemMessageAsync(_recoverPasswordEmailCode);
                if (_emailFormat.IsNull())
                {
                    return;
                }
                string _subject = _emailFormat.MessageTitle;
                string _body = await BuildRecoverPasswordEmailBodyAsync(emailTo, _emailFormat.MessageContent);
                if (_body.IsNull())
                {
                    return;
                }
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailFromAddress);
                    mail.To.Add(emailTo);
                    mail.Subject = _subject;
                    mail.Body = _body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
                        smtp.EnableSsl = _enableSSL;
                        smtp.SendCompleted += SendCompletedCallback;
                        await smtp.SendMailAsync(mail).ConfigureAwait(false);
                    }
                    //mark the email as sent in db
                    if (_isEMailSent)
                    {
                        await new EmailDAL().CreateEmailLog(emailTo);
                    }
                }

            }
            catch (Exception)
            {
                //throw;
            }
        }

        public async Task<bool> SendSubscribeEmailAsync(string emailTo)
        {
            string _subscribeEmailCode = "I3";
            try
            {
                SystemMessage _emailFormat = await new SystemMessageDAL().GetSystemMessageAsync(_subscribeEmailCode);
                if (_emailFormat.IsNull())
                {
                    return false;
                }
                string _subject = _emailFormat.MessageTitle;
                string _body = BuildSubscribeEmailBodyAsync(_emailFormat.MessageContent);
                if (_body.IsNull())
                {
                    return false;
                }
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailFromAddress);
                    mail.To.Add(emailTo);
                    mail.Subject = _subject;
                    mail.Body = _body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
                        smtp.EnableSsl = _enableSSL;
                        smtp.SendCompleted += SendCompletedCallback;
                        await smtp.SendMailAsync(mail).ConfigureAwait(false);
                    }
                    //mark the email as sent in db
                    if (_isEMailSent)
                    {
                        await new EmailDAL().CreateEmailLog(emailTo);
                    }
                }
                return _isEMailSent;

            }
            catch (Exception)
            {
                //throw;
                return _isEMailSent;
            }
        }

        public async Task<bool> SendContactUsEmailAsync(ContactRequest request)
        {
            string emailTo = "tal@rombe.me";
            try
            {

                string _subject = "Contact us from the web";
                string _body = BuildContactUsEmailBodyAsync(request);
                if (_body.IsNull())
                {
                    return false;
                }
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_emailFromAddress);
                    mail.To.Add(emailTo);
                    mail.Subject = _subject;
                    mail.Body = _body;
                    mail.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
                        smtp.EnableSsl = _enableSSL;
                        smtp.SendCompleted += SendCompletedCallback;
                        await smtp.SendMailAsync(mail).ConfigureAwait(false);
                    }
                    //mark the email as sent in db
                    if (_isEMailSent)
                    {
                        await new EmailDAL().CreateEmailLog(emailTo);
                    }
                }
                return _isEMailSent;

            }
            catch (Exception)
            {
                //throw;
                return _isEMailSent;
            }
        }
        private async Task<string> BuildActivationEmailBodyAsync(string email, string emailBody)
        {
            ActivationEmailObject _activationEmailData = await new EmailDAL().GetActivationEmailDateAsync(email);
            if (_activationEmailData.IsNull())
            {
                return null;
            }
            string _emailActivationUrl = SystemConfigurationHelper.EmailActivationUrl;
            _emailActivationUrl = _emailActivationUrl.Replace("{code}", _activationEmailData.ActivationCode);
            emailBody = emailBody.Replace("<%Email%>", _activationEmailData.Email);
            emailBody = emailBody.Replace("<%Link%>", _emailActivationUrl);
            emailBody = emailBody.Replace("<%CurrentYear%", DateTime.Now.Year.ToString());

            return emailBody;

        }

        private async Task<string> BuildRecoverPasswordEmailBodyAsync(string email, string emailBody)
        {
            User _user = await new UserDAL().GetUserByEmailAddressAsync(email);
            if (_user.IsNull())
            {
                return null;
            }
            string _emailActivationUrl = SystemConfigurationHelper.EmailActivationUrl;
            emailBody = emailBody.Replace("<%FirstName%>", _user.FirstName);
            emailBody = emailBody.Replace("<%LastName%>", _user.LastName);
            emailBody = emailBody.Replace("<%Password%>", _user.Password);

            return emailBody;

        }

        private string BuildContactUsEmailBodyAsync(ContactRequest request)
        {

            string emailBody = string.Format("Name: {0} \n Subject: {1} \n Message: {2} \n Email: {3} \n",
                                       request.Name, request.Subject, request.Message, request.Email);

            return emailBody.Replace("\n", Environment.NewLine);

        }

        private string BuildSubscribeEmailBodyAsync(string emailBody)
        {

            emailBody = emailBody.Replace("<%CurrentYear%>", DateTime.Now.Year.ToString());

            return emailBody;

        }
        void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

            if (!e.Cancelled && e.Error == null)
            {
                _isEMailSent = true;
            }
        }
    }
}
