using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReceptiAPI;
using ReceptiAPI.Mapiranja;
using ReceptiAPI.Modeli;
using ReceptiAPI.PristupPodacima;
using ReceptiAPI.PristupPodacima.Interfejsi;
using ReceptiAPI.Servisi;
using ReceptiAPI.Servisi.Interfejsi;

[assembly: WebJobsStartup(typeof(Pokretanje))]
namespace ReceptiAPI
{
    public class Pokretanje : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder graditelj)
        {
            graditelj.Services.AddSingleton<IKonfiguracijaServis, KonfiguracijaServis>();

            var maperPodesavanja = new MapperConfiguration(mp =>
            {
                mp.AddProfile(new ProfilMapiranja());
            });

            graditelj.Services.AddSingleton(maperPodesavanja.CreateMapper());

            graditelj.Services.AddLogging();
            graditelj.Services.AddScoped<IRepozitorijum<Recept>, Repozitorijum<Recept>>();
            graditelj.Services.AddScoped<IRepozitorijum<Sastojak>, Repozitorijum<Sastojak>>();
            graditelj.Services.AddScoped<IRepozitorijum<Namirnica>, Repozitorijum<Namirnica>>();
            graditelj.Services.AddScoped<IRepozitorijum<KorakPripreme>, Repozitorijum<KorakPripreme>>();

            graditelj.Services.AddScoped<IReceptiServis, ReceptiServis>();
            graditelj.Services.AddScoped<INamirniceServis, NamirniceServis>();
        }
    }
}
