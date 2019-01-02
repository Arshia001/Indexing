public static class UserProfileUtils
{
    public static GrainIndexManager_Unique<string, IUserProfile> ByUsername = new GrainIndexManager_Unique<string, IUserProfile>("up_un", 16384, new StringHashGenerator());

    public static Task<bool> SetNameIfUnique(IGrainFactory GrainFactory, IUserProfile UserProfile, string Name)
    {
        return ByUsername.UpdateIndexIfUnique(GrainFactory, Name, UserProfile);
    }
}
