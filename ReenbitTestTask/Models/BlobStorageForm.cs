using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using ReenbitTestTask.Attributes;

namespace ReenbitTestTask.Models;

public class BlobStorageForm
{
    [Required]
    [DocxFile]
    public IBrowserFile? File { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}