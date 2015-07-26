using System;
using System.ComponentModel.DataAnnotations;

namespace NonFactors.Mvc.Grid.Tests
{
    public class GridModel
    {
        [Display(Name = "Text")]
        public String Text { get; set; }

        public Boolean? NIsChecked { get; set; }
        public Boolean IsChecked { get; set; }
        public DateTime? NDate { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public Int32? NSum { get; set; }
        public Int32 Sum { get; set; }
    }
}
