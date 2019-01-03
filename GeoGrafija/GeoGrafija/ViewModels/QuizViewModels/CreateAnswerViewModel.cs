using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeoGrafija.ViewModels.QuizViewModels
{
    public class CreateAnswerViewModel
    {
        [DisplayName("Текс на Одговор :")]
        [Required(ErrorMessage = "Мора да внесете текст на одговорот")]
        [StringLength(999, ErrorMessage = "Одговорите мора да бидат минимум 3 карактери", MinimumLength = 3)]
        public string AnswerText { get; set; }

        [DisplayName("Идентификатор на прашање :")]
        [Required(ErrorMessage = "Одговорот мора да има прашање родител")]
        public int QuestionId { get; set; }

        [DisplayName("Идентификатор на одговор :")]
        [Required(ErrorMessage = "Одговорот мора да има идентификатор")]
        public int Id { get; set;}

        [DisplayName("Одговорот е точен :")]
        [Required(ErrorMessage = "Одговорот мора да биде означен како точен или неточен")]
        public bool IsTrue { get; set; }
    }
}