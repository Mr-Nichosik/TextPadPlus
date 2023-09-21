# TextPad+

Это простой текстовый редактор, сделанный на C# WinForms.
Зависимости: моя система лоирования Logging System https://github.com/Mr-Nichosik/LoggingSystem и Custom TabControl, который я только начал разрабатывать https://github.com/Mr-Nichosik/CustomTabControl.

Я сделал в нём несколько классов: 4 класса с формами, основной класс Program и класс с логикой работы TextEditor.
Все формы имеют две локализации: стандартную - она же русская (файлы Form.resx), и английскую (файлы Form.en.resx)

Для локализации MessageBox'ов и некоторых других элементов, которые напрямую в форму не запихнуть, я сделал 2 файла: Localization.resx и Localization.en-US.resx.

У программы есть внешний установщик обновлений (Updater), реализованный в другом проекте https://github.com/Mr-Nichosik/TextPadPlus-Updater. Тут просто вызывается его .exe файл. В методе GetUpdate() класса FormMainUI

Так же есть файл с конфигурацией в формате xml. Он лежит в папке Resources. После компиляции программы его надо кинуть в папку к exe файлу. Он нужен для передачи информации Updater'у.
Работает на .NET 6!
