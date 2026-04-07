using CampusNavigator.Models;

namespace CampusNavigator.Services
{
    public static class ProfileService
    {
        private const string KEY_SPECIALIZARE = "specializare";
        private const string KEY_AN = "an";
        private const string KEY_LIMBA = "limba";
        private const string KEY_GRUPA = "grupa";
        private const string KEY_SEMIGRUPA = "semigrupa";
        private const string KEY_SETUP_DONE = "setup_done";

        public static void Save(UserProfile profile)
        {
            Preferences.Set(KEY_SPECIALIZARE, profile.Specializare);
            Preferences.Set(KEY_AN, profile.An);
            Preferences.Set(KEY_LIMBA, profile.Limba);
            Preferences.Set(KEY_GRUPA, profile.Grupa);
            Preferences.Set(KEY_SEMIGRUPA, profile.Semigrupa);
            Preferences.Set(KEY_SETUP_DONE, true);
        }

        public static UserProfile? Load()
        {
            if (!Preferences.Get(KEY_SETUP_DONE, false))
                return null;

            return new UserProfile
            {
                Specializare = Preferences.Get(KEY_SPECIALIZARE, "Calculatoare"),
                An = Preferences.Get(KEY_AN, 1),
                Limba = Preferences.Get(KEY_LIMBA, "romana"),
                Grupa = Preferences.Get(KEY_GRUPA, 0),
                Semigrupa = Preferences.Get(KEY_SEMIGRUPA, 1),
            };
        }

        public static bool IsSetupDone()
        {
            return Preferences.Get(KEY_SETUP_DONE, false) && Preferences.Get(KEY_GRUPA, 0) != 0;
        }

        public static void Clear()
        {
            Preferences.Remove(KEY_SETUP_DONE);
        }
    }
}
