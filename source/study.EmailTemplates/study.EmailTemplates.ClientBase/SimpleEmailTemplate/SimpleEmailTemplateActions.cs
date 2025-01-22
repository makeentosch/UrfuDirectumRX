using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sungero.Core;
using Sungero.CoreEntities;
using study.EmailTemplates.SimpleEmailTemplate;
using study.TemplateSolution.Shared;
using study.TemplateSolution.OfficialDocument;
using Sungero.Domain.Shared;

namespace study.EmailTemplates.Client
{
    partial class SimpleEmailTemplateActions
    {
        public virtual void GenerateTemplateAction(Sungero.Domain.Client.ExecuteActionArgs e)
        {
            var variables = GetVars(_obj.DocumentRef);
            var generatedTemplate = Functions.SimpleEmailTemplate.GenerateTemplate(_obj, variables);

            var temp = generatedTemplate[0];
            var noHtmlTemp = RemoveHtmlTags(generatedTemplate[0]);
            var email = generatedTemplate[1];

            var taskDialog = Dialogs.CreateTaskDialog("Сгенерированный шаблон", noHtmlTemp);
            var emailButton = taskDialog.Buttons.AddCustom("Отправить по эл. почте");

            if (taskDialog.Show() == emailButton)
            {
                Functions.SimpleEmailTemplate.Remote.SendEmail(_obj, temp, email);
                Dialogs.ShowMessage("Письмо успешно отправлено");
            }

            taskDialog.Buttons.AddOk();
            taskDialog.Show();
        }

        public virtual bool CanGenerateTemplateAction(Sungero.Domain.Client.CanExecuteActionArgs e)
        {
            return true;
        }

        public static Dictionary<string, string> GetVars(Sungero.Docflow.IOfficialDocument document)
        {
            var properties = new Dictionary<string, string>();
            ProcessProperties(document, properties, 0);
            return properties;
        }

        private static void ProcessProperties(object entity, Dictionary<string, string> properties, int count, string prefix = "")
        {
            if (entity == null || count == 5)
                return;

            count++;
            var type = entity.GetType();
            foreach (var property in type.GetProperties())
            {
                try
                {
                    var value = property.GetValue(entity);
                    var propertyName = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix} -> {property.Name}";

                    if (value is Sungero.Domain.Shared.IEntity linkedEntity)
                    {
                        ProcessProperties(linkedEntity, properties, count, propertyName);
                    }
                    else
                    {
                        properties[propertyName] = value?.ToString() ?? "Пусто";
                    }
                }
                catch (Exception ex)
                {
                    var propertyName = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix} -> {property.Name}";
                    properties[propertyName] = "Ошибка";
                }
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
    }
}