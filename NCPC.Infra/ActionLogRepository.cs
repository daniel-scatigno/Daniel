using NCPC.Infra;
using NCPC.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace NCPC.Infra
{
   public class ActionLogRepository:Repository<ActionLog>
   {
      public ActionLogRepository(DbContext context):base(context)
      {

      }

   }

}