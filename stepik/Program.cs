using MySql.Data.MySqlClient;
using stepik.Models;
using stepik.Services;

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.WriteLine(@"
************************************************
* Добро пожаловать на онлайн платформу Stepik! *
************************************************

Выберите действие (введите число и нажмите Enter):

1. Войти
2. Зарегистрироваться
3. Закрыть приложение

************************************************
");

            var ans = Console.ReadLine();
            switch (ans)
            {
                case "1": LoginUser(); break;
                case "2": RegisterUser(); break;
                case "3": Console.WriteLine("До свидания!"); return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    public static void LoginUser()
    {
        Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");

        string? fullName = Console.ReadLine();

        if (string.IsNullOrEmpty(fullName))
        {
            Console.WriteLine("Пользователь не найден, произведен выход на главную страницу\n");
            return;
        }

        var user = UsersService.Get(fullName);

        if (user == null)
        {
            Console.WriteLine("Пользователь не найден, произведен выход на главную страницу\n");
            return;
        }

        Console.WriteLine($"Пользователь '{user.FullName}' успешно вошел\n");
    }

    public static void RegisterUser()
    {
        Console.WriteLine("Введите имя и фамилию через пробел и нажмите Enter:");
        string? fullName = Console.ReadLine();

        if (string.IsNullOrEmpty(fullName))
        {
            Console.WriteLine("Произошла ошибка, произведен выход на главную страницу\n");
            return;
        }

        var user = new User { FullName = fullName };

        bool result = UsersService.Add(user);

        if (result)
            Console.WriteLine($"Пользователь '{user.FullName}' успешно добавлен.\n");
        else
            Console.WriteLine("Произошла ошибка, произведен выход на главную страницу\n");
    }
}