using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AOLadderer.Blazor.Models
{
    public class ExportToAunoModel : IReadOnlyCollection<ExportToAunoModel.FinalImplant>
    {
        private static readonly string BaseUrl = "http://auno.org/ao/equip.php?noedit=1&";

        public class FinalImplant
        {
            public FinalImplant(Implant implant)
            {
                AunoSlotID = implant.ImplantSlot.AunoSlotID;
                AOID = implant.ImplantTemplate.AOID;
                QL = implant.QL;
            }

            public int AunoSlotID { get; }
            public int AOID { get; }
            public int QL { get; }

            public IEnumerable<string> QueryParameters
            {
                get
                {
                    yield return $"id3-{AunoSlotID}={AOID}";
                    yield return $"ql3-{AunoSlotID}={QL}";
                }
            }
        }

        private readonly IReadOnlyCollection<FinalImplant> implants;

        public ExportToAunoModel(LadderProcess ladder)
        {
            implants = ladder.OrderedFinalImplants
                .Select(i => new FinalImplant(i))
                .ToArray();
        }

        public string BuildExportUrl()
            => BaseUrl + string.Join("&", this.SelectMany(i => i.QueryParameters));

        public int Count => implants.Count;
        public IEnumerator<FinalImplant> GetEnumerator() => implants.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => implants.GetEnumerator();
    }
}
