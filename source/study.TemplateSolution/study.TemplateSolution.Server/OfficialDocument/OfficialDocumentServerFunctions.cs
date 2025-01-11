using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using study.TemplateSolution.OfficialDocument;

namespace study.TemplateSolution.Server
{
  partial class OfficialDocumentFunctions
  {
    /// <summary>
    /// 
    /// </summary>       
    [Public, Remote]
    public void SendEmail(string body, string email)
    {
      var message = Mail.CreateMailMessage();
      message.Subject = "Новое письмо";
      message.Body = body;
      message.IsBodyHtml = true;
      message.Priority = MailPriority.High;
      message.To.Add(email);
      message.FromDisplayName = "Отправитель";
      
      Mail.Send(message);     
    }
  }
}