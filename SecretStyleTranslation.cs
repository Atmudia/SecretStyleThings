using Secret_Style_Things.Utils;

namespace Secret_Style_Things
{
    internal class SecretStyleTranslation
    {
        public string basicName;
        public string original;
        public string replacement;

        public SecretStyleTranslation()
        {
        }

        public SecretStyleTranslation(Identifiable.Id id)
            : this(id, SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(id).GetAppearanceForSet(SlimeAppearance.AppearanceSaveSet.SECRET_STYLE))
        {
        }

        public SecretStyleTranslation(Identifiable.Id id, SlimeAppearance ssAppearance)
        {
            basicName = id.BasicName();
            original = Identifiable.GetName(id, false).RemoveBefore(Main.namePrefix).RemoveAfterLast(Main.nameSuffix);
            replacement = SRSingleton<GameContext>.Instance.MessageDirector.Get("actor", ssAppearance.NameXlateKey);
        }

        public SecretStyleTranslation(Identifiable.Id id, string originalName, string ssName)
        {
            basicName = id.BasicName();
            original = originalName.RemoveBefore(Main.namePrefix).RemoveAfterLast(Main.nameSuffix);
            replacement = ssName;
        }

        public void TryAdd()
        {
            if (string.IsNullOrEmpty(original) || string.IsNullOrEmpty(replacement))
                return;
            int index = Main.activesecrets.IndexOf(j => j.original.Length < original.Length);
            if (index < 0)
                index = Main.activesecrets.Count;
            Main.activesecrets.Insert(index, this);
        }
    }
}