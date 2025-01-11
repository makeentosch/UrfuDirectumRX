using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using study.EmailTemplates.SimpleEmailTemplate;

namespace study.EmailTemplates.Shared
{
  partial class SimpleEmailTemplateFunctions
  {
    [Public]
    public virtual string[] GenerateTemplate(System.Collections.Generic.Dictionary<string, string> vars)
    {
      var result = _obj.TemplateBody;
      string sender = "ПодготовилФИО";
      string organization = "Наша орг.Имя";
      string departament = "ПодразделениеНаименование";
      string content = "Содержание";
      string correspondent = "КорреспондентФИО";
      string email = "КорреспондентЭл. почта";
      
      foreach (var pair in vars)
      {
        if (pair.Key.Contains("ПодготовилФИО") || pair.Key.Contains("BusinessUnit -> CEO -> DisplayValue"))
          sender = pair.Value;
        if (pair.Key.Contains("Наша орг.Имя") || pair.Key.Contains("BusinessUnit -> Company -> Name"))
          organization = pair.Value;
        if (pair.Key.Contains("ПодразделениеНаименование") || pair.Key.Contains("Department -> DisplayValue"))
          departament = pair.Value;
        if (pair.Key.Contains("Содержание") || pair.Key.Contains("Subject"))
          content = pair.Value;
        if (pair.Key.Contains("КорреспондентФИО") || pair.Key.Contains("Correspondent -> Name"))
          correspondent = pair.Value;
        if (pair.Key.Contains("КорреспондентЭл. почта") || pair.Key.Contains("Correspondent -> Email"))
          email = pair.Value;
      }

      result = result.Replace("{Исполнитель}", sender);
      result = result.Replace("{Наша орг.}", organization);
      result = result.Replace("{Подразделение}", departament);
      result = result.Replace("{Содержание}", content);
      result = result.Replace("{Корреспондент}", correspondent);     

      return new string[] {result, email};
    }
  }
}