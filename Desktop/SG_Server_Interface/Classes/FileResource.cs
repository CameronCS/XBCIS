﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Classes {
    public class FileResource {
        public int id {
            get; set;
        }

        public string title {
            get; set;
        }

        public string file_path {
            get; set;
        }

        public string uploaded_by {
            get; set;
        }

        public DateTime upload_date {
            get; set;
        }

        public FileResource(int id, string title, string file_path, string uploaded_by, DateTime upload_date) {
            this.id = id;
            this.title = title;
            this.file_path = file_path;
            this.uploaded_by = uploaded_by;
            this.upload_date = upload_date;
        }
    }
}
