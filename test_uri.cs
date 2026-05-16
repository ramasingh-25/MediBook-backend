using System;

class Program {
    static void Main() {
        string connString = "postgres://user:pass@host/db";
        if (connString != null && connString.StartsWith("postgres://"))
        {
            Console.WriteLine("Matched postgres://");
            var uri = new Uri(connString);
            var userInfo = uri.UserInfo.Split(':');
            connString = $"Host={uri.Host};Port={(uri.Port > 0 ? uri.Port : 5432)};Database={uri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
        }
        else if (connString != null && connString.StartsWith("postgresql://"))
        {
            Console.WriteLine("Matched postgresql://");
        }
        Console.WriteLine(connString);
    }
}
