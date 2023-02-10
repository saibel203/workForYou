using WorkForYou.Core.Models;

namespace WorkForYou.WebUI.ViewModels.AdditionalViewModels;

public class ConnectMainUsersDataViewmodel
{
    public IReadOnlyList<EnglishLevel>? EnglishLevels { get; set; } = new List<EnglishLevel>();
    public IReadOnlyList<WorkCategory>? WorkCategories { get; set; } = new List<WorkCategory>();
    public IReadOnlyList<CommunicationLanguage>? CommunicationLanguages { get; set; } = new List<CommunicationLanguage>();

}
