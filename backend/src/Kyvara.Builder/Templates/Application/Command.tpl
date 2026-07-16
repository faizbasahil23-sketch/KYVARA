namespace {{Namespace}}.Commands;

public sealed record Create{{Entity}}Command(
    Guid Id
);

public sealed record Update{{Entity}}Command(
    Guid Id
);

public sealed record Delete{{Entity}}Command(
    Guid Id
);
