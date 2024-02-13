using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace ReenbitTestTask.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class DocxFileAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not IBrowserFile file) return false;
        
        var fileName = file.Name;
        return fileName.EndsWith(".docx");
    }
}