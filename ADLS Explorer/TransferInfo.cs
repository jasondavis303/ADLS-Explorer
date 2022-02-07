namespace ADLS_Explorer
{
    public class TransferInfo
    {
        public TransferInfo() { }

        public TransferInfo(AZFSO cloudObject, string localObject)
        {
            CloudObject = cloudObject;
            LocalObject = localObject;
        }

        public AZFSO CloudObject { get; set; }

        public bool CloudObjectExists { get; set; }

        public string LocalObject { get; set; }
    }
}
