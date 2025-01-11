using System;
using System.Collections.Generic;
using System.Linq;
using Sungero.Core;
using Sungero.CoreEntities;
using study.TemplateSolution.OfficialDocument;
using Sungero.Domain.Shared;

namespace study.TemplateSolution.Shared
{
  partial class OfficialDocumentFunctions
  {
    public virtual System.Collections.Generic.Dictionary<string, string> GetVars()
    {
        var changedProperties = new Dictionary<string, string>();
        
        // Получаем тип объекта.
        var objType = _obj.GetType();
        var objMetadata = objType.GetEntityMetadata();

        // Перебираем свойства объекта.
        foreach (var propertyMetadata in objMetadata.Properties)
        {
            if (propertyMetadata.PropertyType == Sungero.Metadata.PropertyType.Collection)
            {
                // Если свойство — коллекция, обрабатываем её отдельно.
                var collectionValue = (Sungero.Domain.Shared.IChildEntityCollection<Sungero.Domain.Shared.IChildEntity>)propertyMetadata.GetValue(_obj);
                
                foreach (Sungero.Domain.Shared.IChildEntity line in collectionValue)
                {
                    var lineType = line.GetType();
                    var lineMetadata = lineType.GetEntityMetadata();

                    foreach (var linePropertyMetadata in lineMetadata.Properties)
                    {
                        if (Equals(_obj, linePropertyMetadata.GetValue(line)))
                            continue;

                        AddChangedProperty(line, linePropertyMetadata, changedProperties, $"{propertyMetadata.GetLocalizedName()} -> ");
                    }
                }
            }
            else
            {
                // Проверяем изменения одиночных свойств.
                AddChangedProperty(_obj, propertyMetadata, changedProperties);
            }
        }

        return changedProperties;
    }
    
    private void AddChangedProperty(Sungero.Domain.Shared.IEntity entity, 
                                     Sungero.Metadata.PropertyMetadata propertyMetadata, 
                                     Dictionary<string, string> changedProperties, 
                                     string prefix = "")
    {
        var stateProperties = entity.State.Properties;
        var propertyState = stateProperties.GetType().GetProperty(propertyMetadata.Name)?.GetValue(stateProperties);

        if (propertyState == null)
            return;

        var newValue = propertyMetadata.GetValue(entity);
        var originalValue = propertyState.GetType().GetProperty("OriginalValue")?.GetValue(propertyState);

        // Проверяем на изменения.
        if (Equals(newValue, originalValue) || 
            (string.IsNullOrEmpty(newValue?.ToString()) && string.IsNullOrEmpty(originalValue?.ToString())))
            return;
        
        // Если свойство — ссылочный тип, обрабатываем его рекурсивно.
        if (newValue is Sungero.Domain.Shared.IEntity linkedEntity)
        {
            // Префикс для вложенных свойств.
            var linkedPrefix = $"{prefix}{propertyMetadata.GetLocalizedName()}";
            AddReferenceEntityProperties(linkedEntity, changedProperties, 0, linkedPrefix);
            return;
        }

        // Обрабатываем перечислимые свойства (Enumeration).
        if (propertyMetadata.PropertyType == Sungero.Metadata.PropertyType.Enumeration)
        {
            var infoProperties = entity.Info.Properties;
            var propertyInfo = infoProperties.GetType().GetProperty(propertyMetadata.Name)?.GetValue(infoProperties) 
                               as Sungero.Domain.Shared.EnumPropertyInfo;

            newValue = propertyInfo?.GetLocalizedValue(newValue as Sungero.Core.Enumeration?) ?? newValue;
        }

        var propertyName = $"{prefix}{propertyMetadata.GetLocalizedName()}";
        var propertyValue = newValue?.ToString() ?? "Пусто";
        
        // Добавляем в словарь.
        changedProperties[propertyName] = propertyValue;
    }
    
    private void AddReferenceEntityProperties(Sungero.Domain.Shared.IEntity linkedEntity,
                                          Dictionary<string, string> changedProperties,
                                          int count,
                                          string prefix = "")
    {
      if (count == 5)
        return;
      // Получаем метаданные вложенного объекта.
      var linkedEntityType = linkedEntity.GetType();
      var linkedMetadata = linkedEntityType.GetEntityMetadata();
      count++;
  
      foreach (var linkedPropertyMetadata in linkedMetadata.Properties)
      {
          var linkedPropertyValue = linkedPropertyMetadata.GetValue(linkedEntity);
  
          // Если свойство снова является ссылочным объектом, обрабатываем рекурсивно.
          if (linkedPropertyValue is Sungero.Domain.Shared.IEntity nestedEntity)
          {
              var nestedPrefix = $"{prefix}{linkedPropertyMetadata.GetLocalizedName()} -> ";
              AddReferenceEntityProperties(nestedEntity, changedProperties, count, nestedPrefix);
              continue;
          }
  
          // Добавляем значение свойства в словарь.
          var propertyName = $"{prefix}{linkedPropertyMetadata.GetLocalizedName()}";
          var propertyValue = linkedPropertyValue?.ToString() ?? "Пусто";
          changedProperties[propertyName] = propertyValue;
      }
   }
  }
}