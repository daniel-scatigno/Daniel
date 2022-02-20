using System.Text.Json;
namespace NCPC.Utils
{
   public class ApiResponse
   {
      public JsonElement Result { get; set; }
      public int StatusCode { get; set; }

   }
}