﻿using AutoMapper;
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

[assembly: WebJobsStartup(typeof(Startup))]
namespace ReceptiAPI
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder graditelj)
        {

            var cosmosDbNazivBaze = System.Environment.GetEnvironmentVariable("cosmosDbNazivBaze");
            var cosmosDbUrl = System.Environment.GetEnvironmentVariable("cosmosDbUrl");
            var cosmosDbAutKljuc = System.Environment.GetEnvironmentVariable("cosmosDbAutKljuc");

            var maperPodesavanja = new MapperConfiguration(mp =>
            {
                mp.AddProfile(new ProfilMapiranja());
            });

            graditelj.Services.AddSingleton(maperPodesavanja.CreateMapper());

            graditelj.Services.AddLogging();
            graditelj.Services.AddScoped<IRepozitorijum<Recept>, Repozitorijum<Recept>>();

            graditelj.Services.AddSingleton<IKonfiguracijaServis, KonfiguracijaServis>();
            graditelj.Services.AddScoped<IReceptiServis, ReceptiServis>();



        }
    }
}
