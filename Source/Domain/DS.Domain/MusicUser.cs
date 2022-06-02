using DS.Common.Exceptions;
using DS.Common.Extensions;

namespace DS.Domain;

public class MusicUser : IEquatable<MusicUser>
{
#pragma warning disable CS8618
    protected MusicUser() { }
#pragma warning restore CS8618

    public MusicUser(Guid id, string name, string profilePictureUri = "")
    {
        if (id == Guid.Empty)
            throw new GuidIsEmptyException(nameof(Guid));

        Id = id;
        Name = name.ThrowIfNull();
        ProfilePictureUri = profilePictureUri;
        MediaLibrary = new MediaLibrary(id);
        ListeningQueue = new ListeningQueue(id);
    }
    public Guid Id { get; private init; }
    public string Name { get; set; }
    public MediaLibrary MediaLibrary { get; private init; }
    public string? ProfilePictureUri { get; set; }
    public ListeningQueue ListeningQueue { get; private init; }

    public bool Equals(MusicUser? other) => other?.Id.Equals(Id) ?? false;
    public override bool Equals(object? obj) => Equals(obj as MusicUser);
    public override int GetHashCode() => Id.GetHashCode();
}