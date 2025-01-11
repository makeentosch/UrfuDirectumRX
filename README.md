Текущий репозиторий был получен путём экспорта разработки из среды DirectumRX. Часть настроек, производимых в среде, экспортировать невозможно. В видеопрезентации показан результат работы всех реализованных модулей вместе.

Пожалуйста, учтите, что здесь могут содержаться служебные файлы и пустой код. 

Для вашего ориентирования:
```bash
├── source
│   ├── study.EmailTemplates
│   │   ├── **study.EmailTemplates.ClientBase
│   │   │   ├── SimpleEmailTemplate
│   │   │   │   ├── SimpleEmailTemplateActions.cs # Обработчик события нажатия на кнопку "Сгенерировать шаблон" из справочника
│   │   │   │   ├── ...
│   │   │   ├── ...
│   │   ├── **study.EmailTemplates.Server
│   │   │   ├── SimpleEmailTemplate
│   │   │   │   ├── SimpleEmailTemplateServerFunctions.cs # Метод отправки письма по электронной почте
│   │   │   │   ├── ...
│   │   │   ├── ...
│   │   ├── **study.EmailTemplates.Shared
│   │   │   ├── SimpleEmailTemplate
│   │   │   │   ├── SimpleEmailTemplateSharedFunctions.cs # Заполение шаблона полученными данными
│   │   │   │   ├── ...
│   │   │   ├── ...
│   ├── study.TemplateSolution
│   │   ├── **study.EmailTemplates.ClientBase
│   │   │   ├── OfficialDocument
│   │   │   │   ├── OfficialDocumentActions.cs # Обработчик события нажатия на кнопку "Сгенерировать шаблон" из карточки документа
│   │   │   │   ├── ...
│   │   │   ├── ...
│   │   ├── **study.EmailTemplates.Shared
│   │   │   ├── OfficialDocument
│   │   │   │   ├── OfficialDocumentSharedFunctions.cs # Получение данных из карточки документа через рефлексию
│   │   │   │   ├── ...
│   │   │   ├── ...
│   │   ├── **study.EmailTemplates.Server
├── client
├── isolated
├── server
├── settings
├── shared
├── PackageInfo.xml
└── README.md
```
