using System.ComponentModel;

namespace GeoGrafija.ViewModels.Profiles
{
    public  class MyStudentInfoViewModel
    {
        [DisplayName("Идентификатор на Студент")]
        public int StudentId { get; set; }

        [DisplayName("Име на Студент")]
        public string UserName { get; set; }

        [DisplayName("Освоени Поени на Студент")]
        public int StudentPoints { get; set; }

        [DisplayName("Вкупно Поени на квизови на Студент")]
        public int TotalPoints { get; set; }

        [DisplayName("Ранк  на Студент")]
        public int Rank { get; set; }
    }
}