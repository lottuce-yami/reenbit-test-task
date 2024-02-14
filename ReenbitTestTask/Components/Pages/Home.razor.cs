﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ReenbitTestTask.Models;
using ReenbitTestTask.Services;

namespace ReenbitTestTask.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    public ILogger<Home> Logger { get; set; } = default!;

    [Inject]
    public BlobStorageService BlobStorageService { get; set; } = default!;
    
    [SupplyParameterFromForm(FormName = "BlobStorageForm")]
    public BlobStorageForm? Model { get; set; }
    
    protected override void OnInitialized() => Model ??= new BlobStorageForm();

    private void AddFile(InputFileChangeEventArgs e)
    {
        Model!.File = e.File;
    }
    
    private async void Submit()
    {
        Logger.LogInformation("File: {file}, E-mail: {email}", Model!.File!.Name, Model.Email);
        Logger.LogInformation("{response}", await BlobStorageService.UploadAsync("documents", Model));
    }
}