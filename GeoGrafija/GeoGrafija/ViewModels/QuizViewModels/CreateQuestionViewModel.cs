using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeoGrafija.ViewModels.QuizViewModels
{
    public class CreateQuestionViewModel
    {
        public CreateQuestionViewModel()
        {
            Answers = new List<CreateAnswerViewModel>();
        }

        [DisplayName("Tекст на прашање :")]
        [Required(ErrorMessage = "Мора да внесете текст на прашањето")]
        [StringLength(999, ErrorMessage = "Прашањата мора да бидат минимум 3 карактери", MinimumLength = 3)]
        public string Text { get; set; }

        [DisplayName("Идентификатор  на прашање :")]
        [Required(ErrorMessage = "Мора да внесете идентификатор на прашањето")]
        public int Id { get; set; }

        [DisplayName("Oдговори :")]
        public List<CreateAnswerViewModel> Answers { get; set; }

        //[DisplayName("Поени :")]
        //public int Points { get; set; }

        //[DisplayName("Локација :")]
        //public int LocationId { get; set; }
    }
}