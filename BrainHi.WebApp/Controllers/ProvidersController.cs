﻿using BrainHi.Data;
using BrainHi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainHi.WebApp.Controllers
{
    [Route("/providers")]
    public class ProvidersController : Controller
    {
        //  Application repository of the application
        private readonly IApplicationRepository _repository;

        //  Repository will be provided by dependency injection
        public ProvidersController(IApplicationRepository repository) => _repository = repository;

        //  Displays view with a list of providers
        [HttpGet]
        public async Task<ViewResult> List(string specialty = null, string name = null, string query = null)
        {
            //  Query all providers from repository
            IEnumerable<Provider> providers = await _repository.GetEntitiesAsync<Provider>();

            //  Filter providers to display depending on the provided parameters
            IEnumerable<Provider> filterProviders = providers;

            if (!string.IsNullOrEmpty(specialty))
            {
                //  Filter by specialty (use non case sensitive string comparison)
                filterProviders = filterProviders.Where(provider => provider.Specialty.Equals(specialty, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(name))
            {
                //  Filter by name (use non case sensitive contains operation)
                filterProviders = filterProviders.Where(provider => provider.ProviderFullName.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(query))
            {
                //  Filter by name or specialty that contains the provided query (use non case sensitive contains operation)
                filterProviders = filterProviders.Where(provider => provider.ProviderFullName.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                    provider.Specialty.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            return View(model: (providers, filterProviders));
        }

        //  Displays view with details of a specific provider
        [HttpGet("{id}")]
        public async Task<ViewResult> Display(int? id)
        {
            //  ID is required
            if (!id.HasValue) return View("NotFound");

            //  Get provider from repository
            Provider provider = await _repository.GetEntityAsync<Provider>(id.Value);

            //  Return view result
            return View(provider);
        }
    }
}