using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Dtos.Autores
{
    public class AutorCreateDto
    {
        [Display(Name = "Nombre")]
        [StringLength(70, ErrorMessage = "El {0} permite un maximo de {1} caracteres")]
        [Required(ErrorMessage = "El {0} es requerido")]
        public string Name { get; set; }
    }
}
