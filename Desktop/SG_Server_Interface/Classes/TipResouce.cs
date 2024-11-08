using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Classes {
    public class TipResouce {
        public int id {
            get; set;
        }

        public string title {
            get; set;
        }

        public string tip_text {
            get; set;
        }

        public string uploaded_by {
            get; set;
        }
        public DateTime upload_date {
            get; set;
        }

        public TipResouce(int id, string title, string tip_text, string uploaded_by, DateTime upload_date) {
            this.id = id;
            this.title = title;
            this.tip_text = tip_text;
            this.uploaded_by = uploaded_by;
            this.upload_date = upload_date;
        }
    }
}
