using System.Windows.Controls;

namespace Yaoya.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
