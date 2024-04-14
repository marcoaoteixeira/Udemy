using SQLite;

namespace LearningWPF.ContactsApp.Entities {

    public class Contact {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
