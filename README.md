# TextPad+

Это простой текстовый редактор, сделанный на C# WinForms.

Зависимости
-----------

Основной файл решщения (TextPad+.sln) включает в себя 4 проекта: 

* TextPad+, он же в этом репозитории
* Проект моей системы логирования Logging System https://github.com/Mr-Nichosik/LoggingSystem;
* Проект Custom TabControl, который я только начал разрабатывать https://github.com/Mr-Nichosik/CustomTabControl;
* Проект Modified TextBox - модифицированного мною текстового поля RichTextBox https://github.com/Mr-Nichosik/ModifiedTextBox.
В релизах в этих репозиториях есть архивы с уже собранными файлами этих проектов, их нужно скачать и указать к ним путь в студии.

TextPad+.csproj
---------------

Я сделал в нём несколько классов: 4 класса с формами, основной класс Program и класс с логикой работы TextEditor.
Все формы имеют две локализации: стандартную - она же русская (файлы Form.resx), и английскую (файлы Form.en.resx)

Для локализации MessageBox'ов и некоторых других элементов, которые напрямую в форму не запихнуть, я сделал 2 файла: Localization.resx и Localization.en-US.resx.

Работает на .NET 6.