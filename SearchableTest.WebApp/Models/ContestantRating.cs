using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.Models
{
    public class ContestantRating
    {
        public int Id { get; set; }
        public int ContestantId { get; set; }
        public int Rating { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? RatedDate { get; set; }
        public virtual Contestant Contestant { get; set; }
    }
}