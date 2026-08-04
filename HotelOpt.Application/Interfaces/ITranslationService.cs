namespace HotelOpt.Application.Interfaces;

public interface ITranslationService
{
    Task<string> TranslateAsync(string text, string targetLanguage);
}