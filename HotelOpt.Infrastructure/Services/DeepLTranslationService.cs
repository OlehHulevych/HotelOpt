using HotelOpt.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using DeepL;

namespace HotelOpt.Infrastructure.Services;

public class DeepLTranslationService:ITranslationService
{
    private readonly Translator _translator;

    public DeepLTranslationService(IConfiguration config)
    {
        _translator = new Translator(config["DeepL:key"] ?? throw new InvalidOperationException("DeepL:key is not configured"));

    }
    public async Task<string> TranslateAsync(string text, string targetLanguage)
    {
        var result = await _translator.TranslateTextAsync(text,null,targetLanguage);
        return result.Text;
    }
}