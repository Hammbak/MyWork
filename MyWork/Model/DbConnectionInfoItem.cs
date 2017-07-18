using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork.Model
{
    public class DbConnectionInfoItem
    {
        public string Description { get; set; } = string.Empty;
        public string ConnectionIp { get; set; } = "";
        public string ConnectionDatabase { get; set; } = "";
        public string ConnectionId { get; set; } = "";
        public string ConnectionPassword { get; set; } = "";
        public bool Checked { get; set; } = false;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DataBasePurpose Purpose { get; set; } = DataBasePurpose.Dev;
    }
}
