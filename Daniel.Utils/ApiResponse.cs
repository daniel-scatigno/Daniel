using System.Text.Json;
namespace Daniel.Utils
{
   public class ApiResponse
   {
      public JsonElement Result { get; set; }
      public int StatusCode { get; set; }

   }
}