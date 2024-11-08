namespace SG_Server_Interface.Classes {
    public class Child {
        public int id {
            get; set;
        }

        public string first_name {
            get; set; 
        }

        public string last_name {
            get; set;
        }

        public string class_name {
            get; set;
        }

        public string image_path {
            get; set;
        }

        public string notes {
            get; set;
        }

        public Child(int id, string first_name, string last_name, string class_name, string image_path, string notes) {
            this.id = id;
            this.first_name = first_name;
            this.last_name = last_name;
            this.class_name = class_name;
            this.image_path = image_path;
            this.notes = notes;
        }

        public override bool Equals(object? obj) {
            if (obj == null) {
                return false;
            }

            if (obj.GetType() != typeof(Child)) {
                return false;
            }

            Child c = (Child)obj;

            return (
                c.id == this.id
                &&
                c.first_name == this.first_name
                &&
                c.last_name == this.last_name
                &&
                c.class_name == this.class_name
                &&
                c.image_path == this.image_path
                &&
                c.notes == this.notes
            );
        }

        public override int GetHashCode() {
            // Use a prime number to combine hash codes of the properties
            int hash = 17;
            hash = hash * 31 + id.GetHashCode();
            hash = hash * 31 + (first_name != null ? first_name.GetHashCode() : 0);
            hash = hash * 31 + (last_name != null ? last_name.GetHashCode() : 0);
            hash = hash * 31 + (class_name != null ? class_name.GetHashCode() : 0);
            hash = hash * 31 + (image_path != null ? image_path.GetHashCode() : 0);
            hash = hash * 31 + (notes != null ? notes.GetHashCode() : 0);

            return hash;
        }

    }
}
