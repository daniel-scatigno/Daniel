using System;
using System.Text.Json;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Daniel.Utils
{
   public static class Extensions
   {
      public static string ToNumbersOnly(this string text)
      {
         if (text == null)
         {
            return null;
         }
         var result = string.Empty;

         int n;
         foreach (var c in text.ToCharArray())
         {
            if (int.TryParse(c.ToString(), out n))
            {
               result += n.ToString();
            }
         }
         return string.IsNullOrEmpty(result) ? null : result;
      }

      /// <summary>
      /// Retorna a mensagem de erro da exceção e das innerExceptions
      /// </summary>
      /// <param name="exception"></param>
      /// <returns></returns>
      public static string FullErrorMessage(this Exception exception)
      {
         string message = exception.Message + (exception.InnerException != null ? "\r\n" + exception.InnerException.FullErrorMessage() : "");
         return message;

      }

      public static T JsonElementToType<T>(this JsonElement elem, JsonSerializerOptions options)
      {
         var resText = elem.GetRawText();
         var result = JsonSerializer.Deserialize<T>(resText, options);
         return result;

      }

      public static bool IsBetween(this DateTime date, DateTime Start, DateTime End)
      {
         bool result = ((date >= Start && date <= End));
         return result;
      }

      public static void CopyPrimitiveValuesFrom(this object destination, object origin)
      {
         var props = destination.GetType().GetProperties().Where(x => x.PropertyType.IsSimpleType());
         foreach (var prop in props)
         {
            prop.SetValue(destination, prop.GetValue(origin));
         }
      }

      public static bool IsSimpleType(this Type type)
      {
         return
             type.IsPrimitive ||
             new Type[] {
            typeof(string),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
             }.Contains(type) ||
             type.IsEnum ||
             Convert.GetTypeCode(type) != TypeCode.Object ||
             (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]))
             ;
      }

      /// <summary>
      /// Gets human-readable version of enum.
      /// </summary>
      /// <returns>effective DisplayAttribute.Name of given enum.</returns>
      public static string GetDisplayName<T>(this T enumValue) where T : IComparable, IFormattable, IConvertible
      {
         if (!typeof(T).IsEnum)
            throw new ArgumentException("Argument must be of type Enum");

         DisplayAttribute displayAttribute = enumValue.GetType().GetMember(enumValue.ToString())
                                                      .First().GetCustomAttribute<DisplayAttribute>();

         string displayName = displayAttribute?.GetName();

         return displayName ?? enumValue.ToString();
      }

      public static DateTime FirstDayOfMonth(this DateTime value)
      {
         return value.Date.AddDays(1 - value.Day).Date;
      }
      public static DateTime LastDayOfMonth(this DateTime value)
      {
         return value.FirstDayOfMonth().AddDays(DateTime.DaysInMonth(value.Year, value.Month) - 1).Date;
      }

      public static string ToShortTimeString(this TimeSpan value)
      {
         return DateTime.Today.AddTicks(value.Ticks).ToShortTimeString();
      }

   }
}
