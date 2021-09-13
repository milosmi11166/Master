using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using ReceptiAPI.DTO;
using ReceptiAPI.Izuzeci;
using ReceptiAPI.Konstante;
using ReceptiAPI.Servisi.Interfejsi;
using System.Threading.Tasks;

namespace ReceptiAPI.Testovi.FunkcijeTestovi
{
    [TestFixture]
    public class ReceptiFunkcijeTestovi
    {
        private ReceptiFunkcije _receptiFunkcije;
        private Mock<IReceptiServis> _receptiServisMok;

        [OneTimeSetUp]
        public void Podesavanje()
        {
            _receptiServisMok = new Mock<IReceptiServis>();

            _receptiFunkcije = new ReceptiFunkcije(
                new NullLoggerFactory(),
                _receptiServisMok.Object);
        }

        [Test]
        public async Task KreirajRecept_SaUspesnimUpisom_TrebaDaVratiUspesanOdgovor()
        {
            //Podesi
            ReceptDTO recept = new ReceptDTO
            {
                Id = "123",
                Naziv = "Pita sa jabukama",
                Opis = "Opis"
            };

            _receptiServisMok.Setup(x => x.Kreiraj(It.IsAny<ReceptDTO>()))
                .ReturnsAsync(recept);
            
            //Izvrsi
            var odgovor = await _receptiFunkcije.KreirajRecept(recept);
            ReceptDTO odgovorDTO = (ReceptDTO)odgovor.Value;

            //Potvrdi
            Assert.AreEqual(201, odgovor.StatusCode);
            Assert.AreEqual("123", odgovorDTO.Id);
            Assert.AreEqual("Pita sa jabukama", odgovorDTO.Naziv);
            Assert.AreEqual("Opis", odgovorDTO.Opis);
        }

        [Test]
        public async Task KreirajRecept_SaNeuspesnimUpisom_TrebaDaVratiGresku()
        {
            //Podesi
            ReceptDTO recept = new ReceptDTO
            {
                Id = "123",
                Naziv = "Pita sa jabukama",
                Opis = "Opis"
            };

            _receptiServisMok.Setup(x => x.Kreiraj(It.IsAny<ReceptDTO>()))
                .ThrowsAsync(new ReceptiAPIIzuzetak(500, KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka));

            //Izvrsi
            var odgovor = await _receptiFunkcije.KreirajRecept(recept);
            GreskaDTO odgovorDTO = (GreskaDTO)odgovor.Value;

            //Potvrdi
            Assert.AreEqual(500, odgovor.StatusCode);
            Assert.AreEqual(KonstantneVrednosti.GreskaPrilikomPristupaBaziPodataka, odgovorDTO.PorukaGreske);
        }
    }
}
