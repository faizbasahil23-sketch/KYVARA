namespace {{Namespace}}.Queries;

public sealed record Get{{Entity}}ByIdQuery(
    Guid Id
);

public sealed record GetAll{{Entity}}Query();

public sealed record Search{{Entity}}Query(
    string? Keyword
);
