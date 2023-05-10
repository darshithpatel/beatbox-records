using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatBox.Models
{
	public class AlbumFormat
	{
		[Key]
		public int Id { get; set; }
		[Display(Name ="Format")]
		[Required]
		[MaxLength(50)]

		public string Name { get; set; }
	}
}
