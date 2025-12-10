using FluentValidation;

namespace ElasticSentinel.Application.Features.Documents;

public class ProcessDocumentsValidator : AbstractValidator<ProcessDocumentsRequest>
{
    public ProcessDocumentsValidator()
    {
        // ProcessDocumentsRequest is a record with no properties currently
        // Add validation rules when properties are added
    }
}
