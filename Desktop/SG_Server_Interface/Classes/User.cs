using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Server_Interface.Classes {
    public class User {
        public int id {
            get; set;
        }

        public string first_name {
            get; set;
        }

        public string last_name {
            get; set;
        }

        public string username {
            get; set;
        }

        public string image_path {
            get; set;
        }

        public ushort is_admin {
            get; set;
        }

        public string contact_no {
            get; set;
        }

        public User(int id, string first_name, string last_name, string username, string image_path,  ushort is_admin, string contact_no) {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.username = username;
            this.image_path = image_path;
            this.is_admin = is_admin;
            this.contact_no = contact_no;
        }


        public override bool Equals(object? obj) {
            if (obj == null) {
                return false;
            }

            if (obj.GetType() != typeof(User)) {
                return false;
            }

            User u = (User)obj;

            return (
                u.id == this.id
                &&
                u.first_name == this.first_name
                &&
                u.last_name == this.last_name
                &&
                u.username == this.username
                &&
                u.image_path == this.image_path
                &&
                u.is_admin == this.is_admin
                &&
                u.contact_no == this.contact_no
            );
        }

        public override int GetHashCode() {
            int hash = 17;
            hash = hash * 31 + id.GetHashCode();
            hash = hash * 31 + (first_name != null ? first_name.GetHashCode() : 0);
            hash = hash * 31 + (last_name != null ? last_name.GetHashCode() : 0);
            hash = hash * 31 + (username != null ? username.GetHashCode() : 0);
            hash = hash * 31 + (image_path != null ? image_path.GetHashCode() : 0);
            hash = hash * 31 + is_admin.GetHashCode(); // Boolean doesn't need null check
            hash = hash * 31 + (contact_no != null ? contact_no.GetHashCode() : 0);

            return hash;
        }

    }
}
