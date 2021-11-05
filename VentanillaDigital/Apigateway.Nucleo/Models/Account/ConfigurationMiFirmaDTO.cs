using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Account
{
    public class ConfigurationMiFirmaDTO
    {
        public string status { get; set; }
        public long statusCode { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
    public partial class Data
    {
        public Guid configurationGuid { get; set; }
        public appearance appearance { get; set; }
        public Uri endPointDelivery { get; set; }
        public bool digitalSignature { get; set; }
        public bool authenticateSigner { get; set; }
    }
    public partial class appearance
    {
        public bool isThirdParty { get; set; }
        public string thirdPartyLoaderUrl { get; set; }
        public string thirdPartyLogoUrl { get; set; }
        public string thirdPartyColor { get; set; }
        public bool thirdPartyExpiration { get; set; }
        public int thirdPartyExpirationDays { get; set; }
        public bool thirdPartyAutoReminders { get; set; }
        public string thirdPartyConfirmTitle { get; set; }
        public string thirdPartyConfirmSend { get; set; }
        public bool thirdPartyFolders { get; set; }
        public bool thirdPartyOneDriveEnabled { get; set; }
        public bool thirdPartySignerFlowMe { get; set; }
        public bool thirdPartySignerFlowOthersAndMe { get; set; }
        public bool thirdPartySignerFlowOthers { get; set; }
        public string thirdPartySignerFlowMeTitle { get; set; }
        public string thirdPartySignerFlowOthersAndMeTitle { get; set; }
        public string thirdPartySignerFlowOthersTitle { get; set; }
        public string thirdPartyDocumentsTitle { get; set; }
        public string thirdPartySignerFlowTitle { get; set; }
        public bool thirdPartySendCopy { get; set; }
        public string thirdPartyCustomEndingMessage { get; set; }
        public string thirdPartyCcMessage { get; set; }
        public string thirdPartyInnerLogo { get; set; }
        public string thirdPartyName { get; set; }
        public string tpMainFolder { get; set; }
        public bool tpAllowToolbar { get; set; }
        public bool tpBasicFieldInitials { get; set; }
        public bool tpBasicFieldDate { get; set; }
        public bool tpBasicFieldText { get; set; }
        public bool tpLeftMenuEnabled { get; set; }
        public bool tpUserMenuEnabled { get; set; }
        public string tpOtherSignersEndingTitle { get; set; }
        public string tpOtherSignersEndingContent { get; set; }
        public bool tpPlantillasDisabled { get; set; }
    }
}
