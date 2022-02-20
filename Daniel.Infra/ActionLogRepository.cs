using Daniel.Infra;
using Daniel.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Daniel.Infra
{
   public class ActionLogRepository:Repository<ActionLog>
   {
      public ActionLogRepository(DbContext context):base(context)
      {

      }

   }

}