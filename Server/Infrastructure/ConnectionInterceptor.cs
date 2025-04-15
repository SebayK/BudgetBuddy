   using Microsoft.EntityFrameworkCore.Diagnostics;
   using System.Data.Common;
   
   namespace BudgetBuddy.Infrastructure;

   public class ConnectionInterceptor : DbConnectionInterceptor
   {
       public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
       {
           Console.WriteLine($"Database connection opened successfully: {connection.ConnectionString}");
           base.ConnectionOpened(connection, eventData);
       }
   }
   