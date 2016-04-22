using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLM.Dal.Models
{
    public class StoreProcedure
    {
        private SLM_DBEntities slmdb = null;

        public StoreProcedure()
        {
            slmdb = new SLM_DBEntities();
        }

        public string GenerateTicketId()
        {
            try
            {
                var ticketId = slmdb.kkslm_sp_generate_ticket_id().Select(p => p.Trim()).FirstOrDefault();
                if (ticketId != null)
                    return ticketId.ToString();
                else
                    throw new Exception("Ticket ID cannot be generated.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
