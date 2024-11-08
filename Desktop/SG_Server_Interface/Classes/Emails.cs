namespace SG_Server_Interface.Classes {
    public class Emails {
        public int id {
            get; set;
        }

        public string sender_username {
            get; set;
        }

        public string receiver_username {
            get; set;
        }

        public string subject {
            get; set;
        }

        public string body {
            get; set;
        }

        public string status {
            get; set;
        }

        public DateTime timestamp {
            get; set;
        }

        public Emails(int id, string sender_username, string receiver_username, string subject, string body, string status, DateTime timestamp) {
            this.id = id;
            this.sender_username = sender_username;
            this.receiver_username = receiver_username;
            this.subject = subject;
            this.body = body;
            this.status = status;
            this.timestamp = timestamp;
        }
        public override bool Equals(object? obj) {
            if (obj == null) {
                return false;
            }

            if (obj.GetType() != typeof(Emails)) {
                return false;
            }

            Emails e = (Emails)obj;

            /*bool all_is_good = */ return(
                e.id == this.id
                &&
                e.sender_username == this.sender_username
                &&
                e.receiver_username == this.receiver_username
                &&
                e.subject == this.subject
                &&
                e.body == this.body
                &&
                e.status == this.status
            );
/*
            int comp = DateTime.Compare(e.timestamp, this.timestamp);
            bool timestampcheck = comp == 0;

            return all_is_good && timestampcheck;
*/
        }


        public override int GetHashCode() {
            int hash = 17;
            hash = hash * 31 + id.GetHashCode();
            hash = hash * 31 + (sender_username != null ? sender_username.GetHashCode() : 0);
            hash = hash * 31 + (receiver_username != null ? receiver_username.GetHashCode() : 0);
            hash = hash * 31 + (subject != null ? subject.GetHashCode() : 0);
            hash = hash * 31 + (body != null ? body.GetHashCode() : 0);
            hash = hash * 31 + (status != null ? status.GetHashCode() : 0);
            hash = hash * 31 + timestamp.GetHashCode(); // DateTime is a struct and doesn't need null check

            return hash;
        }

    }
}
