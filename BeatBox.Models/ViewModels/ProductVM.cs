using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBox.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        // For Dropdown of categories
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        // For Dropdown of Albumformats
        [ValidateNever]
        public IEnumerable<SelectListItem> AlbumFormatList { get; set; }
    }
}
