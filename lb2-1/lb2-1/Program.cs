using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lab1_4
{
  class Worker
  {
    public string Name { get; set; }
    public int ID { get; set; }

  }
  class Program
  {
    static async Task Main(string[] args)
    {
      string ex = "1";
      int n = 0;
      while (n == 0)
      {
        Console.Clear();
        Console.WriteLine("Добро пожаловать в меню!\n\n");
        Console.WriteLine("Информация о дисках      - 1 ");
        Console.WriteLine("Работа с файлами         - 2 ");
        Console.WriteLine("Работа с форматом JSON   - 3 ");
        Console.WriteLine("Работа с форматом XML    - 4 ");
        Console.WriteLine("Создание zip архива      - 5 ");
        Console.WriteLine("Выход из программы       - 0 ");
        Console.Write("Введите номер команды: ");
        int vibor = Convert.ToInt32(Console.ReadLine());
        switch (vibor)
        {
          case 1:
            {
              Console.Clear();
              DriveInfo[] drives = DriveInfo.GetDrives();

              foreach (DriveInfo drive in drives)
              {
                Console.WriteLine($"Название: {drive.Name}");
                Console.WriteLine($"Тип: {drive.DriveType}");
                if (drive.IsReady)
                {
                  Console.WriteLine($"Имя файловой системы: {drive.DriveFormat}");
                  Console.WriteLine($"Объем диска: {drive.TotalSize}");
                  Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
                  Console.WriteLine($"Метка: {drive.VolumeLabel}");
                }
                Console.WriteLine();
              }
              break;
            }
          case 2:
            {
              Console.Clear();
              string path = @"D:\Documents";
              DirectoryInfo dirInfo = new DirectoryInfo(path);
              if (!dirInfo.Exists)
              {
                dirInfo.Create();
              }
              Console.WriteLine("Введите строку для записи в файл:");
              string text = Console.ReadLine();

              using (FileStream fstream = new FileStream($"{path}\\stringinFile.txt", FileMode.Create))
              {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Строка записана в файл");
              }

              using (FileStream fstream = File.OpenRead($"{path}\\stringinFile.txt"))
              {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                Console.WriteLine($"Строка из файла: {textFromFile}");
              }
              Console.WriteLine("Удалить файл? y/n");
              string choose = Console.ReadLine();
              switch (choose)
              {
                case "y":
                  path = @"D:\Documents\stringinFile.txt";
                  FileInfo fileInf = new FileInfo(path);
                  if (fileInf.Exists)
                  {
                    fileInf.Delete();
                  }
                  Console.WriteLine("Файл удален");
                  break;

                case "n":
                  Console.WriteLine("Выбрано значение 'Не удалять'");
                  break;

                default:
                  Console.WriteLine("Значение не выбрано");
                  break;
              }
              Console.ReadLine();
              break;
            }
          case 3:
            {
              Console.Clear();
              using (FileStream fs = new FileStream("person.json", FileMode.OpenOrCreate))
              {
                Worker tom = new Worker() { Name = "kolya", ID = 1234567 };
                await JsonSerializer.SerializeAsync<Worker>(fs, tom);
                Console.WriteLine("Данные сохранены в файл");
              }

              using (FileStream fs = new FileStream("person.json", FileMode.Open))
              {
                Worker restoredPerson = await JsonSerializer.DeserializeAsync<Worker>(fs);
                Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.ID}");
              }
              File.Delete("person.json");
              break;
            }
          case 4:
            {
              Console.Clear();
              XmlDocument xDoc = new XmlDocument();
              XDocument xdoc = new XDocument();
              Console.WriteLine("Сколько пользователей нужно внести?");
              int count = Convert.ToInt32(Console.ReadLine());
              XElement list = new XElement("list");
              for (int i = 1; i <= count; i++)
              {
                XElement chel = new XElement("chel");
                Console.WriteLine("ВВедите имя");
                XAttribute username = new XAttribute("name", Console.ReadLine());
                Console.WriteLine("ВВедите компанию");
                XElement userage = new XElement("company", Console.ReadLine());
                Console.WriteLine("Введите возраст");
                XElement usercompany = new XElement("age", Convert.ToInt32(Console.ReadLine()));
                chel.Add(username);
                chel.Add(userage);
                chel.Add(usercompany);
                list.Add(chel);
              }

              xdoc.Add(list);
              xdoc.Save("users.xml");
              Console.WriteLine("Прочитать только что записанный xml файл? y/n");
              switch (Console.ReadLine())
              {
                case "y":
                  Console.WriteLine();
                  xDoc.Load("users.xml");
                  XmlElement xRoot = xDoc.DocumentElement;
                  foreach (XmlNode xnode in xRoot)
                  {
                    if (xnode.Attributes.Count > 0)
                    {
                      XmlNode attr = xnode.Attributes.GetNamedItem("name");
                      if (attr != null)
                        Console.WriteLine($"Имя: {attr.Value}");
                    }

                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                      if (childnode.Name == "company")
                      {
                        Console.WriteLine($"Компания: {childnode.InnerText}");
                      }

                      if (childnode.Name == "age")
                      {
                        Console.WriteLine($"Возраст: {childnode.InnerText}");
                      }
                    }
                  }
                  Console.WriteLine();
                  Console.WriteLine("Удалить созданный xml файл? y/n");
                  switch (Console.ReadLine())
                  {
                    case "y":
                      FileInfo xmlfilecheck = new FileInfo("users.xml");
                      if (xmlfilecheck.Exists)
                      {
                        xmlfilecheck.Delete();
                      }
                      break;
                    case "n":
                      break;
                    default:
                      Console.WriteLine("Вы не выбрали значение");
                      break;
                  }
                  Console.WriteLine();
                  break;

                case "n":
                  Console.WriteLine("Удалить созданный xml файл? y/n");
                  switch (Console.ReadLine())
                  {
                    case "y":
                      FileInfo xmlfilecheck = new FileInfo("users.xml");
                      if (xmlfilecheck.Exists)
                      {
                        xmlfilecheck.Delete();
                      }
                      break;
                    case "n":
                      break;
                    default:
                      Console.WriteLine("Вы не выбрали значение");
                      break;
                  }
                  break;

                default:
                  Console.WriteLine("Вы не выбрали значение");
                  break;
              }
              break;
            }
          case 5:
            {
              Console.Clear();
              string[] namelist = { ".//archive", ".//text_dezip", "text.zip" };
              FileInfo check = new FileInfo(namelist[2]);
              if (check.Exists)
              {
                File.Delete(namelist[2]);
                try
                {
                  Directory.Delete(namelist[0], true);
                  Directory.Delete(namelist[1], true);
                }
                catch
                { }
              }
              else
              {
                DirectoryInfo dirInfo = new DirectoryInfo("archive");
                try
                {
                  dirInfo.Create();
                }
                catch (Exception e)
                {
                  Console.WriteLine(e);
                  throw;
                }
              }
              Console.WriteLine("Введите строку для записи в файл:");
              string text = Console.ReadLine();
              DirectoryInfo dirInfo1 = new DirectoryInfo(namelist[0]);
              dirInfo1.Create();
              using (FileStream fstream = new FileStream($"{namelist[0]}\\note.txt", FileMode.OpenOrCreate))
              {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
              }
              Console.WriteLine();
              ZipFile.CreateFromDirectory(namelist[0], namelist[2]);
              Console.WriteLine($"Папка {namelist[0]} архивирована в файл {namelist[2]}");
              Console.WriteLine();
              FileInfo fileInf = new FileInfo(namelist[2]);
              if (fileInf.Exists)
              {
                Console.WriteLine("Имя файла: {0}", fileInf.Name);
                Console.WriteLine("Время создания: {0}", fileInf.CreationTime);
                Console.WriteLine("Тип файла: {0}", fileInf.Extension);
                Console.WriteLine("Размер: {0}", fileInf.Length);
              }
              Console.WriteLine("Нажмите Enter, чтобы продолжить...");
              Console.ReadLine();
              DirectoryInfo dirInfo2 = new DirectoryInfo(namelist[1]);
              dirInfo2.Create();
              ZipFile.ExtractToDirectory(namelist[2], namelist[1]);
              Console.WriteLine($"Файл {namelist[2]} распакован в папку {namelist[1]}");
              FileInfo fileInf2 = new FileInfo(".//text_dezip//note.txt");
              if (fileInf.Exists)
              {
                Console.WriteLine("Имя файла: {0}", fileInf2.Name);
                Console.WriteLine("Время создания: {0}", fileInf2.CreationTime);
                Console.WriteLine("Тип файла: {0}", fileInf2.Extension);
                Console.WriteLine("Размер: {0}", fileInf2.Length);
              }
              Console.WriteLine();
              Console.WriteLine("Нажмите Enter, чтобы продолжить...");
              Console.ReadLine();
              break;
            }
          case 0:
            {
              ex = "exit";
              break;
            }

          default:
            Console.WriteLine("\nВВЕДЕНЫ НЕПРАВИЛЬНЫЕ ДАННЫЕ!");
            break;
        }

        if (ex == "exit")
        {
          break;
        }

        Console.WriteLine("\nХотите продолжить?(y/n)");
        string vibor2 = Console.ReadLine();
        if (vibor2 == "n")
        {
          n++; //break
          Console.Clear();
        }
        else if (vibor2 != "y") { break; }
      }
    }
  }
}