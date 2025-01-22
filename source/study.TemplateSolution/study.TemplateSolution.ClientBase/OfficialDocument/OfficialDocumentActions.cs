using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sungero.Core;
using Sungero.CoreEntities;
using study.TemplateSolution.OfficialDocument;

namespace study.TemplateSolution.Client
{
    partial class OfficialDocumentActions
    {
        public virtual void CreateTemplatestudy(Sungero.Domain.Client.ExecuteActionArgs e)
        {
            var d = Dialogs.CreateInputDialog("Выберите шаблон");
            var template = d.AddSelect("Шаблон", true, EmailTemplates.SimpleEmailTemplates.Null);

            if (d.Show() == DialogButtons.Ok)
            {
                var variables = Functions.OfficialDocument.GetVars(_obj);
                var result = study.EmailTemplates.PublicFunctions.SimpleEmailTemplate.GenerateTemplate(template.Value, variables);

                var temp = result[0];
                var noHtmlTemp = RemoveHtmlTags(result[0]);
                var email = result[1];

                var taskDialog = Dialogs.CreateTaskDialog("Сгенерированный шаблон", noHtmlTemp);
                var emailButton = taskDialog.Buttons.AddCustom("Отправить по эл. почте");

                if (taskDialog.Show() == emailButton)
                {
                    Functions.OfficialDocument.Remote.SendEmail(_obj, temp, email);
                    Dialogs.ShowMessage("Письмо успешно отправлено");
                }

                taskDialog.Buttons.AddOk();
                taskDialog.Show();
            }
        }

        private string RemoveHtmlTags(string text)
        {
            var result = Regex.Replace(text, "<br\\s*/?>", "\n");
            result = Regex.Replace(result, "<p.*?>", "\n");
            result = Regex.Replace(result, "</p>", "\n");
            result = Regex.Replace(result, "<.*?>", "");
            result = result.Replace("&nbsp;", " ")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&amp;", "&");

            result = Regex.Replace(result, @"\s{2,}", "\n").Trim();

            return result;
        }


        public virtual bool CanCreateTemplatestudy(Sungero.Domain.Client.CanExecuteActionArgs e)
        {
            return true;
        }
    }
}