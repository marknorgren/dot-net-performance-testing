using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

public class Todo
{
    [Required]
    [Description("Item description")]
    [SwaggerSchema("Item description")]
    public string Item { get; set; }

    [Required]
    [Description("Completion status")]
    [SwaggerSchema(@"Item description.
                    <br>Additional info on a new line.
                    <br>And another one.")]
    public bool Complete { get; set; }
}