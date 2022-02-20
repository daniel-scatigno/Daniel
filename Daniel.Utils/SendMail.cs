using Daniel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;


namespace Daniel.Utils
{
   public class SendMail
   {
      private readonly static int numeroMaximoTentativas = 3;
      private readonly string userName;
      private readonly string password;
      private readonly string host;
      private readonly int port;
      private readonly bool enableSSL;

      public static bool IsDevelopment { get; set; }

      public SendMail(string host, int port, bool enableSSL, string userName, string password)
      {
         this.host = host;
         this.port = port;
         this.enableSSL = enableSSL;
         this.userName = userName;
         this.password = password;
      }

      public static bool SendMailMessage(PerfilEmail perfil, List<string> destinatarios, string assunto, string corpo, int tentativa = 1
         , List<Attachment> attachments = null, List<AlternateView> alternateViews = null)
      {

         if (perfil == null) throw new Exception($"perfil de e-mail não cadastrado");

         MailMessage mensagem = new MailMessage();
         mensagem.From = new MailAddress(perfil.Email);

         if (IsDevelopment)
         {
            destinatarios = new List<string>() { "zmp@outlook.com" };
         }

         foreach (var mail in destinatarios)
         {
            mail.Split(';');
            if (mail.Trim() != "")
               mensagem.To.Add(new MailAddress(mail));
         }

         if (perfil.EmailCopia != null)
            foreach (var mail in perfil.EmailCopia.Split(';'))
            {
               if (mail.Trim() != "")
                  mensagem.CC.Add(new MailAddress(mail));
            }

         if (perfil.EmailCopiaOculta != null)
            foreach (var mail in perfil.EmailCopiaOculta.Split(';'))
            {
               if (mail.Trim() != "")
                  mensagem.Bcc.Add(new MailAddress(mail));
            }

         mensagem.Subject = assunto;
         mensagem.IsBodyHtml = true;
         mensagem.Body = corpo;
         if (alternateViews!=null)
         {
            
            alternateViews.ForEach(alt =>
            {
               mensagem.AlternateViews.Add(alt);
            });
         }
         
         if (attachments != null)
         {
            attachments.ForEach(att =>
            {
               mensagem.Attachments.Add(att);
            });
         }

         var smtpClient = new SmtpClient
         {
            Host = perfil.Servidor,
            Port = perfil.Porta,
            EnableSsl = perfil.AutenticacaoSSL
         };

         smtpClient.UseDefaultCredentials = false;
         smtpClient.Credentials = new NetworkCredential(perfil.Usuario, Daniel.Utils.EncryptUtils.Decrypt(perfil.Senha));
         smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
         smtpClient.ServicePoint.MaxIdleTime = Timeout.Infinite;
         smtpClient.ServicePoint.ConnectionLeaseTimeout = Timeout.Infinite;
         smtpClient.ServicePoint.ConnectionLimit = 10;

         try
         {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //if (!System.Diagnostics.Debugger.IsAttached)
            smtpClient.SendMailAsync(mensagem).Wait();
            return true;
         }
         catch (SmtpException ex)
         {
            if (ex.StatusCode == SmtpStatusCode.MustIssueStartTlsFirst && tentativa <= numeroMaximoTentativas)
               return SendMailMessage(perfil, destinatarios, assunto, corpo, tentativa + 1);
            else
               return false;
            //tratamento de erro e/ou log
         }
         catch (Exception ex)
         {
            //tratamento de erro e/ou log
            throw ex;
         }
         finally
         {
            mensagem.Dispose();
            smtpClient.Dispose();
         }
      }
   }
}
