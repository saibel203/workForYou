using Microsoft.Extensions.Logging;
using Moq;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.SharedForTests;

public class TestsBase<T> : IDisposable
{
    protected readonly WorkForYouDbContext Context;
    protected readonly ILogger<T> Logger;

    protected const string CandidateRole = "candidate";
    protected const string EmployerRole = "employer";

    protected TestsBase()
    {
        Logger = Mock.Of<ILogger<T>>();
        Context = WorkForYouDbContextFactory.CreateWithData();
    }

    public void Dispose()
    {
        WorkForYouDbContextFactory.Destroy(Context);
    }
}
