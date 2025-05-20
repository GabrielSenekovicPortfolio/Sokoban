using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

public static class CreateObsidianFromGoogleDoc
{
    public static void Create(string url, string outputDirectory, List<string> logLines)
    {
        var credential = GoogleCredential.FromFile("Assets/service_account.json")
        .CreateScoped(DocsService.Scope.DocumentsReadonly);
        var service = new DocsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "UnityObsidianSync"
        });
        var doc = service.Documents.Get(url).Execute();
    }
}
