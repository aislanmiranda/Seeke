
namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Branch
{
    public Guid Id { get; private set; }
    public string? BranchName { get; private set; }

    public Branch(Guid id, string? branchName)
    {
        Id = id;
        BranchName = branchName;
    }
}